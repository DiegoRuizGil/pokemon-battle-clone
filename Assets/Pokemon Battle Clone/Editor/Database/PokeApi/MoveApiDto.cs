using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Moves.Domain;

namespace Pokemon_Battle_Clone.Editor.Database.PokeApi
{
    public class MoveApiDto
    {
        public int Id { get; }
        public string Name { get; }
        public ElementalType Type { get; }
        public MoveCategory Category { get; }
        public int Pp { get; }
        public int Accuracy { get; }
        public int Power { get; }
        public int Priority { get; }

        public MoveApiDto(int id, string name, ElementalType type, MoveCategory category,
            int pp, int accuracy, int power, int priority)
        {
            Id = id;
            Name = name;
            Type = type;
            Category = category;
            Pp = pp;
            Accuracy = accuracy;
            Power = power;
            Priority = priority;
        }
    }
}