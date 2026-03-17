using System;
using System.Threading.Tasks;
using PokeApiNet;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Database;
using Pokemon_Battle_Clone.Runtime.Stats.Domain;
using Pokemon_Battle_Clone.Runtime.TeamBuilder;
using UnityEditor;
using UnityEngine;

namespace Pokemon_Battle_Clone.Editor.Database
{
    [CustomEditor(typeof(PokemonConfig))]
    public class PokemonConfigEditor : UnityEditor.Editor
    {
        private PokemonConfig _target;
        private PokeApiClient _pokeClient;
        private SpritesLoader _spritesLoader;

        private void OnEnable()
        {
            _target = target as PokemonConfig;
            _pokeClient = new PokeApiClient();
            _spritesLoader = new SpritesLoader("Assets/Pokemon Battle Clone/Sprites/Pokemon");
        }

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
                LoadData();
            
            Repaint();
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

        private async void LoadData()
        {
            try
            {
                var pokemon = await _pokeClient.GetResourceAsync<PokeApiNet.Pokemon>(_target.pokemonName);
                
                ApplyPokemonData(pokemon);
                await LoadSprites(pokemon);
                
                EditorUtility.SetDirty(_target);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading pokemon \"{_target.pokemonName}\": {e.Message}");
            }
        }

        private void ApplyPokemonData(PokeApiNet.Pokemon pokemon)
        {
            _target.ID = pokemon.Id;
            _target.baseStats = new StatSet(
                hp: pokemon.Stats[0].BaseStat,
                attack: pokemon.Stats[1].BaseStat,
                defense: pokemon.Stats[2].BaseStat,
                spcAttack: pokemon.Stats[3].BaseStat,
                spcDefense: pokemon.Stats[4].BaseStat,
                speed: pokemon.Stats[5].BaseStat
            );
            _target.type1 = ElementalTypeUtils.GetType(pokemon.Types[0].Type.Name);
            if (pokemon.Types.Count > 1)
                _target.type2 = ElementalTypeUtils.GetType(pokemon.Types[1].Type.Name);
        }
        
        private async Task LoadSprites(PokeApiNet.Pokemon pokemon)
        {
            try
            {
                _target.backSprite = await _spritesLoader.LoadSprite(pokemon, SpriteType.Back);
                _target.frontSprite = await _spritesLoader.LoadSprite(pokemon, SpriteType.Front);
                _target.iconSprite = await _spritesLoader.LoadSprite(pokemon, SpriteType.Icon);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }
}