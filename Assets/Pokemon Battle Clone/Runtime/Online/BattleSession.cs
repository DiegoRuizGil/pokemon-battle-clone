using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Trainers.Control;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online
{
    [CreateAssetMenu(menuName = "Pokemon Battle Clone/Online/Battle Session", fileName = "Battle Session")]
    public class BattleSession : ScriptableObject
    {
        public Battle Battle;
        public PlayerTrainer LocalTrainer;
        public NetworkTrainer RemoteTrainer;
    }
}