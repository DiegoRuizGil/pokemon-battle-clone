using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon_Battle_Clone.Runtime.Online.Lobby.UI
{
    public class JoinGamePanel : MonoBehaviour
    {
        [SerializeField] private LobbyManager lobbyManager;
        
        [Header("UI")]
        [SerializeField] private GameObject content;
        [SerializeField] private TMP_InputField sessionNameInput;
        [SerializeField] private Button createButton;
        [SerializeField] private Button joinButton;
        [SerializeField] private TextMeshProUGUI errorText;

        private void Start()
        {
            UpdateJoinButton();
            ShowError(string.Empty);
        }

        private void OnEnable() => sessionNameInput.onValueChanged.AddListener(OnInputChanged);
        private void OnDisable() => sessionNameInput.onValueChanged.RemoveListener(OnInputChanged);

        public void SetActive(bool active) => content.SetActive(active);
        
        public async void OnCreatePressed()
        {
            SetInteractable(false);
            var result = await lobbyManager.CreateGame();
            ShowError(result.ErrorMessage);
            SetInteractable(true);
        }

        public async void OnJoinPressed()
        {
            SetInteractable(false);
            var result = await lobbyManager.JoinGame(sessionNameInput.text);
            ShowError(result.ErrorMessage);
            SetInteractable(true);
        }
        
        private void OnInputChanged(string value) => UpdateJoinButton();
        private void UpdateJoinButton() => joinButton.interactable = !string.IsNullOrEmpty(sessionNameInput.text);
        
        private void ShowError(string message) => errorText.text = message;
        private void SetInteractable(bool value)
        {
            createButton.interactable = value;
            joinButton.interactable = value;
        }
    }
}