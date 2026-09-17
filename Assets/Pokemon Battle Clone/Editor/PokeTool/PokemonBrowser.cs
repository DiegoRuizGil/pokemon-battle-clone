using System;
using System.Collections.Generic;
using System.Linq;
using Pokemon_Battle_Clone.Editor.Database;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool
{
    [UxmlElement]
    public partial class PokemonBrowser : VisualElement
    {
        private readonly PokemonConfigRepository _repository;
        private readonly PokemonListView _listView;
        
        public event Action<PokemonConfig> OnPokemonSelected;
        
        public PokemonBrowser() { }
        
        public PokemonBrowser(PokemonConfigRepository repository)
        {
            _repository = repository;
            
            var toolbar = new BrowserToolbar(repository);
            _listView = new PokemonListView(repository.FindAll());
            
            toolbar.OnPokemonCreated += pokemon =>
            {
                _listView.SetEntries(_repository.FindAll());
                _listView.SetFocusAt(pokemon);
            };
            toolbar.OnSearchListChanged += list => _listView.SetEntries(list);
            
            _listView.selectionChanged += OnSelectionChanged;
            _listView.OnDeleteRequested += HandleDeleteRequest;
            
            this.Add(toolbar);
            this.Add(_listView);
            
            var adjustAssetsNameButton = new Button(AdjustAssetsName);
            adjustAssetsNameButton.text = "Adjust Assets Name";
            this.Add(adjustAssetsNameButton);
        }

        private void OnSelectionChanged(IEnumerable<object> obj)
        {
            OnPokemonSelected?.Invoke(obj.FirstOrDefault() as PokemonConfig);
        }

        private void HandleDeleteRequest(PokemonConfig pokemon)
        {
            var actionConfirmed = EditorUtility.DisplayDialog(
                "Delete Pokemon",
                $"Are you sure you want to delete this pokemon ({pokemon.pokemonName})?",
                "Delete", "Cancel"
            );

            if (actionConfirmed)
            {
                _repository.DeleteAsset(pokemon);
                _listView.SetEntries(_repository.FindAll());
            }
        }

        private void AdjustAssetsName()
        {
            var pokemonList = _repository.FindAll();
            foreach (var pokemonConfig in pokemonList)
            {
                var newName = pokemonConfig.ID.ToString();
                var newPath = AssetDatabase.GetAssetPath(pokemonConfig);
                AssetDatabase.RenameAsset(newPath, newName);
            }
        }
    }
}