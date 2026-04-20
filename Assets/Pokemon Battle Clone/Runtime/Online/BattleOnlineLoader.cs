using System;
using Fusion;
using Pokemon_Battle_Clone.Runtime.Battles.Infrastructure;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online
{
    public enum BattleLoadState { WaitingForPlayers, Ready }

    public struct BattleLoadData : INetworkStruct
    {
        public BattleLoadState State;
        public int Seed;
    }
    
    public class BattleOnlineLoader : NetworkBehaviour
    {
        [Header("Scene Management")]
        [SerializeField] private string battleSceneName;

        [Header("Dependencies")]
        [SerializeField] private BattleSettings battleSettings;
        [SerializeField] private TeamCollection teamCollection;

        [Networked, OnChangedRender(nameof(OnBattleLoadDataChanged))]
        private BattleLoadData LoadData { get; set; }

        [Networked]
        private int ReadyCount { get; set; }

        public void SetReady()
        {
            var teamIndex = teamCollection.IndexOf(battleSettings.playerTeamConfig);
            RPC_SetRivalTeam(teamIndex);
            
            RPC_NotifyReady();
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_NotifyReady()
        {
            ReadyCount++;
            if (ReadyCount >= 2)
            {
                LoadData = new BattleLoadData
                {
                    State = BattleLoadState.Ready,
                    Seed = GenerateSeed()
                };
            }
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_SetRivalTeam(int teamIndex, RpcInfo info = default)
        {
            if (info.Source == Runner.LocalPlayer) return;
            
            var teamConfig = teamCollection[teamIndex];
            battleSettings.rivalTeamConfig = teamConfig;
        }
        
        private void OnBattleLoadDataChanged()
        {
            if (LoadData.State != BattleLoadState.Ready) return;

            battleSettings.battleSeed = LoadData.Seed;
            TryStartBattle();
        }

        private void TryStartBattle()
        {
            if (HasStateAuthority)
                Runner.LoadScene(battleSceneName);
        }
        
        private static int GenerateSeed() => Guid.NewGuid().GetHashCode();
    }
}