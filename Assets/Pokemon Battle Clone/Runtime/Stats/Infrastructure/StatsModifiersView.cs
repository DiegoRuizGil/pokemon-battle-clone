using System.Collections.Generic;
using Pokemon_Battle_Clone.Runtime.Stats.Domain;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Stats.Infrastructure
{
    public class StatsModifiersView : MonoBehaviour
    {
        [SerializeField] private StatModifierTag statModifierTagPrefab;
        
        private readonly Dictionary<Stat, StatModifierTag> _tags = new();

        public void Set(StatsModifier modifiers)
        {
            EnsureTags();

            foreach (var (stat, modifierTag) in _tags)
                modifierTag.SetInfo(stat, modifiers);
        }

        private void EnsureTags()
        {
            if (_tags.Count > 0) return;

            foreach (var stat in StatInfo.Battle)
                _tags.Add(stat, Instantiate(statModifierTagPrefab, transform));
        }
    }
}