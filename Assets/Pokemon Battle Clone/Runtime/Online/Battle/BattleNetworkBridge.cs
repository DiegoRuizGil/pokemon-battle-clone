using Fusion;
using Pokemon_Battle_Clone.Runtime.Trainers.Control;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain.Actions;

namespace Pokemon_Battle_Clone.Runtime.Online.Battle
{
    public class BattleNetworkBridge : NetworkBehaviour
    {
        private Battles.Domain.Battle _battle;
        private NetworkTrainer _remotePlayer;

        public void Init(Battles.Domain.Battle battle, PlayerTrainer player, NetworkTrainer rival)
        {
            _battle = battle;
            _remotePlayer = rival;
            
            player.OnActionSelected += SendLocalAction;
        }

        private void SendLocalAction(TrainerAction action)
        {
            var dto = Serialize(action);
            RPC_ReceiveAction(dto);
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_ReceiveAction(ActionDTO dto, RpcInfo info = default)
        {
            if (info.Source == Runner.LocalPlayer) return;
            
            var action = Deserialize(dto);
            _remotePlayer.ReceiveRemoteAction(action);
        }

        private ActionDTO Serialize(TrainerAction action)
        {
            var side = action.Side;
            var team = _battle.GetTeam(side);

            return action switch
            {
                MoveAction move => new ActionDTO
                {
                    Type = ActionType.Move,
                    Index = team.FirstPokemon.MoveSet.Moves.IndexOf(move.Move)
                },
                SwapPokemonAction swap => new ActionDTO
                {
                    Type = ActionType.Swap,
                    Index = swap.PokemonIndex,
                    WithdrawFirst = swap.WithdrawFirstPokemon
                },
                _ => default
            };
        }

        private TrainerAction Deserialize(ActionDTO dto)
        {
            var side = _remotePlayer.Side;
            var team = _battle.GetTeam(side);
            var speed = team.FirstPokemon.Stats.Speed;

            return dto.Type switch
            {
                ActionType.Move => new MoveAction(
                    side: side,
                    pokemonInFieldSpeed: speed,
                    move: team.FirstPokemon.MoveSet.Moves[dto.Index]
                ),
                ActionType.Swap => new SwapPokemonAction(
                    side: side,
                    pokemonIndex: dto.Index,
                    withdrawFirstPokemon: dto.WithdrawFirst
                ),
                _ => null
            };
        }
    }
}