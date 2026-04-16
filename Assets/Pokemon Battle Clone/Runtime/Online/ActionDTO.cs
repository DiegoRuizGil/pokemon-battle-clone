using Fusion;

namespace Pokemon_Battle_Clone.Runtime.Online
{
    public enum ActionType : byte
    {
        Move, Swap
    }
    
    public struct ActionDTO : INetworkStruct
    {
        public ActionType Type;
        /// <summary>
        /// Either move index (move action) or the pokemon index (swap action)
        /// </summary>
        public int Index;
        /// <summary>
        /// Only relevant for swap actions
        /// </summary>
        public NetworkBool WithdrawFirst;
    }
}