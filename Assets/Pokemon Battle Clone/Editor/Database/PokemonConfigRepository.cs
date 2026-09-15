using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEditor;
using UnityEngine;
using WebSocketSharp;

namespace Pokemon_Battle_Clone.Editor.Database
{
    public class PokemonConfigRepository
    {
        private readonly string _folderPath;

        public PokemonConfigRepository(string folderPath)
        {
            _folderPath = folderPath;
        }

        public void CreateAsset(PokemonConfig pokemonConfig)
        {
            var name = pokemonConfig.pokemonName;
            var assetPath = Path.Combine(_folderPath, name + ".asset");
            
            AssetDatabase.CreateAsset(pokemonConfig, assetPath);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public void DeleteAsset(PokemonConfig pokemonConfig)
        {
            var assetPath = AssetDatabase.GetAssetPath(pokemonConfig);
            if (assetPath.IsNullOrEmpty())
            {
                Debug.LogWarning($"The PokemonConfig Asset with name {pokemonConfig.pokemonName} was not found.");
                return;
            }

            AssetDatabase.DeleteAsset(assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public List<PokemonConfig> FindAll()
        {
            var guids = AssetDatabase.FindAssets($"t:{nameof(PokemonConfig)}", new[] { _folderPath });
            var paths = guids.Select(AssetDatabase.GUIDToAssetPath);
            var assets = paths.Select(AssetDatabase.LoadAssetAtPath<PokemonConfig>)
                .OrderBy(config => config.ID).ToList();
            return assets;
        }

        public List<PokemonConfig> FindByName(string text)
        {
            var guids = AssetDatabase.FindAssets($"t:{nameof(PokemonConfig)}", new[] { _folderPath });
            var paths = guids.Select(AssetDatabase.GUIDToAssetPath);
            var assets = paths.Select(AssetDatabase.LoadAssetAtPath<PokemonConfig>)
                .Where(config =>
                {
                    var name = config.pokemonName.ToLower();
                    return name.Contains(text.ToLower());
                })
                .OrderBy(config => config.ID).ToList();
            return assets;
        }
    }
}