using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool
{
    [UxmlElement]
    public partial class PokeTool : VisualElement
    {
        public PokeTool()
        {
            var browser = new PokemonBrowser();
            var dataEditor = new PokemonDataEditor();

            browser.OnPokemonSelected += dataEditor.BindPokemon;
            
            this.Add(browser);
            this.Add(dataEditor);
        }
    }
}
