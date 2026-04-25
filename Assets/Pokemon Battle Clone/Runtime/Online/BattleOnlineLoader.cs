using System;
using System.Collections.Generic;
using Fusion;
using Pokemon_Battle_Clone.Runtime.Battles.Infrastructure;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online
{
    public class BattleOnlineLoader : NetworkBehaviour, IPlayerLeft
    {
        [Header("Scene Management")]
        [SerializeField] private string battleSceneName;

        [Header("Dependencies")]
        [SerializeField] private BattleSettings battleSettings;
        [SerializeField] private TeamCollection teamCollection;

        private readonly HashSet<PlayerRef> _readyPlayers = new();
        private int ReadyCount => _readyPlayers.Count;

        public void PlayerLeft(PlayerRef player)
        {
            Debug.Log($"{player} left");
            _readyPlayers.Remove(player);
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