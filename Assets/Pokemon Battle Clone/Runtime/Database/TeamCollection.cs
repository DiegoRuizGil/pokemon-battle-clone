using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Database
{
    [CreateAssetMenu(menuName = "Pokemon Battle Clone/Database/Team Collection", fileName = "Team Collection")]
    public class TeamCollection : ScriptableObject
    {
        [SerializeField] private List<TeamConfig> teamConfigs = new List<TeamConfig>();
        
        public List<TeamConfig> TeamConfigs => teamConfigs;
        
        public int IndexOf(TeamConfig teamConfig) => teamConfigs.IndexOf(teamConfig);
        public TeamConfig this[int index] => teamConfigs[index];
        
#if UNITY_EDITOR
        [ContextMenu("Load Team Configs")]
        private void LoadPokemonConfigs()
        {
            const string configsPath = "Assets/Pokemon Battle Clone/Database/Teams";
            var guids = AssetDatabase.FindAssets($"t:{nameof(TeamConfig)}", new [] { configsPath });
            var paths = guids.Select(AssetDatabase.GUIDToAssetPath);
            var configs = paths.Select(AssetDatabase.LoadAssetAtPath<TeamConfig>);

            teamConfigs = configs.ToList();
        }
#endif
    }
}