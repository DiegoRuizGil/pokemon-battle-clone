using System;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online.Lobby
{
    [CreateAssetMenu(menuName = "Pokemon Battle Clone/Online/Game Session", fileName = "Game Session")]
    public class GameSession : ScriptableObject
    {
        public BattleOnlineLoader BattleLoader { get; set; }
        
        public event Action<GameState> OnGameStateChanged = delegate { };

        public void RaiseGameStateChanged(GameState state) => OnGameStateChanged.Invoke(state);
        
        
        public event Action OnJoinGame = delegate { };
        public event Action OnLeaveGame = delegate { };
        
        public void RaiseJoinGame() => OnJoinGame.Invoke();
        public void RaiseLeaveGame() => OnLeaveGame.Invoke();
    }
}