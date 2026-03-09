using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Battles.Domain.Events;
using Pokemon_Battle_Clone.Runtime.Battles.Infrastructure.Dialogs;

namespace Pokemon_Battle_Clone.Runtime.Battles.Control.EventHandlers
{
    public class SendPokemonEventHandler : IBattleEventHandler<SendPokemonEvent>
    {
        private readonly IBattleContext _battleContext;
        private readonly IDialogDisplay _dialogDisplayer;

        public SendPokemonEventHandler(IBattleContext battleContext, IDialogDisplay dialogDisplayer)
        {
            _battleContext = battleContext;
            _dialogDisplayer = dialogDisplayer;
        }
        
        public async Task Handle(SendPokemonEvent battleEvent)
        {
            if (battleEvent.ActionSide == Side.Player)
                await _dialogDisplayer.DisplayAsync($"Go ahead, {battleEvent.Pokemon.Name}!");
            else if (battleEvent.ActionSide == Side.Rival)
                await _dialogDisplayer.DisplayAsync($"The opponent brings out {battleEvent.Pokemon.Name}!");
            
            var view = _battleContext.GetTeamView(battleEvent.ActionSide);
            await view.SendPokemon(battleEvent.Pokemon);
        }
    }
}