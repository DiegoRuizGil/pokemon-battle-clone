using System;
using System.Linq;
using Pokemon_Battle_Clone.Runtime.Battles.Control;
using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Battles.Infrastructure;
using Pokemon_Battle_Clone.Runtime.Battles.Infrastructure.Dialogs;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Core.Infrastructure;
using Pokemon_Battle_Clone.Runtime.RNG;
using Pokemon_Battle_Clone.Runtime.TeamBuilder.TeamDisplayer;
using Pokemon_Battle_Clone.Runtime.Trainers.Control;
using Pokemon_Battle_Clone.Runtime.Trainers.Infrastructure.Actions;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online
{
    public class OnlineBattleBootstrapper : MonoBehaviour, IBattleContext
    {
        public BattleController battleController;
        public BattleNetworkBridge battleNetworkBridge;
        
        [Header("Data")]
        public BattleSettings battleSettings;
        
        [Header("UI")]
        public TeamView playerTeamView;
        public TeamView rivalTeamView;
        public ActionsHUD actionsHUD;
        public TeamInfoDisplayer teamInfoDisplayer;
        public DialogDisplayer dialogDisplayer;

        private void Awake()
        {
            var playerTeam = battleSettings.PlayerTeamConfig.Build();
            var rivalTeam = battleSettings.RivalTeamConfig.Build();
            
            var battle = new Battle(playerTeam, rivalTeam, new DefaultRandom(seed: DateTime.Now.GetHashCode()));
            
            var player = SetupPlayer(playerTeam);
            var rival = SetupRival(rivalTeam);

            var actionsResolver = new ActionsResolver(this, dialogDisplayer);
            var turn = new Turn(actionsResolver, battle, player, rival);
            
            actionsHUD.Hide();
            battleController.Init(turn, player, rival);
            
            battleNetworkBridge.Init(battle, player, rival);
        }

        private PlayerTrainer SetupPlayer(Team team)
        {
            var trainer = new PlayerTrainer(team, Side.Player, actionsHUD, teamInfoDisplayer);
            playerTeamView.Init(team.PokemonList.Select(p => p.ID).ToList());

            return trainer;
        }

        private NetworkTrainer SetupRival(Team team)
        {
            var trainer = new NetworkTrainer(team, Side.Rival);
            rivalTeamView.Init(team.PokemonList.Select(p => p.ID).ToList());

            return trainer;
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