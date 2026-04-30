using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pokemon_Battle_Clone.Runtime.Core.Infrastructure
{
    public class LoadSceneComponent : MonoBehaviour
    {
        [SerializeField] private string sceneName;

        public void LoadScene() => SceneManager.LoadScene(sceneName);
    }
}