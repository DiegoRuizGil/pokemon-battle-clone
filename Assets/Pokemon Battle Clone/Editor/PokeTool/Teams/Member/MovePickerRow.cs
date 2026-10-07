using Pokemon_Battle_Clone.Editor.PokeTool.Icons;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool.Teams.Member
{
    public class MovePickerRow : VisualElement
    {
        private const int IconSize = 32;
        private const string NoValue = "—";
        
        private readonly Label _name = new();
        private readonly VisualElement _icons = new();
        private readonly LabeledValue _power = new("Power");
        private readonly LabeledValue _accuracy = new("Accuracy");
        private readonly LabeledValue _pp = new("PP");

        public MovePickerRow()
        {
            this.AddToClassList("picker-row");
            
            _name.AddToClassList("picker-move-name");
            _icons.AddToClassList("picker-move-icons");
            
            var numbers = new VisualElement();
            numbers.AddToClassList("picker-numbers");
            numbers.Add(_power);
            numbers.Add(_accuracy);
            numbers.Add(_pp);
            
            this.Add(_name);
            this.Add(_icons);
            this.Add(numbers);
        }

        public void Bind(MoveConfig move)
        {
            _name.text = move.moveName;
            
            _icons.Clear();
            if (move.type != ElementalType.None)
                _icons.Add(PokeToolIcons.GetImage(PokeToolIcons.TypeToIconId(move.type), IconSize));
            _icons.Add(PokeToolIcons.GetImage(PokeToolIcons.MoveCategoryToIconId(move.category), IconSize));
            
            _power.SetValue(move.power > 0 ? move.power.ToString() : NoValue);
            _accuracy.SetValue(move.accuracy.ToString());
            _pp.SetValue(move.pp.ToString());
        }
    }
}