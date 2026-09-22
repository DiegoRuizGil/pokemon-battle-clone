using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Database;
using Pokemon_Battle_Clone.Runtime.TeamBuilder.TeamDisplayer;
using Pokemon_Battle_Clone.Runtime.Trainers.Infrastructure.Actions;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.TeamBuilder.Selector
{
    public class TrainerTeamSelection : MonoBehaviour
    {
        [SerializeField] private TeamInfoDisplayer teamInfoDisplayer;
        [SerializeField] private List<PokemonSelectorButton> pokemonButtons;
        
        private TeamConfig _teamConfig;
        private IPokemonSpriteProvider _spriteProvider;
        
        public bool HasTeamSelected => _teamConfig != null;

        private void Start()
        {
            for (var i = 0; i < pokemonButtons.Count; i++)
            {
                pokemonButtons[i].index = i;
                pokemonButtons[i].gameObject.SetActive(false);
                pokemonButtons[i].OnClick += OnInfoDisplayed;
            }
        }
        
        public void Init(IPokemonSpriteProvider spriteProvider) => _spriteProvider = spriteProvider;

        public void SetTeam(TeamConfig teamConfig)
        {
            _teamConfig = teamConfig;
            DisplayIcons();
        }

        private async void DisplayIcons()
        {
            for (var i = 0; i < pokemonButtons.Count; i++)
            {
                if (_teamConfig.pokemonList.Count > i)
                {
                    var pokemon = _teamConfig.pokemonList[i].BuildPokemon();
                    var icon = await _spriteProvider.GetIconSprite(pokemon.ID);
                    pokemonButtons[i].SetData(pokemon, icon);
                    pokemonButtons[i].gameObject.SetActive(true);
                }
                else
                {
                    pokemonButtons[i].gameObject.SetActive(false);
                }
            }
        }

        private void OnInfoDisplayed(int pokemonIndex)
        {
            var pokemonList = _teamConfig.Build().PokemonList.ToList();
            teamInfoDisplayer.Display(pokemonList, pokemonIndex);
        }
    }
}