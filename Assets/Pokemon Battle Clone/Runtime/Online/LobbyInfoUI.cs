using TMPro;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online
{
    public class LobbyInfoUI : MonoBehaviour
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
            player1StatusText.text = $"Local Player: {state.LocalPlayer.IsReady}";
            player2StatusText.text = state.RemotePlayer.IsPresent ?
                $"Remote Player: {state.RemotePlayer.IsReady}" :
                $"Waiting for rival...";
        }
    }
}