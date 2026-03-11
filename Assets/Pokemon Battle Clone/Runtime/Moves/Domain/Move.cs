using System.Collections.Generic;
using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Battles.Domain.Events;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Moves.Domain.Effects;
using UnityEngine.Assertions;

namespace Pokemon_Battle_Clone.Runtime.Moves.Domain
{
    public enum MoveCategory
    {
        Physical, Special, Status
    }
    
    public class Move
    {
        public string Name { get; }
        public ElementalType Type { get; }
        public MoveCategory Category { get; }
        public ClampedInt PP { get; private set; }
        public int Accuracy { get; }
        public int Power { get; }
        public int Priority { get; }

        private readonly IMoveEffect _mainEffect;
        private readonly List<ConditionalEffect> _additionalEffects = new List<ConditionalEffect>();

        public Move(string name, ElementalType type, MoveCategory category, uint pp, uint accuracy, uint power, int priority, IMoveEffect mainEffect)
        {
            Name = name;
            Type = type;
            Category = category;
            PP = new ClampedInt((int)pp, 0, (int)pp);
            Accuracy = (int)accuracy;
            Power = (int)power;
            Priority = priority;
            _mainEffect = mainEffect;
        }

        public void AddEffects(IEnumerable<ConditionalEffect> effects)
        {
            foreach (var effect in effects)
                AddEffect(effect);
        }
        
        public void AddEffect(ConditionalEffect effect) => _additionalEffects.Add(effect);
        
        public IEnumerable<IBattleEvent> Execute(Battle  battle, Side side)
        {
            Assert.IsTrue(PP > 0);
            PP.Value--;
            
            var events = new List<IBattleEvent>();
            events.Add(new ExecuteMoveEvent(side, battle.GetFirstPokemon(side).Name, this.Name));

            var target = battle.GetOpponentFirstPokemon(side);
            if (target.Type1.IsImmuneTo(this.Type) || target.Type2.IsImmuneTo(this.Type))
            {
                events.Add(new ImmuneMoveEvent(target.Name));
                return events;
            }
            
            var effectEvent = _mainEffect.Apply(move: this, battle, side);
            events.Add(effectEvent);

            foreach (var effect in _additionalEffects)
            {
                var additionalEvent = effect.TryApply(move: this, battle, side);
                events.Add(additionalEvent);
            }

            return events;
        }
    }
}