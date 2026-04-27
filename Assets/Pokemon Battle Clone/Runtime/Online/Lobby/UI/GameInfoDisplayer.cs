using TMPro;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online.Lobby.UI
{
    public class GameInfoDisplayer : MonoBehaviour
    {
        [Header("UI")]
        public TextMeshProUGUI sessionNameText;
        public TextMeshProUGUI p1StatusText;
        public TextMeshProUGUI p2StatusText;

        public void Display(GameState state)
        {
            sessionNameText.text = $"Session: {state.SessionCode}";
            
            var p1Status = state.LocalPlayer.IsReady ? "Ready" : "Waiting";
            p1StatusText.text = $"Local Player: {p1Status}";
            
            var p2Status = state.RemotePlayer.IsReady ? "Ready" : "Waiting";
            p2StatusText.text = state.RemotePlayer.IsPresent ?
                $"Remote Player: {p2Status}" :
                "Waiting for rival...";
        }
    }
}