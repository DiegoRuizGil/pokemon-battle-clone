using System;
using System.Collections.Generic;
using Pokemon_Battle_Clone.Editor.Database;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool
{
    public class CreatePokemonPopup : PopupWindowContent
    {
        private readonly PokemonConfigRepository _repository;
        private readonly Action<int, string> _onConfirm;
        
        private readonly Label _label;
        private readonly IntegerField _idField;
        private readonly TextField _nameField;
        private readonly Button _confirmButton;
        private readonly HelpBox _errorBox;
        
        public CreatePokemonPopup(PokemonConfigRepository repository, int suggestedId, Action<int, string> onConfirm)
        {
            
            _repository = repository;
            _onConfirm = onConfirm;
            
            _label = new Label("Create Pokemon");
            _label.AddToClassList("header");
            _idField = new IntegerField("ID") { value = suggestedId };
            _nameField = new TextField("Name") { value = "unknown" };
            _confirmButton = new Button(OnConfirmClicked) { text = "Confirm"};

            _errorBox = new HelpBox("", HelpBoxMessageType.Warning);
            _errorBox.style.display = DisplayStyle.None;
        }

        public override void OnOpen()
        {
            var root = editorWindow.rootVisualElement;
            root.AddToClassList("create-popup");
            
            root.Add(_label);
            root.Add(_idField);
            root.Add(_nameField);
            root.Add(_confirmButton);
            root.Add(_errorBox);
            
            StyleElements();
        }

        private void OnConfirmClicked()
        {
            var errors = Validate(_idField.value, _nameField.value);
            if (errors.Count > 0)
            {
                ShowErrors(errors);
                return;
            }
            
            _onConfirm(_idField.value, _nameField.value);
            editorWindow.Close();
        }

        private List<string> Validate(int id, string name)
        {
            var errors = new List<string>();

            if (id < 0)
                errors.Add("ID must be greater than or equal to 0.");
            else if (!_repository.IsValidId(id))
                errors.Add("This ID already exists.");
            
            if (string.IsNullOrEmpty(name))
                errors.Add("Name cannot be empty.");
            else if (_repository.FindByName(name).Count != 0)
                errors.Add("This name already exists.");
            
            return errors;
        }

        private void ShowErrors(List<string> errors)
        {
            _errorBox.text = string.Join("\n", errors);
            _errorBox.style.display = DisplayStyle.Flex;
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