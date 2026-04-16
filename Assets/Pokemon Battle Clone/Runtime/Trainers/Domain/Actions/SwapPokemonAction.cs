using System.Collections.Generic;
using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Battles.Domain.Events;
using Pokemon_Battle_Clone.Runtime.Core.Domain;

namespace Pokemon_Battle_Clone.Runtime.Trainers.Domain.Actions
{
    public class SwapPokemonAction : TrainerAction
    {
        public override int Priority => int.MaxValue;

        public int PokemonIndex { get; }
        public bool WithdrawFirstPokemon { get; }
        
        public SwapPokemonAction(Side side, int pokemonIndex, bool withdrawFirstPokemon)
            : base(side, pokemonInFieldSpeed: int.MaxValue)
        {
            PokemonIndex = pokemonIndex;
            WithdrawFirstPokemon = withdrawFirstPokemon;
        }

        public override IEnumerable<IBattleEvent> Execute(Battle battle)
        {
            var team = battle.GetTeam(Side);
            var events = new List<IBattleEvent>();
            
            if (WithdrawFirstPokemon)
                events.Add(new WithdrawPokemonEvent(Side, team.FirstPokemon.Name));
            team.SwapActivePokemon(PokemonIndex);
            events.Add(new SendPokemonEvent(Side, team.FirstPokemon));

            return events;
        }
    }
}