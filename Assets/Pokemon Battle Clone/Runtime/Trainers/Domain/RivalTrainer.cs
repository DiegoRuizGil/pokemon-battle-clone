using System;
using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.CustomLogs;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain.Actions;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain.Strategies;

namespace Pokemon_Battle_Clone.Runtime.Trainers.Domain
{
    public class RivalTrainer : Trainer
    {
        private readonly ITrainerStrategy _trainerStrategy;
        
        public override Side Side => Side.Rival;

        public RivalTrainer(Team team, ITrainerStrategy strategy) : base(team)
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