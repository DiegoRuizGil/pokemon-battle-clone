using Pokemon_Battle_Clone.Runtime.Core.Infrastructure.Types;
using Pokemon_Battle_Clone.Runtime.Moves.Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon_Battle_Clone.Runtime.TeamBuilder.TeamDisplayer
{
    public class MoveCard : MonoBehaviour
    {
        [SerializeField] private ElementalTypesConfig elementalTypesConfig;
        [Space(10)]
        [SerializeField] private Image icon;
        [SerializeField] private Image background;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI ppText;
        
        public void Display(MoveDTO move)
        {
            icon.sprite = elementalTypesConfig.GetIcon(move.Type);
            background.color = elementalTypesConfig.GetColor(move.Type);
            nameText.text = move.Name;
            ppText.text = $"PP {move.CurrentPP}/{move.MaxPP}";
        }
    }
}