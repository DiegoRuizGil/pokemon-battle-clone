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

        private Dictionary<int, PokemonConfig> _db;
        private Dictionary<int, PokemonConfig> DB
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
            _db = new Dictionary<int, PokemonConfig>();
            if (pokemonConfigs != null)
            {
                foreach (var config in pokemonConfigs)
                    _db[config.ID] = config;
            }
        }

        public PokemonConfig GetPokemonConfig(int id) => DB[id];
        public Sprite GetBackSprite(int id) => DB[id].backSprite;
        public Sprite GetFrontSprite(int id) => DB[id].frontSprite;
        public Sprite GetIconSprite(int id) => DB[id].iconSprite;
        
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