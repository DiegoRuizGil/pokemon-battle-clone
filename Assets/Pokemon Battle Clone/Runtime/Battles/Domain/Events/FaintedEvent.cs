namespace Pokemon_Battle_Clone.Runtime.Battles.Domain.Events
{
    public class FaintedEvent : IBattleEvent
    {
        public Side Side { get; }
        public string PokemonName { get; }
        public uint PokemonID { get; }
        
        public FaintedEvent(Side side, string pokemonName, uint pokemonID)
        {
            Side = side;
            PokemonName = pokemonName;
            PokemonID = pokemonID;
        }
    }
}