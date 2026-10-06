using Pokemon_Battle_Clone.Runtime.Stats.Domain;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool.Teams.Member
{
    public static class StatBarScale
    {
        private const int MaxStat = 500;
        private const int MaxHP = 650;
        
        public static int MaxFor(Stat stat) => stat == Stat.HP ? MaxHP : MaxStat;
    }
    
    public class StatBar : VisualElement
    {
        private readonly VisualElement _fill = new();

        public StatBar()
        {
            AddToClassList("stat-bar");
            _fill.AddToClassList("stat-bar-fill");
            this.Add(_fill);
        }

        public void SetValue(int value, int max)
        {
            var fraction = max > 0 ? Mathf.Clamp01((float)value / max) : 0f;
            var hue = fraction * 0.6f;

            _fill.style.width = Length.Percent(fraction * 100f);
            _fill.style.backgroundColor = Color.HSVToRGB(hue, 0.75f, 0.85f);
            this.tooltip = value.ToString();
        }
    }
}