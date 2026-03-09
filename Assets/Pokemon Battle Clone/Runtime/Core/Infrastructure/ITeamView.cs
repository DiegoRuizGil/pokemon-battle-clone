using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Stats.Domain;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Core.Infrastructure
{
    public interface ITeamView
    {
        Task SendPokemon(Pokemon pokemon);
        void UpdateHealth(int max, int current, bool animated);
        void SetStatModifier(StatsModifier modifier);
        void SetPokemonAsDefeated(uint pokemonID);

        Task PlayAttackAnimation();
        Task PlayHitAnimation();
        Task PlayFaintAnimation();
        Task PlaySendAnimation();
        Task PlayWithdrawAnimation();
    }
}