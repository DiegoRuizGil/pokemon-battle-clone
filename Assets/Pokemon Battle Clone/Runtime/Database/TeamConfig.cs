using System.Collections.Generic;
using System.Linq;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Database
{
    [CreateAssetMenu(menuName = "Pokemon Battle Clone/Database/Team", fileName = "Team Config")]
    public class TeamConfig : ScriptableObject
    {
        [System.Serializable]
        public struct PokemonAndMoves
        {
            public PokemonConfig pokemonConfig;
            public List<MoveConfig> movesConfig;

            public Pokemon BuildPokemon()
            {
                var pokemon = pokemonConfig.Build();
                pokemon.MoveSet.AddMoves(movesConfig.Select(m => m.Build()));
                return pokemon;
            }
        }

        public List<PokemonAndMoves> pokemonList;

        public Team Build()
        {
            return new Team(pokemonList.Select(p => p.BuildPokemon()));
        }
    }
}