using System.Collections.Generic;
using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Battles.Domain.Events;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain.Actions;

namespace Pokemon_Battle_Clone.Runtime.Trainers.Control
{
    public abstract class Trainer
    {
        protected readonly Team Team;

        public bool Defeated => Team.Defeated;
        public bool IsFirstPokemonDefeated => Team.FirstPokemon.Defeated;
        public abstract Side Side { get; }

        protected Trainer(Team team)
        {
            Team = team;
        }

        public TrainerAction Init() => SendFirstPokemon();

        public abstract Task<TrainerAction> SelectAction(Battle battle);

        public abstract Task<SwapPokemonAction> SelectSwapPokemon(Battle battle);

        public abstract Task<T> SelectActionOfType<T>(bool forceSelection, Battle battle) where T : TrainerAction;

        protected virtual TrainerAction SendFirstPokemon() => new SendPokemonAction(Side);
    }
}