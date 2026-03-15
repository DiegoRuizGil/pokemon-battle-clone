using System;
using System.Threading.Tasks;
using PokeApiNet;
using Pokemon_Battle_Clone.Runtime.Builders;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Stats.Domain;
using Pokemon_Battle_Clone.Runtime.TeamBuilder;
using UnityEditor;
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
        public string name;
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

        private static PokeApiClient _pokeClient;
        private static SpritesLoader _spritesLoader;

        private static PokeApiClient PokeClient
        {
            get
            {
                if (_pokeClient == null)
                    _pokeClient = new PokeApiClient();
                return _pokeClient;
            }
        }

        private static SpritesLoader SpritesLoader
        {
            get
            {
                if (_spritesLoader == null)
                    _spritesLoader = new SpritesLoader("Assets/Pokemon Battle Clone/Sprites/Pokemon");
                return _spritesLoader;
            }
        }

        public Pokemon Build()
        {
            return A.Pokemon.WithID((uint)ID)
                .WithName(name)
                .WithLevel(level)
                .WithBaseStats(baseStats)
                .WithTypes(type1, type2)
                .WithNature(GetNature(nature))
                .WithIVs(ivs)
                .WithEVs(evs);
        }

        public async Task LoadFromAPI()
        {
            try
            {
                var pokemon = await PokeClient.GetResourceAsync<PokeApiNet.Pokemon>(name);
                
                ApplyPokemonData(pokemon);
                await LoadSprites(pokemon);
                
                EditorUtility.SetDirty(this);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading pokemon \"{name}\": {e.Message}");
            }
        }

        private void ApplyPokemonData(PokeApiNet.Pokemon pokemon)
        {
            ID = pokemon.Id;
            baseStats = new StatSet(
                hp: pokemon.Stats[0].BaseStat,
                attack: pokemon.Stats[1].BaseStat,
                defense: pokemon.Stats[2].BaseStat,
                spcAttack: pokemon.Stats[3].BaseStat,
                spcDefense: pokemon.Stats[4].BaseStat,
                speed: pokemon.Stats[5].BaseStat
            );
            type1 = ElementalTypeUtils.GetType(pokemon.Types[0].Type.Name);
            if (pokemon.Types.Count > 1)
                type2 = ElementalTypeUtils.GetType(pokemon.Types[1].Type.Name);
        }

        private async Task LoadSprites(PokeApiNet.Pokemon pokemon)
        {
            try
            {
                backSprite = await SpritesLoader.LoadSprite(pokemon, SpriteType.Back);
                frontSprite = await SpritesLoader.LoadSprite(pokemon, SpriteType.Front);
                iconSprite = await SpritesLoader.LoadSprite(pokemon, SpriteType.Icon);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
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