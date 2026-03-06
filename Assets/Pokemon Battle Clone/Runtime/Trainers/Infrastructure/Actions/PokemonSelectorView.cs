using System;
using System.Collections.Generic;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Core.Infrastructure;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon_Battle_Clone.Runtime.Trainers.Infrastructure.Actions
{
    public class PokemonSelectorView : MonoBehaviour, ISelectorView<Team>
    {
        [SerializeField] private List<PokemonSelectorButton> pokemonButtons;
        [SerializeField] private Button backButton;

        public event Action<int> OnPokemonSelected = delegate { };

        public void Init()
        {
            for (int i = 0; i < pokemonButtons.Count; i++)
            {
                pokemonButtons[i].index = i + 1;
                pokemonButtons[i].OnClick += index => OnPokemonSelected(index);
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            backButton.interactable = true;
        }

        public void Show(bool forceSelection, Team team)
        {
            gameObject.SetActive(true);
            backButton.interactable = !forceSelection;
            SetData(team);
        }

        private void SetData(Team team)
        {
            for (int i = 0; i < pokemonButtons.Count; i++)
            {
                var pokemonIndex = pokemonButtons[i].index;
                if (team.PokemonList.Count > pokemonIndex)
                {
                    pokemonButtons[i].gameObject.SetActive(true);
                    pokemonButtons[i].SetData(team.PokemonList[pokemonIndex]);
                }
                else
                {
                    pokemonButtons[i].gameObject.SetActive(false);
                }
            }
        }
    }
}