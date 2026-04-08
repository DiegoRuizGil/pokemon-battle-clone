using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain.Actions;

namespace Pokemon_Battle_Clone.Runtime.Trainers.Domain.Strategies
{
    public interface ITrainerStrategy
    {
        TrainerAction Evaluate(Battle battle, Side side);
        MoveAction SelectMove(Battle battle, Side side);
        TrainerAction SelectPokemon(Battle battle, Side side);
    }
}