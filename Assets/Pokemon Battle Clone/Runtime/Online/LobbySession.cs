using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online
{
    [CreateAssetMenu(menuName = "Pokemon Battle Clone/Online/Lobby Session", fileName = "Lobby Session")]
    public class LobbySession : ScriptableObject
    {
        public string currentSessionCode;
        
        public event Action<PlayerRef> OnPlayerJoined = delegate { };
        public event Action<PlayerRef> OnPlayerLeft = delegate { };
        public event Action<GameState> OnGameStateChanged = delegate { };
        
        public void RaisePlayerJoined(PlayerRef player) => OnPlayerJoined.Invoke(player);
        public void RaisePlayerLeft(PlayerRef player) => OnPlayerLeft.Invoke(player);
        public void RaiseGameStateChanged(GameState state) => OnGameStateChanged.Invoke(state);
    }
}