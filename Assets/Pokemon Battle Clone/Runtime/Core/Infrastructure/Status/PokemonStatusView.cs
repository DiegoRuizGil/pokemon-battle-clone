using Pokemon_Battle_Clone.Runtime.Stats.Domain;
using Pokemon_Battle_Clone.Runtime.Stats.Infrastructure;
using TMPro;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Core.Infrastructure.Status
{
    public class PokemonStatusView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private HealthView healthView;
        [SerializeField] private StatsModifiersView statsModifiersView;
        
        public void UpdateHealth(int max, int current, bool animated) => healthView.SetHealth(max, current, animated);
        public void SetStatsModifier(StatsModifier modifier) => statsModifiersView.Set(modifier);

        public void SetInfo(string name, int level)
        {
            nameText.text = name;
            levelText.text = $"Lv. {level}";
        }
    }
}