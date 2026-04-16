using System.Collections.Generic;
using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Battles.Domain.Events;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Moves.Domain;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Trainers.Domain.Actions
{
    public class MoveAction : TrainerAction
    {
        public override int Priority { get; }
        public Move Move { get; }

        public MoveAction(Side side, int pokemonInFieldSpeed, Move move)
            : base(side, pokemonInFieldSpeed)
        {
            Move = move;
            Priority = move.Priority;
        }
        
        public override IEnumerable<IBattleEvent> Execute(Battle battle)
        {
            var events = new List<IBattleEvent>();

            var hit = battle.Random.Roll(Move.Accuracy);
            if (!hit)
            {
                var pokemon = battle.GetFirstPokemon(Side);
                events.Add(new FailedMoveEvent(pokemon.Name, Move.Name));
                return events;
            }
            
            var moveEvents =  Move.Execute(battle, Side);
            events.AddRange(moveEvents);
            return events;
        }
    }
}