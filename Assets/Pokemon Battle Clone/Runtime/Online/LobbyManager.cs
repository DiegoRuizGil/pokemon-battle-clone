using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pokemon_Battle_Clone.Runtime.Online
{
    public class LobbyManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        public LobbySession lobbySession;
        
        private NetworkRunner _runner;
        private bool Initialized => _runner != null;

        private async void Start()
        {
            Init();
            await JoinLobbyAsync();
        }

        private void Init()
        {
            _runner = CreateRunner();
        }

        private NetworkRunner CreateRunner()
        {
            var go = new GameObject("Network Runner");
            var runner = go.AddComponent<NetworkRunner>();
            go.AddComponent<NetworkSceneManagerDefault>();
            runner.AddCallbacks(this);
            return runner;
        }

        private async Task ShutdownAsync()
        {
            if (Initialized)
                await _runner.Shutdown();
        }

        private async Task JoinLobbyAsync()
        {
            if (!Initialized) Init();

            await _runner.JoinSessionLobby(SessionLobby.Shared);
        }

        private async Task<StartGameResult> CreateAndJoinGameAsync()
        {
            var result = await JoinGameAsync(GenerateSessionCode());
            return result;
        }

        private async Task<StartGameResult> JoinGameAsync(string sessionName)
        {
            if (!Initialized) Init();

            var result = await ConnectToGameAsync(sessionName);
            return result;
        }

        private async Task<StartGameResult> ConnectToGameAsync(string sessionName)
        {
            var result = await _runner.StartGame(new StartGameArgs
            {
                GameMode    = GameMode.Shared,
                SessionName = sessionName,
                PlayerCount = 2,
                Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
                SceneManager = _runner.SceneManager,
            });

            if (result.Ok)
                lobbySession.currentSessionCode = sessionName;
            else
            {
                Debug.Log($"Try to connect to game, but couldn't: {result.ErrorMessage}");
            }
            
            return result;
        }


        public async void CreateGame()
        {
            await CreateAndJoinGameAsync();
        }
        
        public async void ReturnToLobby()
        {
            await ShutdownAsync();
            await JoinLobbyAsync();
        }

        #region CALLBACKS
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log($"Jugador entró: {player} | Soy master client? {runner.LocalPlayer} -> {runner.IsSharedModeMasterClient}");
            lobbySession.RaisePlayerJoined(player);
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log($"Jugador salió: {player} | Soy master client? {runner.LocalPlayer} -> {runner.IsSharedModeMasterClient}");
            lobbySession.RaisePlayerLeft(player);
        }

        public void OnInput(NetworkRunner runner, NetworkInput input) { }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnConnectedToServer(NetworkRunner runner) { }
        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            var currentSessions = string.Join(',', sessionList.Select(info => info.Name));
            Debug.Log($"Current Sessions: {currentSessions}");
        }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }
        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }

        #endregion

        private static string GenerateSessionCode()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            var code = new System.Text.StringBuilder(6);
            var rng = new System.Random();
            for (int i = 0; i < 6; i++)
                code.Append(chars[rng.Next(chars.Length)]);
            return code.ToString();
        }
    }
}