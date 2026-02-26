using Pokemon_Battle_Clone.Runtime.Core.Domain;

namespace Pokemon_Battle_Clone.Runtime.Moves.Domain.Effects
{
    public class EmptyMoveEffect : IMoveEffect
    {
        // intentionally left blank
        public void Apply(Move move, Battle battle, Side side) { }
    }
}