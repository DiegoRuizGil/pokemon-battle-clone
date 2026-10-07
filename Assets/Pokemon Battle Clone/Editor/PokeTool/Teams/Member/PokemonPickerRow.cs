using System.Collections.Generic;
using Pokemon_Battle_Clone.Editor.PokeTool.Icons;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Database;
using Pokemon_Battle_Clone.Runtime.Stats.Domain;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool.Teams.Member
{
    public class PokemonPickerRow : VisualElement
    {
        private const int TypeIconSize = 32;

        private readonly Image _icon = new();
        private readonly Label _name = new();
        private readonly VisualElement _types = new();
        private readonly List<(Stat stat, LabeledValue cell)> _stats = new();

        public PokemonPickerRow()
        {
            this.AddToClassList("picker-row");
            
            _icon.AddToClassList("picker-icon");
            var nameSection = new VisualElement();
            nameSection.AddToClassList("picker-section-name");
            nameSection.Add(_icon);
            nameSection.Add(_name);
            
            _types.AddToClassList("picker-types");

            var statsSection = new VisualElement();
            statsSection.AddToClassList("picker-stats");
            foreach (var stat in StatInfo.All)
            {
                var cell = new LabeledValue(stat.ShortName());
                _stats.Add((stat, cell));
                statsSection.Add(cell);
            }
            
            this.Add(nameSection);
            this.Add(_types);
            this.Add(statsSection);
        }

        public void Bind(PokemonConfig config, Sprite icon)
        {
            _icon.sprite = icon;
            _name.text = config.pokemonName;
            
            _types.Clear();
            AddType(config.type1);
            if (config.type2 != ElementalType.None) AddType(config.type2);
            
            foreach (var (stat, cell) in _stats)
                cell.SetValue(config.baseStats[stat].ToString());
        }

        private void AddType(ElementalType type)
        {
            var image = PokeToolIcons.GetImage(PokeToolIcons.TypeToIconId(type), TypeIconSize);
            image.AddToClassList("type-icon");
            _types.Add(image);
        }
    }
}