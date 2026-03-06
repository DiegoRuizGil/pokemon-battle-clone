using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Core.Infrastructure;
using Pokemon_Battle_Clone.Runtime.CustomLogs;
using Pokemon_Battle_Clone.Runtime.Moves.Domain;
using Pokemon_Battle_Clone.Runtime.RNG;
using Pokemon_Battle_Clone.Runtime.Trainers.Control;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain.Strategies;
using Pokemon_Battle_Clone.Runtime.Trainers.Infrastructure.Actions;
using UnityEngine;
using LogManager = Pokemon_Battle_Clone.Runtime.CustomLogs.LogManager;

namespace Pokemon_Battle_Clone.Runtime.Battles.Control
{
    public class BattleController : MonoBehaviour, IBattleContext
    {
        public TeamView playerTeamView;
        public TeamView rivalTeamView;
        public ActionsHUD actionsHUD;
        
        private Battle _battle;
        private Turn _turn;

        private Trainer _playerTrainer;
        private Trainer _rivalTrainer;
        
        private int _turnCount;
        private bool _battleFinished;
        
        private void Start()
        {
            var spriteLoader = new SpritesLoader("Assets/Pokemon Battle Clone/Sprites/Pokemon");
            
            var playerTeam = BuildPlayerTeam();
            var rivalTeam = BuildRivalTeam();
            
            _battle = new Battle(playerTeam, rivalTeam, new DefaultRandom(seed: DateTime.Now.GetHashCode()));
            _turn = new Turn(this);
            
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
            await _turn.Init(_battle, _playerTrainer, _rivalTrainer);
            
            LogManager.Log("Battle started!", FeatureType.Battle);
            
            while (!_battleFinished)
            {
                await _turn.Next(_battle, _playerTrainer, _rivalTrainer);
                _battleFinished = CheckBattleEnd();
            }
            
            LogManager.Log("Battle finished!", FeatureType.Battle);
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