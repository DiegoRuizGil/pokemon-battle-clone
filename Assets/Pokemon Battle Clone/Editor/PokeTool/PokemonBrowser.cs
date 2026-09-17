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
        private readonly PokemonConfigRepository _pokemonRepository;
        private readonly Button _createButton;
        private readonly PokemonListView _listView;
        
        public event Action<PokemonConfig> OnPokemonSelected;
        
        public PokemonBrowser() { }
        
        public PokemonBrowser(PokemonConfigRepository pokemonRepository)
        {
            _pokemonRepository = pokemonRepository;

            var toolbar = new VisualElement();
            toolbar.AddToClassList("browser-toolbar");
            toolbar.name = "Toolbar";

            _createButton = new Button(OnCreateClicked);
            _createButton.iconImage = EditorGUIUtility.IconContent("Toolbar Plus").image as Texture2D;
            var searchField = new ToolbarSearchField();
            searchField.AddToClassList("browser-search-field");
            searchField.RegisterValueChangedCallback(OnSearchValueChanged);

            _listView = new PokemonListView(_pokemonRepository.FindAll());
            _listView.selectionChanged += OnSelectionChanged;
            
            toolbar.Add(_createButton);
            toolbar.Add(searchField);
            this.Add(toolbar);
            this.Add(_listView);

            
            var adjustAssetsNameButton = new Button(AdjustAssetsName);
            adjustAssetsNameButton.text = "Adjust Assets Name";
            this.Add(adjustAssetsNameButton);
        }

        private void OnCreateClicked()
        {
            var popup = new CreatePokemonPopup(_pokemonRepository, _pokemonRepository.GenerateValidId(), CreatePokemonAsset);
            UnityEditor.PopupWindow.Show(_createButton.worldBound, popup);
        }

        private void CreatePokemonAsset(int pokemonId, string pokemonName)
        {
            var pokemonConfig = ScriptableObject.CreateInstance<PokemonConfig>();
            pokemonConfig.ID = pokemonId;
            pokemonConfig.pokemonName = pokemonName;
            
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

        private void AdjustAssetsName()
        {
            var pokemonList = _pokemonRepository.FindAll();
            foreach (var pokemonConfig in pokemonList)
            {
                var newName = pokemonConfig.ID.ToString();
                var newPath = AssetDatabase.GetAssetPath(pokemonConfig);
                AssetDatabase.RenameAsset(newPath, newName);
            }
        }
    }
}