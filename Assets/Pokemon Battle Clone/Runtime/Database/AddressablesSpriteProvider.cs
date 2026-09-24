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
        private readonly HashSet<string> _missingAddresses = new();
        
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
            _missingAddresses.Clear();
        }

        private async Task<Sprite> GetSprite(uint id, SpriteType type)
        {
            var address = PokemonSpritePaths.GetPath((int)id, type);

            if (!_cache.ContainsKey(address) && !_missingAddresses.Contains(address))
            {
                if (!await AddressExists(address) && _missingAddresses.Add(address))
                    Debug.LogWarning($"Sprite not found at '{address}'. Using the default {type} sprite.");
            }

            if (_missingAddresses.Contains(address))
                address = PokemonSpritePaths.GetDefaultPath(type);
            
            return await Load(address);
        }
        
        private Task<Sprite> Load(string address)
        {
            if (!_cache.TryGetValue(address, out var handle))
            {
                handle = Addressables.LoadAssetAsync<Sprite>(address);
                _cache[address] = handle;
            }

            return handle.Task;
        }

        private static async Task<bool> AddressExists(string address)
        {
            var handle = Addressables.LoadResourceLocationsAsync(address, typeof(Sprite));
            var locations = await handle.Task;
            var exists = locations.Count > 0;
            Addressables.Release(handle);
            return exists;
        }
    }
}