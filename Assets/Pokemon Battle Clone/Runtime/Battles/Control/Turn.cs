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
        private readonly Battle _battle;
        private readonly Trainer _playerTrainer;
        private readonly Trainer _rivalTrainer;
        private int _count;

        public Turn(ActionsResolver actionsResolver, IActionHUD actionsHUD, Battle battle, Trainer playerTrainer, Trainer rivalTrainer)
        {
            _actionsResolver = actionsResolver;
            _actionsHUD = actionsHUD;
            _battle = battle;
            _playerTrainer = playerTrainer;
            _rivalTrainer = rivalTrainer;
        }

        public async Task Init() // not entirely convinced by this approach
        {
            _actionsHUD.Hide();
            
            await _actionsResolver.Resolve(_battle, _rivalTrainer.Init());
            await _actionsResolver.Resolve(_battle, _playerTrainer.Init());
        }
        
        public async Task Next()
        {
            _count++;

            await StartTurnAsync();
            await FinishTurnAsync();
        }

        private async Task StartTurnAsync()
        {
            var actions = await SelectPreTurnActions();
            await ExecuteActionsAsync(actions);
        }

        private async Task FinishTurnAsync()
        {
            var actions = await SelectActionsAsync();
            await ExecuteActionsAsync(actions);
        }

        private async Task<List<TrainerAction>> SelectPreTurnActions()
        {
            var tasks = new List<Task<TrainerAction>>();
            
            if (_playerTrainer.IsFirstPokemonDefeated)
                tasks.Add(_playerTrainer.SelectSwapPokemon());
            if (_rivalTrainer.IsFirstPokemonDefeated)
                tasks.Add(_rivalTrainer.SelectSwapPokemon());

            await Task.WhenAll(tasks);
            return tasks.Select(x => x.Result).ToList();
        }
        
        private async Task<List<TrainerAction>> SelectActionsAsync()
        {
            var playerActionTask = _playerTrainer.SelectAction();
            var rivalActionTask = _rivalTrainer.SelectAction();
            await Task.WhenAll(playerActionTask, rivalActionTask);
            
            return new List<TrainerAction> { playerActionTask.Result, rivalActionTask.Result };
        }
        
        private async Task ExecuteActionsAsync(List<TrainerAction> actions)
        {
            var orderedActions = _battle.OrderActions(actions);

            foreach (var action in orderedActions)
            {
                if (_battle.PokemonFainted(action.Side))
                    continue;
                await _actionsResolver.Resolve(_battle, action);

                // I need to think of another approach 
                await CheckForFaintedPokemon(Side.Player);
                await CheckForFaintedPokemon(Side.Rival);
            }
        }

        private async Task CheckForFaintedPokemon(Side side)
        {
            if (_battle.PokemonFainted(side))
            {
                var faintedPokemon = _battle.GetFirstPokemon(side);
                await _actionsResolver.HandleEvent(new FaintedEvent(side, faintedPokemon.Name, faintedPokemon.ID));
            }
        }
    }
}