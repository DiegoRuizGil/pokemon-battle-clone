using System.Collections.Generic;
using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Battles.Domain.Events;
using Pokemon_Battle_Clone.Runtime.CustomLogs;
using Pokemon_Battle_Clone.Runtime.Trainers.Control;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain.Actions;

namespace Pokemon_Battle_Clone.Runtime.Battles.Control
{
    public class Turn
    {
        private readonly ActionsResolver _actionsResolver;
        private int _count;

        public Turn(IBattleContext battleContext)
        {
            _actionsResolver = new ActionsResolver(battleContext);
        }

        public async Task Init(Battle battle, Trainer player, Trainer rival) // not entirely convinced by this approach
        {
            await _actionsResolver.Resolve(battle, player.Init());
            await _actionsResolver.Resolve(battle, rival.Init());
        }
        
        public async Task Next(Battle battle, Trainer player, Trainer rival)
        {
            _count++;
            LogManager.Log($"--- TURN {_count} ---", FeatureType.Battle);

            await StartTurnAsync(battle, player, rival);
            var actions = await SelectActionsAsync(player, rival);
            await ExecuteActionsAsync(battle, actions);
            await EndTurnAsync();
        }
        
        private async Task StartTurnAsync(Battle battle, Trainer player, Trainer rival)
        {
            LogManager.Log("Start turn...", FeatureType.Battle);
            
            var tasks = new List<Task<SwapPokemonAction>>();
            if (player.IsFirstPokemonDefeated)
                tasks.Add(player.SelectActionOfType<SwapPokemonAction>(forceSelection: true));
            if (rival.IsFirstPokemonDefeated)
                tasks.Add(rival.SelectActionOfType<SwapPokemonAction>(forceSelection: true));
            
            if (tasks.Count > 0)
            {
                await Task.WhenAll(tasks);
                foreach (var task in tasks)
                    await _actionsResolver.Resolve(battle, task.Result);
            }
        }
        
        private async Task<List<TrainerAction>> SelectActionsAsync(Trainer player, Trainer rival)
        {
            LogManager.Log("Selecting actions...", FeatureType.Battle);
            var playerActionTask = player.SelectActionTask();
            var rivalActionTask = rival.SelectActionTask();

            await Task.WhenAll(playerActionTask, rivalActionTask);
            
            return new List<TrainerAction> { playerActionTask.Result, rivalActionTask.Result };
        }
        
        private async Task ExecuteActionsAsync(Battle battle, List<TrainerAction> actions)
        {
            var orderedActions = battle.OrderActions(actions);

            foreach (var action in orderedActions)
            {
                if (battle.PokemonFainted(action.Side))
                    continue;
                await _actionsResolver.Resolve(battle, action);
            }
        }

        private async Task EndTurnAsync()
        {
            LogManager.Log("End turn...", FeatureType.Battle);
        }
    }
}