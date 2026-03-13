using System.Collections.Generic;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Moves.Domain;
using Pokemon_Battle_Clone.Runtime.Moves.Domain.Effects;

namespace Pokemon_Battle_Clone.Runtime.Builders
{
    public class MoveBuilder : IBuilder<Move>
    {
        private string _name = "???";
        private ElementalType _type = ElementalType.None;
        private MoveCategory _category = MoveCategory.Status;
        private uint _pp;
        private uint _accuracy = 100;
        private uint _power;
        private int _priority;
        private IMoveEffect _mainEffect = new EmptyMoveEffect();
        private readonly List<ConditionalEffect> _effects = new List<ConditionalEffect>();

        public MoveBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public MoveBuilder WithType(ElementalType type)
        {
            _type = type;
            return this;
        }

        public MoveBuilder WithCategory(MoveCategory category)
        {
            _category = category;
            return this;
        }

        public MoveBuilder WithPP(uint pp)
        {
            _pp = pp;
            return this;
        }

        public MoveBuilder WithAccuracy(uint accuracy)
        {
            _accuracy = accuracy;
            return this;
        }

        public MoveBuilder WithPower(uint power)
        {
            _power = power;
            return this;
        }

        public MoveBuilder WithPriority(int priority)
        {
            _priority = priority;
            return this;
        }

        public MoveBuilder WithMainEffect(IMoveEffect effect)
        {
            _mainEffect = effect;
            return this;
        }

        public MoveBuilder WithAdditionalEffect(IMoveEffect effect, int chancePercent)
        {
            _effects.Add(new ConditionalEffect(effect, chancePercent));
            return this;
        }

        public MoveBuilder WithAdditionalEffects(IEnumerable<ConditionalEffect> effects)
        {
            _effects.AddRange(effects);
            return this;
        }
        
        public Move Build()
        {
            var move = new Move(_name, _type, _category, _pp, _accuracy, _power, _priority, _mainEffect);
            move.AddEffects(_effects);
            return move;
        }

        public static implicit operator Move(MoveBuilder builder) => builder.Build();
    }
}