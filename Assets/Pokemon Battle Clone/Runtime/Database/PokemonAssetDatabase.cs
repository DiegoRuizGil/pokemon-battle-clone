using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Pokemon_Battle_Clone.Runtime.Database
{
    [CreateAssetMenu(menuName = "Pokemon Battle Clone/Database/Pokemon Asset Database", fileName = "Pokemon Asset Database")]
    public class PokemonAssetDatabase : ScriptableObject
    {
        [SerializeField] private List<PokemonConfig> pokemonConfigs;

        private Dictionary<uint, PokemonConfig> _db;
        private Dictionary<uint, PokemonConfig> DB
        {
            get
            {
                if (_db == null)
                    Initialize();
                return _db;
            }
        }

        private void Initialize()
        {
            _db = new Dictionary<uint, PokemonConfig>();
            if (pokemonConfigs != null)
            {
                foreach (var config in pokemonConfigs)
                    _db[(uint)config.ID] = config;
            }
        }

        public PokemonConfig GetPokemonConfig(uint id) => DB[id];
        public Sprite GetBackSprite(uint id) => DB[id].backSprite;
        public Sprite GetFrontSprite(uint id) => DB[id].frontSprite;
        public Sprite GetIcon(uint id) => DB[id].iconSprite;

        public Dictionary<uint, Sprite> GetBackSpritesOf(List<uint> pokemonIDs)
        {
            var sprites = new Dictionary<uint, Sprite>();
            foreach (var id in pokemonIDs)
                sprites[id] = GetBackSprite(id);
            return sprites;
        }
        
        public Dictionary<uint, Sprite> GetFrontSpritesOf(List<uint> pokemonIDs)
        {
            var sprites = new Dictionary<uint, Sprite>();
            foreach (var id in pokemonIDs)
                sprites[id] = GetFrontSprite(id);
            return sprites;
        }
        
        public Dictionary<uint, Sprite> GetIconsOf(List<uint> pokemonIDs)
        {
            var sprites = new Dictionary<uint, Sprite>();
            foreach (var id in pokemonIDs)
                sprites[id] = GetIcon(id);
            return sprites;
        }
        
#if UNITY_EDITOR
        [ContextMenu("Load Pokemon Configs")]
        private void LoadPokemonConfigs()
        {
            const string configsPath = "Assets/Pokemon Battle Clone/Database/Pokemon";
            var guids = AssetDatabase.FindAssets($"t:{nameof(PokemonConfig)}", new [] { configsPath });
            var paths = guids.Select(AssetDatabase.GUIDToAssetPath);
            var configs = paths.Select(AssetDatabase.LoadAssetAtPath<PokemonConfig>);

            pokemonConfigs = configs.ToList();
        }
#endif
    }
}