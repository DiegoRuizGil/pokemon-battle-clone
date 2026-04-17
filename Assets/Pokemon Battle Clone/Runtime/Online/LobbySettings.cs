using Fusion;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online
{
    [CreateAssetMenu(menuName = "Pokemon Battle Clone/Online/Lobby Settings", fileName = "Lobby Settings")]
    public class LobbySettings : ScriptableObject
    {
        public NetworkRunner runner;
        public PlayerRef LocalPlayer => runner.LocalPlayer;
    }
}