using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Battles.Domain.Events;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Moves.Domain.Effects
{
    [System.Serializable]
    public class ConditionalEffect
    {
        [SerializeReference, SubclassSelector, SerializeField] private IMoveEffect _effect;
        [SerializeField] private int _chancePercent;

        public ConditionalEffect(IMoveEffect effect, int chancePercent)
        {
            _effect = effect;
            _chancePercent = chancePercent;
        }

        public IBattleEvent TryApply(Move move, Battle battle, Side side)
        {
            var hit = battle.Random.Roll(_chancePercent);
            if (hit)
                return _effect.Apply(move, battle, side);
            return new EmptyEvent();
        }
    }
}