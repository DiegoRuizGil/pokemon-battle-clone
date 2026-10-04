using UnityEngine;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool.Teams.Member
{
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

            _fill.style.width = Length.Percent(fraction * 100f);
            _fill.style.backgroundColor = Color.HSVToRGB(fraction * 0.33f, 0.75f, 0.85f);
            this.tooltip = value.ToString();
        }
    }
}