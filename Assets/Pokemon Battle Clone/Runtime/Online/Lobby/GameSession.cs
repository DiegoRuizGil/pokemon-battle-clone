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
        public BattleOnlineLoader BattleLoader { get; set; }
        
        public SessionState State { get; private set; } = SessionState.Disconnected;
        public GameState CurrentGameState { get; private set; }
        
        public bool IsConnecting => State == SessionState.Connecting;
        public bool IsInLobby => State == SessionState.InLobby;
        public bool IsInSession => State == SessionState.InSession;
        
        public event Action<SessionState> OnSessionStateChanged;
        public event Action<GameState> OnGameStateChanged;
        
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
    }
}