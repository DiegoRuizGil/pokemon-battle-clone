using System;
using System.IO;
using Pokemon_Battle_Clone.Editor.Database;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool.CreatePopup
{
    public class CreateTeamPopup : PopupWindowContent, IConfigCreatePopup<TeamConfig>
    {
        private const string StyleSheetPath = "Assets/Pokemon Battle Clone/Editor/PokeTool/CreatePopup/CreatePopup.uss";

        public event Action<TeamConfig> OnConfirm;

        private readonly ConfigRepository<TeamConfig> _repository;

        private readonly Label _label;
        private readonly TextField _nameField;
        private readonly Button _confirmButton;
        private readonly HelpBox _errorBox;

        public CreateTeamPopup(ConfigRepository<TeamConfig> repository)
        {
            _repository = repository;

            _label = new Label("Create Team");
            _label.AddToClassList("header");

            _nameField = new TextField("Name");

            _confirmButton = new Button(OnConfirmClicked) { text = "Confirm" };
            _confirmButton.AddToClassList("confirm-button");

            _errorBox = new HelpBox("", HelpBoxMessageType.Warning);
            _errorBox.style.display = DisplayStyle.None;
        }

        public override Vector2 GetWindowSize() => new(260, 150);

        public override void OnOpen()
        {
            var root = editorWindow.rootVisualElement;
            AttachStyleSheet(root);
            root.AddToClassList("create-popup");

            root.Add(_label);
            root.Add(_nameField);
            root.Add(_confirmButton);
            root.Add(_errorBox);

            _nameField.Focus();
        }

        private void OnConfirmClicked()
        {
            _errorBox.style.display = DisplayStyle.None;

            var name = _nameField.value.Trim();
            var error = GetError(name);
            if (error != null)
            {
                _errorBox.text = error;
                _errorBox.style.display = DisplayStyle.Flex;
                return;
            }

            var team = ScriptableObject.CreateInstance<TeamConfig>();
            team.name = name;

            OnConfirm?.Invoke(team);
            if (editorWindow != null) editorWindow.Close();
        }

        private string GetError(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "The team name cannot be empty.";
            if (name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0) return "The name contains invalid characters.";
            if (!_repository.IsValidName(name)) return $"There's already a team named {name} in the db.";
            return null;
        }
        
        private static void AttachStyleSheet(VisualElement root)
        {
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(StyleSheetPath);
            if (styleSheet == null)
            {
                Debug.LogWarning($"Could not find the popup stylesheet at '{StyleSheetPath}'.");
                return;
            }
            
            root.styleSheets.Add(styleSheet);
        }
    }
}