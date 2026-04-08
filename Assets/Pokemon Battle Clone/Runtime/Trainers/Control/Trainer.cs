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
        public Pokemon FirstPokemon => Team.FirstPokemon;
        public bool IsFirstPokemonDefeated => FirstPokemon.Defeated;
        public abstract Side Side { get; }

        protected Trainer(Team team)
        {
            Team = team;
        }

        public TrainerAction Init() => SendFirstPokemon();

        public abstract Task<TrainerAction> SelectAction();

        public abstract Task<TrainerAction> SelectSwapPokemon();

        protected virtual TrainerAction SendFirstPokemon() => new SendPokemonAction(Side);

        public FaintedEvent FaintedEvent() => new(Side, Team.FirstPokemon.Name, Team.FirstPokemon.ID);
    }
}