using System;
using System.Net.Http;
using System.Threading.Tasks;
using PokeApiNet;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Database;
using Pokemon_Battle_Clone.Runtime.Stats.Domain;
using UnityEditor;
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
        
        public async Task LoadFromPokeApi(PokemonConfig target, string search, bool overwriteSprites = false)
        {
            var pokemon = await _pokeClient.GetResourceAsync<PokeApiNet.Pokemon>(search);
                
            ApplyData(target, pokemon);
            await DownloadSprites(pokemon, overwriteSprites);
                
            EditorUtility.SetDirty(target);
        }

        private void ApplyData(PokemonConfig target, PokeApiNet.Pokemon pokemon)
        {
            target.ID = pokemon.Id;
            target.pokemonName = char.ToUpper(pokemon.Name[0]) + pokemon.Name.Substring(1);
            target.baseStats = new StatSet(
                hp: pokemon.Stats[0].BaseStat,
                attack: pokemon.Stats[1].BaseStat,
                defense: pokemon.Stats[2].BaseStat,
                spcAttack: pokemon.Stats[3].BaseStat,
                spcDefense: pokemon.Stats[4].BaseStat,
                speed: pokemon.Stats[5].BaseStat
            );
            target.type1 = ElementalTypeUtils.GetType(pokemon.Types[0].Type.Name);
            if (pokemon.Types.Count > 1)
                target.type2 = ElementalTypeUtils.GetType(pokemon.Types[1].Type.Name);
        }

        private async Task DownloadSprites(PokeApiNet.Pokemon pokemon, bool overwrite)
        {
            var spritesType = (SpriteType[])Enum.GetValues(typeof(SpriteType));
            foreach (var type in spritesType)
            {
                if (!overwrite && _spritesRepository.Exists(pokemon.Id, type))
                    continue;

                var url = GetSpriteUrl(pokemon, type);
                if (string.IsNullOrEmpty(url))
                {
                    Debug.LogWarning($"PokeApi has no {type} sprite for {pokemon.Name} (id {pokemon.Id})");
                    continue;
                }

                try
                {
                    var bytes = await SpriteHttpClient.GetByteArrayAsync(url);
                    _spritesRepository.Save(pokemon.Id, type, bytes, overwrite);
                }
                catch (HttpRequestException e)
                {
                    Debug.LogWarning($"Could not download the {type} sprite of {pokemon.Name}: {e.Message}");
                }
            }
        }

        private static string GetSpriteUrl(PokeApiNet.Pokemon pokemon, SpriteType type) => type switch
        {
            SpriteType.Back => pokemon.Sprites.BackDefault,
            SpriteType.Front => pokemon.Sprites.FrontDefault,
            SpriteType.Icon => pokemon.Sprites.Versions.GenerationVIII.Icons.FrontDefault,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}