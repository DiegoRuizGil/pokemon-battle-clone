using System;
using System.Collections.Generic;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Trainers.Infrastructure.Actions
{
    public class PokemonSelectorView : MonoBehaviour
    {
        [SerializeField] private List<PokemonSelectorButton> pokemonButtons;
        [SerializeField] private HUDButton backButton;

        public event Action<int> OnPokemonSelected = delegate { };
        public event Action<int> OnDisplayInfo = delegate { };

        public void Init()
        {
            for (int i = 0; i < pokemonButtons.Count; i++)
            {
                pokemonButtons[i].index = i + 1;
                pokemonButtons[i].OnClick += index => OnPokemonSelected(index);
                pokemonButtons[i].OnInfoSelected += index => OnDisplayInfo(index);
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            backButton.SetInteraction(true);
        }

        public void Show(bool forceSelection, Team team, Dictionary<uint, Sprite> pokemonIcons)
        {
            gameObject.SetActive(true);
            backButton.SetInteraction(!forceSelection);
            SetData(team, pokemonIcons);
        }

        private void SetData(Team team, Dictionary<uint, Sprite> pokemonIcons)
        {
            for (int i = 0; i < pokemonButtons.Count; i++)
            {
                var pokemonIndex = pokemonButtons[i].index;
                if (team.PokemonList.Count > pokemonIndex)
                {
                    var pokemon = team.PokemonList[pokemonIndex];
                    pokemonButtons[i].SetData(team.PokemonList[pokemonIndex], pokemonIcons[pokemon.ID]);
                    pokemonButtons[i].gameObject.SetActive(true);
                }
                else
                {
                    pokemonButtons[i].gameObject.SetActive(false);
                }
            }
        }
    }
}