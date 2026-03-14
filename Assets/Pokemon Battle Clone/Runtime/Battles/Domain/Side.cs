namespace Pokemon_Battle_Clone.Runtime.Battles.Domain
{
    public enum Side
    {
        Player, Rival
    }

    public static class SideExtensions
    {
        public static Side Opposite(this Side side)
        {
            return side == Side.Player ? Side.Rival : Side.Player;
        }
    }
}