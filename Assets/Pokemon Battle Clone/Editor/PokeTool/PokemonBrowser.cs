using System;
using System.Collections.Generic;
using System.Linq;
using Pokemon_Battle_Clone.Editor.Database;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool
{
    [UxmlElement]
    public partial class PokemonBrowser : VisualElement
    {
        private const string DatabasePath = "Assets/Pokemon Battle Clone/Database";

        private readonly PokemonConfigRepository _pokemonRepository;
        private readonly PokemonListView _listView;
        
        public event Action<PokemonConfig> OnPokemonSelected;
        
        public PokemonBrowser()
        {
            _pokemonRepository = new PokemonConfigRepository(DatabasePath);
            
            var searchField = new ToolbarSearchField();
            searchField.RegisterValueChangedCallback(OnSearchValueChanged);

            _listView = new PokemonListView(_pokemonRepository.FindAll());
            _listView.selectionChanged += OnSelectionChanged;
            
            this.Add(searchField);
            this.Add(_listView);
        }

        private void OnSearchValueChanged(ChangeEvent<string> evt)
        {
            var searchString = evt.newValue;
            var pokemonList = _pokemonRepository.FindByName(searchString);
            _listView.SetEntries(pokemonList);
        }

        private void OnSelectionChanged(IEnumerable<object> obj)
        {
            OnPokemonSelected?.Invoke(obj.FirstOrDefault() as PokemonConfig);
        }
    }
}