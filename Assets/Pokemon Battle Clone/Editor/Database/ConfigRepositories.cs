using Pokemon_Battle_Clone.Runtime.Database;

namespace Pokemon_Battle_Clone.Editor.Database
{
    public static class ConfigRepositories
    {
        public static ConfigRepository<PokemonConfig> Pokemon()
            => new(ProjectPaths.PokemonConfigs,
                getName: p => p.pokemonName,
                getFileName: p => p.ID.ToString(),
                orderBy: p => p.ID);

        public static ConfigRepository<MoveConfig> Move()
            => new(ProjectPaths.MoveConfigs,
                getName: m => m.moveName,
                getFileName: p => p.moveName.ToLowerInvariant().Replace(" ", "-"),
                orderBy: m => m.id);

        public static ConfigRepository<TeamConfig> Team()
            => new(ProjectPaths.TeamConfigs, getName: t => t.name);
    }
}