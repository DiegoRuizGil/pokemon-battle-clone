using Pokemon_Battle_Clone.Runtime.Builders;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Moves.Domain.Effects;
using Pokemon_Battle_Clone.Runtime.Stats.Domain;

namespace Pokemon_Battle_Clone.Runtime.Moves.Domain
{
    public static class MoveFactory
    {
        public static Move IceFang()
        {
            return A.Move.WithName("Ice Fang")
                .WithAccuracy(95)
                .WithPower(65)
                .WithPP(15)
                .WithCategory(MoveCategory.Physical)
                .WithType(ElementalType.Ice)
                .WithMainEffect(new DamageEffect());
        }

        public static Move WaterGun()
        {
            return A.Move.WithName("Water Gun")
                .WithAccuracy(100)
                .WithPower(40)
                .WithPP(25)
                .WithCategory(MoveCategory.Special)
                .WithType(ElementalType.Water)
                .WithMainEffect(new DamageEffect());
        }

        public static Move QuickAttack()
        {
            return A.Move.WithName("Quick Attack")
                .WithAccuracy(100)
                .WithPower(40)
                .WithPP(30)
                .WithCategory(MoveCategory.Physical)
                .WithType(ElementalType.Normal)
                .WithPriority(1)
                .WithMainEffect(new DamageEffect());
        }

        public static Move WingAttack()
        {
            return A.Move.WithName("Wing attack")
                .WithAccuracy(100)
                .WithPower(60)
                .WithPP(35)
                .WithCategory(MoveCategory.Physical)
                .WithType(ElementalType.Flying)
                .WithMainEffect(new DamageEffect());
        }

        public static Move Leer()
        {
            var statsModiferEffect = new StatsModifierEffect(applyToTarget: true, new StatSet(0, 1, -1, 2, -2, 3));
            return A.Move.WithName("Leer")
                .WithAccuracy(100)
                .WithPP(30)
                .WithCategory(MoveCategory.Status)
                .WithType(ElementalType.Normal)
                .WithMainEffect(statsModiferEffect);
        }
        
        public static Move MegaNerf()
        {
            var statsModifierEffect = new StatsModifierEffect(applyToTarget: true, new StatSet(0, -1, -1, -1, -1, -1));
            return A.Move.WithName("Mega Nerf")
                .WithAccuracy(100)
                .WithPP(30)
                .WithCategory(MoveCategory.Status)
                .WithType(ElementalType.Normal)
                .WithMainEffect(statsModifierEffect);
        }

        public static Move ShadowBall()
        {
            var statsModifierEffect = new StatsModifierEffect(applyToTarget: true, new StatSet(0, 0, 0, 0, -1, 0));
            return A.Move.WithName("Shadow Ball")
                .WithAccuracy(100)
                .WithPower(80)
                .WithPP(15)
                .WithCategory(MoveCategory.Special)
                .WithType(ElementalType.Ghost)
                .WithMainEffect(new DamageEffect())
                .WithAdditionalEffect(statsModifierEffect, chancePercent: 20);
        }
    }
}