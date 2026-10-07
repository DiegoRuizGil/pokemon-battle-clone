using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool.Teams.Member
{
    public class LabeledValue : VisualElement
    {
        private readonly Label _value = new();

        public LabeledValue(string label)
        {
            this.AddToClassList("labeled-value");

            var labelElement = new Label(label);
            labelElement.AddToClassList("labeled-value-label");
            _value.AddToClassList("labeled-value-value");
            
            this.Add(labelElement);
            this.Add(_value);
        }
        
        public void SetValue(string value) => _value.text = value;
    }
}