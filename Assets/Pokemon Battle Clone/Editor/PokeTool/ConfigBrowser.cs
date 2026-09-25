using System;
using System.Collections.Generic;
using System.Linq;
using Pokemon_Battle_Clone.Editor.Database;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool
{
    public class ConfigBrowser<T> : VisualElement where T : ScriptableObject
    {
        private readonly ConfigRepository<T> _repository;
        private readonly ConfigListView<T> _listView;
        private readonly string _deleteDialogTitle;
        private readonly Func<T, string> _getDeleteMessage;
        private readonly Action<T> _onBeforeDelete;

        public event Action<T> OnItemSelected;

        public ConfigBrowser(
            ConfigRepository<T> repository,
            ConfigListView<T> listView,
            string deleteDialogTitle,
            Func<T, string> getDeleteMessage,
            Action<T> onBeforeDelete
        )
        {
            _repository = repository;
            _listView = listView;
            _deleteDialogTitle = deleteDialogTitle;
            _getDeleteMessage = getDeleteMessage;
            _onBeforeDelete = onBeforeDelete ?? (_ => { });
            
            _listView.selectionChanged += OnSelectionChanged;
            _listView.OnDeleteRequested += HandleDeleteRequest;
            
            this.Add(_listView);
        }

        public void SetEntries(List<T> entries) => _listView.SetEntries(entries);

        public void RefreshAndFocus(T item)
        {
            _listView.SetEntries(_repository.FindAll());
            _listView.SetFocusAt(item);
        }

        private void OnSelectionChanged(IEnumerable<object> selection)
            => OnItemSelected?.Invoke(selection.FirstOrDefault() as T);

        private void HandleDeleteRequest(T item)
        {
            var confirmed = EditorUtility.DisplayDialog(
                _deleteDialogTitle, _getDeleteMessage(item), "Delete", "Cancel");

            if (!confirmed) return;
            
            _onBeforeDelete?.Invoke(item);
            _repository.DeleteAsset(item);
            _listView.SetEntries(_repository.FindAll());
        }
    }
}