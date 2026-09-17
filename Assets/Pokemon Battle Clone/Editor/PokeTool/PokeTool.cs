using Pokemon_Battle_Clone.Editor.Database;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool
{
    [UxmlElement]
    public partial class PokeTool : VisualElement
    {
        private const string DatabasePath = "Assets/Pokemon Battle Clone/Database/Pokemon";
        
        public PokeTool()
        {
            var repository = new PokemonConfigRepository(DatabasePath);
            
            var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
            var browser = new PokemonBrowser(repository);
            var dataEditor = new PokemonDataEditor(repository);

            browser.OnPokemonSelected += dataEditor.BindPokemon;
            
            splitView.Add(browser);
            splitView.Add(dataEditor);
            this.Add(splitView);
        }
    }
}
