using System.Collections.Generic;
using System.Linq;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEditor;

namespace Pokemon_Battle_Clone.Editor.PokeTool
{
    public static class MovesReconfiguration
    {
        [MenuItem("PokeTool/Adjust Move Assets")]
        public static async void AdjustMoveAssets()
        {
            var moves = FindAll();
            foreach (var moveConfig in moves)
            {
                await moveConfig.LoadFromAPI();
                EditorUtility.SetDirty(moveConfig);
            }
            
            AssetDatabase.SaveAssets();
        }

        private static List<MoveConfig> FindAll()
        {
            var guids = AssetDatabase.FindAssets($"t:{nameof(MoveConfig)}", new[] { ProjectPaths.MoveConfigs });
            var paths = guids.Select(AssetDatabase.GUIDToAssetPath);
            var assets = paths.Select(AssetDatabase.LoadAssetAtPath<MoveConfig>).ToList();
            return assets;
        }

        private static void UpdateMoveAssetName(MoveConfig moveConfig)
        {
            var assetPath = AssetDatabase.GetAssetPath(moveConfig);
            var newName = moveConfig.moveName.ToLowerInvariant().Replace(" ", "-");

            AssetDatabase.RenameAsset(assetPath, newName);
        }
    }
}