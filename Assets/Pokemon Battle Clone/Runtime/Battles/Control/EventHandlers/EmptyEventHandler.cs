using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Battles.Domain.Events;

namespace Pokemon_Battle_Clone.Runtime.Battles.Control.EventHandlers
{
    public class EmptyEventHandler : IBattleEventHandler<EmptyEvent>
    {
        public async Task Handle(EmptyEvent battleEvent) => await Task.CompletedTask;
    }
}