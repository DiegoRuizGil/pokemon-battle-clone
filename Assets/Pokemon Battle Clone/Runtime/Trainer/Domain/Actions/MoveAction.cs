using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Moves.Domain;
using Pokemon_Battle_Clone.Runtime.RNG;

namespace Pokemon_Battle_Clone.Runtime.Trainer.Domain.Actions
{
    public class MoveAction : TrainerAction
    {
        public override int Priority { get; }

        private readonly Move _move;

        public MoveAction(Side side, int pokemonInFieldSpeed, Move move)
            : base(side, pokemonInFieldSpeed)
        {
            _move = move;
            Priority = move.Priority;
        }
        
        public override TrainerActionResult Execute(Battle battle)
        {
            var user = battle.GetFirstPokemon(Side);
            var target = battle.GetOpponentFirstPokemon(Side);

            var targetInitialHealth = target.Health.Current;
            var hit = battle.Random.Roll(_move.Accuracy);
            
            if (hit) _move.Execute(user, target, battle.Random);

            return new MoveActionResult
            {
                Side = Side,
                MoveName = _move.Name,
                UserName = user.Name,
                Failed = hit,
                TargetFainted = target.Defeated,
                TargetDamaged = targetInitialHealth > target.Health.Current,
            };
        }
    }
}