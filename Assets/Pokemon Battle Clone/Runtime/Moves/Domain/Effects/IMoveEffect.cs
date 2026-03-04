using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.RNG;
using Pokemon_Battle_Clone.Runtime.Trainer.Domain.BattleEvents;

namespace Pokemon_Battle_Clone.Runtime.Moves.Domain.Effects
{
    public interface IMoveEffect
    {
        IBattleEvent Apply(Move move, Battle battle, Side side);
    }
}