using System;
using System.Collections.Generic;
using Pokemon_Battle_Clone.Editor.Database;
using Pokemon_Battle_Clone.Editor.PokeTool.CreatePopup;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool
{
    public class BrowserToolbar<T> : VisualElement where T : ScriptableObject
    {
        private readonly ConfigRepository<T> _repository;
        private readonly Func<PopupWindowContent> _createPopup;
        
        private readonly Button _createButton;
        private readonly ToolbarSearchField _searchField;
        
        public event Action<T> OnItemCreated;
        public event Action<List<T>> OnSearchListChanged;
        
        public BrowserToolbar(ConfigRepository<T> repository, Func<PopupWindowContent> createPopup)
        {
            _repository = repository;
            _createPopup = createPopup;

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
            var popup = _createPopup();
            if (popup is IConfigCreatePopup<T> creator)
                creator.OnConfirm += CreateAsset;
            UnityEditor.PopupWindow.Show(_createButton.worldBound, popup);
        }

        private void CreateAsset(T config)
        {
            _repository.CreateAsset(config);
            OnItemCreated?.Invoke(config);
        }

        private void OnSearchValueChanged(ChangeEvent<string> evt)
            => OnSearchListChanged?.Invoke(_repository.FindByName(evt.newValue));
    }
}