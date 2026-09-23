using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using PokeApiNet;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Database;
using Pokemon_Battle_Clone.Runtime.Stats.Domain;
using UnityEngine;

namespace Pokemon_Battle_Clone.Editor.Database
{
    public class PokemonApiLoader
    {
        private static readonly HttpClient SpriteHttpClient = new();
        
        private readonly PokeApiClient _pokeClient;
        private readonly PokemonSpritesRepository _spritesRepository;

        public PokemonApiLoader(PokemonSpritesRepository spritesRepository)
        {
            _pokeClient = new PokeApiClient();
            _spritesRepository = spritesRepository;
        }

        public async Task<PokemonApiDto> Fetch(string search)
        {
            var pokemon = await _pokeClient.GetResourceAsync<PokeApiNet.Pokemon>(search.Trim().ToLowerInvariant());
            return ToApiData(pokemon);
        }

        public PokemonConfig CreateConfig(PokemonApiDto dto)
        {
            var config = ScriptableObject.CreateInstance<PokemonConfig>();
            config.ID = dto.Id;
            config.pokemonName = dto.Name;
            config.baseStats = dto.BaseStats;
            config.type1 = dto.Type1;
            config.type2 = dto.Type2;
            return config;
        }

        public async Task DownloadSprites(PokemonApiDto dto, bool overwrite = false)
        {
            foreach (var pair in dto.SpriteUrls)
            {
                var type = pair.Key;
                var url = pair.Value;

                if (!overwrite && _spritesRepository.Exists(dto.Id, type))
                    continue;

                if (string.IsNullOrEmpty(url))
                {
                    Debug.LogWarning($"PokeApi has no {type} sprite for {dto.Name} (id {dto.Id})");
                    continue;
                }

                try
                {
                    var bytes = await SpriteHttpClient.GetByteArrayAsync(url);
                    _spritesRepository.Save(dto.Id, type, bytes, overwrite);
                }
                catch (HttpRequestException e)
                {
                    Debug.LogWarning($"Could not download the {type} sprite of {dto.Name}: {e.Message}");
                }
            }
        }

        private PokemonApiDto ToApiData(PokeApiNet.Pokemon pokemon)
        {
            var spriteUrls = new Dictionary<SpriteType, string>
            {
                [SpriteType.Back] = pokemon.Sprites.BackDefault,
                [SpriteType.Front] = pokemon.Sprites.FrontDefault,
                [SpriteType.Icon] = pokemon.Sprites.Versions.GenerationVIII.Icons.FrontDefault
            };

            var baseStats = new StatSet(
                hp: pokemon.Stats[0].BaseStat,
                attack: pokemon.Stats[1].BaseStat,
                defense: pokemon.Stats[2].BaseStat,
                spcAttack: pokemon.Stats[3].BaseStat,
                spcDefense: pokemon.Stats[4].BaseStat,
                speed: pokemon.Stats[5].BaseStat
            );

            var type1 = ElementalTypeUtils.GetType(pokemon.Types[0].Type.Name);
            var type2 = pokemon.Types.Count > 1
                ? ElementalTypeUtils.GetType(pokemon.Types[1].Type.Name)
                : ElementalType.None;

            return new PokemonApiDto(
                pokemon.Id,
                char.ToUpper(pokemon.Name[0]) + pokemon.Name.Substring(1),
                baseStats, type1, type2, spriteUrls
            );
        }
    }
}