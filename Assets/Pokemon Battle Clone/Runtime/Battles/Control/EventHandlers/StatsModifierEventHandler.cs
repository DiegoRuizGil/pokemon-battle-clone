using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Battles.Domain.Events;
using Pokemon_Battle_Clone.Runtime.Battles.Infrastructure.Dialogs;
using Pokemon_Battle_Clone.Runtime.Stats.Domain;

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
            var view = battleEvent.ApplyToTarget ? 
                _battleContext.GetTeamView(battleEvent.ActionSide.Opposite())
                : _battleContext.GetTeamView(battleEvent.ActionSide);
            view.SetStatModifier(battleEvent.Modifier);
            await DisplayMessages(battleEvent.Modifier);
        }

        private async Task DisplayMessages(StatsModifier modifier)
        {
            if (modifier.AttackLevel != 0)
                await _dialogDisplayer.DisplayAsync(GetMessage("Attack", modifier.AttackLevel));
            if (modifier.DefenseLevel != 0)
                await _dialogDisplayer.DisplayAsync(GetMessage("Defense", modifier.DefenseLevel));
            if (modifier.SpcAttackLevel != 0)
                await _dialogDisplayer.DisplayAsync(GetMessage("Sp. Atk", modifier.SpcAttackLevel));
            if (modifier.SpcDefenseLevel != 0)
                await _dialogDisplayer.DisplayAsync(GetMessage("Sp. Defense", modifier.SpcDefenseLevel));
            if (modifier.SpeedLevel != 0)
                await _dialogDisplayer.DisplayAsync(GetMessage("Speed", modifier.SpeedLevel));
        }

        private string GetMessage(string statistic, int level)
        {
            if (level == 1)
                return $"{statistic} rose!";
            if (level == 2)
                return $"{statistic} rose sharply!";
            if (level >= 3)
                return $"{statistic} rose drastically!";
            if (level == -1)
                return $"{statistic} fell!";
            if (level == -2)
                return $"{statistic} harshly fell!";
            if (level <= -3)
                return $"{statistic} severely fell";
            
            return string.Empty;
        }
    }
}