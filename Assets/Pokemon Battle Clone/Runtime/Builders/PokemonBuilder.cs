using System.Collections.Generic;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Moves.Domain;
using Pokemon_Battle_Clone.Runtime.Stats.Domain;

namespace Pokemon_Battle_Clone.Runtime.Builders
{
    public class PokemonBuilder : IBuilder<Pokemon>
    {
        private uint _id;
        private string _name = "???";
        private int _level = 1;
        private StatSet _baseStats = new(50, 50, 50, 50, 50, 50);
        private Nature _nature = Nature.Bashful();
        private StatSet _evs = new();
        private StatSet _ivs = new();
        private ElementalType _type1 = ElementalType.None;
        private ElementalType _type2 = ElementalType.None;
        private List<Move> _moves = new();

        public PokemonBuilder WithID(uint id)
        {
            _id = id;
            return this;
        }
        
        public PokemonBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public PokemonBuilder WithLevel(int level)
        {
            _level = level;
            return this;
        }

        public PokemonBuilder WithBaseStats(StatSet baseStats)
        {
            _baseStats = baseStats;
            return this;
        }

        public PokemonBuilder WithNature(Nature nature)
        {
            _nature = nature;
            return this;
        }

        public PokemonBuilder WithEVs(StatSet evs)
        {
            _evs = evs;
            return this;
        }

        public PokemonBuilder WithIVs(StatSet ivs)
        {
            _ivs = ivs;
            return this;
        }

        public PokemonBuilder WithTypes(ElementalType type1, ElementalType type2 = ElementalType.None)
        {
            _type1 = type1;
            _type2 = type2;
            return this;
        }

        public PokemonBuilder WithMoves(params Move[] moves)
        {
            _moves = new List<Move>(moves);
            return this;
        }
        
        public Pokemon Build()
        {
            var stats = new StatsData(_level, _baseStats, _nature, _evs, _ivs);

            var pokemon = new Pokemon(_id, _name, stats, _type1, _type2);
            pokemon.MoveSet.AddMoves(_moves);
            
            return pokemon;
        }

        public static implicit operator Pokemon(PokemonBuilder builder) => builder.Build();
    }
}