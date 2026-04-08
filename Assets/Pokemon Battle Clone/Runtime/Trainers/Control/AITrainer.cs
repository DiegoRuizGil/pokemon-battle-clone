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
        private readonly Battle _battle;
        
        public override Side Side => Side.Rival;

        public AITrainer(Battle battle, Team team, ITrainerStrategy strategy) : base(team)
        {
            _battle = battle;
            _trainerStrategy = strategy;
        }
        
        public override Task<TrainerAction> SelectAction()
        {
            var action = _trainerStrategy.Evaluate(_battle, Side);
            
            return Task.FromResult(action);
        }

        public override Task<TrainerAction> SelectSwapPokemon()
        {
            return Task.FromResult(_trainerStrategy.SelectPokemon(_battle, Side));
        }
    }
}