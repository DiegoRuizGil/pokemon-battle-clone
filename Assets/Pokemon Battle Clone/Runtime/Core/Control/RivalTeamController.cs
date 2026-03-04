using System;
using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Core.Infrastructure;
using Pokemon_Battle_Clone.Runtime.CustomLogs;
using Pokemon_Battle_Clone.Runtime.Trainer.Domain.Actions;
using Pokemon_Battle_Clone.Runtime.Trainer.Domain.Strategies;

namespace Pokemon_Battle_Clone.Runtime.Core.Control
{
    public class RivalTeamController : TeamController
    {
        private readonly ITrainerStrategy _trainerStrategy;

        public RivalTeamController(Team team, ITrainerStrategy strategy, ITeamView view)
            : base(team, view)
        {
            _trainerStrategy = strategy;
        }

        public override Task<TrainerAction> SelectActionTask()
        {
            LogManager.Log("Rival - select action task", FeatureType.Rival);
            var action = _trainerStrategy.SelectMove(Team);
            
            return Task.FromResult<TrainerAction>(action);
        }

        public override Task<T> SelectActionOfType<T>(bool forceSelection)
        {
            TrainerAction action = typeof(T) switch
            {
                var t when t == typeof(MoveAction) => _trainerStrategy.SelectMove(Team),
                var t when t == typeof(SwapPokemonAction) => _trainerStrategy.SelectPokemon(Team),
                _ => throw new InvalidOperationException($"Unsupported action type {typeof(T)}")
            };

            return Task.FromResult((T)action);
        }
    }
}