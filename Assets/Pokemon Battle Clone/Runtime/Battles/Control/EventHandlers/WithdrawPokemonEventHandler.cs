using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Battles.Domain.Events;
using Pokemon_Battle_Clone.Runtime.Battles.Infrastructure.Dialogs;

namespace Pokemon_Battle_Clone.Runtime.Battles.Control.EventHandlers
{
    public class WithdrawPokemonEventHandler : IBattleEventHandler<WithdrawPokemonEvent>
    {
        private readonly IBattleContext _battleContext;
        private readonly IDialogDisplay _dialogDisplayer;

        public WithdrawPokemonEventHandler(IBattleContext battleContext, IDialogDisplay dialogDisplayer)
        {
            _battleContext = battleContext;
            _dialogDisplayer = dialogDisplayer;
        }
        
        public async Task Handle(WithdrawPokemonEvent battleEvent)
        {
            await _dialogDisplayer.DisplayAsync($"Withdrawing {battleEvent.PokemonName} from side {battleEvent.ActionSide}");
            var view = _battleContext.GetTeamView(battleEvent.ActionSide);
            await view.PlayWithdrawAnimation();
        }
    }
}