using System.Collections.Generic;
using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Stats.Domain;
using Pokemon_Battle_Clone.Runtime.Stats.Infrastructure;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Core.Infrastructure
{
    public class TeamView : MonoBehaviour, ITeamView
    {
        [SerializeField] private PokemonStatusView pokemonStatusView;
        [SerializeField] private PokemonView pokemonView;
        [SerializeField] private StatsModifiersView statsModifiersView;

        private Dictionary<uint, Sprite> _sprites = new Dictionary<uint, Sprite>();

        public void Init(Dictionary<uint, Sprite> sprites)
        {
            _sprites = new Dictionary<uint, Sprite>(sprites);
        }
        
        public async Task SendPokemon(Pokemon pokemon)
        {
            var sprite = _sprites[pokemon.ID];
            SetStaticData(sprite, pokemon.Name, pokemon.Stats.Level);
            UpdateHealth(pokemon.Health.Max, pokemon.Health.Current, animated: false);
            SetStatModifier(pokemon.Stats.Modifiers);

            await PlaySendAnimation(); // change to send to field animation
        }

        public void UpdateHealth(int max, int current, bool animated) => pokemonStatusView.UpdateHealth(max, current, animated);
        public void SetStatModifier(StatsModifier modifier) => statsModifiersView.Set(modifier);

        public Task PlayAttackAnimation() => pokemonView.PlayAttackAnimation();
        public Task PlayHitAnimation() => pokemonView.PlayHitAnimation();
        public Task PlayFaintAnimation() => pokemonView.PlayFaintAnimation();
        public Task PlaySendAnimation() => pokemonView.PlaySendAnimation();
        public Task PlayWithdrawAnimation() => pokemonView.PlayWithdrawAnimation();

        private void SetStaticData(Sprite sprite, string name, int level)
        {
            pokemonView.SetSprite(sprite);
            pokemonStatusView.SetInfo(name, level);
        }
    }
}