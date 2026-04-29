using System.Text;
using Fusion;
using Pokemon_Battle_Clone.Runtime.Battles.Infrastructure;
using Pokemon_Battle_Clone.Runtime.Database;
using Pokemon_Battle_Clone.Runtime.Online.Lobby.Events;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online.Lobby
{
    public struct PlayerLobbyInfo : INetworkStruct
    {
        public NetworkBool IsReady;
        public int TeamIndex;
    }

    public class BattleOnlineLoader : NetworkBehaviour
    {
        [Header("Scene Management")]
        [SerializeField] private string battleSceneName;

        [Header("Dependencies")]
        [SerializeField] private GameSession gameSession;
        [SerializeField] private NetworkEventsChannel networkEventsChannel;
        [SerializeField] private BattleSettings battleSettings;
        [SerializeField] private TeamCollection teamCollection;

        [Networked, OnChangedRender(nameof(OnGameStateChanged)), Capacity(2)]
        private NetworkDictionary<PlayerRef, PlayerLobbyInfo> Players => default;

        [Networked] private int BattleSeed { get; set; }


        [ContextMenu("Debug Players")]
        private void DebugPlayers()
        {
            var players = new StringBuilder();
            foreach (var kvp in Players)
                players.AppendLine($"{kvp.Key}-> IsReady: {kvp.Value.IsReady}, TeamIndex: {kvp.Value.TeamIndex}");
            
            Debug.Log(players.ToString());
        }
        
        
        public void Init()
        {
            if (!HasStateAuthority) return;
            
            BattleSeed = GenerateSeed();
        }

        public override void Spawned()
        {
            networkEventsChannel.OnPlayerJoined += HandlePlayerJoined;
            networkEventsChannel.OnPlayerLeft += HandlePlayerLeft;
            gameSession.OnPlayerSetReady += SetReady;
            
            RPC_RequestRegister(Runner.LocalPlayer);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            LoadBattleSettings(); // load settings before loading battle scene
            
            networkEventsChannel.OnPlayerJoined -= HandlePlayerJoined;
            networkEventsChannel.OnPlayerLeft -= HandlePlayerLeft;
            gameSession.OnPlayerSetReady -= SetReady;
        }

        private void HandlePlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            RPC_RequestRegister(player);
        }

        private void HandlePlayerLeft(NetworkRunner runner, PlayerRef player)
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
        private void RPC_SetReady(PlayerRef rpcSender, int teamIndex)
        {
            if (!HasStateAuthority)
                return;
            if (IsPlayerReady(rpcSender))
                return;

            Players.Set(rpcSender, new PlayerLobbyInfo { IsReady = true, TeamIndex = teamIndex });
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_RequestRegister(PlayerRef player)
        {
            if (!Players.ContainsKey(player))
                Players.Set(player, new PlayerLobbyInfo { IsReady = false, TeamIndex = 0 });
        }

        private void StartBattle() => Runner.LoadScene(battleSceneName);

        private bool IsPlayerReady(PlayerRef player) => Players.TryGet(player, out var info) && info.IsReady;

        private bool CheckAllReady()
        {
            if (Players.Count < 2) return false;

            foreach (var kvp in Players)
                if (!kvp.Value.IsReady) return false;
            
            return true;
        }

        private void OnGameStateChanged()
        {
            gameSession.SetGameState(GetState());

            if (HasStateAuthority && CheckAllReady())
                StartBattle();
        }

        private GameState GetState()
        {
            var localPlayerState = new PlayerState { IsPresent = true };
            if (TryGetLocalPlayerInfo(out var localInfo))
                localPlayerState.IsReady = localInfo.IsReady;

            var remotePlayerState = new PlayerState { IsPresent = false };
            if (TryGetRemotePlayerInfo(out var remoteInfo))
            {
                remotePlayerState.IsPresent = true;
                remotePlayerState.IsReady = remoteInfo.IsReady;
            }
            
            var state = new GameState
            {
                SessionCode = Runner.SessionInfo.Name,
                LocalPlayer = localPlayerState,
                RemotePlayer = remotePlayerState
            };

            return state;
        }

        private void LoadBattleSettings()
        {
            if (TryGetRemotePlayerInfo(out var remoteInfo))
                battleSettings.rivalTeamConfig = teamCollection[remoteInfo.TeamIndex];
            
            battleSettings.battleSeed = BattleSeed;
        }

        private bool TryGetLocalPlayerInfo(out PlayerLobbyInfo info) => Players.TryGet(Runner.LocalPlayer, out info);

        private bool TryGetRemotePlayerInfo(out PlayerLobbyInfo info)
        {
            foreach (var kvp in Players)
            {
                if (kvp.Key == Runner.LocalPlayer) continue;
                info = kvp.Value;
                return true;
            }
            
            info = default;
            return false;
        }
        
        private static int GenerateSeed() => System.Guid.NewGuid().GetHashCode();
    }
}