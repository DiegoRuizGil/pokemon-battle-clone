using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool
{
    [UxmlElement]
    public partial class PokeTool : VisualElement
    {
        public PokeTool()
        {
            var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
            var browser = new PokemonBrowser();
            var dataEditor = new PokemonDataEditor();

            browser.OnPokemonSelected += dataEditor.BindPokemon;
            
            splitView.Add(browser);
            splitView.Add(dataEditor);
            this.Add(splitView);
        }
    }
}
