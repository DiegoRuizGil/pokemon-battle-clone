using System.Collections.Generic;
using Pokemon_Battle_Clone.Runtime.Core.Domain;

namespace Pokemon_Battle_Clone.Runtime.TeamBuilder.TeamDisplayer
{
    public interface ITeamInfoDisplayer
    {
        void Display(List<Pokemon> pokemonList, int currentPokemonToDisplay = 0);
    }
}