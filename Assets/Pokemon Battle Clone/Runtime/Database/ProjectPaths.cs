using System;

namespace Pokemon_Battle_Clone.Runtime.Database
{
    public enum SpriteType
    {
        Back, Front, Icon
    }
    
    public static class ProjectPaths
    {
        public const string Root = "Assets/Pokemon Battle Clone";
        public const string PokemonConfigs = Root + "/Database/Pokemon";
        public const string PokemonSprites = Root + "/Sprites/Pokemon";
    }

    public static class PokemonSpritePaths
    {
        public static string GetPath(int pokemonId, SpriteType type)
            => $"{ProjectPaths.PokemonSprites}/{GetFolder(type)}/{pokemonId}.png";

        private static string GetFolder(SpriteType type) => type switch
        {
            SpriteType.Back => "Back",
            SpriteType.Front => "Front",
            SpriteType.Icon => "Icon",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}