using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Battles.Control.EventHandlers;
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
        private readonly Dictionary<Type, Func<IBattleEvent, Task>> _handlers = new();

        public ActionsResolver(IBattleContext battleContext, IDialogDisplay dialogDisplayer)
        {
            RegisterHandler(new EmptyEventHandler());
            RegisterHandler(new ExecuteMoveEventHandler(battleContext, dialogDisplayer));
            RegisterHandler(new FailedMoveEventHandler(dialogDisplayer));
            RegisterHandler(new DamageEventHandler(battleContext, dialogDisplayer));
            RegisterHandler(new StatsModifierEventHandler(battleContext, dialogDisplayer));
            RegisterHandler(new SendPokemonEventHandler(battleContext, dialogDisplayer));
            RegisterHandler(new WithdrawPokemonEventHandler(battleContext, dialogDisplayer));
            RegisterHandler(new FaintedEventHandler(battleContext, dialogDisplayer));
            RegisterHandler(new ImmuneMoveEventHandler(dialogDisplayer));
        }

        private void RegisterHandler<T>(IBattleEventHandler<T> handler) where T : IBattleEvent
        {
            _handlers[typeof(T)] = battleEvent => handler.Handle((T)battleEvent);
        }
        
        public async Task Resolve(Battle battle, TrainerAction action)
        {
            var events = action.Execute(battle);
            foreach (var battleEvent in events)
                await HandleEvent(battleEvent);
        }

        public async Task HandleEvent(IBattleEvent battleEvent)
        {
            if (_handlers.TryGetValue(battleEvent.GetType(), out var handler))
            {
                await handler(battleEvent);
            }
            else
            {
                LogManager.LogError($"Unsupported battle event of type {battleEvent.GetType().Name}", FeatureType.Action);
                await Task.CompletedTask;
            }
        }
    }
}