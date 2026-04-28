using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fusion;
using Pokemon_Battle_Clone.Runtime.Online.Lobby.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pokemon_Battle_Clone.Runtime.Online.Lobby
{
    public class LobbyManager : MonoBehaviour
    {
        public GameSession gameSession;
        public NetworkEventsChannel eventsChannel;
        public BattleOnlineLoader battleOnlineLoaderPrefab;
        
        private NetworkRunner _runner;
        private bool Initialized => _runner != null;
        
        private BattleOnlineLoader _battleOnlineLoader;

        private string _lastSessionCodeGenerated;

        private async void Start()
        {
            // Init();
            // await JoinLobbyAsync();

            _runner = GetOrCreateRunner();

            if (_runner.IsRunning)
            {
                var state = _runner.SessionInfo.IsValid
                    ? SessionState.InSession
                    : SessionState.InLobby;
                gameSession.SetSessionState(state);
                return;
            }

            // await ConnecToLobbyAsync();
        }

        private void Init()
        {
            eventsChannel.OnSessionListUpdated += OnSessionListUpdated;
            eventsChannel.OnPlayerJoined += OnPlayerJoined;
            _runner = CreateRunner();
        }

        private void OnDestroy()
        {
            eventsChannel.OnSessionListUpdated -= OnSessionListUpdated;
            eventsChannel.OnPlayerJoined -= OnPlayerJoined;
        }

        public async void CreateGame()
        {
            await CreateAndJoinGameAsync();
            
            gameSession.RaiseJoinGame();
        }

        public async void JoinGame(string sessionName)
        {
            await JoinGameAsync(sessionName);
            
            gameSession.RaiseJoinGame();
        }
        
        public async void LeaveGame()
        {
            await ShutdownAsync();
            await JoinLobbyAsync();
            
            gameSession.RaiseLeaveGame();
        }


        private NetworkRunner GetOrCreateRunner()
        {
            return FindFirstObjectByType<NetworkRunner>() == null ?
                CreateRunner() :
                FindFirstObjectByType<NetworkRunner>();
        }
        
        
        
        
        private NetworkRunner CreateRunner()
        {
            var go = new GameObject("Network Runner");
            var runner = go.AddComponent<NetworkRunner>();
            go.AddComponent<NetworkSceneManagerDefault>();
            var eventsCallbacks = go.AddComponent<NetworkSessionEvents>();
            
            eventsCallbacks.EventsChannel = eventsChannel;
            runner.AddCallbacks(eventsCallbacks);
            
            return runner;
        }

        private async Task ShutdownAsync()
        {
            if (!Initialized) return;
            
            await _runner.Shutdown();
            _runner = null;
        }

        private async Task JoinLobbyAsync()
        {
            if (!Initialized) Init();

            await _runner.JoinSessionLobby(SessionLobby.Shared);
            Debug.Log("Connected to lobby", this);
        }

        private async Task<StartGameResult> CreateAndJoinGameAsync()
        {
            _lastSessionCodeGenerated = GenerateSessionCode();
            var result = await JoinGameAsync(_lastSessionCodeGenerated);
            return result;
        }

        private async Task<StartGameResult> JoinGameAsync(string sessionName)
        {
            if (!Initialized) Init();

            var result = await ConnectToGameAsync(sessionName);
            return result;
        }

        private async Task<StartGameResult> ConnectToGameAsync(string sessionName)
        {
            var result = await _runner.StartGame(new StartGameArgs
            {
                GameMode    = GameMode.Shared,
                SessionName = sessionName,
                PlayerCount = 2,
                Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
                SceneManager = _runner.SceneManager,
            });

            if (result.Ok)
                _lastSessionCodeGenerated = sessionName;
            else
                Debug.Log($"Try to connect to game, but couldn't: {result.ErrorMessage}");
            
            return result;
        }


        

        private void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            var currentSessions = string.Join(',', sessionList.Select(info => info.Name));
            Debug.Log($"Current Sessions: {currentSessions}");
        }
        
        private void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (_runner.IsSharedModeMasterClient && _battleOnlineLoader == null)
            {
                _battleOnlineLoader = _runner.Spawn(battleOnlineLoaderPrefab);
                _battleOnlineLoader.Init(_lastSessionCodeGenerated);
            }
        }

        private static string GenerateSessionCode()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            var code = new System.Text.StringBuilder(6);
            var rng = new System.Random();
            for (int i = 0; i < 6; i++)
                code.Append(chars[rng.Next(chars.Length)]);
            return code.ToString();
        }
    }
}