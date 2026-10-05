using System;

namespace Pokemon_Battle_Clone.Runtime.Stats.Domain
{
    public class StatsModifier
    {
        public int AttackLevel => _modifiers.Attack;
        public int DefenseLevel => _modifiers.Defense;
        public int SpcAttackLevel => _modifiers.SpAttack;
        public int SpcDefenseLevel => _modifiers.SpDefense;
        public int SpeedLevel => _modifiers.Speed;

        public float AttackBoost => GetBoost(_modifiers.Attack);
        public float DefenseBoost => GetBoost(_modifiers.Defense);
        public float SpcAttackBoost => GetBoost(_modifiers.SpAttack);
        public float SpcDefenseBoost => GetBoost(_modifiers.SpDefense);
        public float SpeedBoost => GetBoost(_modifiers.Speed);
        
        private readonly StatSet _modifiers;

        public StatsModifier()
        {
            _modifiers = new StatSet();
            _modifiers.SetMinMaxValues(-6, 6);
        }

        public void Apply(StatSet boost) => _modifiers.Add(boost);

        public void Clear() => _modifiers.Clear();

        private float GetBoost(int boostLevel)
        {
            var numerator = 0;
            var denominator = 0;

            if (boostLevel > 0) numerator = boostLevel;
            else if (boostLevel < 0) denominator = Math.Abs(boostLevel);
            
            return (2f + numerator) / (2f + denominator);
        }
    }
}