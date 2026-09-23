using System;
using System.Collections.Generic;
using Pokemon_Battle_Clone.Editor.Database;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool
{
    public class BrowserToolbar : VisualElement
    {
        private readonly PokemonConfigRepository _repository;
        private readonly PokemonApiLoader _apiLoader;
        
        private readonly Button _createButton;
        private readonly ToolbarSearchField _searchField;
        
        public event Action<PokemonConfig> OnPokemonCreated;
        public event Action<List<PokemonConfig>> OnSearchListChanged;
        
        public BrowserToolbar(PokemonConfigRepository repository, PokemonApiLoader apiLoader)
        {
            _repository = repository;
            _apiLoader = apiLoader;

            _createButton = new Button(OnCreateClicked);
            _createButton.iconImage = EditorGUIUtility.IconContent("Toolbar Plus").image as Texture2D;
            
            _searchField = new ToolbarSearchField();
            _searchField.AddToClassList("browser-search-field");
            _searchField.RegisterValueChangedCallback(OnSearchValueChanged);
            
            this.AddToClassList("browser-toolbar");
            
            this.Add(_createButton);
            this.Add(_searchField);
        }

        private void OnCreateClicked()
        {
            var popup = new CreatePokemonPopup(_repository, _apiLoader, _repository.GenerateValidId());
            popup.OnConfirm += CreateAsset;
            UnityEditor.PopupWindow.Show(_createButton.worldBound, popup);
        }

        private void CreateAsset(PokemonConfig pokemonConfig)
        {
            _repository.CreateAsset(pokemonConfig);
            OnPokemonCreated?.Invoke(pokemonConfig);
        }

        private void OnSearchValueChanged(ChangeEvent<string> evt)
        {
            var search = evt.newValue;
            var list = _repository.FindByName(search);
            OnSearchListChanged?.Invoke(list);
        }
    }
}