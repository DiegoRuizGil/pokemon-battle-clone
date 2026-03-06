using System.Collections.Generic;
using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain.Actions;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain.BattleEvents;

namespace Pokemon_Battle_Clone.Runtime.Trainers.Domain
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

        public IEnumerable<IBattleEvent> Init() => SendFirstPokemon();

        public abstract Task<TrainerAction> SelectActionTask();

        public abstract Task<T> SelectActionOfType<T>(bool forceSelection) where T : TrainerAction;

        protected virtual IEnumerable<IBattleEvent> SendFirstPokemon()
        {
            var sendPokemonEvent = new SendPokemonEvent(Side, Team.FirstPokemon);
            return new List<IBattleEvent> { sendPokemonEvent };
        }
    }
}