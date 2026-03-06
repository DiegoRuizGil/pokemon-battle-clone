using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Core.Infrastructure;
using Pokemon_Battle_Clone.Runtime.CustomLogs;
using Pokemon_Battle_Clone.Runtime.Moves.Domain;
using Pokemon_Battle_Clone.Runtime.RNG;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain.Actions;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain.BattleEvents;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain.Strategies;
using Pokemon_Battle_Clone.Runtime.Trainers.Infrastructure.Actions;
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
        private ActionsResolver _actionsResolver;
        
        private Trainer _playerTrainer;
        private Trainer _rivalTrainer;
        
        private int _turnCount;
        private bool _battleFinished;
        
        private void Start()
        {
            _actionsResolver = new ActionsResolver(this);
            
            var spriteLoader = new SpritesLoader("Assets/Pokemon Battle Clone/Sprites/Pokemon");
            
            var playerTeam = BuildPlayerTeam();
            var rivalTeam = BuildRivalTeam();
            
            _battle = new Battle(playerTeam, rivalTeam, new DefaultRandom(seed: DateTime.Now.GetHashCode()));
            
            var playerSprites = spriteLoader.LoadAllBack(playerTeam.PokemonList.Select(pokemon => pokemon.ID).ToList());
            _playerTrainer = new PlayerTrainer(playerTeam, actionsHUD);
            playerTeamView.Init(playerSprites);
            
            var rivalSprites = spriteLoader.LoadAllFront(rivalTeam.PokemonList.Select(pokemon => pokemon.ID).ToList());
            var strategy = new BasicTrainerStrategy();
            _rivalTrainer = new RivalTrainer(rivalTeam, strategy);
            rivalTeamView.Init(rivalSprites);

            _ = RunBattleAsync();
        }

        private async Task RunBattleAsync()
        {
            var playerEvents = _playerTrainer.Init();
            await _actionsResolver.Resolve(new Queue<IBattleEvent>(playerEvents));
            var rivalEvents = _rivalTrainer.Init();
            await _actionsResolver.Resolve(new Queue<IBattleEvent>(rivalEvents));
            
            LogManager.Log("Battle started!", FeatureType.Battle);
            
            while (!_battleFinished)
            {
                _turnCount++;
                LogManager.Log($"--- TURN {_turnCount} ---", FeatureType.Battle);

                await StartTurnAsync();
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
            if (_playerTrainer.IsFirstPokemonDefeated)
                tasks.Add(_playerTrainer.SelectActionOfType<SwapPokemonAction>(forceSelection: true));
            if (_rivalTrainer.IsFirstPokemonDefeated)
                tasks.Add(_rivalTrainer.SelectActionOfType<SwapPokemonAction>(forceSelection: true));
            
            if (tasks.Count > 0)
            {
                await Task.WhenAll(tasks);
                foreach (var task in tasks)
                {
                    var eventSequence = task.Result.Execute(_battle);
                    await _actionsResolver.Resolve(new Queue<IBattleEvent>(eventSequence));
                }
            }
        }

        private async Task<List<TrainerAction>> SelectActionsAsync()
        {
            LogManager.Log("Selecting actions...", FeatureType.Battle);
            var playerActionTask = _playerTrainer.SelectActionTask();
            var rivalActionTask = _rivalTrainer.SelectActionTask();

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
                var eventSequence = action.Execute(_battle);
                await _actionsResolver.Resolve(new Queue<IBattleEvent>(eventSequence));
            }
        }

        private async Task EndTurnAsync()
        {
            LogManager.Log("End turn...", FeatureType.Battle);
            await Task.Delay(500);
        }

        private bool CheckBattleEnd()
        {
            if (_playerTrainer.Defeated)
            {
                LogManager.Log("The rival has won!", FeatureType.Battle);
                return true;
            }
            if (_rivalTrainer.Defeated)
            {
                LogManager.Log("The player has won!", FeatureType.Battle);
                return true;
            }

            return false;
        }

        public ITeamView GetTeamView(Side side)
        {
            return side switch
            {
                Side.Player => playerTeamView,
                Side.Rival => rivalTeamView,
                _ => null
            };
        }
        
        public ITeamView GetOpponentTeamView(Side side)
        {
            return side switch
            {
                Side.Player => rivalTeamView,
                Side.Rival => playerTeamView,
                _ => null
            };
        }

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