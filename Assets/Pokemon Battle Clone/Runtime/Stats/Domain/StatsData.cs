using System;
using Pokemon_Battle_Clone.Runtime.Moves.Domain;
using UnityEngine;
using UnityEngine.Assertions;

namespace Pokemon_Battle_Clone.Runtime.Stats.Domain
{
    public class StatsData
    {
        public int Level { get; }
        public StatSet BaseStats { get; }
        public StatSet EVs { get; }
        public StatSet IVs { get; }
        public Nature Nature { get; }
        public StatSet Stats { get; }
        public StatsModifier Modifiers { get; }

        public int HP => Stats.HP;
        public int Attack => Mathf.FloorToInt(Stats.Attack * Modifiers.AttackBoost);
        public int Defense => Mathf.FloorToInt(Stats.Defense * Modifiers.DefenseBoost);
        public int SpcAttack => Mathf.FloorToInt(Stats.SpcAttack * Modifiers.SpcAttackBoost);
        public int SpcDefense => Mathf.FloorToInt(Stats.SpcDefense * Modifiers.SpcDefenseBoost);
        public int Speed => Mathf.FloorToInt(Stats.Speed * Modifiers.SpeedBoost);


        public StatsData(int level, StatSet baseStats, Nature nature, StatSet evs, StatSet ivs)
        {
            Assert.IsTrue(evs.Sum <= 510);

            Level = Math.Clamp(level, 1, 100);
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
                MoveCategory.Special => SpcAttack,
                _ => 0
            };
        }
        
        public int GetDefenseByCategory(MoveCategory category)
        {
            return category switch
            {
                MoveCategory.Physical => Defense,
                MoveCategory.Special => SpcDefense,
                _ => 0
            };
        }

        private static StatSet CalculateStats(int level, StatSet baseStats, StatSet evs, StatSet ivs, Nature nature)
        {
            return new StatSet(
                CalculateHPStat(level, baseStats.HP, evs.HP, ivs.HP),
                CalculateStat(level, baseStats.Attack, evs.Attack, ivs.Attack, nature.Attack),
                CalculateStat(level, baseStats.Defense, evs.Defense, ivs.Defense, nature.Defense),
                CalculateStat(level, baseStats.SpcAttack, evs.SpcAttack, ivs.SpcAttack, nature.SpcAttack),
                CalculateStat(level, baseStats.SpcDefense, evs.SpcDefense, ivs.SpcDefense, nature.SpcDefense),
                CalculateStat(level, baseStats.Speed, evs.Speed, ivs.Speed, nature.Speed));
        }

        private static int CalculateHPStat(int level, int baseHP, int hpEV, int hpIV)
        {
            return Mathf.FloorToInt((2f * baseHP + hpIV + Mathf.FloorToInt(hpEV / 4f)) * level / 100) + level + 10;
        }

        private static int CalculateStat(int level, int baseStat, int ev, int iv, float natureModifier)
        {
            return Mathf.FloorToInt(((2 * baseStat + iv + ev / 4) * level / 100 + 5) * natureModifier);
        }
    }
}