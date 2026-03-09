using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Battles.Domain.Events;
using Pokemon_Battle_Clone.Runtime.Battles.Infrastructure.Dialogs;

namespace Pokemon_Battle_Clone.Runtime.Battles.Control.EventHandlers
{
    public class FaintedEventHandler : IBattleEventHandler<FaintedEvent>
    {
        private readonly IBattleContext _battleContext;
        private readonly IDialogDisplay _dialogDisplayer;

        public FaintedEventHandler(IBattleContext battleContext, IDialogDisplay dialogDisplayer)
        {
            _battleContext = battleContext;
            _dialogDisplayer = dialogDisplayer;
        }
        
        public async Task Handle(FaintedEvent battleEvent)
        {
            var view = _battleContext.GetTeamView(battleEvent.Side);

            await view.PlayFaintAnimation();
            view.SetPokemonAsDefeated(battleEvent.PokemonID);
            await _dialogDisplayer.DisplayAsync($"{battleEvent.PokemonName} fainted!");
        }
    }
}