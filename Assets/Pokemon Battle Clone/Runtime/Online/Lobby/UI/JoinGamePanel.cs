using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon_Battle_Clone.Runtime.Online.Lobby.UI
{
    public class JoinGamePanel : MonoBehaviour
    {
        [SerializeField] private LobbyManager lobbyManager;
        
        [Header("UI")]
        [SerializeField] private TMP_InputField sessionNameInput;
        [SerializeField] private Button createButton;
        [SerializeField] private Button joinButton;

        private void Start() => UpdateJoinButton();

        private void OnEnable() => sessionNameInput.onValueChanged.AddListener(OnInputChanged);
        private void OnDisable() => sessionNameInput.onValueChanged.RemoveListener(OnInputChanged);

        private void OnInputChanged(string value) => UpdateJoinButton();
        private void UpdateJoinButton() => joinButton.interactable = !string.IsNullOrEmpty(sessionNameInput.text);
        
        public void OnCreatePressed() => lobbyManager.CreateGame();
        public void OnJoinPressed() => lobbyManager.JoinGame(sessionNameInput.text);
    }
}