using Pokemon_Battle_Clone.Runtime.Stats.Domain;
using TMPro;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Stats.Infrastructure
{
    public class StatModifierTag : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        public void SetInfo(Stat stat, StatsModifier modifiers)
        {
            var multiplier = modifiers.GetMultiplier(stat);
            var stage = modifiers.GetStage(stat);
            
            text.text = $"{stat.ShortName()} x{multiplier:0.###}";
            gameObject.SetActive(stage != 0);
        }
    }
}