using System;
using System.IO;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEditor;
using UnityEngine;

namespace Pokemon_Battle_Clone.Editor.Database
{
    public enum SpriteSaveResult
    {
        Saved, Skipped
    }
    
    public class PokemonSpritesRepository
    {
        public bool Exists(int pokemonId, SpriteType type)
            => File.Exists(Path.GetFullPath(PokemonSpritePaths.GetPath(pokemonId, type)));

        public Sprite Load(int pokemonId, SpriteType type)
            => AssetDatabase.LoadAssetAtPath<Sprite>(PokemonSpritePaths.GetPath(pokemonId, type));

        public SpriteSaveResult Save(int pokemonId, SpriteType type, byte[] bytes, bool overwrite = false)
        {
            var relativePath = PokemonSpritePaths.GetPath(pokemonId, type);
            var fullPath = Path.GetFullPath(relativePath);

            if (!overwrite && File.Exists(fullPath))
                return SpriteSaveResult.Skipped;

            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
            File.WriteAllBytes(fullPath, bytes);
            AssetDatabase.ImportAsset(relativePath);

            return SpriteSaveResult.Saved;
        }

        public void Delete(int pokemonId)
        {
            var spriteTypes = (SpriteType[])Enum.GetValues(typeof(SpriteType));
            foreach (var type in spriteTypes)
                AssetDatabase.DeleteAsset(PokemonSpritePaths.GetPath(pokemonId, type));
        }
    }
}