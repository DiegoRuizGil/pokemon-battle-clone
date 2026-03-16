using Pokemon_Battle_Clone.Runtime.Stats.Domain;
using TMPro;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.TeamBuilder.TeamDisplayer
{
    public class PokemonCardStats : MonoBehaviour
    {
        [System.Serializable]
        class Stat
        {
            public string name;
            public Color color;
            public TextMeshProUGUI text;

            public void Display(int value)
            {
                var hexColor = ColorUtility.ToHtmlStringRGB(color);
                text.text = $"<color=#{hexColor}>{name}</color>: {value}";
            }
        }
        
        [SerializeField] private Stat atkStat;
        [SerializeField] private Stat defStat;
        [SerializeField] private Stat spAtkStat;
        [SerializeField] private Stat spDefStat;
        [SerializeField] private Stat speedStat;

        public void Display(StatSet stats)
        {
            atkStat.Display(stats.Attack);
            defStat.Display(stats.Defense);
            spAtkStat.Display(stats.SpcAttack);
            spDefStat.Display(stats.SpcDefense);
            speedStat.Display(stats.Speed);
        }
    }
}