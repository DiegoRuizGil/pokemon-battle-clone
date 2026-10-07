using Pokemon_Battle_Clone.Runtime.Builders;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Stats.Domain;
using UnityEngine;
using Nature = Pokemon_Battle_Clone.Runtime.Stats.Domain.Nature;
using Pokemon = Pokemon_Battle_Clone.Runtime.Core.Domain.Pokemon;

namespace Pokemon_Battle_Clone.Runtime.Database
{
    [CreateAssetMenu(menuName = "Pokemon Battle Clone/Database/Pokemon", fileName = "Pokemon Config")]
    public class PokemonConfig : ScriptableObject
    {
        public int ID;
        public string pokemonName = "unknown";
        public StatSet baseStats = new StatSet();
        public ElementalType type1 = ElementalType.Normal;
        public ElementalType type2 = ElementalType.None;
        
        public PokemonBuilder ToBuilder()
            => A.Pokemon.WithID((uint)ID)
                .WithName(pokemonName)
                .WithBaseStats(baseStats)
                .WithTypes(type1, type2);
    }
}