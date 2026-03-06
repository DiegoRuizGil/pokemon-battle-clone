using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain.Actions;

namespace Pokemon_Battle_Clone.Runtime.Trainers.Domain.Strategies
{
    public interface ITrainerStrategy
    {
        MoveAction SelectMove(Team team);
        SwapPokemonAction SelectPokemon(Team team);
    }
}