using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEditor;
using UnityEngine;

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
            var fileName = pokemonConfig.ID;
            var assetPath = Path.Combine(_folderPath, $"{fileName}.asset");
            
            AssetDatabase.CreateAsset(pokemonConfig, assetPath);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public void DeleteAsset(PokemonConfig pokemonConfig)
        {
            var assetPath = AssetDatabase.GetAssetPath(pokemonConfig);
            if (string.IsNullOrEmpty(assetPath))
            {
                Debug.LogWarning($"The PokemonConfig '{pokemonConfig.ID}.asset' was not found.");
                return;
            }

            var spritesManager = new SpritesManager(ProjectPaths.PokemonSprites);
            spritesManager.DeleteSprites(pokemonConfig.ID);
            
            AssetDatabase.DeleteAsset(assetPath);
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

        public int GenerateValidId() => FindAll().Max(p => p.ID) + 1;

        public bool IsValidId(int id) => FindAll().TrueForAll(p => p.ID != id);
    }
}