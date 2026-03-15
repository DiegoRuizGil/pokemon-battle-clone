using System;
using System.Linq;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Database;
using Pokemon_Battle_Clone.Runtime.Moves.Domain;
using Pokemon_Battle_Clone.Runtime.Moves.Infrastructure;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Trainers.Infrastructure.Actions
{
    public class ActionsHUD : MonoBehaviour, IActionHUD
    {
        [SerializeField] private PokemonAssetDatabase assetDatabase;
        [SerializeField] private ActionSelector selector;
        [SerializeField] private PokemonSelectorView pokemonSelector;
        [SerializeField] private MoveSetView moveSetView;

        private void Start()
        {
            Init();
        }

        public void Init()
        {
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
            var icons = assetDatabase.GetIconsOf(team.PokemonList.Select(p => p.ID).ToList());
            
            selector.Hide();
            moveSetView.Hide();
            pokemonSelector.Show(forceSelection, team, icons);
        }

        public void RegisterMoveSelectedListener(Action<int> listener) => moveSetView.OnMoveSelected += listener;
        public void RegisterMoveButtonPressedListener(Action listener) => selector.OnMoveButtonPressed += listener;

        public void RegisterPokemonSelectedListener(Action<int> listener) => pokemonSelector.OnPokemonSelected += listener;
        public void RegisterPokemonButtonPressedListener(Action listener) => selector.OnPokemonButtonPressed += listener;
        
        public void RegisterDisplayTeamInfoListener(Action<int> listener) => pokemonSelector.OnDisplayInfo += listener;
    }
}