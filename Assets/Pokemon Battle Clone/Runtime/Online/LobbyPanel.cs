using TMPro;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online
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
            lobbySession.OnLobbyStateChanged += HandleLobbyStateChanged;
        }

        private void OnDisable()
        {
            lobbySession.OnLobbyStateChanged -= HandleLobbyStateChanged;
        }

        private void HandleLobbyStateChanged(LobbyState state)
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