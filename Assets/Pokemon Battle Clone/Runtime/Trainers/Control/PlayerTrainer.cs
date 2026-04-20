using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Moves.Infrastructure;
using Pokemon_Battle_Clone.Runtime.TeamBuilder.TeamDisplayer;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain.Actions;
using Pokemon_Battle_Clone.Runtime.Trainers.Infrastructure.Actions;

namespace Pokemon_Battle_Clone.Runtime.Trainers.Control
{
    public class PlayerTrainer : Trainer
    {
        private readonly IActionHUD _actionsHUD;
        private readonly ITeamInfoDisplayer _teamInfoDisplayer;
        private TaskCompletionSource<TrainerAction> _actionTcs;

        private readonly Dictionary<Type, Action<bool>> _selectorMap;
        
        public event Action<TrainerAction> OnActionSelected;

        public PlayerTrainer(Team team, Side side, IActionHUD actionsHUD, ITeamInfoDisplayer teamInfoDisplayer)
            : base(team, side)
        {
            _actionsHUD = actionsHUD;
            _teamInfoDisplayer = teamInfoDisplayer;
            _selectorMap = new Dictionary<Type, Action<bool>>
            {
                { typeof(MoveAction), ShowMoveSelector },
                { typeof(SwapPokemonAction), ShowPokemonSelector }
            };
            
            _actionsHUD.RegisterMoveSelectedListener(OnMoveSelected);
            _actionsHUD.RegisterMoveButtonPressedListener(() => ShowMoveSelector(false));
            
            _actionsHUD.RegisterPokemonSelectedListener(OnPokemonSelected);
            _actionsHUD.RegisterPokemonButtonPressedListener(() => ShowPokemonSelector(false));
            
            _actionsHUD.RegisterDisplayTeamInfoListener(OnDisplayTeamInfo);
        }

        public override async Task<TrainerAction> SelectAction()
        {
            _actionsHUD.Show();
            _actionTcs = new TaskCompletionSource<TrainerAction>();
            var result = await _actionTcs.Task;
            _actionsHUD.Hide();
            return result;
        }

        public override async Task<TrainerAction> SelectSwapPokemon()
        {
            return await SelectActionOfType<SwapPokemonAction>(forceSelection: true);
        }

        private async Task<T> SelectActionOfType<T>(bool forceSelection) where T : TrainerAction
        {
            _actionsHUD.Show();
            _actionTcs = new TaskCompletionSource<TrainerAction>();
            
            if (_selectorMap.TryGetValue(typeof(T), out var showSelector))
                showSelector(forceSelection);
            
            var result = await _actionTcs.Task;
            _actionsHUD.Hide();
            return (T)result;
        }
        
        protected override TrainerAction SendFirstPokemon()
        {
            _actionsHUD.HideSelectors();
            
            return base.SendFirstPokemon();
        }

        private void OnMoveSelected(int index)
        {
            if (_actionTcs == null || _actionTcs.Task.IsCompleted)
                return;

            _actionsHUD.HideSelectors();
            
            var move = Team.FirstPokemon.MoveSet.Moves[index];
            var action = new MoveAction(Side, Team.FirstPokemon.Stats.Speed, move);
            
            OnActionSelected?.Invoke(action);
            _actionTcs.SetResult(action);
        }

        private void ShowMoveSelector(bool forceSelection)
        {
            var moveSet = MoveSetDTO.Get(Team.FirstPokemon.MoveSet);
            _actionsHUD.ShowMoveSelector(forceSelection, moveSet);
        }

        private void OnPokemonSelected(int index)
        {
            if (_actionTcs == null || _actionTcs.Task.IsCompleted)
                return;

            _actionsHUD.HideSelectors();
            
            var action = new SwapPokemonAction(Side, index, withdrawFirstPokemon: !Team.FirstPokemon.Defeated);
            
            OnActionSelected?.Invoke(action);
            _actionTcs.SetResult(action);
        }

        private void ShowPokemonSelector(bool forceSelection)
        {
            _actionsHUD.ShowPokemonSelector(forceSelection, Team);
        }

        private void OnDisplayTeamInfo(int pokemonIndex)
        {
            _teamInfoDisplayer.Display(Team.PokemonList.ToList(), pokemonIndex);
        }
    }
}