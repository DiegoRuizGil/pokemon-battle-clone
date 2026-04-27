using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online.Lobby.UI
{
    public class LobbyPanel : MonoBehaviour
    {
        public LobbySession lobbySession;
        
        [Header("UI")]
        public SessionPanel sessionPanel;
        public JoinGamePanel joinGamePanel;

        private void Start() => ShowJoinGamePanel();

        private void OnEnable()
        {
            lobbySession.OnJoinGame += ShowSessionPanel;
            lobbySession.OnLeaveGame += ShowJoinGamePanel;
        }

        private void OnDisable()
        {
            lobbySession.OnJoinGame -= ShowSessionPanel;
            lobbySession.OnLeaveGame -= ShowJoinGamePanel;
        }

        private void ShowJoinGamePanel()
        {
            joinGamePanel.gameObject.SetActive(true);
            sessionPanel.gameObject.SetActive(false);
        }
        
        private void ShowSessionPanel()
        {
            joinGamePanel.gameObject.SetActive(false);
            sessionPanel.gameObject.SetActive(true);
        }
    }
}