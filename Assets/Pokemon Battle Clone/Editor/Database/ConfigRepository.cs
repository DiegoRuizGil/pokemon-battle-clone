using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pokemon_Battle_Clone.Editor.Database
{
    public class ConfigRepository<T> where T : ScriptableObject
    {
        private readonly string _folderPath;
        private readonly Func<T, string> _getName;
        private readonly Func<T, string> _getFileName;
        private readonly Func<T, IComparable> _orderBy;

        public ConfigRepository(
            string folderPath,
            Func<T, string> getName,
            Func<T, string> getFileName = null,
            Func<T, IComparable> orderBy = null
        )
        {
            _folderPath = folderPath;
            _getName = getName;
            _getFileName = getFileName ?? getName;
            _orderBy = orderBy ?? getName;
        }

        public void CreateAsset(T config)
        {
            var assetPath = Path.Combine(_folderPath, $"{_getFileName(config)}.asset");
            AssetDatabase.CreateAsset(config, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public void DeleteAsset(T config)
        {
            var assetPath = AssetDatabase.GetAssetPath(config);
            if (string.IsNullOrEmpty(assetPath))
            {
                Debug.LogWarning($"The {typeof(T).Name} '{_getFileName(config)}' was not found.");
                return;
            }
            AssetDatabase.DeleteAsset(assetPath);
            AssetDatabase.Refresh();
        }

        public List<T> FindAll()
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { _folderPath });
            return guids.Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<T>)
                .OrderBy(_orderBy).ToList();
        }

        public List<T> FindByName(string text)
            => FindAll().Where(c => _getName(c).ToLower().Contains(text.ToLower())).ToList();

        public bool IsValidName(string name)
            => FindAll().TrueForAll(c => !string.Equals(_getName(c), name, StringComparison.OrdinalIgnoreCase));
    }
}