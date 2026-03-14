using System.Collections.Generic;
using System.Linq;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.RNG;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain.Actions;

namespace Pokemon_Battle_Clone.Runtime.Battles.Domain
{
    public class Battle
    {
        private readonly Team _playerTeam;
        private readonly Team _rivalTeam;
        
        public IRandom Random { get; }

        public Battle(Team playerTeam, Team rivalTeam, IRandom random)
        {
            _playerTeam = playerTeam;
            _rivalTeam = rivalTeam;
            Random = random;
        }
        public Pokemon GetFirstPokemon(Side side) => GetTeam(side).FirstPokemon;

        public Team GetTeam(Side side)
        {
            return side switch
            {
                Side.Player => _playerTeam,
                Side.Rival => _rivalTeam,
                _ => null
            };
        }

        public bool PokemonFainted(Side side)
        {
            return side switch
            {
                Side.Player => _playerTeam.FirstPokemon.Defeated,
                Side.Rival => _rivalTeam.FirstPokemon.Defeated,
                _ => false
            };
        }
        
        public List<TrainerAction> OrderActions(List<TrainerAction> actionsToOrder)
        {
            return actionsToOrder.OrderByDescending(a => a.Priority)
                .ThenByDescending(a => a.PokemonInFieldSpeed)
                .ThenBy(_ => Random.Next())
                .ToList();
        }
    }
}