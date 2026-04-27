using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online.Lobby.UI
{
    public class SessionPanel : MonoBehaviour
    {
        [SerializeField] private LobbySession lobbySession;
        [SerializeField] private LobbyManager lobbyManager;
        [SerializeField] private GameInfoDisplayer gameInfoDisplayer;

        private void OnEnable() => lobbySession.OnGameStateChanged += gameInfoDisplayer.Display;
        private void OnDisable() => lobbySession.OnGameStateChanged -= gameInfoDisplayer.Display;

        public void OnLeavePressed() => lobbyManager.LeaveGame();
    }
}