using System.Collections.Generic;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.TeamBuilder.UI
{
    public class TeamInfoDisplayer : MonoBehaviour, ITeamInfoDisplayer
    {
        [SerializeField] private PokemonAssetDatabase assetDatabase;
        [SerializeField] private PokemonCard pokemonCard;

        public void Display(List<Pokemon> pokemonList, int currentPokemonToDisplay = 0)
        {
            var pokemon = pokemonList[currentPokemonToDisplay];
            var icon = assetDatabase.GetIcon(pokemon.ID);
            
            pokemonCard.Display(pokemon, icon);
            
            gameObject.SetActive(true);
        }
    }
}