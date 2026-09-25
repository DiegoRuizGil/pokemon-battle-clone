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
        public const string MoveConfigs = Root + "/Database/Moves";

        public const int CustomContentIdStart = 11_000;
    }

    public static class PokemonSpritePaths
    {
        private const string DefaultName = "default";
        
        public static string GetPath(int pokemonId, SpriteType type) => BuildPath(type, pokemonId.ToString());
        public static string GetDefaultPath(SpriteType type) => BuildPath(type, DefaultName);

        private static string BuildPath(SpriteType type, string fileName)
            => $"{ProjectPaths.PokemonSprites}/{GetFolder(type)}/{fileName}.png";
        
        private static string GetFolder(SpriteType type) => type switch
        {
            SpriteType.Back => "Back",
            SpriteType.Front => "Front",
            SpriteType.Icon => "Icon",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}