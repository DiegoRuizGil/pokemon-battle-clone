using System;
using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Battles.Domain.Events;
using Pokemon_Battle_Clone.Runtime.Battles.Infrastructure.Dialogs;
using Pokemon_Battle_Clone.Runtime.CustomLogs;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain.Actions;
using LogManager = Pokemon_Battle_Clone.Runtime.CustomLogs.LogManager;

namespace Pokemon_Battle_Clone.Runtime.Battles.Control
{
    public class ActionsResolver
    {
        private readonly IBattleContext _battleContext;
        private readonly IDialogDisplay _dialogDisplayer;

        public ActionsResolver(IBattleContext battleContext, IDialogDisplay dialogDisplayer)
        {
            _battleContext = battleContext;
            _dialogDisplayer = dialogDisplayer;
        }
        
        public async Task Resolve(Battle battle, TrainerAction action)
        {
            var events = action.Execute(battle);
            foreach (var battleEvent in events)
                await HandleEvent(battleEvent);
        }

        public async Task HandleEvent(IBattleEvent battleEvent)
        {
            await (battleEvent switch
            {
                EmptyEvent _ => Task.CompletedTask,
                ExecuteMoveEvent moveEvent => HandleExecuteMoveEvent(moveEvent),
                FailedMoveEvent failedMoveEvent => HandleFailedMoveEvent(failedMoveEvent),
                DamageEvent damageEvent => HandleDamageEvent(damageEvent),
                StatsModifierEvent statsEvent => HandleStatsModifierEvent(statsEvent),
                SendPokemonEvent sendEvent => HandleSendPokemonEvent(sendEvent),
                WithdrawPokemonEvent withdrawEvent => HandleWithdrawPokemonEvent(withdrawEvent),
                FaintedEvent faintedEvent => HandleFaintedEvent(faintedEvent),
                _ => HandleUnsupportedEvent(battleEvent)
            });
        }

        private async Task HandleExecuteMoveEvent(ExecuteMoveEvent moveEvent)
        {
            var view = _battleContext.GetTeamView(moveEvent.ActionSide);
            
            _dialogDisplayer.Display($"{moveEvent.PokemonName} used {moveEvent.MoveName}!");
            
            await view.PlayAttackAnimation();
            // add move animation here
            await Task.Delay(TimeSpan.FromSeconds(1));
            
            _dialogDisplayer.Close();
        }

        private async Task HandleFailedMoveEvent(FailedMoveEvent failedMoveEvent)
        {
            await _dialogDisplayer.DisplayAsync($"{failedMoveEvent.PokemonName} failed to use {failedMoveEvent.MoveName}!");
        }

        private async Task HandleDamageEvent(DamageEvent damageEvent)
        {
            var view = _battleContext.GetOpponentTeamView(damageEvent.ActionSide);
            
            view.UpdateHealth(max: damageEvent.TargetHealth.Max, current: damageEvent.TargetHealth.Current, animated: true);
            await view.PlayHitAnimation();
            
            if (damageEvent.Effectiveness > 1f)
                await _dialogDisplayer.DisplayAsync("It was super effective!");
            else if (damageEvent.Effectiveness < 1f)
                await _dialogDisplayer.DisplayAsync("It was not very effective...");
        }

        private async Task HandleStatsModifierEvent(StatsModifierEvent statsEvent)
        {
            var view = _battleContext.GetOpponentTeamView(statsEvent.ActionSide);
            view.SetStatModifier(statsEvent.Modifier);
        }

        private async Task HandleSendPokemonEvent(SendPokemonEvent sendEvent)
        {
            if (sendEvent.ActionSide == Side.Player)
                await _dialogDisplayer.DisplayAsync($"Go ahead, {sendEvent.Pokemon.Name}!");
            else if (sendEvent.ActionSide == Side.Rival)
                await _dialogDisplayer.DisplayAsync($"The opponent brings out {sendEvent.Pokemon.Name}!");
            
            var view = _battleContext.GetTeamView(sendEvent.ActionSide);
            await view.SendPokemon(sendEvent.Pokemon);
        }

        private async Task HandleWithdrawPokemonEvent(WithdrawPokemonEvent withdraw)
        {
            await _dialogDisplayer.DisplayAsync($"Withdrawing {withdraw.PokemonName} from side {withdraw.ActionSide}");
            var view = _battleContext.GetTeamView(withdraw.ActionSide);
            await view.PlayWithdrawAnimation();
        }

        private async Task HandleFaintedEvent(FaintedEvent faintedEvent)
        {
            var view = _battleContext.GetTeamView(faintedEvent.Side);

            await view.PlayFaintAnimation();
            await _dialogDisplayer.DisplayAsync($"{faintedEvent.PokemonName} fainted!");
        }

        private async Task HandleUnsupportedEvent(IBattleEvent battleEvent)
        {
            LogManager.LogError($"Unsupported battle event of type {battleEvent.GetType().Name}", FeatureType.Action);
            await Task.CompletedTask;
        }
    }
}