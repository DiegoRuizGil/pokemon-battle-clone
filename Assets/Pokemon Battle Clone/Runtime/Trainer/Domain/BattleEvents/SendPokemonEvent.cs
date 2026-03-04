using Pokemon_Battle_Clone.Runtime.Core.Domain;

namespace Pokemon_Battle_Clone.Runtime.Trainer.Domain.BattleEvents
{
    public class SendPokemonEvent : IBattleEvent
    {
        public Side ActionSide { get; }
        public Pokemon Pokemon { get; }
        
        public SendPokemonEvent(Side side, Pokemon pokemon)
        {
            ActionSide = side;
            Pokemon = pokemon;
        }
    }
}