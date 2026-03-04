using Pokemon_Battle_Clone.Runtime.Core.Domain;

namespace Pokemon_Battle_Clone.Runtime.Trainer.Domain.BattleEvents
{
    public class SendPokemonEvent : IBattleEvent
    {
        public Side ActionSide { get; }
        public string PokemonName { get; }
        
        public SendPokemonEvent(Side side, string pokemonName)
        {
            ActionSide = side;
            PokemonName = pokemonName;
        }
    }
}