using System;
using Pokemon_Battle_Clone.Editor.Database;
using UnityEditor;
using UnityEngine;

namespace Pokemon_Battle_Clone.Editor.CustomAssetsImporter
{
    public class SpriteImportPostprocessor : AssetPostprocessor
    {
        private void OnPreprocessTexture()
        {
            var importer = assetImporter as TextureImporter;

            if (!ShouldApplySettings()) return;

            ApplySpriteSettings(importer);
        }

        private bool ShouldApplySettings()
        {
            return assetPath.StartsWith(ProjectPaths.PokemonSprites, StringComparison.Ordinal);
        }

        private void ApplySpriteSettings(TextureImporter importer)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.spritePixelsPerUnit = 100;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.filterMode = FilterMode.Point;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
        }
    }
}