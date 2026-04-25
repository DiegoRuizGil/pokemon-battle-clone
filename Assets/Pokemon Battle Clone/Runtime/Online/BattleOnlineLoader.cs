using System;
using Fusion;
using Pokemon_Battle_Clone.Runtime.Battles.Infrastructure;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online
{
    public class BattleOnlineLoader : NetworkBehaviour
    {
        [Header("Scene Management")]
        [SerializeField] private string battleSceneName;

        [Header("Dependencies")]
        [SerializeField] private BattleSettings battleSettings;
        [SerializeField] private TeamCollection teamCollection;

        [Networked] private int ReadyCount { get; set; }

        public void SetReady()
        {
            var teamIndex = teamCollection.IndexOf(battleSettings.playerTeamConfig);
            RPC_SetRivalTeam(teamIndex);
            RPC_NotifyReady();
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_SetRivalTeam(int teamIndex, RpcInfo info = default)
        {
            if (info.Source == Runner.LocalPlayer) return;
            
            var teamConfig = teamCollection[teamIndex];
            battleSettings.rivalTeamConfig = teamConfig;
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_NotifyReady()
        {
            Debug.Log($"{Runner.LocalPlayer} increment ready count");
            ReadyCount++;
            if (ReadyCount >= 2)
            {
                var seed = GenerateSeed();
                Debug.Log($"{Runner.LocalPlayer} generating battle seed: {seed}");
                RPC_StartBattle(seed);
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_StartBattle(int battleSeed)
        {
            Debug.Log($"[{DateTime.Now.Millisecond}] - {Runner.LocalPlayer} assigning battle seed: {battleSeed}");
            battleSettings.battleSeed = battleSeed;
            if (HasStateAuthority)
                Runner.LoadScene(battleSceneName);
        }

        private static int GenerateSeed() => Guid.NewGuid().GetHashCode();
    }
}