using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Pokemon_Battle_Clone.Editor.Database;
using Pokemon_Battle_Clone.Editor.Database.PokeApi;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool.CreatePopup
{
    public class CreateConfigPopup<T, Dto> : PopupWindowContent where T : ScriptableObject
    {
        private const string StyleSheetPath = "Assets/Pokemon Battle Clone/Editor/PokeTool/CreatePopup/CreatePopup.uss";
        
        public event Action<T> OnConfirm;

        private readonly ConfigRepository<T> _repository;
        private readonly IConfigApiLoader<T, Dto> _apiLoader;
        private readonly Func<int, string, T> _createManually;

        private readonly Label _label;
        private readonly Toggle _loadFromApiField;
        private readonly TextField _searchField;
        private readonly IntegerField _idField;
        private readonly TextField _nameField;
        private readonly Button _confirmButton;
        private readonly HelpBox _errorBox;
        private readonly HelpBox _loadingDataBox;

        public CreateConfigPopup(
            string title,
            ConfigRepository<T> repository,
            IConfigApiLoader<T, Dto> apiLoader,
            Func<int, string, T> createManually,
            int suggestedId
        )
        {
            _repository = repository;
            _apiLoader = apiLoader;
            _createManually = createManually;
            
            _label = new Label(title);
            _label.AddToClassList("header");
            
            _loadFromApiField = new Toggle("Load From PokeApi");
            _loadFromApiField.RegisterValueChangedCallback(evt => UpdateFieldsVisibility(evt.newValue));
            
            _searchField = new TextField("Search");
            _idField = new IntegerField("ID") { value = suggestedId };
            _nameField = new TextField("Name") { value = "unknown" };
            
            _confirmButton = new Button(OnConfirmClicked) { text = "Confirm"};
            _confirmButton.AddToClassList("confirm-button");
            
            _errorBox = new HelpBox("", HelpBoxMessageType.Warning);
            _errorBox.style.display = DisplayStyle.None;
            
            _loadingDataBox = new HelpBox("Loading data from PokeApi, please wait.", HelpBoxMessageType.Info);
            _loadingDataBox.style.display = DisplayStyle.None;
        }
        
        public override void OnOpen()
        {
            var root = editorWindow.rootVisualElement;
            AttachStyleSheet(root);
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
        }
        
        private async void OnConfirmClicked()
        {
            HideErrors();

            var errors = ValidateFields();
            if (errors.Count > 0) { ShowErrors(errors); return; }

            SetBusy(true);
            try
            {
                var config = _loadFromApiField.value ? await CreateFromApi() : CreateManually();
                if (config == null)
                    return;
                
                OnConfirm?.Invoke(config);
                CloseWindow();
            }
            catch (HttpRequestException e)
            {
                var message = e.Message.Contains("404")
                    ? $"No results were found for the search term \"{_searchField.value}\" in the PokeApi."
                    : "Could not connect to the PokeApi. Check your internet connection.";
                ShowErrors(new List<string> { message });
            }
            catch (Exception e)
            {
                ShowErrors(new List<string> { $"Unexpected error: {e.Message}" });
            }
            finally
            {
                SetBusy(false);
            }
        }
        
        private async Task<T> CreateFromApi()
        {
            var dto = await _apiLoader.Fetch(_searchField.value);

            var errors = ValidateAvailability(_apiLoader.GetId(dto), _apiLoader.GetName(dto));
            if (errors.Count > 0) { ShowErrors(errors); return null; }

            await _apiLoader.AfterFetch(dto);
            return _apiLoader.CreateConfig(dto);
        }
        
        private T CreateManually()
        {
            var id = _idField.value;
            var name = _nameField.value.Trim();

            var errors = ValidateAvailability(id, name);
            if (errors.Count > 0) { ShowErrors(errors); return null; }
            
            return _createManually(id, name);
        }
        
        private List<string> ValidateFields()
        {
            var errors = new List<string>();

            if (_loadFromApiField.value)
            {
                if (string.IsNullOrWhiteSpace(_searchField.value))
                    errors.Add("Search field cannot be empty.");
            }
            else
            {
                if (_idField.value < 0)
                    errors.Add("Pokemon ID cannot be less than 0.");
                if (string.IsNullOrWhiteSpace(_nameField.value))
                    errors.Add("Pokemon name cannot be empty.");
            }

            return errors;
        }
        
        private List<string> ValidateAvailability(int id, string name)
        {
            var errors = new List<string>();

            if (!_repository.IsValidId(id))
                errors.Add($"There's already a Pokemon with the ID {id} in the db.");
            if (!_repository.IsValidName(name))
                errors.Add($"There's already a Pokemon named {name} in the db.");
            
            return errors;
        }
        
        private void ShowErrors(List<string> errors)
        {
            _errorBox.text = string.Join("\n", errors);
            _errorBox.style.display = DisplayStyle.Flex;
        }
        
        private void HideErrors() => _errorBox.style.display = DisplayStyle.None;
        
        private void SetBusy(bool busy)
        {
            _loadFromApiField.SetEnabled(!busy);
            _searchField.SetEnabled(!busy);
            _idField.SetEnabled(!busy);
            _nameField.SetEnabled(!busy);
            _confirmButton.SetEnabled(!busy);

            var showLoading = busy && _loadFromApiField.value;
            _loadingDataBox.style.display = showLoading ? DisplayStyle.Flex : DisplayStyle.None;
        }
        
        private void UpdateFieldsVisibility(bool loadFromApi)
        {
            _idField.style.display = loadFromApi ? DisplayStyle.None : DisplayStyle.Flex;
            _nameField.style.display = loadFromApi ? DisplayStyle.None : DisplayStyle.Flex;
            _searchField.style.display = loadFromApi ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void CloseWindow()
        {
            if (editorWindow != null)
                editorWindow.Close();
        }

        private static void AttachStyleSheet(VisualElement root)
        {
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(StyleSheetPath);
            if (styleSheet == null)
            {
                Debug.LogWarning($"Could not find the PokeTool stylesheet at '{StyleSheetPath}'.");
                return;
            }
            
            root.styleSheets.Add(styleSheet);
        }
    }
}