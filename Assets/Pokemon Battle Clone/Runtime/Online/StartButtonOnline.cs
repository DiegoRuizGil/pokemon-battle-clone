using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online
{
    public class StartButtonOnline : MonoBehaviour
    {
        [SerializeField] private LobbySettings lobbySettings;
        [SerializeField] private string battleSceneName;

        public void StartBattle()
        {
            lobbySettings.runner.LoadScene(battleSceneName);
        }
    }
}