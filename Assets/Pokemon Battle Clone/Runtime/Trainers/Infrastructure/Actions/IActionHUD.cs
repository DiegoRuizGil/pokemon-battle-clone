using System;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Moves.Domain;
using Pokemon_Battle_Clone.Runtime.Moves.Infrastructure;

namespace Pokemon_Battle_Clone.Runtime.Trainers.Infrastructure.Actions
{
    public interface IActionHUD
    {
        void HideSelectors();
        void ShowMoveSelector(bool forceSelection, MoveSetDTO moveSet);
        void ShowPokemonSelector(bool forceSelection, Team team);
        void SetData(Team team, MoveSet moveSet);

        void RegisterMoveSelectedListener(Action<int> listener);
        void RegisterMoveButtonPressedListener(Action listener);
        void RegisterPokemonSelectedListener(Action<int> listener);
        void RegisterPokemonButtonPressedListener(Action listener);
    }
}