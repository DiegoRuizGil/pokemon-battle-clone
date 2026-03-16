using System.Collections.Generic;
using Pokemon_Battle_Clone.Runtime.Database;
using Pokemon_Battle_Clone.Runtime.Trainers.Infrastructure.Actions;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.TeamBuilder.Selector
{
    public class TrainerTeamSelection : MonoBehaviour
    {
        [SerializeField] private PokemonAssetDatabase assetDatabase;
        [SerializeField] private List<PokemonSelectorButton> pokemonButtons;
        
        private TeamConfig _teamConfig;
        
        public bool HasTeamSelected => _teamConfig != null;

        private void Start()
        {
            foreach (var button in pokemonButtons)
                button.gameObject.SetActive(false);
        }

        public void SetTeam(TeamConfig teamConfig)
        {
            _teamConfig = teamConfig;
            for (var i = 0; i < pokemonButtons.Count; i++)
            {
                if (_teamConfig.pokemonList.Count > i)
                {
                    var pokemon = _teamConfig.pokemonList[i].BuildPokemon();
                    var icon = assetDatabase.GetIcon(pokemon.ID);
                    pokemonButtons[i].SetData(pokemon, icon);
                    pokemonButtons[i].gameObject.SetActive(true);
                }
                else
                {
                    pokemonButtons[i].gameObject.SetActive(false);
                }
            }
        }
    }
}