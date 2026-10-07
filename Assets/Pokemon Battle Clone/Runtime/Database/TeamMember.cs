using System.Collections.Generic;
using System.Linq;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Stats.Domain;

namespace Pokemon_Battle_Clone.Runtime.Database
{
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
                .WithNature(Nature.FromEnum(nature))
                .WithIVs(ivs)
                .WithEVs(evs)
                .WithMoves(moves.Where(m => m != null).Select(m => m.Build()).ToArray());

        public StatsData BuildStatsData()
        {
            var baseStats = pokemonConfig != null ? pokemonConfig.baseStats : new StatSet();
            return new StatsData(level, baseStats, Nature.FromEnum(nature), evs, ivs);
        }
    }
}