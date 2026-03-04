using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Stats.Domain;

namespace Pokemon_Battle_Clone.Runtime.Trainer.Domain.BattleEvents
{
    public class StatsModifierEvent : IBattleEvent
    {
        public Side ActionSide { get; }
        public StatsModifier Modifier { get; }
        public string UserName { get; }
        public string TargetName { get; }
        
        public StatsModifierEvent(Side side, StatsModifier modifier, string userName, string targetName)
        {
            ActionSide = side;
            Modifier = modifier;
            UserName = userName;
            TargetName = targetName;
        }
    }
}