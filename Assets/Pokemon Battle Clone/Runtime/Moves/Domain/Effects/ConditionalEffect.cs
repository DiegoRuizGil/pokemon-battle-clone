using Pokemon_Battle_Clone.Runtime.Core.Domain;

namespace Pokemon_Battle_Clone.Runtime.Moves.Domain.Effects
{
    public class ConditionalEffect
    {
        private readonly IMoveEffect _effect;
        private readonly int _chancePercent;

        public ConditionalEffect(IMoveEffect effect, int chancePercent)
        {
            _effect = effect;
            _chancePercent = chancePercent;
        }

        public void TryApply(Move move, Battle battle, Side side)
        {
            var hit = battle.Random.Roll(_chancePercent);
            if (hit) _effect.Apply(move, battle, side);
        }
    }
}