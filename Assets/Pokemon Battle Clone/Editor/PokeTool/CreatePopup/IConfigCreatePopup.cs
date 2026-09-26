using System;

namespace Pokemon_Battle_Clone.Editor.PokeTool.CreatePopup
{
    public interface IConfigCreatePopup<T>
    {
        event Action<T> OnConfirm;
    }
}