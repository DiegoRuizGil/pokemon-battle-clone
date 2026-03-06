using Pokemon_Battle_Clone.Runtime.Core.Domain;

namespace Pokemon_Battle_Clone.Runtime.Trainers.Domain.BattleEvents
{
    public class DamageEvent : IBattleEvent
    {
        public Side ActionSide { get; }
        public Health TargetHealth { get; }
        public string UserName { get; }
        public string TargetName { get; }
        public float Effectiveness { get; }

        public DamageEvent(Side side, Health targetHealth, string userName, string targetName, float effectiveness)
        {
            ActionSide = side;
            TargetHealth = targetHealth;
            UserName = userName;
            TargetName = targetName;
            Effectiveness = effectiveness;
        }
    }
}