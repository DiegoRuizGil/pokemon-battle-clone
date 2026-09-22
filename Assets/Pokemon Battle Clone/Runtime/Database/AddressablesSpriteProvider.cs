using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Pokemon_Battle_Clone.Runtime.Database
{
    public class AddressablesSpriteProvider : IPokemonSpriteProvider, IDisposable
    {
        private const string BackFolder = "Back";
        private const string FrontFolder = "Front";
        private const string IconFolder = "Icon";

        private readonly Dictionary<string, AsyncOperationHandle<Sprite>> _cache = new();
        
        public Task<Sprite> GetBackSprite(uint id) => GetSprite(id, BackFolder);
        public Task<Sprite> GetFrontSprite(uint id) => GetSprite(id, FrontFolder);
        public Task<Sprite> GetIconSprite(uint id) => GetSprite(id, IconFolder);

        public void Dispose()
        {
            foreach (var handle in _cache.Values)
            {
                if (handle.IsValid())
                    Addressables.Release(handle);
            }
            _cache.Clear();
        }

        private async Task<Sprite> GetSprite(uint id, string subFolder)
        {
            var address = $"{ProjectPaths.PokemonSprites}/{subFolder}/{id}.png";

            if (!_cache.TryGetValue(address, out var handle))
            {
                handle = Addressables.LoadAssetAsync<Sprite>(address);
                _cache[address] = handle;
            }

            return await handle.Task;
        }
    }
}