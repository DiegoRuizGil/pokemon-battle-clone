using Fusion;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online
{
    public class BattleOnlineLoader : NetworkBehaviour
    {
        [SerializeField] private string battleSceneName;
        
        [Networked]
        private int ReadyCount { get; set; }

        public void SetReady()
        {
            RPC_NotifyReady();
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_NotifyReady()
        {
            ReadyCount++;
        }

        public override void Render()
        {
            if (ReadyCount >= 2)
                TryStartBattle();
        }

        private void TryStartBattle()
        {
            if (!HasStateAuthority) return;

            Runner.LoadScene(battleSceneName);
        }
    }
}