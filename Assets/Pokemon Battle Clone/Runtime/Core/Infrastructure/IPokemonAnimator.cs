using System.Threading.Tasks;

namespace Pokemon_Battle_Clone.Runtime.Core.Infrastructure
{
    public interface IPokemonAnimator
    {
        Task PlayAttackAnimation();
        Task PlayHitAnimation();
        Task PlayFaintAnimation();
    }
}