using System.Text;
using Pokemon_Battle_Clone.Runtime.Battles.Infrastructure;
using TMPro;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online
{
    public class BattleDataPanel : MonoBehaviour
    {
        public BattleSettings battleSettings;
        public TextMeshProUGUI textData;

        private void Start()
        {
            var text = new StringBuilder();
            text.AppendLine($"Seed: {battleSettings.battleSeed}");
            
            textData.text = text.ToString();
        }
    }
}