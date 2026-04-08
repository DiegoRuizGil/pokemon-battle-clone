using System;
using System.Linq;
using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Battles.Infrastructure;
using Pokemon_Battle_Clone.Runtime.Core.Infrastructure;
using Pokemon_Battle_Clone.Runtime.RNG;
using Pokemon_Battle_Clone.Runtime.Trainers.Control;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain.Strategies;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Battles.Control
{
    public class BattleController : MonoBehaviour
    {
        [Header("UI")]
        public BattleEndPanel battleEndPanel;
        
        private Turn _turn;

        private Trainer _playerTrainer;
        private Trainer _rivalTrainer;
        
        private bool _battleFinished;

        public void Init(Turn turn, Trainer player, Trainer rival)
        {
            _turn = turn;
            _playerTrainer = player;
            _rivalTrainer = rival;

            _ = RunBattleAsync();
        }

        private async Task RunBattleAsync()
        {
            await _turn.Init();
            
            while (!_battleFinished)
            {
                await _turn.Next();
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
    }
}