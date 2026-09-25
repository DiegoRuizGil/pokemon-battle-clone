using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool
{
    public class ConfigListView<T> : ListView where T : ScriptableObject
    {
        public event Action<T> OnDeleteRequested;

        private readonly Func<T, string> _getLabel;

        public ConfigListView(List<T> entries, Func<T, string> getLabel)
        {
            _getLabel = getLabel;
            this.itemsSource = entries;
            this.makeItem = () => new ConfigListEntry<T>();
            this.bindItem = (element, i) =>
            {
                var entry = element as ConfigListEntry<T>;
                var item = (T)itemsSource[i];
                entry!.Bind(item, _getLabel(item));
                entry.OnDeleteRequested += HandleOnDeleteRequest;
            };
            this.unbindItem = (element, i) =>
            {
                var entry = element as ConfigListEntry<T>;
                entry!.OnDeleteRequested -= HandleOnDeleteRequest;
            };
            
            this.showAlternatingRowBackgrounds = AlternatingRowBackground.ContentOnly;
        }
        
        public void SetEntries(List<T> entries)
        {
            this.itemsSource = entries;
            this.RefreshItems();
            this.ClearSelection();
        }

        public void SetFocusAt(T pokemon)
        {
            var index = this.itemsSource.IndexOf(pokemon);
            this.ClearSelection();
            this.AddToSelection(index);
            this.ScrollToItem(index);
        }

        private void HandleOnDeleteRequest(T pokemon) => OnDeleteRequested?.Invoke(pokemon);
    }
}