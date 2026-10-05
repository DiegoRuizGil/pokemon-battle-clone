using System;

namespace Pokemon_Battle_Clone.Runtime.Stats.Domain
{
    public enum Stat
    {
        HP, Attack, Defense, SpAttack, SpDefense, Speed
    }

    public static class StatInfo
    {
        public static readonly Stat[] All =
        {
            Stat.HP, Stat.Attack, Stat.Defense, Stat.SpAttack, Stat.SpDefense, Stat.Speed
        };

        public static string ShortName(this Stat stat) => stat switch
        {
            Stat.HP => "HP",
            Stat.Attack => "Atk",
            Stat.Defense => "Def",
            Stat.SpAttack => "SpA",
            Stat.SpDefense => "SpD",
            Stat.Speed => "Spe",
            _ => throw new ArgumentOutOfRangeException(nameof(stat))
        };
        
        public static string FullName(this Stat stat) => stat switch
        {
            Stat.HP => "HP",
            Stat.Attack => "Attack",
            Stat.Defense => "Defense",
            Stat.SpAttack => "Special Attack",
            Stat.SpDefense => "Special Defense",
            Stat.Speed => "Speed",
            _ => throw new ArgumentOutOfRangeException(nameof(stat))
        };
    }
}