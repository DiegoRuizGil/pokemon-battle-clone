using TMPro;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online.Lobby.UI
{
    public class LobbyPanel : MonoBehaviour
    {
        public LobbySession lobbySession;
        
        [Header("UI")]
        public TextMeshProUGUI sessionNameText;
        public TextMeshProUGUI player1StatusText;
        public TextMeshProUGUI player2StatusText;

        private void OnEnable()
        {
            lobbySession.OnGameStateChanged += HandleGameStateChanged;
        }

        private void OnDisable()
        {
            lobbySession.OnGameStateChanged -= HandleGameStateChanged;
        }

        private void HandleGameStateChanged(GameState state)
        {
            sessionNameText.text = $"Session: {state.SessionCode}";
            
            var p1Status = state.LocalPlayer.IsReady ? "Ready" : "Waiting";
            player1StatusText.text = $"Local Player: {p1Status}";
            
            var p2Status = state.RemotePlayer.IsReady ? "Ready" : "Waiting";
            player2StatusText.text = state.RemotePlayer.IsPresent ?
                $"Remote Player: {p2Status}" :
                "Waiting for rival...";
        }
    }
}