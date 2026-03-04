using System.Collections.Generic;
using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.CustomLogs;
using Pokemon_Battle_Clone.Runtime.Trainer.Domain.BattleEvents;
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
            var userTeam = _battleContext.GetTeam(moveEvent.ActionSide);
            
            LogManager.Log($"{moveEvent.PokemonName} used the move {moveEvent.MoveName}", FeatureType.Action);
            await userTeam.View.PlayAttackAnimation();
            
            // add move animation here
        }

        private async Task HandleDamage(DamageEvent damageEvent)
        {
            var rivalTeam = _battleContext.GetOpponentTeam(damageEvent.ActionSide);
            
            rivalTeam.View.UpdateHealth(max: damageEvent.TargetHealth.Max, current: damageEvent.TargetHealth.Current, animated: true);
            if (damageEvent.TargetHealth.Current == 0)
                await rivalTeam.View.PlayFaintAnimation();
            else
                await rivalTeam.View.PlayHitAnimation();
        }

        private async Task HandleStatsModifierEvent(StatsModifierEvent statsEvent)
        {
            var rivalTeam = _battleContext.GetOpponentTeam(statsEvent.ActionSide);
            rivalTeam.SetStatsModifier();
        }

        private async Task HandleSendPokemonEvent(SendPokemonEvent sendEvent)
        {
            LogManager.Log($"Sending {sendEvent.PokemonName} from side {sendEvent.ActionSide}", FeatureType.Action);
            var team = _battleContext.GetTeam(sendEvent.ActionSide);
            // await team.View.SendPokemon(sendEvent.Pokemon, sprite: ...);
            await team.SendFirstPokemon();
        }

        private async Task HandleWithdrawPokemonEvent(WithdrawPokemon withdraw)
        {
            LogManager.Log($"Withdrawing {withdraw.PokemonName} from side {withdraw.ActionSide}", FeatureType.Action);
            var team = _battleContext.GetTeam(withdraw.ActionSide);
            await team.View.PlayWithdrawAnimation();
        }
    }
}