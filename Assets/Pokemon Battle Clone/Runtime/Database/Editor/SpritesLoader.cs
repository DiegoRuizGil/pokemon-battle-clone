using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using PokeApiNet;
using UnityEditor;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.TeamBuilder
{
    public enum SpriteType
    {
        Back, Front, Icon
    }
    
    public class SpritesLoader
    {
        private readonly string _basePath;
        private readonly HttpClient _httpClient;
        
        private const string BackFolder = "Back";
        private const string FrontFolder = "Front";
        private const string IconFolder = "Icon";
        
        public SpritesLoader(string basePath)
        {
            _basePath = basePath;
            _httpClient = new HttpClient();
        }

        public async Task<Sprite> LoadSprite(PokeApiNet.Pokemon pokemon, SpriteType spriteType)
        {
            var filePath = BuildFilePath(pokemon.Id.ToString(), spriteType);
            
            if (File.Exists(filePath))
                return Load(pokemon.Id.ToString(), spriteType);
            return await LoadFromAPI(pokemon, spriteType);
        }

        private async Task<Sprite> LoadFromAPI(PokeApiNet.Pokemon pokemon, SpriteType spriteType)
        {
            var url = spriteType switch
            {
                SpriteType.Back => pokemon.Sprites.BackDefault,
                SpriteType.Front => pokemon.Sprites.FrontDefault,
                SpriteType.Icon => pokemon.Sprites.Versions.GenerationVIII.Icons.FrontDefault,
                _ => pokemon.Sprites.FrontDefault
            };

            if (string.IsNullOrEmpty(url))
                return null;

            var relativePath = $"{_basePath}/{GetSubFolder(spriteType)}/{pokemon.Id}.png";
            var filePath = Application.dataPath + "/" + relativePath.Replace("Assets/", "");

            await DownloadSprite(url, filePath);
            
            AssetDatabase.ImportAsset(relativePath);
            
            return Load(pokemon.Id.ToString(), spriteType);
        }

        private async Task DownloadSprite(string url, string filePath)
        {
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogWarning("Sprite url is null or empty");
                return;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            
            var bytes = await _httpClient.GetByteArrayAsync(url);
            await File.WriteAllBytesAsync(filePath, bytes);
        }

        public Dictionary<uint, Sprite> LoadAllFront(IList<uint> ids) => LoadAllOfType(ids, SpriteType.Front);
        public Dictionary<uint, Sprite> LoadAllBack(IList<uint> ids) => LoadAllOfType(ids, SpriteType.Back);
        public Dictionary<uint, Sprite> LoadAllIcon(IList<uint> ids) => LoadAllOfType(ids, SpriteType.Icon);
        
        private Dictionary<uint, Sprite> LoadAllOfType(IList<uint> ids, SpriteType type)
        {
            var sprites = new Dictionary<uint, Sprite>();
            foreach (var id in ids)
                sprites.Add(id, Load(id.ToString(), type));
            return sprites;
        }
        
        private Sprite Load(string id, SpriteType type)
        {
            string subFolder = GetSubFolder(type);
            var path = $"{_basePath}/{subFolder}/{id}.png";

            var sprite = (Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));
            
            if (sprite == null)
                Debug.Log($"Sprite with id:{id} was not found, at route:{path}");
            
            return sprite;
        }

        private string BuildFilePath(string pokemonId, SpriteType spriteType)
        {
            var relativePath = $"{_basePath}/{GetSubFolder(spriteType)}/{pokemonId}.png";
            var filePath = Application.dataPath + "/" + relativePath.Replace("Assets/", "");

            return filePath;
        }

        private string GetSubFolder(SpriteType type)
        {
            return type switch
            {
                SpriteType.Back => BackFolder,
                SpriteType.Front => FrontFolder,
                SpriteType.Icon => IconFolder,
                _ => ""
            };
        }
    }
}