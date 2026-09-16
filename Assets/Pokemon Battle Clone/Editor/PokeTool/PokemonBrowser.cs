using System;
using System.Collections.Generic;
using System.Linq;
using Pokemon_Battle_Clone.Editor.Database;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool
{
    [UxmlElement]
    public partial class PokemonBrowser : VisualElement
    {
        private const string DatabasePath = "Assets/Pokemon Battle Clone/Database/Pokemon";

        private readonly PokemonConfigRepository _pokemonRepository;
        private readonly PokemonListView _listView;
        
        public event Action<PokemonConfig> OnPokemonSelected;
        
        public PokemonBrowser()
        {
            _pokemonRepository = new PokemonConfigRepository(DatabasePath);

            var toolbar = new VisualElement();
            toolbar.AddToClassList("browser-toolbar");

            var createButton = new Button(OnCreateClicked);
            createButton.iconImage = EditorGUIUtility.IconContent("Toolbar Plus").image as Texture2D;
            var searchField = new ToolbarSearchField();
            searchField.AddToClassList("browser-search-field");
            searchField.RegisterValueChangedCallback(OnSearchValueChanged);

            _listView = new PokemonListView(_pokemonRepository.FindAll());
            _listView.selectionChanged += OnSelectionChanged;
            
            toolbar.Add(createButton);
            toolbar.Add(searchField);
            this.Add(toolbar);
            this.Add(_listView);

            
            var adjustAssetsNameButton = new Button(() =>
            {
                var pokemonList = _pokemonRepository.FindAll();
                foreach (var pokemonConfig in pokemonList)
                {
                    var newName = pokemonConfig.ID.ToString();
                    var newPath = AssetDatabase.GetAssetPath(pokemonConfig);
                    AssetDatabase.RenameAsset(newPath, newName);
                }
            });
            adjustAssetsNameButton.text = "Adjust Assets Name";
            this.Add(adjustAssetsNameButton);
        }

        private void OnCreateClicked()
        {
            var newId = _pokemonRepository.GenerateValidId();
            var pokemonConfig = ScriptableObject.CreateInstance<PokemonConfig>();
            pokemonConfig.ID = newId;
            
            _pokemonRepository.CreateAsset(pokemonConfig);
            _listView.AddEntry(pokemonConfig);
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