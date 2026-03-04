using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.CustomLogs
{
    public static class LogManager
    {
        private const string LOGS_PATH = "Assets/Pokemon Battle Clone/Log Data.asset";
        private static FeatureLogData _logData;

        private static void LoadDebugConfig()
        {
#if UNITY_EDITOR
            _logData = UnityEditor.AssetDatabase.LoadAssetAtPath<FeatureLogData>(LOGS_PATH);
#endif
            if (_logData == null)
                Debug.LogError($"Log Data was not found at: {LOGS_PATH}");
        }

        private static void LogConsole(string text, FeatureType feature)
        {
            var featureLog = _logData.GetFeatureLog(feature);

            if (featureLog is { enabled: true })
            {
                var hexColor = ToHex(featureLog.customColor);
                string header = $"[<b><color={hexColor}>{feature}</color></b>]";
                Debug.Log($"{header} {text}");
            }
        }

        private static void LogWarningConsole(string text, FeatureType feature)
        {
            var featureLog = _logData.GetFeatureLog(feature);

            if (featureLog is { enabled: true })
            {
                var hexColor = ToHex(featureLog.customColor);
                string header = $"[<b><color={hexColor}>{feature}</color></b>]";
                Debug.LogWarning($"{header} {text}");
            }
        }
        
        private static void LogErrorConsole(string text, FeatureType feature)
        {
            var featureLog = _logData.GetFeatureLog(feature);

            if (featureLog is { enabled: true })
            {
                var hexColor = ToHex(featureLog.customColor);
                string header = $"[<b><color={hexColor}>{feature}</color></b>]";
                Debug.LogError($"{header} {text}");
            }
        }

        private static string ToHex(Color color)
        {
            var col = (Color32)color;
            return $"#{col.r:x2}{col.g:x2}{col.b:x2}";
        }
        
        public static void Log(string text, FeatureType feature)
        {
#if UNITY_EDITOR
            if (_logData == null)
                LoadDebugConfig();
            
            LogConsole(text, feature);
#endif
        }

        public static void LogWarning(string text, FeatureType feature)
        {
#if UNITY_EDITOR
            if (_logData == null)
                LoadDebugConfig();
            
            LogWarningConsole(text, feature);
#endif
        }
        
        public static void LogError(string text, FeatureType feature)
        {
#if UNITY_EDITOR
            if (_logData == null)
                LoadDebugConfig();
            
            LogErrorConsole(text, feature);
#endif
        }
    }
}