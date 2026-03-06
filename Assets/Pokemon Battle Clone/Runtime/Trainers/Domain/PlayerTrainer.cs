using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.CustomLogs;
using Pokemon_Battle_Clone.Runtime.Moves.Infrastructure;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain.Actions;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain.BattleEvents;
using Pokemon_Battle_Clone.Runtime.Trainers.Infrastructure.Actions;

namespace Pokemon_Battle_Clone.Runtime.Trainers.Domain
{
    public class PlayerTrainer : Trainer
    {
        private readonly IActionHUD _actionsHUD;
        private TaskCompletionSource<TrainerAction> _actionTcs;

        private readonly Dictionary<Type, Action<bool>> _selectorMap;

        public override Side Side => Side.Player;

        public PlayerTrainer(Team team, IActionHUD actionsHUD) : base(team)
        {
            _actionsHUD = actionsHUD;
            _selectorMap = new Dictionary<Type, Action<bool>>
            {
                { typeof(MoveAction), ShowMoveSelector },
                { typeof(SwapPokemonAction), ShowPokemonSelector }
            };
            
            _actionsHUD.RegisterMoveSelectedListener(OnMoveSelected);
            _actionsHUD.RegisterMoveButtonPressedListener(() => ShowMoveSelector(false));
            
            _actionsHUD.RegisterPokemonSelectedListener(OnPokemonSelected);
            _actionsHUD.RegisterPokemonButtonPressedListener(() => ShowPokemonSelector(false));
        }

        public override Task<TrainerAction> SelectActionTask()
        {
            _actionTcs = new TaskCompletionSource<TrainerAction>();
            return _actionTcs.Task;
        }

        public override Task<T> SelectActionOfType<T>(bool forceSelection)
        {
            _actionTcs = new TaskCompletionSource<TrainerAction>();
            
            if (_selectorMap.TryGetValue(typeof(T), out var showSelector))
                showSelector(forceSelection);
            
            return _actionTcs.Task.ContinueWith(t => (T)t.Result);
        }
        
        protected override IEnumerable<IBattleEvent> SendFirstPokemon()
        {
            _actionsHUD.SetData(Team, Team.FirstPokemon.MoveSet);
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
            _actionTcs.SetResult(action);
            
            LogManager.Log("Move selected", FeatureType.Player);
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
            _actionTcs.SetResult(action);
            
            LogManager.Log("Pokemon selected", FeatureType.Player);
        }

        private void ShowPokemonSelector(bool forceSelection)
        {
            _actionsHUD.ShowPokemonSelector(forceSelection, Team);
        }
    }
}