using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon_Battle_Clone.Runtime.Core.Infrastructure
{
    public class PokemonView : MonoBehaviour, IPokemonAnimator
    {
        [SerializeField] private Image pokemonImage;
        [SerializeField] private Animator animator;

        private TaskCompletionSource<bool> _animationTcs;

        private void Update()
        {
            DebugAnimations();
        }

        private void DebugAnimations()
        {
            if (Input.GetKeyDown(KeyCode.W))
                PlayAttackAnimation();
            if (Input.GetKeyDown(KeyCode.E))
                PlayHitAnimation();
            if (Input.GetKeyDown(KeyCode.R))
                PlayFaintAnimation();
        }

        public void SetSprite(Sprite sprite) => pokemonImage.sprite = sprite;

        public Task PlayAttackAnimation() => PlayAnimation("Attack");
        public Task PlayHitAnimation() => PlayAnimation("Hit");
        public Task PlayFaintAnimation() => PlayAnimation("Faint");
        public Task PlaySendAnimation() => PlayAnimation("Send");
        public Task PlayWithdrawAnimation() => PlayAnimation("Withdraw");

        private Task PlayAnimation(string stateName)
        {
            _animationTcs = new TaskCompletionSource<bool>();
            animator.Play(stateName);
            return _animationTcs.Task;
        }
        
        // animation key frame event
        public void OnAnimationFinished() => _animationTcs.TrySetResult(true);
    }
}