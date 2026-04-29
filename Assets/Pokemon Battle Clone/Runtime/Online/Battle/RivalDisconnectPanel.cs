using Fusion;
using Pokemon_Battle_Clone.Runtime.Online.Lobby.Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Pokemon_Battle_Clone.Runtime.Online.Battle
{
    public class RivalDisconnectPanel : MonoBehaviour
    {
        public NetworkEventsChannel networkEventsChannel;

        public string lobbySceneName;

        [Header("UI")]
        public GameObject panel;
        public Button backButton;
        
        private void OnEnable()
        {
            networkEventsChannel.OnPlayerLeft += OnPlayerLeft;
            backButton.onClick.AddListener(LoadLobbyScene);
        }

        private void OnDisable()
        {
            networkEventsChannel.OnPlayerLeft -= OnPlayerLeft;
            backButton.onClick.RemoveListener(LoadLobbyScene);
        }

        private void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            Time.timeScale = 0;
            panel.gameObject.SetActive(true);
        }

        private void LoadLobbyScene()
        {
            Time.timeScale = 1;
            networkEventsChannel.Runner.LoadScene(lobbySceneName);
        }
    }
}