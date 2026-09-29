using System;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool.Teams
{
    public class PanelHost : VisualElement
    {
        private Action _onClosed;

        public PanelHost()
        {
            style.display = DisplayStyle.None;
        }

        public void Open(VisualElement panel, Action onClosed = null)
        {
            var previousOnClosed = _onClosed;
            _onClosed = onClosed;

            if (panel.parent != this)
                this.Add(panel);
            
            foreach (var child in Children())
                child.style.display = child == panel ? DisplayStyle.Flex : DisplayStyle.None;
            
            style.display = DisplayStyle.Flex;
            
            previousOnClosed?.Invoke();
        }

        public void Close()
        {
            var onClosed = _onClosed;
            _onClosed = null;
            
            style.display = DisplayStyle.None;
            
            onClosed?.Invoke();
        }
    }
}