using System;
using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain.Actions;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain.Strategies;

namespace Pokemon_Battle_Clone.Runtime.Trainers.Control
{
    public class AITrainer : Trainer
    {
        private readonly ITrainerStrategy _trainerStrategy;
        
        public override Side Side => Side.Rival;

        public AITrainer(Team team, ITrainerStrategy strategy) : base(team)
        {
            _trainerStrategy = strategy;
        }
        
        public override Task<TrainerAction> SelectAction(Battle battle)
        {
            var action = _trainerStrategy.Evaluate(battle, Side);
            
            return Task.FromResult(action);
        }

        public override Task<SwapPokemonAction> SelectSwapPokemon(Battle battle)
        {
            return Task.FromResult(_trainerStrategy.SelectPokemon(battle, Side));
        }

        public override Task<T> SelectActionOfType<T>(bool forceSelection, Battle battle)
        {
            TrainerAction action = typeof(T) switch
            {
                var t when t == typeof(MoveAction) => _trainerStrategy.SelectMove(battle, Side),
                var t when t == typeof(SwapPokemonAction) => _trainerStrategy.SelectPokemon(battle, Side),
                _ => throw new InvalidOperationException($"Unsupported action type {typeof(T)}")
            };

            return Task.FromResult((T)action);
        }
    }
}