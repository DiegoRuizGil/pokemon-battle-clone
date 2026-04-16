using Fusion;
using Pokemon_Battle_Clone.Runtime.Battles.Domain;
using Pokemon_Battle_Clone.Runtime.Trainers.Control;
using Pokemon_Battle_Clone.Runtime.Trainers.Domain.Actions;

namespace Pokemon_Battle_Clone.Runtime.Online
{
    public class BattleNetworkBridge : NetworkBehaviour
    {
        private Battle _battle;
        private NetworkTrainer _remotePlayer;
        
        public void Init(Battle battle, PlayerTrainer localPlayer, NetworkTrainer remotePlayer)
        {
            _battle = battle;
            _remotePlayer = remotePlayer;
            
            // TODO - player trainer OnActionSelected event
        }

        public void SendLocalAction(TrainerAction action)
        {
            var dto = Serialize(action);
            RPC_ReceiveAction(dto);
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        private void RPC_ReceiveAction(ActionDTO dto)
        {
            if (HasInputAuthority) return;

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
                    // Index = team.FirstPokemon.MoveSet.Moves.IndexOf(move.)
                },
                SwapPokemonAction swap => new ActionDTO
                {
                    Type = ActionType.Swap,
                    // Index = 
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