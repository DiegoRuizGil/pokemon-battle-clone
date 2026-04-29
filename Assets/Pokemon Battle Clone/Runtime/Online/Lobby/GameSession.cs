using System;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online.Lobby
{
    public enum SessionState
    {
        Disconnected, Connecting, InLobby, InSession
    }
    
    [CreateAssetMenu(menuName = "Pokemon Battle Clone/Online/Game Session", fileName = "Game Session")]
    public class GameSession : ScriptableObject
    {
        public SessionState State { get; private set; } = SessionState.Disconnected;
        public GameState CurrentGameState { get; private set; }
        
        public event Action<SessionState> OnSessionStateChanged;
        public event Action<GameState> OnGameStateChanged;
        public event Action OnPlayerSetReady;
        
        public void SetSessionState(SessionState state)
        {
            State = state;
            OnSessionStateChanged?.Invoke(State);
        }

        public void SetGameState(GameState state)
        {
            CurrentGameState = state;
            OnGameStateChanged?.Invoke(CurrentGameState);
        }
        
        public void SetLocalPlayerReady() => OnPlayerSetReady?.Invoke();
    }
}