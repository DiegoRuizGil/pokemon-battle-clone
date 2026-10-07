using System.Collections.Generic;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Database;
using Pokemon_Battle_Clone.Runtime.Stats.Domain;

namespace Pokemon_Battle_Clone.Editor.Database.PokeApi
{
    public class PokemonApiDto
    {
        public int Id { get; }
        public string Name { get; }
        public StatSet BaseStats { get; }
        public ElementalType Type1 { get; }
        public ElementalType Type2 { get; }
        public IReadOnlyDictionary<SpriteType, string> SpriteUrls { get; }

        public PokemonApiDto(int id, string name, StatSet baseStats, ElementalType type1, ElementalType type2,
            IReadOnlyDictionary<SpriteType, string> spriteUrls)
        {
            Id = id;
            Name = name;
            BaseStats = baseStats;
            Type1 = type1;
            Type2 = type2;
            SpriteUrls = spriteUrls;
        }
    }
}