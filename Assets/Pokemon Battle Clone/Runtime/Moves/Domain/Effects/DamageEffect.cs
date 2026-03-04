using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Trainer.Domain.BattleEvents;

namespace Pokemon_Battle_Clone.Runtime.Moves.Domain.Effects
{
    public class DamageEffect : IMoveEffect
    {
        public IBattleEvent Apply(Move move, Battle battle, Side side)
        {
            var damage = GetDamage(move, battle, side);
            
            var target = battle.GetOpponentFirstPokemon(side);
            target.Health.Damage(damage);

            return new DamageEvent(
                side: side,
                targetHealth: target.Health,
                userName: battle.GetFirstPokemon(side).Name,
                targetName: target.Name,
                effectiveness: move.Type.EffectivenessAgainst(target.Type1, target.Type2)
            );
        }

        private int GetDamage(Move move, Battle battle, Side side)
        {
            var user = battle.GetFirstPokemon(side);
            var target = battle.GetOpponentFirstPokemon(side);
            
            var level = user.Stats.Level;
            var attack = user.Stats.GetAttackByCategory(move.Category);
            var defense = target.Stats.GetDefenseByCategory(move.Category);
            var power = move.Power;
            var targets = 1f;
            var weather = 1f;
            var critical = 1f;
            var randomFactor = PokemonUtils.RandomDamageFactor(battle.Random);
            var stab = user.STAB(move.Type);
            var effectiveness = move.Type.EffectivenessAgainst(target.Type1, target.Type2);
            var burn = 1f;
            var other = 1f;

            return PokemonUtils.CalculateDamage(level, attack, defense, power, targets, weather, critical, randomFactor, stab,
                effectiveness, burn, other);
        }
    }
}