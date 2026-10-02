using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool.Teams
{
    public class TeamMemberSelector : VisualElement
    {
        public const int MaxMembers = 6;
        
        public event Action<int> OnSlotSelected;

        private readonly List<Button> _buttons = new();

        public TeamMemberSelector()
        {
            AddToClassList("member-selector");
        }

        public void SetMembers(List<Sprite> icons, int selectedIndex)
        {
            this.Clear();
            _buttons.Clear();
            
            for (var i = 0; i < icons.Count; i++)
                AddSlot(i, CreateMemberButton(icons[i]));
            
            if (icons.Count < MaxMembers)
                AddSlot(icons.Count,
                    new Button { iconImage = EditorGUIUtility.IconContent("Toolbar Plus").image as Texture2D});
            
            SetSelected(selectedIndex);
        }

        public void SetSelected(int index)
        {
            for (var i = 0; i < _buttons.Count; i++)
                _buttons[i].EnableInClassList("selected", i == index);
        }

        private void AddSlot(int index, Button button)
        {
            button.AddToClassList("member-button");
            button.clicked += () => OnSlotSelected?.Invoke(index);
            _buttons.Add(button);
            this.Add(button);
        }

        private static Button CreateMemberButton(Sprite icon)
        {
            var button = new Button();
            button.Add(new Image
            {
                sprite = icon,
                scaleMode = ScaleMode.ScaleToFit,
                pickingMode = PickingMode.Ignore
            });
            return button;
        }
    }
}