using System;
using System.Linq;
using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Battles.Infrastructure.Dialogs;
using Pokemon_Battle_Clone.Runtime.Core.Infrastructure;
using Pokemon_Battle_Clone.Runtime.CustomLogs;
using Pokemon_Battle_Clone.Runtime.Database;
using Pokemon_Battle_Clone.Runtime.RNG;
using Pokemon_Battle_Clone.Runtime.TeamBuilder.UI;
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
        public TeamInfoDisplayer teamInfoDisplayer;
        public DialogDisplayer dialogDisplayer;

        public TeamConfig playerTeamConfig;
        public TeamConfig rivalTeamConfig;
        
        private Battle _battle;
        private Turn _turn;

        private Trainer _playerTrainer;
        private Trainer _rivalTrainer;
        
        private int _turnCount;
        private bool _battleFinished;
        
        private void Start()
        {
            var playerTeam = playerTeamConfig.Build();
            var rivalTeam = rivalTeamConfig.Build();
            
            _battle = new Battle(playerTeam, rivalTeam, new DefaultRandom(seed: DateTime.Now.GetHashCode()));
            _turn = new Turn(new ActionsResolver(this, dialogDisplayer), actionsHUD);
            
            _playerTrainer = new PlayerTrainer(playerTeam, actionsHUD, teamInfoDisplayer);
            playerTeamView.Init(playerTeam.PokemonList.Select(p => p.ID).ToList());
            
            _rivalTrainer = new RivalTrainer(rivalTeam, new RandomTrainerStrategy());
            rivalTeamView.Init(rivalTeam.PokemonList.Select(p => p.ID).ToList());
            
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
    }
}