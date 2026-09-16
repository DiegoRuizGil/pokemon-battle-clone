using System.Collections.Generic;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool
{
    [UxmlElement]
    public partial class PokemonListView : ListView
    {
        public PokemonListView() { }

        public PokemonListView(List<PokemonConfig> pokemonEntries)
        {
            this.itemsSource = pokemonEntries;
            this.makeItem = () => new PokemonListEntry();
            this.bindItem = (element, i) => (element as PokemonListEntry).Bind(itemsSource[i] as PokemonConfig);
            
            //styling
            this.showAlternatingRowBackgrounds = AlternatingRowBackground.ContentOnly;
        }

        public void SetEntries(List<PokemonConfig> entries)
        {
            this.itemsSource = entries;
            this.RefreshItems();
        }

        public void AddEntry(PokemonConfig entry)
        {
            this.itemsSource.Add(entry);
            this.RefreshItems();
            
            int index = this.itemsSource.Count - 1;
            this.SetSelection(index);
            this.ScrollToItem(index);
        }
    }
}