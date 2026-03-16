using System.Collections.Generic;
using System.Linq;
using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Database;
using Pokemon_Battle_Clone.Runtime.TeamBuilder.TeamDisplayer;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.TeamBuilder.Selector
{
    public class TeamListScrollView : MonoBehaviour
    {
        [SerializeField] private TeamsAllocator teamsAllocator;
        [SerializeField] private List<TeamConfig> teamConfigs;

        [Header("UI")]
        [SerializeField] private TeamSelector teamSelectorPrefab;
        [SerializeField] private GameObject content;
        [SerializeField] private TeamInfoDisplayer teamInfoDisplayer;
        
        private void Awake()
        {
            foreach (var teamConfig in teamConfigs)
            {
                var selector = Instantiate(teamSelectorPrefab, content.transform);
                selector.Init(teamConfig);
                selector.OnPlayerSelected += OnPlayerSelected;
                selector.OnRivalSelected += OnRivalSelected;
                selector.OnInfoSelected += OnInfoSelected;
            }
        }

        private void OnPlayerSelected(TeamConfig teamConfig) => OnTeamSelected(teamConfig, Side.Player);
        private void OnRivalSelected(TeamConfig teamConfig) => OnTeamSelected(teamConfig, Side.Rival);
        private void OnTeamSelected(TeamConfig teamConfig, Side side) => teamsAllocator.SetTeam(teamConfig, side);

        private void OnInfoSelected(TeamConfig teamConfig)
        {
            var pokemonList = teamConfig.Build().PokemonList.ToList();
            teamInfoDisplayer.Display(pokemonList);
        }
    }
}