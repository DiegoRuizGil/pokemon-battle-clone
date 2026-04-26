namespace Pokemon_Battle_Clone.Runtime.Online
{
    public struct LobbyState
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