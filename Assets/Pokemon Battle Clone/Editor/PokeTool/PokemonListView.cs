using System;
using System.Collections.Generic;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool
{
    [UxmlElement]
    public partial class PokemonListView : ListView
    {
        public event Action<PokemonConfig> OnDeleteRequested; 
        
        public PokemonListView() { }

        public PokemonListView(List<PokemonConfig> pokemonEntries)
        {
            this.itemsSource = pokemonEntries;
            this.makeItem = () => new PokemonListEntry();
            this.bindItem = (element, i) =>
            {
                var entry = element as PokemonListEntry;
                entry!.Bind(itemsSource[i] as PokemonConfig);
                entry.OnDeleteRequested += HandleOnDeleteRequest;
            };
            this.unbindItem = (element, i) =>
            {
                var entry = element as PokemonListEntry;
                entry!.OnDeleteRequested -= HandleOnDeleteRequest;
            };
            
            //styling
            this.showAlternatingRowBackgrounds = AlternatingRowBackground.ContentOnly;
        }

        public void SetEntries(List<PokemonConfig> entries)
        {
            this.itemsSource = entries;
            this.RefreshItems();
            this.ClearSelection();
        }

        public void SetFocusAt(PokemonConfig pokemon)
        {
            var index = this.itemsSource.IndexOf(pokemon);
            this.ClearSelection();
            this.AddToSelection(index);
        }

        private void HandleOnDeleteRequest(PokemonConfig pokemon) => OnDeleteRequested?.Invoke(pokemon);
    }
}