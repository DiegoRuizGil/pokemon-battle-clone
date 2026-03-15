using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PokeApiNet;
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
        public string moveName;
        public ElementalType type;
        public MoveCategory category;
        [Min(0)] public int pp;
        [Min(0)] public int accuracy;
        [Min(0)] public int power;
        public int priority;
        [SerializeReference, SubclassSelector] public IMoveEffect mainEffect;
        public List<ConditionalEffect> additionalEffects;
        
        private static PokeApiClient _pokeClient;
        private static PokeApiClient PokeClient
        {
            get
            {
                if (_pokeClient == null)
                    _pokeClient = new PokeApiClient();
                return _pokeClient;
            }
        }

        public Move Build()
        {
            return A.Move.WithName(moveName)
                .WithAccuracy((uint)accuracy)
                .WithPower((uint)power)
                .WithPP((uint)pp)
                .WithPriority(priority)
                .WithCategory(category)
                .WithType(type)
                .WithMainEffect(mainEffect)
                .WithAdditionalEffects(additionalEffects);
        }

        public async Task LoadFromAPI()
        {
            try
            {
                var move = await PokeClient.GetResourceAsync<PokeApiNet.Move>(moveName);
                ApplyData(move);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading move \"{moveName}\": {e.Message}");
            }
        }

        private void ApplyData(PokeApiNet.Move move)
        {
            type = ElementalTypeUtils.GetType(move.Type.Name);
            category = GetCategory(move.DamageClass.Name);
            pp = move.Pp ?? 0;
            accuracy = move.Accuracy ?? 100;
            power = move.Power ?? 0;
            priority = move.Priority;
        }

        private MoveCategory GetCategory(string categoryName)
        {
            return categoryName switch
            {
                "physical" => MoveCategory.Physical,
                "special" => MoveCategory.Special,
                "status" => MoveCategory.Status,
                _ => MoveCategory.Physical
            };
        }
    }
}