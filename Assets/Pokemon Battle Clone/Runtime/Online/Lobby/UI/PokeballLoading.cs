using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online.Lobby.UI
{
    public class PokeballLoading : MonoBehaviour
    {
        [SerializeField] private float duration = 1f;

        private float _rotationTime;
        
        private void Update()
        {
            Rotate();
        }

        private void OnEnable() => transform.localRotation = Quaternion.identity;

        private void Rotate()
        {
            _rotationTime += Time.deltaTime;
            
            var linearProgress = (_rotationTime / duration) % 1f;
            var smoothProgress = SmoothStep(linearProgress);
            var angle = smoothProgress * 360f;
            
            transform.localRotation = Quaternion.Euler(0, 0, -angle);
        }

        private float SmoothStep(float t)
        {
            return t * t * (3f - 2f * t);
        }
    }
}