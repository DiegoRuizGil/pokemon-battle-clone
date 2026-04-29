using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online.Lobby.UI
{
    public class SessionPanel : MonoBehaviour
    {
        [SerializeField] private GameSession gameSession;
        [SerializeField] private LobbyManager lobbyManager;
        [SerializeField] private GameInfoDisplayer gameInfoDisplayer;

        [SerializeField] private GameObject content;

        private void OnEnable() => gameSession.OnGameStateChanged += gameInfoDisplayer.Display;
        private void OnDisable() => gameSession.OnGameStateChanged -= gameInfoDisplayer.Display;

        public void OnReadyPressed() => gameSession.SetLocalPlayerReady();

        public void OnLeavePressed() => lobbyManager.LeaveGame();
        
        public void SetActive(bool active) => content.SetActive(active);
    }
}