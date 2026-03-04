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
            var events = new List<IBattleEvent>();
            
            events.Add(new WithdrawPokemon(Side, team.FirstPokemon.Name));
            team.SwapActivePokemon(_pokemonIndex);
            events.Add(new SendPokemonEvent(Side, team.FirstPokemon.Name));

            return events;
        }
    }
}