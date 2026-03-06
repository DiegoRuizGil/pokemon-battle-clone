using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain.BattleEvents;

namespace Pokemon_Battle_Clone.Runtime.Moves.Domain.Effects
{
    public class EmptyMoveEffect : IMoveEffect
    {
        public IBattleEvent Apply(Move move, Battle battle, Side side) => new EmptyEvent();
    }
}