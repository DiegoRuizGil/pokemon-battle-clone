using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Core.Infrastructure;
using Pokemon_Battle_Clone.Runtime.Trainer.Domain.Actions;

namespace Pokemon_Battle_Clone.Runtime.Core.Control
{
    public abstract class TeamController
    {
        protected readonly Team Team;
        public ITeamView View { get; }

        public bool Defeated => Team.Defeated;
        public bool IsFirstPokemonDefeated => Team.FirstPokemon.Defeated;

        protected TeamController(Team team, ITeamView view)
        {
            Team = team;
            View = view;
        }

        public async Task Init() => await SendFirstPokemon();

        public abstract Task<TrainerAction> SelectActionTask();

        public abstract Task<T> SelectActionOfType<T>(bool forceSelection) where T : TrainerAction;

        public virtual async Task SendFirstPokemon()
        {
            await View.SendPokemon(Team.FirstPokemon);
        }
    }
}