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
    public class PokemonBrowser : VisualElement
    {
        private readonly PokemonConfigRepository _pokemonConfigRepository;
        private readonly PokemonSpritesRepository _spritesRepository;
        private readonly PokemonListView _listView;
        
        public event Action<PokemonConfig> OnPokemonSelected;
        
        public PokemonBrowser(
            PokemonConfigRepository pokemonConfigRepository,
            PokemonSpritesRepository spritesRepository,
            PokemonApiLoader apiLoader
        )
        {
            _pokemonConfigRepository = pokemonConfigRepository;
            _spritesRepository = spritesRepository;

            var toolbar = new BrowserToolbar(pokemonConfigRepository, apiLoader);
            _listView = new PokemonListView(pokemonConfigRepository.FindAll());
            
            toolbar.OnPokemonCreated += pokemon =>
            {
                _listView.SetEntries(_pokemonConfigRepository.FindAll());
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
                $"Are you sure you want to delete this pokemon ({pokemon.pokemonName})? This action also deletes the pokemon's sprites.",
                "Delete", "Cancel"
            );
            
            if (actionConfirmed)
            {
                _spritesRepository.Delete(pokemon.ID);
                _pokemonConfigRepository.DeleteAsset(pokemon);
                _listView.SetEntries(_pokemonConfigRepository.FindAll());
            }
        }

        private void AdjustAssetsName()
        {
            var pokemonList = _pokemonConfigRepository.FindAll();
            foreach (var pokemonConfig in pokemonList)
            {
                var newName = pokemonConfig.ID.ToString();
                var newPath = AssetDatabase.GetAssetPath(pokemonConfig);
                AssetDatabase.RenameAsset(newPath, newName);
            }
        }
    }
}