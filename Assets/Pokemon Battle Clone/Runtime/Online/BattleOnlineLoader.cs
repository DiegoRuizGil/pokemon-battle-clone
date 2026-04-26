using System;
using System.Collections.Generic;
using Fusion;
using Pokemon_Battle_Clone.Runtime.Battles.Infrastructure;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online
{
    public struct PlayerLobbyInfo : INetworkStruct
    {
        public NetworkBool IsReady;
    }
    
    public class BattleOnlineLoader : NetworkBehaviour
    {
        [Header("Scene Management")]
        [SerializeField] private string battleSceneName;

        [Header("Dependencies")]
        [SerializeField] private LobbySession lobbySession;
        [SerializeField] private BattleSettings battleSettings;
        [SerializeField] private TeamCollection teamCollection;

        [Networked, Capacity(2)]
        private NetworkDictionary<PlayerRef, PlayerLobbyInfo> Players => default;

        public event Action OnLobbyStateChanged = delegate { };
        
        
        private readonly HashSet<PlayerRef> _readyPlayers = new();
        private int ReadyCount => _readyPlayers.Count;


        public override void Spawned()
        {
            lobbySession.OnPlayerJoined += HandlePlayerJoined;
            lobbySession.OnPlayerLeft += HandlePlayerLeft;
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            lobbySession.OnPlayerJoined -= HandlePlayerJoined;
            lobbySession.OnPlayerLeft -= HandlePlayerLeft;
        }

        private void HandlePlayerJoined(PlayerRef player)
        {
            if (HasStateAuthority)
                Players.Set(player, new PlayerLobbyInfo { IsReady = false });
        }

        private void HandlePlayerLeft(PlayerRef player)
        {
            if (HasStateAuthority)
                Players.Remove(player);
        }

        public void SetReady()
        {
            var teamIndex = teamCollection.IndexOf(battleSettings.playerTeamConfig);
            RPC_NotifyReadyAndTeam(teamIndex);
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_NotifyReadyAndTeam(int teamIndex, RpcInfo info = default)
        {
            if (info.Source != Runner.LocalPlayer)
                SetRivalTeam(teamIndex);

            if (HasStateAuthority)
                NotifyReady(info.Source);
        }

        private void SetRivalTeam(int teamIndex)
        {
            var teamConfig = teamCollection[teamIndex];
            battleSettings.rivalTeamConfig = teamConfig;
        }

        private void NotifyReady(PlayerRef playerReady)
        {
            _readyPlayers.Add(playerReady);

            Debug.Log($"Player {playerReady} is ready. Total ready: {ReadyCount}");
            
            if (ReadyCount >= 2)
            {
                var seed = GenerateSeed();
                Debug.Log($"Generating battle seed: {seed}");
                RPC_StartBattle(seed);
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_StartBattle(int battleSeed)
        {
            Debug.Log($"[{DateTime.Now.Millisecond}] - assigning battle seed: {battleSeed}");
            battleSettings.battleSeed = battleSeed;
            if (HasStateAuthority)
                Runner.LoadScene(battleSceneName);
        }

        private static int GenerateSeed() => new System.Random().Next();
    }
}