using System;

namespace Pokemon_Battle_Clone.Runtime.Stats.Domain
{
    public class StatsModifier
    {
        public const int MinStage = -6;
        public const int MaxStage = 6;

        private readonly int[] _stages = new int[StatInfo.All.Length];

        public int GetStage(Stat stat) => _stages[(int)stat];

        public float GetMultiplier(Stat stat)
        {
            var stage = GetStage(stat);
            return stage >= 0 ? (2f + stage) / 2f : 2f / (2f - stage);
        }

        public void Apply(StatSet boost)
        {
            foreach (var stat in StatInfo.Battle)
                _stages[(int)stat] = Math.Clamp(_stages[(int)stat] + boost[stat], MinStage, MaxStage);
        }

        public void Clear() => Array.Clear(_stages, 0, StatInfo.All.Length);
    }
}