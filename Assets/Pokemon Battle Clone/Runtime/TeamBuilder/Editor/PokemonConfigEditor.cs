using Pokemon_Battle_Clone.Runtime.Database;
using UnityEditor;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.TeamBuilder.Editor
{
    [CustomEditor(typeof(PokemonConfig))]
    [CanEditMultipleObjects]
    public class PokemonConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            var pokemon = target as PokemonConfig;
            
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            DrawSpritePreview(pokemon.backSprite, "Back");
            DrawSpritePreview(pokemon.frontSprite, "Front");
            DrawSpritePreview(pokemon.iconSprite, "Icon");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            
            if (GUILayout.Button("Load Data"))
            {
                LoadData(pokemon);
            }
            
            Repaint();
        }

        private async void LoadData(PokemonConfig pokemon)
        {
            try
            {
                await pokemon.LoadFromAPI();
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void DrawSpritePreview(Sprite sprite, string label)
        {
            GUILayout.BeginVertical(GUILayout.Width(100));
            
            var centeredStyle = new GUIStyle(EditorStyles.boldLabel);
            centeredStyle.alignment = TextAnchor.MiddleCenter;
            EditorGUILayout.LabelField(label, centeredStyle, GUILayout.Width(100));

            if (sprite != null)
            {
                var preview = AssetPreview.GetAssetPreview(sprite);
                if (preview != null)
                    GUILayout.Label(preview, GUILayout.Width(96), GUILayout.Height(96));
                else
                    GUILayout.Label(sprite.texture, GUILayout.Width(96), GUILayout.Height(96));
                    
            }
            else
            {
                GUILayout.Box("None", GUILayout.Width(96), GUILayout.Height(96));
            }
            
            GUILayout.EndVertical();
        }
    }
}