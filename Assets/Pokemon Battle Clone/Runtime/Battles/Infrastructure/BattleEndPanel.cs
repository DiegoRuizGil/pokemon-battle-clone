using System;
using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Pokemon_Battle_Clone.Runtime.Battles.Infrastructure
{
    public class BattleEndPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Button backButton;
        [SerializeField] private string teamBuilderScene = "TeamBuilderScene";

        private void OnEnable()
        {
            backButton.onClick.AddListener(LoadTeamBuilderScene);
        }

        private void OnDisable()
        {
            backButton.onClick.RemoveListener(LoadTeamBuilderScene);
        }

        public void Show(Side winner)
        {
            messageText.text = winner == Side.Player ? "YOU WON!!" : "YOU LOST...";
            gameObject.SetActive(true);
        }

        private void LoadTeamBuilderScene()
        {
            SceneManager.LoadScene(teamBuilderScene);
        }
    }
}