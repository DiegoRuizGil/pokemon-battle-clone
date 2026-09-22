using System.Collections.Generic;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.TeamBuilder.TeamDisplayer
{
    public class TeamInfoDisplayer : MonoBehaviour, ITeamInfoDisplayer
    {
        [SerializeField] private PokemonCard pokemonCard;

        private List<Pokemon> _currentList;
        private int _currentIndex;

        private IPokemonSpriteProvider _spriteProvider;
        
        public void Init(IPokemonSpriteProvider spriteProvider) => _spriteProvider = spriteProvider;
        
        public void Close()
        {
            gameObject.SetActive(false);
        }
        
        public void GoLeft()
        {
            _currentIndex = (_currentIndex - 1 + _currentList.Count) % _currentList.Count;
            Display(_currentList, _currentIndex);
        }

        public void GoRight()
        {
            _currentIndex = (_currentIndex + 1) % _currentList.Count;
            Display(_currentList, _currentIndex);
        }

        public async void Display(List<Pokemon> pokemonList, int currentPokemonToDisplay = 0)
        {
            _currentList = pokemonList;
            _currentIndex = currentPokemonToDisplay;
            
            var pokemon = _currentList[_currentIndex];
            var icon = await _spriteProvider.GetIconSprite(pokemon.ID);
            
            pokemonCard.Display(pokemon, icon);
            
            gameObject.SetActive(true);
        }
    }
}