using System;
using System.Collections.Generic;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool.Teams.Member
{
    public class MemberPokemonField : VisualElement
    {
        public event Action<PokemonConfig> OnPicked;

        private readonly TextField _nameField = new();
        private readonly Func<PokemonConfig, Sprite> _getIcon;
        private readonly SearchBinding<PokemonConfig> _search;
        
        public MemberPokemonField(
            PanelHost host,
            Func<List<PokemonConfig>> getItems,
            Func<PokemonConfig, Sprite> getIcon)
        {
            _getIcon = getIcon;

            var picker = new ConfigPicker<PokemonConfig>(
                getName: p => p.pokemonName,
                makeRow: MakePickerRow,
                bindRow: BindPickerRow,
                rowHeight: 40);
            
            _search = new SearchBinding<PokemonConfig>(
                _nameField, picker, host, getItems, p => OnPicked?.Invoke(p));
            
            this.Add(_nameField);
        }

        public void SetName(string pokemonName) => _nameField.SetValueWithoutNotify(pokemonName);

        public void FocusField() => _nameField.schedule.Execute(() => _nameField.Focus());
        
        private VisualElement MakePickerRow()
        {
            var row = new VisualElement();
            row.AddToClassList("picker-row");

            var icon = new Image();
            icon.AddToClassList("picker-icon");
            row.Add(icon);
            row.Add(new Label());
            return row;
        }

        private void BindPickerRow(VisualElement row, PokemonConfig config)
        {
            row.Q<Image>().sprite = _getIcon(config);
            row.Q<Label>().text = config.pokemonName;
        }
    }
}