using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online.Lobby.UI
{
    public class LobbyPanel : MonoBehaviour
    {
        public GameSession gameSession;
        
        [Header("UI")]
        public SessionPanel sessionPanel;
        public JoinGamePanel joinGamePanel;

        private void Start() => ShowJoinGamePanel();

        private void OnEnable()
        {
            gameSession.OnJoinGame += ShowSessionPanel;
            gameSession.OnLeaveGame += ShowJoinGamePanel;
        }

        private void OnDisable()
        {
            gameSession.OnJoinGame -= ShowSessionPanel;
            gameSession.OnLeaveGame -= ShowJoinGamePanel;
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