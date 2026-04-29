using Pokemon_Battle_Clone.Runtime.Battles.Control;
using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Online.Lobby.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon_Battle_Clone.Runtime.Online.Battle.UI
{
    public class BattleOnlineEndPanel : MonoBehaviour
    {
        [SerializeField] private BattleController battleController;
        [SerializeField] private NetworkEventsChannel networkEventsChannel;

        [Header("UI")]
        [SerializeField] private GameObject content;
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Button backButton;
        [SerializeField] private string lobbySceneName;

        private void OnEnable()
        {
            backButton.onClick.AddListener(LoadTeamBuilderScene);
            battleController.OnBattleFinished += Show;
        }

        private void OnDisable()
        {
            backButton.onClick.RemoveListener(LoadTeamBuilderScene);
            battleController.OnBattleFinished -= Show;
        }

        public void Show(Side winner)
        {
            messageText.text = winner == Side.Player ? "YOU WON!!" : "YOU LOST...";
            content.SetActive(true);
        }

        private void LoadTeamBuilderScene()
        {
            networkEventsChannel.Runner.LoadScene(lobbySceneName);
        }
    }
}