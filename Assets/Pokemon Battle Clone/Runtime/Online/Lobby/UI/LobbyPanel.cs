using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online.Lobby.UI
{
    public class LobbyPanel : MonoBehaviour
    {
        public LobbySession lobbySession;
        
        [Header("UI")]
        public GameInfoDisplayer gameInfoDisplayer;

        private void OnEnable()
        {
            lobbySession.OnGameStateChanged += HandleGameStateChanged;
        }

        private void OnDisable()
        {
            lobbySession.OnGameStateChanged -= HandleGameStateChanged;
        }

        private void HandleGameStateChanged(GameState state) => gameInfoDisplayer.Display(state);
    }
}