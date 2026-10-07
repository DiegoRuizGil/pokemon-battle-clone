using Pokemon_Battle_Clone.Editor.PokeTool.Icons;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool
{
    public class PokeToolEditorWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset pokeToolVisualTreeAsset;
        
        [MenuItem("PokeTool/Open _%#Q")]
        public static void ShowWindow()
        {
            var window = GetWindow<PokeToolEditorWindow>();
            window.titleContent = new GUIContent("PokeTool", PokeToolIcons.GetTexture("pokeball"));
            window.minSize = new Vector2(200, 50);
        }

        public void CreateGUI()
        {
            // rootVisualElement.Add(new PokeTool());
            pokeToolVisualTreeAsset.CloneTree(rootVisualElement);
        }
    }
}