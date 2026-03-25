using Pokemon_Battle_Clone.Runtime.Moves.Domain;
using Pokemon_Battle_Clone.Runtime.Stats.Domain;

namespace Pokemon_Battle_Clone.Runtime.Core.Domain
{
    public class Pokemon
    {
        public uint ID { get; }
        public string Name { get; }
        public ElementalType Type1 { get; private set; }
        public ElementalType Type2 { get; private set; }
        public StatsData Stats { get; private set; }
        public MoveSet MoveSet { get; private set; }
        public Health Health { get; }

        public bool Defeated => Health.Current <= 0;

        public Pokemon(uint id, string name, StatsData stats, ElementalType type1, ElementalType type2 = ElementalType.None)
        {
            ID = id;
            Name = name;
            Stats = stats;
            Type1 = type1;
            Type2 = type2;

            MoveSet = new MoveSet();
            Health = new Health(stats.Stats.HP);
        }
        
        public float STAB(ElementalType moveType)
        {
            if (moveType == Type1 || moveType == Type2)
                return 1.5f;
            return 1f;
        }
    }
}