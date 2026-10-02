using Pokemon_Battle_Clone.Editor.Database;
using Pokemon_Battle_Clone.Editor.Database.PokeApi;
using Pokemon_Battle_Clone.Editor.PokeTool.CreatePopup;
using Pokemon_Battle_Clone.Editor.PokeTool.Teams;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Database;
using Pokemon_Battle_Clone.Runtime.Moves.Domain;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool
{
    [UxmlElement]
    public partial class PokeTool : VisualElement
    {
        public PokeTool()
        {
            var teamsTab = new Tab("Teams");
            teamsTab.AddToClassList("poketool-tab");
            var teamsTabContent = BuildTeamsTab();
            teamsTab.Add(teamsTabContent);
            
            var pokemonTab = new Tab("Pokemon");
            pokemonTab.AddToClassList("poketool-tab");
            var pokemonTabContent = BuildPokemonTab();
            pokemonTab.Add(pokemonTabContent);
            
            var movesTab = new Tab("Moves");
            movesTab.AddToClassList("poketool-tab");
            var movesTabContent = BuildMovesTab();
            movesTab.Add(movesTabContent);
            
            var tabView = new TabView();
            tabView.Add(teamsTab);
            tabView.Add(pokemonTab);
            tabView.Add(movesTab);
            this.Add(tabView);
        }

        private VisualElement BuildTeamsTab()
        {
            var repository = ConfigRepositories.Team();
            var spritesRepository = new PokemonSpritesRepository();
            var pokemonRepository = ConfigRepositories.Pokemon();

            var listView = new ConfigListView<TeamConfig>(repository.FindAll(), t => t.name);
            var browser = new ConfigBrowser<TeamConfig>(
                repository,
                listView,
                deleteDialogTitle: "Delete Team",
                getDeleteMessage: t => $"Are you sure you want to delete this team ({t.name})?",
                onBeforeDelete: _ => { }
            );
            var dataEditor = new TeamDataEditor(spritesRepository, pokemonRepository);
            var toolbar = new BrowserToolbar<TeamConfig>(
                repository,
                createPopup: () => new CreateTeamPopup(repository));
            
            toolbar.OnItemCreated += browser.RefreshAndFocus;
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

        private VisualElement BuildPokemonTab()
        {
            var pokemonRepository = ConfigRepositories.Pokemon();
            var pokemonIds = ConfigIdPolicies.Pokemon(pokemonRepository);
            var spritesRepository = new PokemonSpritesRepository();
            var pokemonApiLoader = new PokemonApiLoader(spritesRepository);
            
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
            var toolbar = new BrowserToolbar<PokemonConfig>(
                pokemonRepository,
                createPopup: () =>
                    new CreateConfigPopup<PokemonConfig, PokemonApiDto>(
                        title: "Create Pokemon",
                        pokemonRepository, pokemonIds, pokemonApiLoader,
                        createManually: (id, pokemonName) =>
                        {
                            var config = ScriptableObject.CreateInstance<PokemonConfig>();
                            config.ID = id;
                            config.pokemonName = pokemonName;
                            return config;
                        },
                        suggestedId: pokemonIds.GenerateValidId()
                    )
            );
            
            toolbar.OnItemCreated += browser.RefreshAndFocus;
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
        
        private VisualElement BuildMovesTab()
        {
            var movesRepository = ConfigRepositories.Move();
            var movesIds = ConfigIdPolicies.Move(movesRepository);
            var moveApiLoader = new MoveApiLoader();
            
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
            var toolbar = new BrowserToolbar<MoveConfig>(
                movesRepository,
                createPopup: () =>
                    new CreateConfigPopup<MoveConfig, MoveApiDto>(
                        title: "Create Move",
                        movesRepository, movesIds, moveApiLoader,
                        createManually: (id, moveName) =>
                        {
                            var config = ScriptableObject.CreateInstance<MoveConfig>();
                            config.id = id;
                            config.moveName = moveName;
                            config.type = ElementalType.Normal;
                            config.category = MoveCategory.Physical;
                            return config;
                        },
                        suggestedId: movesIds.GenerateValidId()
                    )
            );
            
            toolbar.OnItemCreated += browser.RefreshAndFocus;
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
    }
}
