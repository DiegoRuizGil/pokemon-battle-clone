using System.Collections.Generic;
using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Core.Infrastructure.Status;
using Pokemon_Battle_Clone.Runtime.Database;
using Pokemon_Battle_Clone.Runtime.Stats.Domain;
using Pokemon_Battle_Clone.Runtime.Stats.Infrastructure;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Core.Infrastructure
{
    public class TeamView : MonoBehaviour, ITeamView
    {
        [SerializeField] private Side side;
        [SerializeField] private PokemonAssetDatabase assetDatabase;
        [SerializeField] private StatusView statusView;
        [SerializeField] private PokemonView pokemonView;
        
        public void Init(List<uint> pokemonIDs)
        {
            statusView.Team.Init(pokemonIDs);
        }
        
        public async Task SendPokemon(Pokemon pokemon)
        {
            var sprite = GetSprite(pokemon.ID);
            SetStaticData(sprite, pokemon.Name, pokemon.Stats.Level);
            UpdateHealth(pokemon.Health.Max, pokemon.Health.Current, animated: false);
            SetStatModifier(pokemon.Stats.Modifiers);
            statusView.Show();

            await PlaySendAnimation();
        }

        public void UpdateHealth(int max, int current, bool animated) => statusView.Pokemon.UpdateHealth(max, current, animated);
        public void SetStatModifier(StatsModifier modifier) => statusView.Pokemon.SetStatsModifier(modifier);
        public void SetPokemonAsDefeated(uint pokemonID) => statusView.Team.SetAsDefeated(pokemonID);

        public Task PlayAttackAnimation() => pokemonView.PlayAttackAnimation();
        public Task PlayHitAnimation() => pokemonView.PlayHitAnimation();
        public Task PlayFaintAnimation()
        {
            statusView.Hide();
            return pokemonView.PlayFaintAnimation();
        }

        public Task PlaySendAnimation() => pokemonView.PlaySendAnimation();
        public Task PlayWithdrawAnimation()
        {
            statusView.Hide();
            return pokemonView.PlayWithdrawAnimation();
        }

        private void SetStaticData(Sprite sprite, string name, int level)
        {
            pokemonView.SetSprite(sprite);
            statusView.Pokemon.SetInfo(name, level);
        }

        private Sprite GetSprite(uint id)
        {
            return side == Side.Player ? assetDatabase.GetBackSprite(id) : assetDatabase.GetFrontSprite(id);
        }
    }
}