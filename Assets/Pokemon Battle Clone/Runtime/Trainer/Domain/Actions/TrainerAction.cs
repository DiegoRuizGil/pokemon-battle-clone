using System.Collections.Generic;
using System.Linq;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.RNG;
using Pokemon_Battle_Clone.Runtime.Trainer.Domain.BattleEvents;

namespace Pokemon_Battle_Clone.Runtime.Trainer.Domain.Actions
{
    public abstract class TrainerAction
    {
        public readonly Side Side;
        public readonly int PokemonInFieldSpeed;
        public abstract int Priority { get; }

        protected TrainerAction(Side side, int pokemonInFieldSpeed)
        {
            Side = side;
            PokemonInFieldSpeed = pokemonInFieldSpeed;
        }

        public abstract IEnumerable<IBattleEvent> Execute(Battle battle);
    }
}