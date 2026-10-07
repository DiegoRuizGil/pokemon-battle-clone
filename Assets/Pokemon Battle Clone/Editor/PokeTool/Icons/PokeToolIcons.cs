using System;
using System.Collections.Generic;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Moves.Domain;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool.Icons
{
    public static class PokeToolIcons
    {
        private const string IconPath = "Assets/Pokemon Battle Clone/Editor/Icons";

        private static readonly Dictionary<string, Texture2D> Cache = new();

        public static Image GetImage(string id, int size = 20)
        {
            var texture = GetTexture(id);

            var image = new Image
            {
                image = texture,
                scaleMode = ScaleMode.ScaleToFit
            };
            
            image.style.width = size;
            image.style.height = size;

            return image;
        }

        public static string TypeToIconId(ElementalType type) => type switch
        {
            ElementalType.Bug => "type-bug",
            ElementalType.Dark => "type-dark",
            ElementalType.Dragon => "type-dragon",
            ElementalType.Electric => "type-electric",
            ElementalType.Fairy => "type-fairy",
            ElementalType.Fighting => "type-fighting",
            ElementalType.Fire => "type-fire",
            ElementalType.Flying => "type-flying",
            ElementalType.Ghost => "type-ghost",
            ElementalType.Grass => "type-grass",
            ElementalType.Ground => "type-ground",
            ElementalType.Ice => "type-ice",
            ElementalType.Normal => "type-normal",
            ElementalType.Poison => "type-poison",
            ElementalType.Psychic => "type-psychic",
            ElementalType.Rock => "type-rock",
            ElementalType.Steel => "type-steel",
            ElementalType.Water => "type-water",
            _ => throw new ArgumentException("Invalid type", nameof(type))
        };

        public static string MoveCategoryToIconId(MoveCategory category) => category switch
        {
            MoveCategory.Status => "move-status",
            MoveCategory.Physical => "move-physical",
            MoveCategory.Special => "move-special",
            _ => throw new ArgumentException("Invalid category", nameof(category))
        };

        public static Texture2D GetTexture(string id)
        {
            if (Cache.TryGetValue(id, out var texture))
            {
                // the asset may have been deleted or unload
                if (texture != null)
                    return texture;

                Cache.Remove(id);
            }

            var path = $"{IconPath}/{id}.png";

            texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

            if (texture == null)
            {
                Debug.LogWarning($"Icon not found: {path}");
                return null;
            }
            
            Cache[id] = texture;
            return texture;
        }
    }
}