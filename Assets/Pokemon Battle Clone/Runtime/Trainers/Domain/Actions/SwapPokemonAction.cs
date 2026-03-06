using System.Collections.Generic;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain.BattleEvents;

namespace Pokemon_Battle_Clone.Runtime.Trainers.Domain.Actions
{
    public class SwapPokemonAction : TrainerAction
    {
        public override int Priority => int.MaxValue;

        private readonly int _pokemonIndex;
        private readonly bool _withdrawFirstPokemon;
        
        public SwapPokemonAction(Side side, int pokemonIndex, bool withdrawFirstPokemon)
            : base(side, pokemonInFieldSpeed: int.MaxValue)
        {
            _pokemonIndex = pokemonIndex;
            _withdrawFirstPokemon = withdrawFirstPokemon;
        }

        public override IEnumerable<IBattleEvent> Execute(Battle battle)
        {
            var team = battle.GetTeam(Side);
            var events = new List<IBattleEvent>();
            
            if (_withdrawFirstPokemon)
                events.Add(new WithdrawPokemon(Side, team.FirstPokemon.Name));
            team.SwapActivePokemon(_pokemonIndex);
            events.Add(new SendPokemonEvent(Side, team.FirstPokemon));

            return events;
        }
    }
}