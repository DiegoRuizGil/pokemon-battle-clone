using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online.Lobby.Events
{
    [CreateAssetMenu(menuName = "Pokemon Battle Clone/Online/Events Channel", fileName = "Network Events Channel")]
    public class NetworkEventsChannel : ScriptableObject
    {
        public event Action<NetworkRunner, NetworkObject, PlayerRef> OnObjectExitAOI;
        public event Action<NetworkRunner, NetworkObject, PlayerRef> OnObjectEnterAOI;
        public event Action<NetworkRunner, PlayerRef>                OnPlayerJoined;
        public event Action<NetworkRunner, PlayerRef>                OnPlayerLeft;
        public event Action<NetworkRunner, ShutdownReason>           OnShutdown;
        public event Action<NetworkRunner, NetDisconnectReason>      OnDisconnectedFromServer;
        public event Action<NetworkRunner, NetworkRunnerCallbackArgs.ConnectRequest, byte[]> OnConnectRequest;
        public event Action<NetworkRunner, NetAddress, NetConnectFailedReason>               OnConnectFailed;
        public event Action<NetworkRunner, SimulationMessagePtr>     OnUserSimulationMessage;
        public event Action<NetworkRunner, PlayerRef, ReliableKey, ArraySegment<byte>> OnReliableDataReceived;
        public event Action<NetworkRunner, PlayerRef, ReliableKey, float>              OnReliableDataProgress;
        public event Action<NetworkRunner, NetworkInput>             OnInput;
        public event Action<NetworkRunner, PlayerRef, NetworkInput>  OnInputMissing;
        public event Action<NetworkRunner>                           OnConnectedToServer;
        public event Action<NetworkRunner, List<SessionInfo>>        OnSessionListUpdated;
        public event Action<NetworkRunner, Dictionary<string, object>> OnCustomAuthenticationResponse;
        public event Action<NetworkRunner, HostMigrationToken>       OnHostMigration;
        public event Action<NetworkRunner>                           OnSceneLoadDone;
        public event Action<NetworkRunner>                           OnSceneLoadStart;

        public void RaiseObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) => OnObjectExitAOI?.Invoke(runner, obj, player);
        public void RaiseObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) => OnObjectEnterAOI?.Invoke(runner, obj, player);
        public void RaisePlayerJoined(NetworkRunner runner, PlayerRef player) => OnPlayerJoined?.Invoke(runner, player);
        public void RaisePlayerLeft(NetworkRunner runner, PlayerRef player) => OnPlayerLeft?.Invoke(runner, player);
        public void RaiseShutdown(NetworkRunner runner, ShutdownReason shutdownReason) => OnShutdown?.Invoke(runner, shutdownReason);
        public void RaiseDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) => OnDisconnectedFromServer?.Invoke(runner, reason);
        public void RaiseConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) => OnConnectRequest?.Invoke(runner, request, token);
        public void RaiseConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) => OnConnectFailed?.Invoke(runner, remoteAddress, reason);
        public void RaiseUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) => OnUserSimulationMessage?.Invoke(runner, message);
        public void RaiseReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) => OnReliableDataReceived?.Invoke(runner, player, key, data);
        public void RaiseReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) => OnReliableDataProgress?.Invoke(runner, player, key, progress);
        public void RaiseInput(NetworkRunner runner, NetworkInput input) => OnInput?.Invoke(runner, input);
        public void RaiseInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) => OnInputMissing?.Invoke(runner, player, input);
        public void RaiseConnectedToServer(NetworkRunner runner) => OnConnectedToServer?.Invoke(runner);
        public void RaiseSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) => OnSessionListUpdated?.Invoke(runner, sessionList);
        public void RaiseCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) => OnCustomAuthenticationResponse?.Invoke(runner, data);
        public void RaiseHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) => OnHostMigration?.Invoke(runner, hostMigrationToken);
        public void RaiseSceneLoadDone(NetworkRunner runner) => OnSceneLoadDone?.Invoke(runner);
        public void RaiseSceneLoadStart(NetworkRunner runner) => OnSceneLoadStart?.Invoke(runner);
    }
}