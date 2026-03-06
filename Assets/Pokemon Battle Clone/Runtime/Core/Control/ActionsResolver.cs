using System.Collections.Generic;
using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.CustomLogs;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain.BattleEvents;
using LogManager = Pokemon_Battle_Clone.Runtime.CustomLogs.LogManager;

namespace Pokemon_Battle_Clone.Runtime.Core.Control
{
    public class ActionsResolver
    {
        private readonly IBattleContext _battleContext;

        public ActionsResolver(IBattleContext battleContext)
        {
            _battleContext = battleContext;
        }
        
        public async Task Resolve(Queue<IBattleEvent> events)
        {
            while (events.Count > 0)
            {
                var battleEvent = events.Dequeue();

                await (battleEvent switch
                {
                    ExecuteMoveEvent moveEvent => HandleExecuteMoveEvent(moveEvent),
                    DamageEvent damageEvent => HandleDamage(damageEvent),
                    StatsModifierEvent statsEvent => HandleStatsModifierEvent(statsEvent),
                    SendPokemonEvent sendEvent => HandleSendPokemonEvent(sendEvent),
                    WithdrawPokemon withdrawEvent => HandleWithdrawPokemonEvent(withdrawEvent),
                    _ => Task.CompletedTask
                });
            }
        }

        private async Task HandleExecuteMoveEvent(ExecuteMoveEvent moveEvent)
        {
            var view = _battleContext.GetTeamView(moveEvent.ActionSide);
            
            LogManager.Log($"{moveEvent.PokemonName} used the move {moveEvent.MoveName}", FeatureType.Action);
            await view.PlayAttackAnimation();
            
            // add move animation here
        }

        private async Task HandleDamage(DamageEvent damageEvent)
        {
            var view = _battleContext.GetOpponentTeamView(damageEvent.ActionSide);
            
            view.UpdateHealth(max: damageEvent.TargetHealth.Max, current: damageEvent.TargetHealth.Current, animated: true);
            if (damageEvent.TargetHealth.Current == 0)
                await view.PlayFaintAnimation();
            else
                await view.PlayHitAnimation();
        }

        private async Task HandleStatsModifierEvent(StatsModifierEvent statsEvent)
        {
            var view = _battleContext.GetOpponentTeamView(statsEvent.ActionSide);
            view.SetStatModifier(statsEvent.Modifier);
        }

        private async Task HandleSendPokemonEvent(SendPokemonEvent sendEvent)
        {
            LogManager.Log($"Sending {sendEvent.Pokemon.Name} from side {sendEvent.ActionSide}", FeatureType.Action);
            var view = _battleContext.GetTeamView(sendEvent.ActionSide);
            await view.SendPokemon(sendEvent.Pokemon);
        }

        private async Task HandleWithdrawPokemonEvent(WithdrawPokemon withdraw)
        {
            LogManager.Log($"Withdrawing {withdraw.PokemonName} from side {withdraw.ActionSide}", FeatureType.Action);
            var view = _battleContext.GetTeamView(withdraw.ActionSide);
            await view.PlayWithdrawAnimation();
        }
    }
}