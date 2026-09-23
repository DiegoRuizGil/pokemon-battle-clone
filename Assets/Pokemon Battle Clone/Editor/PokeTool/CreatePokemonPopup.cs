using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Pokemon_Battle_Clone.Editor.Database;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Pokemon_Battle_Clone.Editor.PokeTool
{
    public class CreatePokemonPopup : PopupWindowContent
    {
        public event Action<PokemonConfig> OnConfirm;
        
        private readonly PokemonConfigRepository _repository;
        private readonly PokemonApiLoader _apiLoader;
        
        private readonly Label _label;
        private readonly Toggle _loadFromApiField;
        private readonly TextField _searchField;
        private readonly IntegerField _idField;
        private readonly TextField _nameField;
        private readonly Button _confirmButton;
        private readonly HelpBox _errorBox;
        private readonly HelpBox _loadingDataBox;
        
        public CreatePokemonPopup(PokemonConfigRepository repository, PokemonApiLoader apiLoader, int suggestedId)
        {
            _repository = repository;
            _apiLoader = apiLoader;
            
            _label = new Label("Create Pokemon");
            _label.AddToClassList("header");
            
            _loadFromApiField = new Toggle("Load From PokeApi");
            _loadFromApiField.RegisterValueChangedCallback(evt => UpdateFieldsVisibility(evt.newValue));
            
            _searchField = new TextField("Search");
            _idField = new IntegerField("ID") { value = suggestedId };
            _nameField = new TextField("Name") { value = "unknown" };
            _confirmButton = new Button(OnConfirmClicked) { text = "Confirm"};
            
            _errorBox = new HelpBox("", HelpBoxMessageType.Warning);
            _errorBox.style.display = DisplayStyle.None;
            
            _loadingDataBox = new HelpBox("Loading data from PokeApi, please wait.", HelpBoxMessageType.Info);
            _loadingDataBox.style.display = DisplayStyle.None;
        }

        public override void OnOpen()
        {
            var root = editorWindow.rootVisualElement;
            root.AddToClassList("create-popup");
            
            root.Add(_label);
            root.Add(_loadFromApiField);
            root.Add(_searchField);
            root.Add(_idField);
            root.Add(_nameField);
            root.Add(_confirmButton);
            root.Add(_errorBox);
            root.Add(_loadingDataBox);
            
            UpdateFieldsVisibility(_loadFromApiField.value);
            
            StyleElements();
        }

        private async void OnConfirmClicked()
        {
            HideBoxes();

            var errors = ValidateFields();
            if (errors.Count > 0)
            {
                ShowErrors(errors);
                return;
            }

            try
            {
                _loadingDataBox.style.display = DisplayStyle.Flex;
                var pokemonConfig = await CreatePokemonConfig();

                errors = ValidatePokemonData(pokemonConfig.ID, pokemonConfig.pokemonName, _loadFromApiField.value);
                if (errors.Count > 0)
                {
                    ShowErrors(errors);
                    Object.DestroyImmediate(pokemonConfig);
                    return;
                }
                
                OnConfirm?.Invoke(pokemonConfig);
                editorWindow.Close();
            }
            catch (HttpRequestException e)
            {
                var message = e.Message.Contains("404")
                    ? $"No Pokemon were found for the search term \"{_searchField.value}\" in the PokeApi."
                    : "Could not connect to the PokeApi. Check your internet connection.";
                ShowErrors(new() { message });
            }
            catch (Exception e)
            {
                ShowErrors(new() { $"Unexpected error: {e.Message}" });
            }
            finally
            {
                SetEnable(true);
                _loadingDataBox.style.display = DisplayStyle.None;
            }
            
        }

        private List<string> ValidateFields()
        {
            var errors = new List<string>();

            if (_loadFromApiField.value)
            {
                if (string.IsNullOrEmpty(_searchField.value))
                    errors.Add("Search field cannot be empty.");
            }
            else
            {
                if (_idField.value < 0)
                    errors.Add("Pokemon ID cannot be less than 0.");
                if (string.IsNullOrEmpty(_nameField.value))
                    errors.Add("Pokemon name cannot be empty.");
            }
            
            return errors;
        }

        private List<string> ValidatePokemonData(int id, string name, bool loadedFromApi)
        {
            var errors = new List<string>();

            var isIdValid = _repository.IsValidId(id);
            var isNameValid = _repository.FindByName(name).Count == 0;
            
            if (loadedFromApi)
            {
                if (!isIdValid || !isNameValid)
                    errors.Add($"The Pokemon {name} already exists in the db.");
            }
            else
            {
                if (!isIdValid)
                    errors.Add("There's already a Pokemon with that id in the db.");
                if (!isNameValid)
                    errors.Add("There's already a Pokemon with that name in the db.");
            }
            
            return errors;
        }

        private void ShowErrors(List<string> errors)
        {
            _errorBox.text = string.Join("\n", errors);
            _errorBox.style.display = DisplayStyle.Flex;
        }

        private void HideBoxes()
        {
            _errorBox.style.display = DisplayStyle.None;
            _loadingDataBox.style.display = DisplayStyle.None;
        }

        private void SetEnable(bool value)
        {
            _idField.SetEnabled(value);
            _nameField.SetEnabled(value);
            _loadFromApiField.SetEnabled(value);
            _searchField.SetEnabled(value);
            _confirmButton.SetEnabled(value);
        }

        private void UpdateFieldsVisibility(bool loadFromApi)
        {
            _idField.style.display = loadFromApi ? DisplayStyle.None : DisplayStyle.Flex;
            _nameField.style.display = loadFromApi ? DisplayStyle.None : DisplayStyle.Flex;
            _searchField.style.display = loadFromApi ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private async Task<PokemonConfig> CreatePokemonConfig()
        {
            var pokemonConfig = ScriptableObject.CreateInstance<PokemonConfig>();
            if (_loadFromApiField.value)
            {
                SetEnable(false);
                _loadingDataBox.style.display = DisplayStyle.Flex;
                await _apiLoader.LoadFromPokeApi(pokemonConfig, _searchField.value);
            }
            else
            {
                pokemonConfig.ID = _idField.value;
                pokemonConfig.pokemonName = _nameField.value;
            }
            
            return pokemonConfig;
        }

        private void StyleElements()
        {
            var root = editorWindow.rootVisualElement;
            root.style.paddingBottom = 2;
            root.style.paddingTop = 2;
            root.style.paddingRight = 2;
            root.style.paddingLeft = 2;
            
            _label.style.unityFontStyleAndWeight = FontStyle.Bold;
            _label.style.fontSize = 16;
            _label.style.marginBottom = 10;
            
            _confirmButton.style.marginTop = 10;
            _confirmButton.style.marginBottom = 10;
        }
    }
}