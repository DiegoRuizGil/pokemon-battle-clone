using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Battles.Infrastructure;
using Pokemon_Battle_Clone.Runtime.Database;
using Pokemon_Battle_Clone.Runtime.Trainers.Infrastructure.Actions;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.TeamBuilder.Selector
{
    public class TeamsAllocator : MonoBehaviour
    {
        [SerializeField] private BattleSettings battleSettings;
        [Space(5)]
        [SerializeField] private TrainerTeamSelection playerSelection;
        [SerializeField] private TrainerTeamSelection rivalSelection;
        [Space(5)]
        [SerializeField] private HUDButton startButton;

        private void Start()
        {
            SetStartButtonInteraction();
        }

        public void SetTeam(TeamConfig teamConfig, Side side)
        {
            if (side == Side.Player)
            {
                playerSelection.SetTeam(teamConfig);
                battleSettings.PlayerTeamConfig = teamConfig;
            }
            else
            {
                rivalSelection.SetTeam(teamConfig);
                battleSettings.RivalTeamConfig = teamConfig;
            }
            
            SetStartButtonInteraction();
        }

        private void SetStartButtonInteraction()
        {
            startButton.SetInteraction(playerSelection.HasTeamSelected && rivalSelection.HasTeamSelected);
        }
    }
}