using System.Collections.Generic;

namespace Pokemon_Battle_Clone.Runtime.Core.Domain
{
    public enum ElementalType
    {
        None, Bug, Dark, Dragon, Electric, Fairy, Fighting, Fire, Flying, Ghost, Grass, Ground, Ice,
        Normal, Poison, Psychic, Rock, Steel, Water
    }

    public static class ElementalTypeUtils
    {
        private static readonly Dictionary<ElementalType, List<ElementalType>> Weaknesses =
            new Dictionary<ElementalType, List<ElementalType>>()
            {
                {
                    ElementalType.Bug,
                    new List<ElementalType> { ElementalType.Fire, ElementalType.Rock, ElementalType.Flying }
                },
                {
                    ElementalType.Dark,
                    new List<ElementalType> { ElementalType.Bug, ElementalType.Fairy, ElementalType.Fighting }
                },
                {
                    ElementalType.Dragon,
                    new List<ElementalType> { ElementalType.Dragon, ElementalType.Fairy, ElementalType.Ice }
                },
                { ElementalType.Electric, new List<ElementalType> { ElementalType.Ground } },
                { ElementalType.Fairy, new List<ElementalType> { ElementalType.Steel, ElementalType.Poison } },
                {
                    ElementalType.Fighting,
                    new List<ElementalType> { ElementalType.Fairy, ElementalType.Psychic, ElementalType.Flying }
                },
                {
                    ElementalType.Fire,
                    new List<ElementalType> { ElementalType.Water, ElementalType.Rock, ElementalType.Ground }
                },
                {
                    ElementalType.Flying,
                    new List<ElementalType> { ElementalType.Electric, ElementalType.Ice, ElementalType.Rock }
                },
                { ElementalType.Ghost, new List<ElementalType> { ElementalType.Ghost, ElementalType.Dark } },
                {
                    ElementalType.Grass,
                    new List<ElementalType>
                    {
                        ElementalType.Bug, ElementalType.Fire, ElementalType.Ice, ElementalType.Poison,
                        ElementalType.Flying
                    }
                },
                {
                    ElementalType.Ground,
                    new List<ElementalType> { ElementalType.Water, ElementalType.Ice, ElementalType.Grass }
                },
                {
                    ElementalType.Ice,
                    new List<ElementalType>
                        { ElementalType.Steel, ElementalType.Fire, ElementalType.Fighting, ElementalType.Rock }
                },
                { ElementalType.Normal, new List<ElementalType> { ElementalType.Fighting } },
                { ElementalType.Poison, new List<ElementalType> { ElementalType.Psychic, ElementalType.Ground } },
                {
                    ElementalType.Psychic,
                    new List<ElementalType> { ElementalType.Bug, ElementalType.Ghost, ElementalType.Dark }
                },
                {
                    ElementalType.Rock,
                    new List<ElementalType>
                    {
                        ElementalType.Steel, ElementalType.Water, ElementalType.Fighting, ElementalType.Grass,
                        ElementalType.Grass
                    }
                },
                {
                    ElementalType.Steel,
                    new List<ElementalType> { ElementalType.Fire, ElementalType.Fighting, ElementalType.Ground }
                },
                { ElementalType.Water, new List<ElementalType> { ElementalType.Electric, ElementalType.Grass } }
            };

