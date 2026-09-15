using System.Collections.Generic;
using System.Linq;
using PokeApiNet;
using Pokemon_Battle_Clone.Editor.Database;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool
{
    [UxmlElement]
    public partial class PokeTool : VisualElement
    {
        public PokeTool()
        {
            this.Add(new PokemonBrowser());
        }
    }

    [UxmlElement]
    public partial class PokemonBrowser : VisualElement
    {
        private const string DatabasePath = "Assets/Pokemon Battle Clone/Database";

        private readonly PokemonConfigRepository _pokemonRepository;
        private readonly PokemonListView _listView;
        
        public PokemonBrowser()
        {
            _pokemonRepository = new PokemonConfigRepository(DatabasePath);
            
            var searchField = new ToolbarSearchField();
            searchField.RegisterValueChangedCallback(OnSearchValueChanged);

            _listView = new PokemonListView(_pokemonRepository.FindAll());
            
            this.Add(searchField);
            this.Add(_listView);
        }
        
        private void OnSearchValueChanged(ChangeEvent<string> evt)
        {
            var searchString = evt.newValue;
            var pokemonList = _pokemonRepository.FindByName(searchString);
            _listView.SetEntries(pokemonList);
        }
    }
    
    [UxmlElement]
    public partial class PokemonListView : ListView
    {
        public PokemonListView() { }

        public PokemonListView(List<PokemonConfig> pokemonEntries)
        {
            this.itemsSource = pokemonEntries;
            this.makeItem = () => new PokemonListEntry();
            this.bindItem = (element, i) => (element as PokemonListEntry).Bind(itemsSource[i] as PokemonConfig);
            
            //styling
            this.showAlternatingRowBackgrounds = AlternatingRowBackground.ContentOnly;
        }

        public void SetEntries(List<PokemonConfig> entries)
        {
            this.itemsSource = entries;
        }
    }

    [UxmlElement]
    public partial class PokemonListEntry : VisualElement
    {
        private readonly Label _label;
        
        public PokemonListEntry()
        {
            _label = new Label();
            this.Add(_label);
        }

        public void Bind(PokemonConfig pokemonConfig)
        {
            _label.text = $"{pokemonConfig.ID} - {pokemonConfig.pokemonName}";
        }
    }
}
