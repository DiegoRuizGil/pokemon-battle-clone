using Pokemon_Battle_Clone.Editor.Database;
using Pokemon_Battle_Clone.Editor.Database.PokeApi;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool
{
    [UxmlElement]
    public partial class PokeTool : VisualElement
    {
        public PokeTool()
        {
            var pokemonRepository = ConfigRepositories.Pokemon();
            var movesRepository = ConfigRepositories.Move();
            var spritesRepository = new PokemonSpritesRepository();
            var apiLoader = new PokemonApiLoader(spritesRepository);
            
            var pokemonTab = new Tab("Pokemon");
            pokemonTab.AddToClassList("poketool-tab");
            var pokemonTabContent = BuildPokemonTab(pokemonRepository, spritesRepository, apiLoader);
            pokemonTab.Add(pokemonTabContent);
            
            var teamsTab = new Tab("Teams");
            teamsTab.AddToClassList("poketool-tab");
            teamsTab.Add(new Label("Content for teams tab"));
            
            var movesTab = new Tab("Moves");
            movesTab.AddToClassList("poketool-tab");
            var movesTabContent = BuildMovesTab(movesRepository);
            movesTab.Add(movesTabContent);
            
            var tabView = new TabView();
            tabView.Add(pokemonTab);
            tabView.Add(teamsTab);
            tabView.Add(movesTab);
            this.Add(tabView);
        }

        private VisualElement BuildPokemonTab(ConfigRepository<PokemonConfig> pokemonRepository, PokemonSpritesRepository spritesRepository, PokemonApiLoader apiLoader)
        {
            var listView = new ConfigListView<PokemonConfig>(
                pokemonRepository.FindAll(), p => $"{p.ID:D3} - {p.pokemonName}");
            var browser = new ConfigBrowser<PokemonConfig>(
                pokemonRepository,
                listView,
                deleteDialogTitle: "Delete Pokemon",
                getDeleteMessage: p => $"Are you sure you want to delete this pokemon ({p.pokemonName})? This action also deletes the pokemon's sprites.",
                onBeforeDelete: p => spritesRepository.Delete(p.ID)
            );
            var dataEditor = new PokemonDataEditor(spritesRepository);
            var toolbar = new BrowserToolbar(pokemonRepository, apiLoader);
            
            toolbar.OnPokemonCreated += browser.RefreshAndFocus;
            toolbar.OnSearchListChanged += browser.SetEntries;
            browser.OnItemSelected += dataEditor.Bind;
            
            var browserContainer = new VisualElement();
            browserContainer.AddToClassList("browser-container");
            browserContainer.Add(toolbar);
            browserContainer.Add(browser);
            
            var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
            splitView.Add(browserContainer);
            splitView.Add(dataEditor);

            return splitView;
        }
        
        private VisualElement BuildMovesTab(ConfigRepository<MoveConfig> movesRepository)
        {
            var listView = new ConfigListView<MoveConfig>(
                movesRepository.FindAll(), m => $"{m.id:D3} - {m.moveName}");
            var browser = new ConfigBrowser<MoveConfig>(
                movesRepository,
                listView,
                deleteDialogTitle: "Delete Move",
                getDeleteMessage: m => $"Are you sure you want to delete this move ({m.moveName})?",
                onBeforeDelete: _ => { }
            );
            var dataEditor = new MoveDataEditor();
            // var toolbar = new BrowserToolbar(movesRepository, apiLoader);
            
            // toolbar.OnPokemonCreated += browser.RefreshAndFocus;
            // toolbar.OnSearchListChanged += browser.SetEntries;
            browser.OnItemSelected += dataEditor.Bind;
            
            var browserContainer = new VisualElement();
            browserContainer.AddToClassList("browser-container");
            // splitLeft.Add(toolbar);
            browserContainer.Add(browser);
            
            var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
            splitView.Add(browserContainer);
            splitView.Add(dataEditor);

            return splitView;
        }
    }
}
