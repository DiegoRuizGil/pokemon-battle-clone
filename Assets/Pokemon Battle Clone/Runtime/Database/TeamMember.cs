using System.Collections.Generic;
using System.Linq;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Stats.Domain;

namespace Pokemon_Battle_Clone.Runtime.Database
{
    public enum NatureEnum
    {
        Hardy, Lonely, Brave, Adamant, Naughty, Bold, Docile, Relaxed, Impish, Lax, Timid, Hasty, Serious, Jolly, Naive,
        Modest, Mild, Quiet, Bashful, Rash, Calm, Gentle, Sassy, Careful, Quirky
    }
    
    [System.Serializable]
    public class TeamMember
    {
        public const int MaxMoves = 4;
        
        public PokemonConfig pokemonConfig;
        public int level = 100;
        public NatureEnum nature = NatureEnum.Bashful;
        public StatSet ivs = new(31, 31, 31, 31, 31, 31);
        public StatSet evs = new();
        public List<MoveConfig> moves = new();

        public Pokemon Build() =>
            pokemonConfig.ToBuilder()
                .WithLevel(level)
                .WithNature(GetNature(nature))
                .WithIVs(ivs)
                .WithEVs(evs)
                .WithMoves(moves.Where(m => m != null).Select(m => m.Build()).ToArray());
        
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