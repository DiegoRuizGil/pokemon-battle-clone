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
            var pokemonName = battleEvent.ApplyToTarget ? battleEvent.TargetName : battleEvent.UserName;
            
            view.SetStatModifier(battleEvent.Modifier);
            await DisplayMessages(pokemonName, battleEvent.Modifier);
        }

        private async Task DisplayMessages(string pokemonName, StatsModifier modifier)
        {
            foreach (var stat in StatInfo.Battle)
            {
                var stage = modifier.GetStage(stat);
                if (stage != 0)
                {
                    var message = GetMessage(pokemonName, stat.FullName(), stage);
                    await _dialogDisplayer.DisplayAsync(message);
                }
            }
        }

        private string GetMessage(string pokemonName, string statistic, int stage)
        {
            var prefix = $"{pokemonName}'s {statistic}";
            if (stage == 1)
                return $"{prefix} rose!";
            if (stage == 2)
                return $"{prefix} rose sharply!";
            if (stage >= 3)
                return $"{prefix} rose drastically!";
            if (stage == -1)
                return $"{prefix} fell!";
            if (stage == -2)
                return $"{prefix} harshly fell!";
            if (stage <= -3)
                return $"{prefix} severely fell";
            
            return string.Empty;
        }
    }
}