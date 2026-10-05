using System;
using Pokemon_Battle_Clone.Runtime.Moves.Domain;
using UnityEngine;
using UnityEngine.Assertions;

namespace Pokemon_Battle_Clone.Runtime.Stats.Domain
{
    public class StatsData
    {
        public const int MinLevel = 1;
        public const int MaxLevel = 100;
        public const int MaxIV = 31;
        public const int MaxEVPerStat = 252;
        public const int MaxTotalEVs = 510;
        
        public int Level { get; }
        public StatSet BaseStats { get; }
        public StatSet EVs { get; }
        public StatSet IVs { get; }
        public Nature Nature { get; }
        public StatSet Stats { get; }
        public StatsModifier Modifiers { get; }

        public int HP => Stats.HP;
        public int Attack => Boosted(Stat.Attack);
        public int Defense => Boosted(Stat.Defense);
        public int SpAttack => Boosted(Stat.SpAttack);
        public int SpDefense => Boosted(Stat.SpDefense);
        public int Speed => Boosted(Stat.Speed);


        public StatsData(int level, StatSet baseStats, Nature nature, StatSet evs, StatSet ivs)
        {
            Assert.IsTrue(evs.Sum <= MaxTotalEVs);

            Level = Math.Clamp(level, MinLevel, MaxLevel);
            BaseStats = baseStats;
            Nature = nature;
            EVs = evs;
            IVs = ivs;
            Modifiers = new StatsModifier();
            Stats = CalculateStats(Level, BaseStats, EVs, IVs, Nature);
        }

        public int GetAttackByCategory(MoveCategory category)
        {
            return category switch
            {
                MoveCategory.Physical => Attack,
                MoveCategory.Special => SpAttack,
                _ => 0
            };
        }
        
        public int GetDefenseByCategory(MoveCategory category)
        {
            return category switch
            {
                MoveCategory.Physical => Defense,
                MoveCategory.Special => SpDefense,
                _ => 0
            };
        }

        private int Boosted(Stat stat) => Mathf.FloorToInt(Stats[stat] * Modifiers.GetMultiplier(stat));
        
        private static StatSet CalculateStats(int level, StatSet baseStats, StatSet evs, StatSet ivs, Nature nature)
        {
            return StatSet.From(stat => stat == Stat.HP 
                ? CalculateHPStat(level, baseStats.HP, evs.HP, ivs.HP)
                : CalculateStat(level, baseStats[stat], evs[stat], ivs[stat], nature[stat]));
        }

        private static int CalculateHPStat(int level, int baseHP, int ev, int iv) =>
            (2 * baseHP + iv + ev / 4) * level / 100 + level + 10;

        private static int CalculateStat(int level, int baseStat, int ev, int iv, float natureModifier) =>
            Mathf.FloorToInt(((2 * baseStat + iv + ev / 4) * level / 100 + 5) * natureModifier);
    }
}