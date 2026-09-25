using Pokemon_Battle_Clone.Editor.Database;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool
{
    [UxmlElement]
    public partial class PokeTool : VisualElement
    {
        public PokeTool()
        {
            // var pokemonConfigRepository = new PokemonConfigRepository(ProjectPaths.PokemonConfigs);
            var pokemonConfigRepository = ConfigRepositories.Pokemon();
            var spritesRepository = new PokemonSpritesRepository();
            var apiLoader = new PokemonApiLoader(spritesRepository);
            
            var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
            var browser = new PokemonBrowser(pokemonConfigRepository, spritesRepository, apiLoader);
            var dataEditor = new PokemonDataEditor(spritesRepository);
            
            browser.OnPokemonSelected += dataEditor.BindPokemon;
            
            splitView.Add(browser);
            splitView.Add(dataEditor);

            var tabView = new TabView();
            
            var pokemonTab = new Tab("Pokemon");
            pokemonTab.AddToClassList("poketool-tab");
            pokemonTab.Add(splitView);
            
            var teamsTab = new Tab("Teams");
            teamsTab.AddToClassList("poketool-tab");
            teamsTab.Add(new Label("Content for teams tab"));
            
            var movesTab = new Tab("Moves");
            movesTab.AddToClassList("poketool-tab");
            movesTab.Add(new Label("Content for moves tab"));
            
            tabView.Add(pokemonTab);
            tabView.Add(teamsTab);
            tabView.Add(movesTab);
            this.Add(tabView);
        }
    }
}
