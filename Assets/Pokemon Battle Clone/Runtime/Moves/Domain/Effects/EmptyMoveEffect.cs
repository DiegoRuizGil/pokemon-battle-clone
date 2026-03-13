using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Battles.Domain.Events;

namespace Pokemon_Battle_Clone.Runtime.Moves.Domain.Effects
{
    [System.Serializable]
    public class EmptyMoveEffect : IMoveEffect
    {
        public IBattleEvent Apply(Move move, Battle battle, Side side) => new EmptyEvent();
    }
}