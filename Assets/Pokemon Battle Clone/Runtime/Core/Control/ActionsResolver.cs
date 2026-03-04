using System.Collections.Generic;
using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.CustomLogs;
using Pokemon_Battle_Clone.Runtime.Trainer.Domain.BattleEvents;
using LogManager = Pokemon_Battle_Clone.Runtime.CustomLogs.LogManager;

namespace Pokemon_Battle_Clone.Runtime.Core.Control
{
    public class ActionsResolver
    {
        public async Task Resolve(Queue<IBattleEvent> events, IBattleContext context)
        {
            while (events.Count > 0)
            {
                var battleEvent = events.Dequeue();

                await (battleEvent switch
                {
                    DamageEvent damageEvent => HandleDamage(damageEvent, context),
                    StatsModifierEvent statsEvent => HandleStatsModifierEvent(statsEvent, context),
                    SwapPokemonEvent swapEvent => HandleSwapPokemonEvent(swapEvent, context),
                    _ => Task.CompletedTask
                });
            }
        }

        private async Task HandleDamage(DamageEvent damageEvent, IBattleContext context)
        {
            var userTeam = context.GetTeam(damageEvent.ActionSide);
            var rivalTeam = context.GetOpponentTeam(damageEvent.ActionSide);

            LogManager.Log($"{damageEvent.UserName} attacked {damageEvent.TargetName}", FeatureType.Action);
            await userTeam.View.PlayAttackAnimation();
            
            // add move animation here
            
            rivalTeam.View.UpdateHealth(max: damageEvent.TargetHealth.Max, current: damageEvent.TargetHealth.Current, animated: true);
            if (damageEvent.TargetHealth.Current == 0)
                await rivalTeam.View.PlayFaintAnimation();
            else
                await rivalTeam.View.PlayHitAnimation();
        }

        private async Task HandleStatsModifierEvent(StatsModifierEvent statsEvent, IBattleContext context)
        {
            var userTeam = context.GetTeam(statsEvent.ActionSide);
            var rivalTeam = context.GetOpponentTeam(statsEvent.ActionSide);

            LogManager.Log($"{statsEvent.UserName} attacked {statsEvent.TargetName}", FeatureType.Action);
            await userTeam.View.PlayAttackAnimation();
            
            rivalTeam.SetStatsModifier();
        }

        private async Task HandleSwapPokemonEvent(SwapPokemonEvent swapEvent, IBattleContext context)
        {
            LogManager.Log($"Sending {swapEvent.PokemonName} from side {swapEvent.ActionSide}", FeatureType.Action);
            var userTeam = context.GetTeam(swapEvent.ActionSide);
            await userTeam.SendFirstPokemon();
        }
    }
}