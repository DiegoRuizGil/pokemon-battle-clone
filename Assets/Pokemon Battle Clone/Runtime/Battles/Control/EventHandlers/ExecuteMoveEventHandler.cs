using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Battles.Domain.Events;
using Pokemon_Battle_Clone.Runtime.Battles.Infrastructure.Dialogs;

namespace Pokemon_Battle_Clone.Runtime.Battles.Control.EventHandlers
{
    public class ExecuteMoveEventHandler : IBattleEventHandler<ExecuteMoveEvent>
    {
        private readonly IBattleContext _battleContext;
        private readonly IDialogDisplay _dialogDisplayer;

        public ExecuteMoveEventHandler(IBattleContext battleContext, IDialogDisplay dialogDisplayer)
        {
            _battleContext = battleContext;
            _dialogDisplayer = dialogDisplayer;
        }
        
        public async Task Handle(ExecuteMoveEvent battleEvent)
        {
            var view = _battleContext.GetTeamView(battleEvent.ActionSide);
            
            await _dialogDisplayer.DisplayAsync($"{battleEvent.PokemonName} used {battleEvent.MoveName}!");
            await view.PlayAttackAnimation();
        }
    }
}