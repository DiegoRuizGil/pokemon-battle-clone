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
        private readonly Dictionary<string, AsyncOperationHandle<Sprite>> _cache = new();
        
        public Task<Sprite> GetBackSprite(uint id) => GetSprite(id, SpriteType.Back);
        public Task<Sprite> GetFrontSprite(uint id) => GetSprite(id, SpriteType.Front);
        public Task<Sprite> GetIconSprite(uint id) => GetSprite(id, SpriteType.Icon);

        public void Dispose()
        {
            foreach (var handle in _cache.Values)
            {
                if (handle.IsValid())
                    Addressables.Release(handle);
            }
            _cache.Clear();
        }

        private async Task<Sprite> GetSprite(uint id, SpriteType type)
        {
            var address = PokemonSpritePaths.GetPath((int)id, type);

            if (!_cache.TryGetValue(address, out var handle))
            {
                handle = Addressables.LoadAssetAsync<Sprite>(address);
                _cache[address] = handle;
            }

            return await handle.Task;
        }
    }
}