using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Pokemon_Battle_Clone.Runtime.Stats.Domain
{
    [System.Serializable]
    public class StatSet
    {
        [field: SerializeField] public int HP { get; private set; }
        [field: SerializeField] public int Attack { get; private set; }
        [field: SerializeField] public int Defense { get; private set; }
        [field: SerializeField] public int SpcAttack { get; private set; }
        [field: SerializeField] public int SpcDefense { get; private set; }
        [field: SerializeField] public int Speed { get; private set; }

        public int Sum => HP + Attack + Defense + SpcAttack + SpcDefense + Speed;

        public static readonly string[] StatNames = { "HP", "Atk", "Def", "SpA", "SpD", "Spe" };
        public int[] Values => new[] { HP, Attack, Defense, SpcAttack, SpcDefense, Speed };
        
        private int _maxStatValue;
        private int _minStatValue;

        public StatSet()
        {
            HP = 0;
            Attack = 0;
            SpcAttack = 0;
            Defense = 0;
            SpcDefense = 0;
            Speed = 0;
            
            _maxStatValue = int.MaxValue;
            _minStatValue = int.MinValue;
        }

        public StatSet(int hp, int attack, int defense, int spcAttack, int spcDefense, int speed)
        {
            HP = hp;
            Attack = attack;
            Defense = defense;
            SpcAttack = spcAttack;
            SpcDefense = spcDefense;
            Speed = speed;
            
            _maxStatValue = int.MaxValue;
            _minStatValue = int.MinValue;
        }

        public static StatSet BlankEVsSet()
        {
            var statSet = new StatSet();
            statSet.SetMinMaxValues(0, 252);

            return statSet;
        }

        public static StatSet BlankIVsSet()
        {
            var statSet = new StatSet();
            statSet.SetMinMaxValues(0, 31);
            
            return statSet;
        }

        /// <summary>
        /// Set min/max stats values and clamps them
        /// </summary>
        public void SetMinMaxValues(int min, int max)
        {
            Assert.IsTrue(min <= max);
            
            _maxStatValue = max;
            _minStatValue = min;
            
            ClampStats();
        }

        /// <summary>
        /// Set all values to 0.
        /// </summary>
        public void Clear()
        {
            HP = 0;
            Attack = 0;
            Defense = 0;
            SpcAttack = 0;
            SpcDefense = 0;
            Speed = 0;
        }

        public void Add(StatSet statSet)
        {
            HP += statSet.HP;
            Attack += statSet.Attack;
            Defense += statSet.Defense;
            SpcAttack += statSet.SpcAttack;
            SpcDefense += statSet.SpcDefense;
            Speed += statSet.Speed;
            
            ClampStats();
        }
        
        private void ClampStats()
        {
            HP = Math.Clamp(HP, _minStatValue, _maxStatValue);
            Attack = Math.Clamp(Attack, _minStatValue, _maxStatValue);
            Defense = Math.Clamp(Defense, _minStatValue, _maxStatValue);
            SpcAttack = Math.Clamp(SpcAttack, _minStatValue, _maxStatValue);
            SpcDefense = Math.Clamp(SpcDefense, _minStatValue, _maxStatValue);
            Speed = Math.Clamp(Speed, _minStatValue, _maxStatValue);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (obj is not StatSet other)
                return false;
            
            return HP == other.HP && Attack == other.Attack && Defense == other.Defense &&
                SpcAttack == other.SpcAttack && SpcDefense == other.SpcDefense && Speed == other.Speed;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(HP, Attack, Defense, SpcAttack, SpcDefense, Speed);
        }

        public override string ToString()
        {
            return $"({HP}, {Attack}, {Defense}, {SpcAttack}, {SpcDefense}, {Speed})";
        }
    }
}