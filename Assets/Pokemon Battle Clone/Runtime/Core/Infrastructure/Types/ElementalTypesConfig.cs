using System.Collections.Generic;
using System.Linq;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Core.Infrastructure.Types
{
    [CreateAssetMenu(menuName = "Pokemon Battle Clone/Elemental Types Config")]
    public class ElementalTypesConfig : ScriptableObject
    {
        [System.Serializable]
        public class TypeData
        {
            [field: SerializeField] public ElementalType Type { get; private set; }
            [field: SerializeField] public Sprite Icon { get; private set; }
            [field: SerializeField] public Color Color { get; private set; }
        }
        
        [SerializeField] private List<TypeData> types = new List<TypeData>();

        public Sprite GetIcon(ElementalType type)
        {
            return types.Where(data => data.Type == type).Select(data => data.Icon).FirstOrDefault();
        }

        public Color GetColor(ElementalType type)
        {
            return types.Where(data => data.Type == type).Select(data => data.Color).First();
        }
    }
}