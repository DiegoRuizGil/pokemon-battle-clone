using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Battles.Domain.Events;
using Pokemon_Battle_Clone.Runtime.Battles.Infrastructure.Dialogs;

namespace Pokemon_Battle_Clone.Runtime.Battles.Control.EventHandlers
{
    public class FailedMoveEventHandler : IBattleEventHandler<FailedMoveEvent>
    {
        private readonly IDialogDisplay _dialogDisplayer;

        public FailedMoveEventHandler(IDialogDisplay dialogDisplayer)
        {
            _dialogDisplayer = dialogDisplayer;
        }
        
        public async Task Handle(FailedMoveEvent battleEvent)
        {
            await _dialogDisplayer.DisplayAsync($"{battleEvent.PokemonName} failed to use {battleEvent.MoveName}!");
        }
    }
}