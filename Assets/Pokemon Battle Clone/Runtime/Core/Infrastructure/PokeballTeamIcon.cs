using UnityEngine;
using UnityEngine.UI;

namespace Pokemon_Battle_Clone.Runtime.Core.Infrastructure
{
    public class PokeballTeamIcon : MonoBehaviour
    {
        [SerializeField] private Color defeatedColor;
        [SerializeField] private Color aliveColor;
        [SerializeField] private Color disableColor;
        
        private Image _pokeballIcon;
        
        public uint PokemonID { get; private set; }

        private void Awake()
        {
            _pokeballIcon = GetComponent<Image>();
        }

        public void Init(uint pokemonID)
        {
            PokemonID = pokemonID;
            SetAsAlive();
        }

        public void SetAsDefeated() => _pokeballIcon.color = defeatedColor;
        public void SetAsAlive() => _pokeballIcon.color = aliveColor;
        public void SetAsDisabled() => _pokeballIcon.color = disableColor;
    }
}