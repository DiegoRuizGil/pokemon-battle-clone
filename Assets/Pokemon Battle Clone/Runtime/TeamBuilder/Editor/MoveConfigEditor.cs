using System;
using UnityEditor;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.TeamBuilder.Editor
{
    [CustomEditor(typeof(MoveConfig))]
    [CanEditMultipleObjects]
    public class MoveConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            var move = target as MoveConfig;
            
            GUILayout.Space(10);
            if (GUILayout.Button("Load Data"))
            {
                LoadData(move);
            }
        }

        private async void LoadData(MoveConfig move)
        {
            try
            {
                await move.LoadFromAPI();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}