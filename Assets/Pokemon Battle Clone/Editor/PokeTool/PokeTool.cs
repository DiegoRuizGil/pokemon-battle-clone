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
            var pokemonRepository = ConfigRepositories.Pokemon();
            var spritesRepository = new PokemonSpritesRepository();
            var apiLoader = new PokemonApiLoader(spritesRepository);
            
            var listView = new ConfigListView<PokemonConfig>(
                pokemonRepository.FindAll(), p => $"{p.ID:D3} - {p.pokemonName}");
            var browser = new ConfigBrowser<PokemonConfig>(
                pokemonRepository,
                listView,
                deleteDialogTitle: "Delete Pokemon",
                getDeleteMessage: p => $"Are you sure you want to delete this pokemon ({p.pokemonName}). This action also deletes the pokemon's sprites.",
                onBeforeDelete: p => spritesRepository.Delete(p.ID)
            );
            var dataEditor = new PokemonDataEditor(spritesRepository);
            var toolbar = new BrowserToolbar(pokemonRepository, apiLoader);
            
            toolbar.OnPokemonCreated += browser.RefreshAndFocus;
            toolbar.OnSearchListChanged += browser.SetEntries;
            browser.OnItemSelected += dataEditor.BindPokemon;
            
            var splitLeft = new VisualElement();
            splitLeft.Add(toolbar);
            splitLeft.Add(browser);
            
            var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
            splitView.Add(splitLeft);
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
