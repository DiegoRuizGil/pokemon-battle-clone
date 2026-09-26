using System.Collections.Generic;
using Pokemon_Battle_Clone.Runtime.Builders;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Moves.Domain.Effects;
using UnityEngine;
using Move = Pokemon_Battle_Clone.Runtime.Moves.Domain.Move;
using MoveCategory = Pokemon_Battle_Clone.Runtime.Moves.Domain.MoveCategory;

namespace Pokemon_Battle_Clone.Runtime.Database
{
    [CreateAssetMenu(menuName = "Pokemon Battle Clone/Database/Move", fileName = "Move Config")]
    public class MoveConfig : ScriptableObject
    {
        public int id;
        public string moveName;
        public ElementalType type;
        public MoveCategory category;
        [Min(0)] public int pp;
        [Min(0)] public int accuracy;
        [Min(0)] public int power;
        public int priority;
        [SerializeReference, SubclassSelector] public IMoveEffect mainEffect;
        public List<ConditionalEffect> additionalEffects;

        public Move Build()
        {
            return A.Move.WithId(id)
                .WithName(moveName)
                .WithAccuracy((uint)accuracy)
                .WithPower((uint)power)
                .WithPP((uint)pp)
                .WithPriority(priority)
                .WithCategory(category)
                .WithType(type)
                .WithMainEffect(mainEffect)
                .WithAdditionalEffects(additionalEffects);
        }
    }
}