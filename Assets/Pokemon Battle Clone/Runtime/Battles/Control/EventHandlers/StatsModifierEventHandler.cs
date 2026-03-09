using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Battles.Domain.Events;
using Pokemon_Battle_Clone.Runtime.Battles.Infrastructure.Dialogs;

namespace Pokemon_Battle_Clone.Runtime.Battles.Control.EventHandlers
{
    public class StatsModifierEventHandler : IBattleEventHandler<StatsModifierEvent>
    {
        private readonly IBattleContext _battleContext;
        private readonly IDialogDisplay _dialogDisplayer;

        public StatsModifierEventHandler(IBattleContext battleContext, IDialogDisplay dialogDisplayer)
        {
            _battleContext = battleContext;
            _dialogDisplayer = dialogDisplayer;
        }

        public async Task Handle(StatsModifierEvent battleEvent)
        {
            var view = _battleContext.GetOpponentTeamView(battleEvent.ActionSide);
            view.SetStatModifier(battleEvent.Modifier);
        }
    }
}