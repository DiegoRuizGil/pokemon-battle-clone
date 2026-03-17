using Pokemon_Battle_Clone.Runtime.Builders;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Stats.Domain;
using UnityEngine;
using Nature = Pokemon_Battle_Clone.Runtime.Stats.Domain.Nature;
using Pokemon = Pokemon_Battle_Clone.Runtime.Core.Domain.Pokemon;

namespace Pokemon_Battle_Clone.Runtime.Database
{
    public enum NatureEnum
    {
        Hardy, Lonely, Brave, Adamant, Naughty, Bold, Docile, Relaxed, Impish, Lax, Timid, Hasty, Serious, Jolly, Naive,
        Modest, Mild, Quiet, Bashful, Rash, Calm, Gentle, Sassy, Careful, Quirky
    }
    
    [CreateAssetMenu(menuName = "Pokemon Battle Clone/Database/Pokemon", fileName = "Pokemon Config")]
    public class PokemonConfig : ScriptableObject
    {
        public int ID;
        public string pokemonName;
        public int level = 100;
        public StatSet baseStats;
        public ElementalType type1;
        public ElementalType type2;
        public NatureEnum nature;
        public StatSet ivs = new StatSet(31, 31, 31, 31, 31, 31);
        public StatSet evs;

        [Space(10)]
        public Sprite backSprite;
        public Sprite frontSprite;
        public Sprite iconSprite;
        
        public Pokemon Build()
        {
            return A.Pokemon.WithID((uint)ID)
                .WithName(pokemonName)
                .WithLevel(level)
                .WithBaseStats(baseStats)
                .WithTypes(type1, type2)
                .WithNature(GetNature(nature))
                .WithIVs(ivs)
                .WithEVs(evs);
        }
        
        private Nature GetNature(NatureEnum natureEnum)
        {
            return natureEnum switch
            {
                NatureEnum.Adamant => Nature.Adamant(),
                NatureEnum.Bashful => Nature.Bashful(),
                NatureEnum.Bold => Nature.Bold(),
                NatureEnum.Brave => Nature.Brave(),
                NatureEnum.Calm => Nature.Calm(),
                NatureEnum.Careful => Nature.Careful(),
                NatureEnum.Docile => Nature.Docile(),
                NatureEnum.Gentle => Nature.Gentle(),
                NatureEnum.Hardy => Nature.Hardy(),
                NatureEnum.Hasty => Nature.Hasty(),
                NatureEnum.Impish => Nature.Impish(),
                NatureEnum.Jolly => Nature.Jolly(),
                NatureEnum.Lax => Nature.Lax(),
                NatureEnum.Lonely => Nature.Lonely(),
                NatureEnum.Mild => Nature.Mild(),
                NatureEnum.Modest => Nature.Modest(),
                NatureEnum.Naive => Nature.Naive(),
                NatureEnum.Naughty => Nature.Naughty(),
                NatureEnum.Quiet => Nature.Quiet(),
                NatureEnum.Quirky => Nature.Quirky(),
                NatureEnum.Rash => Nature.Rash(),
                NatureEnum.Relaxed => Nature.Relaxed(),
                NatureEnum.Sassy => Nature.Sassy(),
                NatureEnum.Serious => Nature.Serious(),
                NatureEnum.Timid => Nature.Timid(),
                _ => Nature.Bashful()
            };
        }
    }
}