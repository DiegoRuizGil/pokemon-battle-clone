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

        public override Task<TrainerAction> SelectSwapPokemon(Battle battle)
        {
            return Task.FromResult(_trainerStrategy.SelectPokemon(battle, Side));
        }
    }
}