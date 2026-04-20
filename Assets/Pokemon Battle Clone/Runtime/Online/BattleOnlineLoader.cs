using System;
using Fusion;
using Pokemon_Battle_Clone.Runtime.Battles.Infrastructure;
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
        [SerializeField] private BattleSettings battleSettings;
        [SerializeField] private string battleSceneName;

        [Networked, OnChangedRender(nameof(OnBattleLoadDataChanged))]
        private BattleLoadData LoadData { get; set; }

        [Networked]
        private int ReadyCount { get; set; }

        public void SetReady() => RPC_NotifyReady();

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