using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Battles.Domain.Events;
using Pokemon_Battle_Clone.Runtime.Battles.Infrastructure.Dialogs;

namespace Pokemon_Battle_Clone.Runtime.Battles.Control.EventHandlers
{
    public class DamageEventHandler : IBattleEventHandler<DamageEvent>
    {
        private readonly IBattleContext _battleContext;
        private readonly IDialogDisplay _dialogDisplayer;

        public DamageEventHandler(IBattleContext battleContext, IDialogDisplay dialogDisplayer)
        {
            _battleContext = battleContext;
            _dialogDisplayer = dialogDisplayer;
        }
        
        public async Task Handle(DamageEvent battleEvent)
        {
            var view = _battleContext.GetTeamView(battleEvent.ActionSide.Opposite());
            
            view.UpdateHealth(max: battleEvent.TargetHealth.Max, current: battleEvent.TargetHealth.Current, animated: true);
            await view.PlayHitAnimation();
            
            if (battleEvent.Effectiveness > 1f)
                await _dialogDisplayer.DisplayAsync("It was super effective!");
            else if (battleEvent.Effectiveness < 1f)
                await _dialogDisplayer.DisplayAsync("It was not very effective...");
        }
    }
}