using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Battles.Domain.Events;

namespace Pokemon_Battle_Clone.Runtime.Battles.Control.EventHandlers
{
    public interface IBattleEventHandler<T> where T : IBattleEvent
    {
        Task Handle(T battleEvent);
    }
}