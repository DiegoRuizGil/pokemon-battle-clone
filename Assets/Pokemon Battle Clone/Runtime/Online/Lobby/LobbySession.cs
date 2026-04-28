using System;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online.Lobby
{
    [CreateAssetMenu(menuName = "Pokemon Battle Clone/Online/Lobby Session", fileName = "Lobby Session")]
    public class LobbySession : ScriptableObject
    {
        public string currentSessionCode;

        public event Action<GameState> OnGameStateChanged = delegate { };

        public void RaiseGameStateChanged(GameState state) => OnGameStateChanged.Invoke(state);
        
        
        public event Action OnJoinGame = delegate { };
        public event Action OnLeaveGame = delegate { };
        
        public void RaiseJoinGame() => OnJoinGame.Invoke();
        public void RaiseLeaveGame() => OnLeaveGame.Invoke();
    }
}