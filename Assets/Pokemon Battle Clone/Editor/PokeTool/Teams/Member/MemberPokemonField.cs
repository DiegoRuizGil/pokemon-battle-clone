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
        private readonly SearchBinding<PokemonConfig> _search;

        private PokemonConfig _current;
        
        public MemberPokemonField(
            PanelHost host,
            Func<List<PokemonConfig>> getItems,
            Func<PokemonConfig, Sprite> getIcon)
        {
            var picker = new ConfigPicker<PokemonConfig>(
                getName: p => p.pokemonName,
                makeRow: () => new PokemonPickerRow(),
                bindRow: (row, config) => ((PokemonPickerRow)row).Bind(config, getIcon(config)),
                rowHeight: 40);
            
            _search = new SearchBinding<PokemonConfig>(
                _nameField, picker, host, getItems,
                getCurrent: () => _current, 
                onPicked: p => OnPicked?.Invoke(p));
            
            this.Add(_nameField);
        }

        public void SetPokemon(PokemonConfig config)
        {
            _current = config;
            _nameField.SetValueWithoutNotify(config != null ? config.pokemonName : "");
        }
        
        public void FocusField() => _nameField.schedule.Execute(() => _nameField.Focus());
    }
}