        private static readonly Dictionary<ElementalType, List<ElementalType>> Resistances =
            new Dictionary<ElementalType, List<ElementalType>>()
            {
                {
                    ElementalType.Bug,
                    new List<ElementalType> { ElementalType.Fighting, ElementalType.Grass, ElementalType.Ground }
                },
                { ElementalType.Dark, new List<ElementalType> { ElementalType.Ghost, ElementalType.Dark } },
                {
                    ElementalType.Dragon,
                    new List<ElementalType>
                        { ElementalType.Water, ElementalType.Electric, ElementalType.Fire, ElementalType.Grass }
                },
                {
                    ElementalType.Electric,
                    new List<ElementalType> { ElementalType.Steel, ElementalType.Electric, ElementalType.Flying }
                },
                {
                    ElementalType.Fairy,
                    new List<ElementalType> { ElementalType.Bug, ElementalType.Fighting, ElementalType.Dark }
                },
                {
                    ElementalType.Fighting,
                    new List<ElementalType> { ElementalType.Bug, ElementalType.Rock, ElementalType.Dark }
                },
                {
                    ElementalType.Fire,
                    new List<ElementalType>
                    {
                        ElementalType.Steel, ElementalType.Bug, ElementalType.Fire, ElementalType.Fairy,
                        ElementalType.Ice, ElementalType.Grass
                    }
                },
                {
                    ElementalType.Flying,
                    new List<ElementalType> { ElementalType.Bug, ElementalType.Fighting, ElementalType.Grass }
                },
                { ElementalType.Ghost, new List<ElementalType> { ElementalType.Bug, ElementalType.Poison } },
                {
                    ElementalType.Grass,
                    new List<ElementalType>
                        { ElementalType.Water, ElementalType.Electric, ElementalType.Grass, ElementalType.Ground }
                },
                { ElementalType.Ground, new List<ElementalType> { ElementalType.Rock, ElementalType.Poison } },
                { ElementalType.Ice, new List<ElementalType> { ElementalType.Ice } },
                { ElementalType.Normal, new List<ElementalType> { } },
                {
                    ElementalType.Poison,
                    new List<ElementalType>
                    {
                        ElementalType.Bug, ElementalType.Fairy, ElementalType.Fighting, ElementalType.Grass,
                        ElementalType.Poison
                    }
                },
                { ElementalType.Psychic, new List<ElementalType> { ElementalType.Fighting, ElementalType.Psychic } },
                {
                    ElementalType.Rock,
                    new List<ElementalType>
                        { ElementalType.Fire, ElementalType.Normal, ElementalType.Poison, ElementalType.Flying }
                },
                {
                    ElementalType.Steel,
                    new List<ElementalType>
                    {
                        ElementalType.Steel, ElementalType.Bug, ElementalType.Dragon, ElementalType.Fairy,
                        ElementalType.Ice, ElementalType.Normal, ElementalType.Grass, ElementalType.Psychic,
                        ElementalType.Rock, ElementalType.Flying
                    }
                },
                {
                    ElementalType.Water,
                    new List<ElementalType>
                        { ElementalType.Steel, ElementalType.Water, ElementalType.Fire, ElementalType.Ice }
                }
            };

        private static readonly Dictionary<ElementalType, List<ElementalType>> Immunities =
            new Dictionary<ElementalType, List<ElementalType>>()
            {
                { ElementalType.Dark, new List<ElementalType> { ElementalType.Psychic } },
                { ElementalType.Fairy, new List<ElementalType> { ElementalType.Dragon } },
                { ElementalType.Flying, new List<ElementalType> { ElementalType.Ground } },
                { ElementalType.Ghost, new List<ElementalType> { ElementalType.Fighting, ElementalType.Normal } },
                { ElementalType.Ground, new List<ElementalType> { ElementalType.Electric } },
                { ElementalType.Normal, new List<ElementalType> { ElementalType.Ghost } },
            };

        public static float EffectivenessAgainst(this ElementalType moveType, ElementalType pokemonType1, ElementalType pokemonType2)
        {
            return moveType.EffectivenessAgainst(pokemonType1) * moveType.EffectivenessAgainst(pokemonType2);
        }

        public static float EffectivenessAgainst(this ElementalType moveType, ElementalType pokemonType)
        {
            if (pokemonType.IsImmuneTo(moveType)) return 0f;
            if (pokemonType.IsWeakTo(moveType)) return 2f;
            if (pokemonType.IsResistanceTo(moveType)) return 0.5f;

            return 1f;
        }

        public static bool IsWeakTo(this ElementalType pokemonType, ElementalType moveType)
        {
            if (Weaknesses.TryGetValue(pokemonType, out var weaknesses))
                if (weaknesses.Contains(moveType))
                    return true;
            return false;
        }

        public static bool IsResistanceTo(this ElementalType pokemonType, ElementalType moveType)
        {
            if (Resistances.TryGetValue(pokemonType, out var resistances))
                if (resistances.Contains(moveType))
                    return true;
            return false;
        }

        public static bool IsImmuneTo(this ElementalType pokemonType, ElementalType moveType)
        {
            if (Immunities.TryGetValue(pokemonType, out var immunities))
                if (immunities.Contains(moveType))
                    return true;
            return false;
        }

        public static ElementalType GetType(string type)
        {
            return type switch
            {
                "bug" => ElementalType.Bug,
                "dark" => ElementalType.Dark,
                "dragon" => ElementalType.Dragon,
                "electric" => ElementalType.Electric,
                "fairy" => ElementalType.Fairy,
                "fighting" => ElementalType.Fighting,
                "fire" => ElementalType.Fire,
                "flying" => ElementalType.Flying,
                "ghost" => ElementalType.Ghost,
                "grass" => ElementalType.Grass,
                "ground" => ElementalType.Ground,
                "ice" => ElementalType.Ice,
                "normal" => ElementalType.Normal,
                "poison" => ElementalType.Poison,
                "psychic" => ElementalType.Psychic,
                "rock" => ElementalType.Rock,
                "steel" => ElementalType.Steel,
                "water" => ElementalType.Water,
                _ => ElementalType.None
            };
        }
    }
}