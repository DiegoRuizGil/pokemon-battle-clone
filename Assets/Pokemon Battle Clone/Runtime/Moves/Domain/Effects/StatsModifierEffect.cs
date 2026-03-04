using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.RNG;
using Pokemon_Battle_Clone.Runtime.Stats.Domain;
using Pokemon_Battle_Clone.Runtime.Trainer.Domain.BattleEvents;

namespace Pokemon_Battle_Clone.Runtime.Moves.Domain.Effects
{
    public class StatsModifierEffect : IMoveEffect
    {
        private readonly bool _applyToTarget;
        private readonly StatSet _statsModifier;

        public StatsModifierEffect(bool applyToTarget, StatSet statsModifier)
        {
            _applyToTarget = applyToTarget;
            _statsModifier = statsModifier;
        }
        
        public IBattleEvent Apply(Move move, Battle battle, Side side)
        {
            var user = battle.GetFirstPokemon(side);
            var target = battle.GetOpponentFirstPokemon(side);
            
            if (_applyToTarget)
                target.Stats.Modifiers.Apply(_statsModifier);
            else
                user.Stats.Modifiers.Apply(_statsModifier);

            return new StatsModifierEvent(side, _applyToTarget ? target.Stats.Modifiers : user.Stats.Modifiers, user.Name, target.Name);
        }
    }
}