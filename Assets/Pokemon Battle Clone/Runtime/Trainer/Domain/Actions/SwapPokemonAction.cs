using System.Collections.Generic;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Trainer.Domain.BattleEvents;

namespace Pokemon_Battle_Clone.Runtime.Trainer.Domain.Actions
{
    public class SwapPokemonAction : TrainerAction
    {
        public override int Priority => int.MaxValue;

        private readonly int _pokemonIndex;
        
        public SwapPokemonAction(Side side, int pokemonIndex)
            : base(side, pokemonInFieldSpeed: int.MaxValue)
        {
            _pokemonIndex = pokemonIndex;
        }

        public override IEnumerable<IBattleEvent> Execute(Battle battle)
        {
            var team = battle.GetTeam(Side);
            team.SwapActivePokemon(_pokemonIndex);

            return new List<IBattleEvent> { new SwapPokemonEvent(Side, team.FirstPokemon.Name) };
        }
    }
}