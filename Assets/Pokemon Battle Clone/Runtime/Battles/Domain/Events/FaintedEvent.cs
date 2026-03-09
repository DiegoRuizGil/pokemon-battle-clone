namespace Pokemon_Battle_Clone.Runtime.Battles.Domain.Events
{
    public class FaintedEvent : IBattleEvent
    {
        public Side Side { get; }
        public string PokemonName { get; }
        
        public FaintedEvent(Side side, string pokemonName)
        {
            Side = side;
            PokemonName = pokemonName;
        }
    }
}