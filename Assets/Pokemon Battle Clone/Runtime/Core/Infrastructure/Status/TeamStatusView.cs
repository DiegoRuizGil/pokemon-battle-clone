using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Core.Infrastructure.Status
{
    public class TeamStatusView : MonoBehaviour
    {
        [SerializeField] private List<PokeballTeamIcon> pokeballIcons;

        public void Init(List<uint> pokemonIDs)
        {
            for (int i = 0; i < pokeballIcons.Count; i++)
            {
                if (i >= pokemonIDs.Count)
                    pokeballIcons[i].SetAsDisabled();
                else
                    pokeballIcons[i].Init(pokemonIDs[i]);
            }
        }

        public void SetAsDefeated(uint pokemonID)
        {
            pokeballIcons
                .Where(pokeball => pokeball.PokemonID == pokemonID)
                .ToList()
                .ForEach(pokeball => pokeball.SetAsDefeated());
        }

        public void SetAsAlive(uint pokemonID)
        {
            pokeballIcons
                .Where(pokeball => pokeball.PokemonID == pokemonID)
                .ToList()
                .ForEach(pokeball => pokeball.SetAsAlive());
        }
    }
}