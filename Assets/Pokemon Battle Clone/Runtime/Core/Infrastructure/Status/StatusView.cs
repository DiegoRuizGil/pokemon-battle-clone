using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Core.Infrastructure.Status
{
    public class StatusView : MonoBehaviour
    {
        [field: SerializeField] public PokemonStatusView Pokemon { get; private set; }
        [field: SerializeField] public TeamStatusView Team { get; private set; }
        
        
    }
}