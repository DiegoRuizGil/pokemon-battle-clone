using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Battles.Domain.Events;
using Pokemon_Battle_Clone.Runtime.Trainers.Control;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain.Actions;
using Pokemon_Battle_Clone.Runtime.Trainers.Infrastructure.Actions;

namespace Pokemon_Battle_Clone.Runtime.Battles.Control
{
    public class Turn
    {
        private readonly ActionsResolver _actionsResolver;
        private readonly IActionHUD _actionsHUD;
        private int _count;

        public Turn(ActionsResolver actionsResolver, IActionHUD actionsHUD)
        {
            _actionsResolver = actionsResolver;
            _actionsHUD = actionsHUD;
        }

        public async Task Init(Battle battle, Trainer player, Trainer rival) // not entirely convinced by this approach
        {
            _actionsHUD.Hide();
            
            await _actionsResolver.Resolve(battle, rival.Init());
            await _actionsResolver.Resolve(battle, player.Init());
        }
        
        public async Task Next(Battle battle, Trainer player, Trainer rival)
        {
            _count++;

            await StartTurnAsync(battle, player, rival);
            var actions = await SelectActionsAsync(battle, player, rival);
            await ExecuteActionsAsync(battle, actions);
        }
        
        private async Task StartTurnAsync(Battle battle, Trainer player, Trainer rival)
        {
            var tasks = new List<Task<TrainerAction>>();
            if (player.IsFirstPokemonDefeated)
                tasks.Add(player.SelectSwapPokemon(battle));
            if (rival.IsFirstPokemonDefeated)
                tasks.Add(rival.SelectSwapPokemon(battle));
            
            if (tasks.Count > 0)
            {
                await Task.WhenAll(tasks);
                await ExecuteActionsAsync(battle, tasks.Select(x => x.Result).ToList());
            }
        }
        
        private async Task<List<TrainerAction>> SelectActionsAsync(Battle battle, Trainer player, Trainer rival)
        {
            var playerActionTask = player.SelectAction(battle);
            var rivalActionTask = rival.SelectAction(battle);
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

                // I need to think of another approach 
                await CheckForFaintedPokemon(battle, Side.Player);
                await CheckForFaintedPokemon(battle, Side.Rival);
            }
        }

        private async Task CheckForFaintedPokemon(Battle battle, Side side)
        {
            if (battle.PokemonFainted(side))
            {
                var faintedPokemon = battle.GetFirstPokemon(side);
                await _actionsResolver.HandleEvent(new FaintedEvent(side, faintedPokemon.Name, faintedPokemon.ID));
            }
        }
    }
}