using System.Collections.Generic;
using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Battles.Domain.Events;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Moves.Domain.Effects
{
    [System.Serializable]
    public class RecoverEffect : IMoveEffect
    {
        [SerializeField] private int _percentage;

        public RecoverEffect(int percentage)
        {
            _percentage = percentage;
        }
        
        public IList<IBattleEvent> Apply(Move move, Battle battle, Side side)
        {
            var user = battle.GetFirstPokemon(side);
            var regainedHealth = user.Health.Max * _percentage / 100;
            
            user.Health.Recover(regainedHealth);
            
            return new List<IBattleEvent> { new RecoverHealthEvent(user.Name, user.Health, side) };
        }
    }
}