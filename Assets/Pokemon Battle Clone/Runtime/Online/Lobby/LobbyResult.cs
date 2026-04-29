namespace Pokemon_Battle_Clone.Runtime.Online.Lobby
{
    public class LobbyResult
    {
        public bool Success { get; private set; }
        public string ErrorMessage { get; private set; }

        public static LobbyResult Ok() => new() { Success = true };
        public static LobbyResult Fail(string message) => new() { Success = false, ErrorMessage = message };
    }
}