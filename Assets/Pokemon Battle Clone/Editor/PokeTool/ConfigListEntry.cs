using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool
{
    public class ConfigListEntry<T> : VisualElement where T : ScriptableObject
    {
        public event Action<T> OnDeleteRequested;

        private readonly Label _label;
        private T _bound;

        public ConfigListEntry()
        {
            _label = new Label();
            var deleteButton = new Button(OnDeleteClicked);
            deleteButton.iconImage = EditorGUIUtility.IconContent("d_TreeEditor.Trash").image as Texture2D;
            
            this.AddToClassList("list-entry");
            deleteButton.AddToClassList("delete-button");
            _label.AddToClassList("entry-label");
            
            this.Add(_label);
            this.Add(deleteButton);
        }
        
        public void Bind(T config, string label)
        {
            _bound = config;
            _label.text = label;
        }

        private void OnDeleteClicked() => OnDeleteRequested?.Invoke(_bound);
    }
}