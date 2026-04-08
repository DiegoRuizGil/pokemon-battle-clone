using System;
using System.Linq;
using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Battles.Infrastructure;
using Pokemon_Battle_Clone.Runtime.Battles.Infrastructure.Dialogs;
using Pokemon_Battle_Clone.Runtime.Core.Infrastructure;
using Pokemon_Battle_Clone.Runtime.RNG;
using Pokemon_Battle_Clone.Runtime.TeamBuilder.TeamDisplayer;
using Pokemon_Battle_Clone.Runtime.Trainers.Control;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain.Strategies;
using Pokemon_Battle_Clone.Runtime.Trainers.Infrastructure.Actions;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Battles.Control
{
    public class BattleController : MonoBehaviour, IBattleContext
    {
        [Header("Data")]
        public BattleSettings battleSettings;
        
        [Header("UI")]
        public TeamView playerTeamView;
        public TeamView rivalTeamView;
        public ActionsHUD actionsHUD;
        public TeamInfoDisplayer teamInfoDisplayer;
        public DialogDisplayer dialogDisplayer;
        public BattleEndPanel battleEndPanel;
        
        private Battle _battle;
        private Turn _turn;

        private Trainer _playerTrainer;
        private Trainer _rivalTrainer;
        
        private bool _battleFinished;
        
        private void Start()
        {
            var playerTeam = battleSettings.PlayerTeamConfig.Build();
            var rivalTeam = battleSettings.RivalTeamConfig.Build();
            
            _battle = new Battle(playerTeam, rivalTeam, new DefaultRandom(seed: DateTime.Now.GetHashCode()));
            _turn = new Turn(new ActionsResolver(this, dialogDisplayer), actionsHUD);
            
            _playerTrainer = new PlayerTrainer(playerTeam, actionsHUD, teamInfoDisplayer);
            playerTeamView.Init(playerTeam.PokemonList.Select(p => p.ID).ToList());
            
            _rivalTrainer = new AITrainer(_battle, rivalTeam, new RandomTrainerStrategy());
            rivalTeamView.Init(rivalTeam.PokemonList.Select(p => p.ID).ToList());
            
            _ = RunBattleAsync();
        }

        private async Task RunBattleAsync()
        {
            await _turn.Init(_battle, _playerTrainer, _rivalTrainer);
            
            while (!_battleFinished)
            {
                await _turn.Next(_battle, _playerTrainer, _rivalTrainer);
                _battleFinished = CheckBattleEnd();
            }
            
            EndBattle();
        }

        private bool CheckBattleEnd()
        {
            if (_playerTrainer.Defeated)
                return true;
            if (_rivalTrainer.Defeated)
                return true;

            return false;
        }

        private void EndBattle()
        {
            var winner = _playerTrainer.Defeated ? Side.Rival : Side.Player;
            battleEndPanel.Show(winner);
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