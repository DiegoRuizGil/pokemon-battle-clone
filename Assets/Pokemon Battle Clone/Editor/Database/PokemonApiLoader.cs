using System;
using System.Threading.Tasks;
using PokeApiNet;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Database;
using Pokemon_Battle_Clone.Runtime.Stats.Domain;
using Pokemon_Battle_Clone.Runtime.TeamBuilder;
using UnityEditor;
using UnityEngine;

namespace Pokemon_Battle_Clone.Editor.Database
{
    public class PokemonApiLoader
    {
        private readonly PokeApiClient _pokeClient;
        private readonly SpritesLoader _spritesLoader;

        public PokemonApiLoader()
        {
            _pokeClient = new PokeApiClient();
            _spritesLoader = new SpritesLoader("Assets/Pokemon Battle Clone/Sprites/Pokemon");
        }
        
        public async Task LoadFromPokeApi(PokemonConfig target, string search)
        {
            try
            {
                var pokemon = await _pokeClient.GetResourceAsync<PokeApiNet.Pokemon>(search);
                
                ApplyData(target, pokemon);
                await LoadSprites(target, pokemon);
                
                EditorUtility.SetDirty(target);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading pokemon \"{search}\": {e.Message}");
            }
        }

        private void ApplyData(PokemonConfig target, PokeApiNet.Pokemon pokemon)
        {
            target.ID = pokemon.Id;
            target.pokemonName = pokemon.Name;
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
        
        private async Task LoadSprites(PokemonConfig target, PokeApiNet.Pokemon pokemon)
        {
            try
            {
                target.backSprite = await _spritesLoader.LoadSprite(pokemon, SpriteType.Back);
                target.frontSprite = await _spritesLoader.LoadSprite(pokemon, SpriteType.Front);
                target.iconSprite = await _spritesLoader.LoadSprite(pokemon, SpriteType.Icon);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }
}