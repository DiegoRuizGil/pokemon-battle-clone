using System;
using Fusion;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online
{
    [CreateAssetMenu(menuName = "Pokemon Battle Clone/Online/Lobby Session", fileName = "Lobby Session")]
    public class LobbySession : ScriptableObject
    {
        public string myCode;
        public NetworkRunner runner;
        public PlayerRef LocalPlayer => runner.LocalPlayer;
        
        public event Action<PlayerRef> OnPlayerJoined = delegate { };
        public event Action<PlayerRef> OnPlayerLeft = delegate { };
        
        public void RaisePlayerJoined(PlayerRef player) => OnPlayerJoined.Invoke(player);
        public void RaisePlayerLeft(PlayerRef player) => OnPlayerLeft.Invoke(player);
    }
}