using Pokemon_Battle_Clone.Runtime.Core.Domain;

namespace Pokemon_Battle_Clone.Runtime.Trainers.Domain.BattleEvents
{
    public class WithdrawPokemon : IBattleEvent
    {
        public Side ActionSide { get; }
        public string PokemonName { get; }
        
        public WithdrawPokemon(Side side, string pokemonName)
        {
            ActionSide = side;
            PokemonName = pokemonName;
        }
    }
}