using Pokemon_Battle_Clone.Runtime.Database;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool
{
    [UxmlElement]
    public partial class PokemonListEntry : VisualElement
    {
        private readonly Label _label;
        
        public PokemonListEntry()
        {
            _label = new Label();
            this.Add(_label);
        }

        public void Bind(PokemonConfig pokemonConfig)
        {
            _label.text = $"{pokemonConfig.ID:D3} - {pokemonConfig.pokemonName}";
        }
    }
}