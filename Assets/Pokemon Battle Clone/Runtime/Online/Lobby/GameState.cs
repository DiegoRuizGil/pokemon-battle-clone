namespace Pokemon_Battle_Clone.Runtime.Online.Lobby
{
    public struct GameState
    {
        public string SessionCode;
        public PlayerState LocalPlayer;
        public PlayerState RemotePlayer;
    }

    public struct PlayerState
    {
        public bool IsReady;
        public bool IsPresent;
    }
}