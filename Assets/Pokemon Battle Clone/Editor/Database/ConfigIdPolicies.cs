using Pokemon_Battle_Clone.Runtime.Database;

namespace Pokemon_Battle_Clone.Editor.Database
{
    public static class ConfigIdPolicies
    {
        public static ConfigIdPolicy<PokemonConfig> Pokemon(ConfigRepository<PokemonConfig> repo)
            => new(repo, getId: p => p.ID, reservedRangeStart: ProjectPaths.CustomContentIdStart);
        
        public static ConfigIdPolicy<MoveConfig> Move(ConfigRepository<MoveConfig> repo)
            => new(repo, getId: m => m.id, reservedRangeStart: ProjectPaths.CustomContentIdStart);
    }
}