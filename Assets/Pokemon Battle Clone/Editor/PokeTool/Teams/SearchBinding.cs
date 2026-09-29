using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool.Teams
{
    public class SearchBinding<T> where T : ScriptableObject
    {
        private readonly TextField _field;
        private readonly ConfigPicker<T> _picker;
        private readonly PanelHost _host;
        private readonly Func<List<T>> _getItems;
        private readonly Action<T> _onPicked;
        
        private bool _active;
        private bool _picked;
        private string _previousText;
        
        public SearchBinding(TextField field, ConfigPicker<T> picker, PanelHost host,
            Func<List<T>> getItems, Action<T> onPicked)
        {
            _field = field;
            _picker = picker;
            _host = host;
            _getItems = getItems;
            _onPicked = onPicked;

            _field.RegisterCallback<FocusInEvent>(_ => Activate());
            _field.RegisterValueChangedCallback(OnTextChanged);
        }
        
        private void Activate()
        {
            if (_active) return;

            _active = true;
            _picked = false;
            _previousText = _field.value;

            _picker.OnPicked += HandlePicked;
            _picker.SetItems(_getItems());
            _host.Open(_picker, Deactivate);
        }
        
        private void OnTextChanged(ChangeEvent<string> evt)
        {
            if (_active) _picker.Filter(evt.newValue);
        }
        
        private void HandlePicked(T item)
        {
            _picked = true;
            _host.Close();
            _onPicked(item);
        }
        
        private void Deactivate()
        {
            if (!_active) return;

            _active = false;
            _picker.OnPicked -= HandlePicked;

            if (!_picked)
                _field.SetValueWithoutNotify(_previousText);
        }
    }
}