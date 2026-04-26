using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online
{
    public class LobbyManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        public LobbySettings lobbySettings;
        
        private NetworkRunner _runner;
        
        private async void Start()
        {
            _runner = GetComponent<NetworkRunner>();
            _runner.AddCallbacks(this);

            lobbySettings.runner = _runner;
            
            await ConnectToGame();
        }
        
        private async Task ConnectToGame()
        {
            var sceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            var sceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>();
            
            var result = await _runner.StartGame(new StartGameArgs
            {
                GameMode    = GameMode.Shared,
                SessionName = "Default Room",
                PlayerCount = 2,
                Scene = SceneRef.FromIndex(sceneIndex),
                SceneManager = sceneManager,
            });

            if (result.Ok)
                Debug.Log($"Conectado!");
            else
                Debug.LogError($"Error: {result.ShutdownReason}");
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log($"Jugador entró: {player} | Soy master client? {runner.LocalPlayer} -> {runner.IsSharedModeMasterClient}");
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log($"Jugador salió: {player} | Soy master client? {runner.LocalPlayer} -> {runner.IsSharedModeMasterClient}");
        }

        // El resto de callbacks obligatorios — vacíos por ahora
        public void OnInput(NetworkRunner runner, NetworkInput input) { }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnConnectedToServer(NetworkRunner runner) { }
        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }
        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    }
}