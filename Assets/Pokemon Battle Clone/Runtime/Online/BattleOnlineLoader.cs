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

        [Networked, OnChangedRender(nameof(OnPlayerChanged)), Capacity(2)]
        private NetworkDictionary<PlayerRef, PlayerLobbyInfo> Players => default;
        
        public event Action OnLobbyStateChanged = delegate { };
        
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
            if (IsPlayerReady(Runner.LocalPlayer)) return;
            
            var teamIndex = teamCollection.IndexOf(battleSettings.playerTeamConfig);
            RPC_SetReady(Runner.LocalPlayer, teamIndex);
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_SetReady(PlayerRef player, int teamIndex)
        {
            if (player != Runner.LocalPlayer)
                SetRivalTeam(teamIndex);

            if (!HasStateAuthority)
                return;
            if (IsPlayerReady(player))
                return;
            
            Players.Set(player, new PlayerLobbyInfo { IsReady = true });
            CheckAllReady();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_StartBattle(int battleSeed)
        {
            Debug.Log($"[{DateTime.Now.Millisecond}] - assigning battle seed: {battleSeed}");
            battleSettings.battleSeed = battleSeed;
            if (HasStateAuthority)
                Runner.LoadScene(battleSceneName);
        }

        private bool IsPlayerReady(PlayerRef player) => Players.TryGet(player, out var info) && info.IsReady;

        private void CheckAllReady()
        {
            if (Players.Count < 2) return;

            foreach (var kvp in Players)
                if (!kvp.Value.IsReady) return;
            
            RPC_StartBattle(GenerateSeed());
        }

        private void SetRivalTeam(int teamIndex)
        {
            var teamConfig = teamCollection[teamIndex];
            battleSettings.rivalTeamConfig = teamConfig;
        }

        private void OnPlayerChanged()
        {
            var state = new Dictionary<PlayerRef, PlayerLobbyInfo>();
            foreach (var kvp in Players)
                state[kvp.Key] = kvp.Value;
            
            lobbySession.RaiseLobbyStateChanged(GetState());
        }

        private LobbyState GetState()
        {
            var localPlayerState = new PlayerState { IsPresent = true };
            if (Players.TryGet(Runner.LocalPlayer, out var localPlayerInfo))
                localPlayerState.IsReady = localPlayerInfo.IsReady;

            var remotePlayerState = new PlayerState();
            foreach (var kvp in Players)
            {
                if (kvp.Key == Runner.LocalPlayer) continue;
                remotePlayerState.IsPresent = true;
                remotePlayerState.IsReady = kvp.Value.IsReady;
                break;
            }
            
            var state = new LobbyState
            {
                SessionCode = lobbySession.currentSessionCode,
                LocalPlayer = localPlayerState,
                RemotePlayer = remotePlayerState
            };

            return state;
        }

        private static int GenerateSeed() => new System.Random().Next();
    }
}