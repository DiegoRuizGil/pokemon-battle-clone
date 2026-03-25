using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Core.Infrastructure;
using Pokemon_Battle_Clone.Runtime.CustomLogs;
using Pokemon_Battle_Clone.Runtime.Moves.Domain;
using Pokemon_Battle_Clone.Runtime.RNG;
using Pokemon_Battle_Clone.Runtime.Trainer.Domain.Actions;
using Pokemon_Battle_Clone.Runtime.Trainer.Domain.Strategies;
using Pokemon_Battle_Clone.Runtime.Trainer.Infrastructure.Actions;
using UnityEngine;
using LogManager = Pokemon_Battle_Clone.Runtime.CustomLogs.LogManager;

namespace Pokemon_Battle_Clone.Runtime.Core.Control
{
    public class BattleController : MonoBehaviour, IBattleContext
    {
        public TeamView playerTeamView;
        public TeamView rivalTeamView;
        public ActionsHUD actionsHUD;
        
        private Battle _battle;
        private readonly ActionsResolver _actionsResolver = new ActionsResolver();
        
        private TeamController _playerTeamController;
        private TeamController _rivalTeamController;
        
        private int _turnCount;
        private bool _battleFinished;

        // En este método estamos creando las clases colaboradoras, lo que podría ser extraído a una clase "Instaladora" para mejor separación de responsabilidades.
        private void Start()
        {
            var spriteLoader = new SpritesLoader("Assets/Pokemon Battle Clone/Sprites/Pokemon");
            
            var playerTeam = BuildPlayerTeam();
            var rivalTeam = BuildRivalTeam();
            
            _battle = new Battle(playerTeam, rivalTeam, new DefaultRandom(seed: DateTime.Now.GetHashCode()));
            
            var playerSprites = spriteLoader.LoadAllBack(playerTeam.PokemonList.Select(pokemon => pokemon.ID).ToList());
            _playerTeamController = new PlayerTeamController(playerTeam, playerSprites, playerTeamView, actionsHUD);
            
            var rivalSprites = spriteLoader.LoadAllFront(rivalTeam.PokemonList.Select(pokemon => pokemon.ID).ToList());
            var strategy = new BasicTrainerStrategy();
            _rivalTeamController = new RivalTeamController(rivalTeam, strategy, rivalSprites, rivalTeamView);

            // Para mencionar en sesión de asincronía: Este método debería ser esperado con await, ya que descartarla hará que no detectemos excepciones por ejemplo.
            // Start puede ser una Task, también se puede async void.
            _ = RunBattleAsync();
        }

        private async Task RunBattleAsync()
        {
            await _playerTeamController.Init();
            await _rivalTeamController.Init();
            
            LogManager.Log("Battle started!", FeatureType.Battle);
            
            while (!_battleFinished)
            {
                _turnCount++;
                LogManager.Log($"--- TURN {_turnCount} ---", FeatureType.Battle);

                await StartTurnAsync();
                // ¿Podríamos mover la obtención de actions a dentro del ExecuteActionsAsync? Así este método opera en un nivel un pelín más alto de abstracción.
                var actions = await SelectActionsAsync();
                await ExecuteActionsAsync(actions);
                await EndTurnAsync();

                _battleFinished = CheckBattleEnd();
            }
            
            LogManager.Log("Battle finished!", FeatureType.Battle);
        }

        private async Task StartTurnAsync()
        {
            LogManager.Log("Start turn...", FeatureType.Battle);
            await Task.Delay(500);
            
            var tasks = new List<Task<SwapPokemonAction>>();
            if (_playerTeamController.IsFirstPokemonDefeated)
                tasks.Add(_playerTeamController.SelectActionOfType<SwapPokemonAction>(forceSelection: true));
            if (_rivalTeamController.IsFirstPokemonDefeated)
                tasks.Add(_rivalTeamController.SelectActionOfType<SwapPokemonAction>(forceSelection: true));
            
            if (tasks.Count > 0)
            {
                await Task.WhenAll(tasks);
                foreach (var task in tasks)
                {
                    var result = task.Result.Execute(_battle);
                    await _actionsResolver.Resolve(result, this);
                }
            }
        }

        private async Task<List<TrainerAction>> SelectActionsAsync()
        {
            LogManager.Log("Selecting actions...", FeatureType.Battle);
            var playerActionTask = _playerTeamController.SelectActionTask();
            var rivalActionTask = _rivalTeamController.SelectActionTask();

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
                var result = action.Execute(_battle);
                await _actionsResolver.Resolve(result, this);
            }
        }

        private async Task EndTurnAsync()
        {
            LogManager.Log("End turn...", FeatureType.Battle);
            await Task.Delay(500);
        }

        private bool CheckBattleEnd()
        {
            if (_playerTeamController.Defeated)
            {
                LogManager.Log("The rival has won!", FeatureType.Battle);
                return true;
            }
            if (_rivalTeamController.Defeated)
            {
                LogManager.Log("The player has won!", FeatureType.Battle);
                return true;
            }

            return false;
        }

        public TeamController GetTeam(Side side)
        {
            return side switch
            {
                Side.Player => _playerTeamController,
                Side.Rival => _rivalTeamController,
                _ => null
            };
        }
        
        public TeamController GetOpponentTeam(Side side)
        {
            return side switch
            {
                Side.Player => _rivalTeamController,
                Side.Rival => _playerTeamController,
                _ => null
            };
        }

// ¿Qué significa esta region? ¿Qué es algo provisional?
#region DEBUG
        private Team BuildPlayerTeam()
        {
            var totodile = PokemonFactory.Totodile();
            totodile.MoveSet.AddMoves(new List<Move>
            {
                MoveFactory.IceFang(),
                MoveFactory.WaterGun(),
                MoveFactory.QuickAttack(),
                MoveFactory.Leer()
            });
            var pidgey = PokemonFactory.Pidgey();
            pidgey.MoveSet.AddMoves(new List<Move>
            {
                MoveFactory.WingAttack(),
                MoveFactory.QuickAttack()
            });

            var team = new Team(new List<Pokemon> { totodile, pidgey });
            LogManager.Log($"Player team: {team}", FeatureType.Action);
            
            return team;
        }

        private Team BuildRivalTeam()
        {
            var pidgey = PokemonFactory.Pidgey();
            pidgey.MoveSet.AddMoves(new List<Move>
            {
                MoveFactory.WingAttack(),
                MoveFactory.MegaNerf(),
            });
            var totodile = PokemonFactory.Totodile();
            totodile.MoveSet.AddMoves(new List<Move>
            {
                MoveFactory.WingAttack()
            });

            var team = new Team(new List<Pokemon> { pidgey, totodile });
            LogManager.Log($"Rival team: {team}", FeatureType.Action);

            return team;
        }
#endregion
    }
}
