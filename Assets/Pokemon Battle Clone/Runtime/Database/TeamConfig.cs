using System.Collections.Generic;
using System.Linq;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Database
{
    [CreateAssetMenu(menuName = "Pokemon Battle Clone/Database/Team", fileName = "Team Config")]
    public class TeamConfig : ScriptableObject
    {
        public List<TeamMember> pokemonList = new();

        public Team Build()
        {
            return new Team(pokemonList.Select(p => p.Build()));
        }
    }
}