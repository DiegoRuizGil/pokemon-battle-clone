using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool.Teams
{
    public class ConfigPicker<T> : VisualElement where T : ScriptableObject
    {
        public event Action<T> OnPicked;

        private readonly VisualElement _currentRow;
        private readonly ListView _listView;
        private readonly Func<T, string> _getName;
        private readonly Action<VisualElement, T> _bindRow;

        private List<T> _allItems = new();
        private List<T> _filteredItems = new();

        private T _current;
        private string _filterText = "";
        
        public ConfigPicker(
            Func<T, string> getName,
            Func<VisualElement> makeRow,
            Action<VisualElement, T> bindRow,
            float rowHeight = 24
        )
        {
            _getName = getName;
            _bindRow = bindRow;

            _currentRow = makeRow();
            _currentRow.AddToClassList("config-picker-current");
            _currentRow.style.height = rowHeight;
            _currentRow.style.display = DisplayStyle.None;

            _listView = new ListView
            {
                itemsSource = _filteredItems,
                fixedItemHeight = rowHeight,
                selectionType = SelectionType.Single,
                makeItem = makeRow,
                bindItem = (element, i) => bindRow(element, _filteredItems[i]),
                showAlternatingRowBackgrounds = AlternatingRowBackground.ContentOnly
            };
            _listView.selectionChanged += OnSelectionChanged;

            this.AddToClassList("config-picker");
            this.Add(_currentRow);
            this.Add(_listView);
        }
        
        public void SetItems(List<T> items)
        {
            _allItems = items;
            Filter("");
        }

        public void SetCurrent(T current)
        {
            _current = current;
            if (current != null) _bindRow(_currentRow, current);
            _currentRow.style.display = current != null ? DisplayStyle.Flex : DisplayStyle.None;
            Filter(_filterText);
        }
        
        public void Filter(string text)
        {
            _filterText = text;
            
            var items = _allItems.Where(item => item != _current);
            _filteredItems = string.IsNullOrWhiteSpace(text)
                ? items.ToList()
                : items.Where(item => _getName(item).IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

            _listView.itemsSource = _filteredItems;
            _listView.RefreshItems();
        }
        
        private void OnSelectionChanged(IEnumerable<object> selection)
        {
            if (selection.FirstOrDefault() is not T item) return;
            
            _listView.SetSelectionWithoutNotify(Array.Empty<int>());
            OnPicked?.Invoke(item);
        }
    }
}