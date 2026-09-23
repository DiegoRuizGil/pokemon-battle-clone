using System.Threading.Tasks;
using PokeApiNet;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Database;
using Pokemon_Battle_Clone.Runtime.Stats.Domain;
using UnityEditor;

namespace Pokemon_Battle_Clone.Editor.Database
{
    public class PokemonApiLoader
    {
        private readonly PokeApiClient _pokeClient;
        private readonly SpritesManager _spritesManager;

        public PokemonApiLoader()
        {
            _pokeClient = new PokeApiClient();
            _spritesManager = new SpritesManager(ProjectPaths.PokemonSprites);
        }
        
        public async Task LoadFromPokeApi(PokemonConfig target, string search)
        {
            var pokemon = await _pokeClient.GetResourceAsync<PokeApiNet.Pokemon>(search);
                
            ApplyData(target, pokemon);
            await _spritesManager.DownloadAllSpritesOf(pokemon);
                
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
    }
}