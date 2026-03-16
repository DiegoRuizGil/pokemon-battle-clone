using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.TeamBuilder.Selector
{
    public class TeamsAllocator : MonoBehaviour
    {
        [SerializeField] private TrainerTeamSelection playerSelection;
        [SerializeField] private TrainerTeamSelection rivalSelection;

        public void SetTeam(TeamConfig teamConfig, Side side)
        {
            if (side == Side.Player)
                playerSelection.SetTeam(teamConfig);
            else
                rivalSelection.SetTeam(teamConfig);
        }
    }
}