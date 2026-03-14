using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Core.Infrastructure;

namespace Pokemon_Battle_Clone.Runtime.Battles.Control
{
    public interface IBattleContext
    {
        ITeamView GetTeamView(Side side);
    }
}