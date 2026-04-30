using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online.Lobby.UI
{
    public class LobbyPanel : MonoBehaviour
    {
        public GameSession gameSession;
        
        [Header("UI")]
        public SessionPanel sessionPanel;
        public JoinGamePanel joinGamePanel;
        public GameObject connectingPanel;

        private void Start() => Refresh(gameSession.State);

        private void OnEnable() => gameSession.OnSessionStateChanged += Refresh;
        private void OnDisable() => gameSession.OnSessionStateChanged -= Refresh;

        private void Refresh(SessionState state)
        {
            sessionPanel.SetActive(state == SessionState.InSession);
            joinGamePanel.SetActive(state == SessionState.InLobby);
            connectingPanel.SetActive(state == SessionState.Connecting);
        }
    }
}