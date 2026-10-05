using System;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Stats.Domain
{
    [System.Serializable]
    public class StatSet
    {
        [field: SerializeField] public int HP { get; private set; }
        [field: SerializeField] public int Attack { get; private set; }
        [field: SerializeField] public int Defense { get; private set; }
        [field: SerializeField] public int SpAttack { get; private set; }
        [field: SerializeField] public int SpDefense { get; private set; }
        [field: SerializeField] public int Speed { get; private set; }

        public int Sum => HP + Attack + Defense + SpAttack + SpDefense + Speed;

        public int this[Stat stat] => stat switch
        {
            Stat.HP => HP,
            Stat.Attack => Attack,
            Stat.Defense => Defense,
            Stat.SpAttack => SpAttack,
            Stat.SpDefense => SpDefense,
            Stat.Speed => Speed,
            _ => throw new ArgumentOutOfRangeException(nameof(stat))
        };
        
        public StatSet()
        {
            HP = 0;
            Attack = 0;
            SpAttack = 0;
            Defense = 0;
            SpDefense = 0;
            Speed = 0;
        }

        public StatSet(int hp, int attack, int defense, int spAttack, int spDefense, int speed)
        {
            HP = hp;
            Attack = attack;
            Defense = defense;
            SpAttack = spAttack;
            SpDefense = spDefense;
            Speed = speed;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (obj is not StatSet other)
                return false;
            
            return HP == other.HP && Attack == other.Attack && Defense == other.Defense &&
                SpAttack == other.SpAttack && SpDefense == other.SpDefense && Speed == other.Speed;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(HP, Attack, Defense, SpAttack, SpDefense, Speed);
        }

        public override string ToString()
        {
            return $"({HP}, {Attack}, {Defense}, {SpAttack}, {SpDefense}, {Speed})";
        }
    }
}