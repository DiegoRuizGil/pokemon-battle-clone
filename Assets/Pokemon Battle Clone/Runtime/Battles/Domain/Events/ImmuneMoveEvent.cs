namespace Pokemon_Battle_Clone.Runtime.Battles.Domain.Events
{
    public class ImmuneMoveEvent : IBattleEvent
    {
        public string TargetName { get; }
        
        public ImmuneMoveEvent(string targetName)
        {
            TargetName = targetName;
        }
    }
}