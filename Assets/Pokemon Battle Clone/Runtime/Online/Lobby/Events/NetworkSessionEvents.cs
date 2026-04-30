using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online.Lobby.Events
{
    public class NetworkSessionEvents : MonoBehaviour, INetworkRunnerCallbacks
    {
        public NetworkEventsChannel EventsChannel { get; set; }

        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) => EventsChannel?.RaiseObjectExitAOI(runner, obj, player);
        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) => EventsChannel?.RaiseObjectEnterAOI(runner, obj, player);
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) => EventsChannel?.RaisePlayerJoined(runner, player);
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) => EventsChannel?.RaisePlayerLeft(runner, player);
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) => EventsChannel?.RaiseShutdown(runner, shutdownReason);
        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) => EventsChannel?.RaiseDisconnectedFromServer(runner, reason);
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) => EventsChannel?.RaiseConnectRequest(runner, request, token);
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) => EventsChannel?.RaiseConnectFailed(runner, remoteAddress, reason);
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) => EventsChannel?.RaiseUserSimulationMessage(runner, message);
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) => EventsChannel?.RaiseReliableDataReceived(runner, player, key, data);
        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) => EventsChannel?.RaiseReliableDataProgress(runner, player, key, progress);
        public void OnInput(NetworkRunner runner, NetworkInput input) => EventsChannel?.RaiseInput(runner, input);
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) => EventsChannel?.RaiseInputMissing(runner, player, input);
        public void OnConnectedToServer(NetworkRunner runner) => EventsChannel?.RaiseConnectedToServer(runner);
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) => EventsChannel?.RaiseSessionListUpdated(runner, sessionList);
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) => EventsChannel?.RaiseCustomAuthenticationResponse(runner, data);
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) => EventsChannel?.RaiseHostMigration(runner, hostMigrationToken);
        public void OnSceneLoadDone(NetworkRunner runner) => EventsChannel?.RaiseSceneLoadDone(runner);
        public void OnSceneLoadStart(NetworkRunner runner) => EventsChannel?.RaiseSceneLoadStart(runner);
    }
}