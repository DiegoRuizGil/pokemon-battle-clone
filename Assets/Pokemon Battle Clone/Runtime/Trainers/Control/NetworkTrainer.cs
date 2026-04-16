using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain.Actions;

namespace Pokemon_Battle_Clone.Runtime.Trainers.Control
{
    public class NetworkTrainer : Trainer
    {
        public override Side Side { get; }

        private TaskCompletionSource<TrainerAction> _pendingAction;
        
        public NetworkTrainer(Team team, Side side) : base(team)
        {
            Side = side;
        }

        public override Task<TrainerAction> SelectAction()
        {
            _pendingAction = new TaskCompletionSource<TrainerAction>();
            return _pendingAction.Task;
        }

        public override Task<TrainerAction> SelectSwapPokemon()
        {
            _pendingAction = new TaskCompletionSource<TrainerAction>();
            return _pendingAction.Task;
        }

        public void ReceiveRemoteAction(TrainerAction action)
        {
            if (_pendingAction == null || _pendingAction.Task.IsCompleted)
                return;
            
            _pendingAction.SetResult(action);
        }
    }
}