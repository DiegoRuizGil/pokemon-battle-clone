using System;
using System.Collections.Generic;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Moves.Domain;
using Pokemon_Battle_Clone.Runtime.Moves.Infrastructure;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Trainers.Infrastructure.Actions
{
    public class ActionsHUD : MonoBehaviour, IActionHUD
    {
        public ActionSelector selector;
        public PokemonSelectorView pokemonSelector;
        public MoveSetView moveSetView;
        
        private Dictionary<uint, Sprite> _pokemonIcons = new();
        
        public void Init(Dictionary<uint, Sprite> pokemonIcons)
        {
            _pokemonIcons = new Dictionary<uint, Sprite>(pokemonIcons);
            
            HideSelectors();
            
            moveSetView.Init();
            pokemonSelector.Init();
        }

        public void Show()
        {
            HideSelectors();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void HideSelectors()
        {
            selector.Show();
            moveSetView.Hide();
            pokemonSelector.Hide();
        }

        public void ShowMoveSelector(bool forceSelection, MoveSetDTO moveSet)
        {
            selector.Hide();
            moveSetView.Show(forceSelection, moveSet);
            pokemonSelector.Hide();
        }

        public void ShowPokemonSelector(bool forceSelection, Team team)
        {
            selector.Hide();
            moveSetView.Hide();
            pokemonSelector.Show(forceSelection, team, _pokemonIcons);
        }

        public void RegisterMoveSelectedListener(Action<int> listener) => moveSetView.OnMoveSelected += listener;
        public void RegisterMoveButtonPressedListener(Action listener) => selector.OnMoveButtonPressed += listener;

        public void RegisterPokemonSelectedListener(Action<int> listener) => pokemonSelector.OnPokemonSelected += listener;
        public void RegisterPokemonButtonPressedListener(Action listener) => selector.OnPokemonButtonPressed += listener;
    }
}