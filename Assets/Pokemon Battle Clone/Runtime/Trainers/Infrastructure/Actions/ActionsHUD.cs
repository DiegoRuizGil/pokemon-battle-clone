using System;
using System.Linq;
using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Database;
using Pokemon_Battle_Clone.Runtime.Moves.Domain;
using Pokemon_Battle_Clone.Runtime.Moves.Infrastructure;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Trainers.Infrastructure.Actions
{
    public class ActionsHUD : MonoBehaviour, IActionHUD
    {
        [SerializeField] private ActionSelector selector;
        [SerializeField] private PokemonSelectorView pokemonSelector;
        [SerializeField] private MoveSetView moveSetView;
        
        private IPokemonSpriteProvider _spriteProvider;

        public void Init(IPokemonSpriteProvider spriteProvider)
        {
            _spriteProvider = spriteProvider;
            
            Hide();
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

        public async void ShowPokemonSelector(bool forceSelection, Team team)
        {
            var ids = team.PokemonList.Select(p => p.ID).ToList();
            var sprites = await Task.WhenAll(ids.Select(id => _spriteProvider.GetIconSprite(id)));
            
            var icons = ids
                .Zip(sprites, (id, sprite) => (id, sprite))
                .ToDictionary(x => x.id, x => x.sprite);
            
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