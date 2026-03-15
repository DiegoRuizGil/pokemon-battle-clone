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
            if (modifier.AttackLevel != 0)
                await _dialogDisplayer.DisplayAsync(GetMessage(pokemonName, "attack", modifier.AttackLevel));
            if (modifier.DefenseLevel != 0)
                await _dialogDisplayer.DisplayAsync(GetMessage(pokemonName, "defense", modifier.DefenseLevel));
            if (modifier.SpcAttackLevel != 0)
                await _dialogDisplayer.DisplayAsync(GetMessage(pokemonName, "sp. atk", modifier.SpcAttackLevel));
            if (modifier.SpcDefenseLevel != 0)
                await _dialogDisplayer.DisplayAsync(GetMessage(pokemonName, "sp. defense", modifier.SpcDefenseLevel));
            if (modifier.SpeedLevel != 0)
                await _dialogDisplayer.DisplayAsync(GetMessage(pokemonName, "speed", modifier.SpeedLevel));
        }

        private string GetMessage(string pokemonName, string statistic, int level)
        {
            var prefix = $"{pokemonName}'s {statistic}";
            if (level == 1)
                return $"{prefix} rose!";
            if (level == 2)
                return $"{prefix} rose sharply!";
            if (level >= 3)
                return $"{prefix} rose drastically!";
            if (level == -1)
                return $"{prefix} fell!";
            if (level == -2)
                return $"{prefix} harshly fell!";
            if (level <= -3)
                return $"{prefix} severely fell";
            
            return string.Empty;
        }
    }
}