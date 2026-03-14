using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Battles.Domain.Events;
using Pokemon_Battle_Clone.Runtime.Stats.Domain;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Moves.Domain.Effects
{
    [System.Serializable]
    public class StatsModifierEffect : IMoveEffect
    {
        [SerializeField] private bool _applyToTarget;
        [SerializeField] private StatSet _statsModifier;

        public StatsModifierEffect(bool applyToTarget, StatSet statsModifier)
        {
            _applyToTarget = applyToTarget;
            _statsModifier = statsModifier;
        }
        
        public IBattleEvent Apply(Move move, Battle battle, Side side)
        {
            var user = battle.GetFirstPokemon(side);
            var target = battle.GetFirstPokemon(side.Opposite());
            
            if (_applyToTarget)
                target.Stats.Modifiers.Apply(_statsModifier);
            else
                user.Stats.Modifiers.Apply(_statsModifier);

            return new StatsModifierEvent(side, _applyToTarget ? target.Stats.Modifiers : user.Stats.Modifiers,
                _applyToTarget, user.Name, target.Name);
        }
    }
}