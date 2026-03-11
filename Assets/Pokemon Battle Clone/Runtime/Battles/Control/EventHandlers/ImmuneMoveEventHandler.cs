using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Battles.Domain.Events;
using Pokemon_Battle_Clone.Runtime.Battles.Infrastructure.Dialogs;

namespace Pokemon_Battle_Clone.Runtime.Battles.Control.EventHandlers
{
    public class ImmuneMoveEventHandler : IBattleEventHandler<ImmuneMoveEvent>
    {
        private readonly IDialogDisplay _dialogDisplayer;

        public ImmuneMoveEventHandler(IDialogDisplay dialogDisplayer)
        {
            _dialogDisplayer = dialogDisplayer;
        }
        
        public async Task Handle(ImmuneMoveEvent battleEvent)
        {
            await _dialogDisplayer.DisplayAsync($"It doesn't affect {battleEvent.TargetName}...");
        }
    }
}