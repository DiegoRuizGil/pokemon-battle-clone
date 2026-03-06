using System.Linq;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.CustomLogs;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain.Actions;

namespace Pokemon_Battle_Clone.Runtime.Trainers.Domain.Strategies
{
    public class BasicTrainerStrategy : ITrainerStrategy
    {
        public MoveAction SelectMove(Team team)
        {
            var pokemon = team.FirstPokemon;
            var move = pokemon.MoveSet.Moves[0];

            LogManager.Log("Move selected", FeatureType.Rival);
            
            return new MoveAction(Side.Rival, pokemon.Stats.Speed, move);
        }

        public SwapPokemonAction SelectPokemon(Team team)
        {
            var firstAlive = team.Bench
                .Select((pokemon, index) => (pokemon: pokemon, index: index + 1))
                .First(p => !p.pokemon.Defeated);
            
            return new SwapPokemonAction(Side.Rival, firstAlive.index, withdrawFirstPokemon: !team.FirstPokemon.Defeated);
        }
    }
}