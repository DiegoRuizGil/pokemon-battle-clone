using Pokemon_Battle_Clone.Runtime.Database;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Battles.Infrastructure
{
    [CreateAssetMenu(menuName = "Pokemon Battle Clone/Database/Battle", fileName = "Battle Settings")]
    public class BattleSettings : ScriptableObject
    {
        public TeamConfig PlayerTeamConfig;
        public TeamConfig RivalTeamConfig;
    }
}