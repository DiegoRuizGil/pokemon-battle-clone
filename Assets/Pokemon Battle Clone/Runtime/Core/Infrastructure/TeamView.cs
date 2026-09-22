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
        [SerializeField] private StatusView statusView;
        [SerializeField] private PokemonView pokemonView;
        
        private IPokemonSpriteProvider _spriteProvider;
        
        public void Init(List<uint> pokemonIDs, IPokemonSpriteProvider spriteProvider)
        {
            statusView.Team.Init(pokemonIDs);
            _spriteProvider = spriteProvider;
        }
        
        public async Task SendPokemon(Pokemon pokemon)
        {
            var sprite = await GetSprite(pokemon.ID);
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

        private Task<Sprite> GetSprite(uint id)
        {
            return side == Side.Player ? _spriteProvider.GetBackSprite(id) : _spriteProvider.GetFrontSprite(id);
        }
    }
}