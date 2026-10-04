using Pokemon_Battle_Clone.Runtime.Stats.Domain;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool.Teams.Member
{
    public class MemberStatsField : VisualElement
    {
        private const int MaxStat = 500;
        private const int MaxHP = 605;
        private const int HpIndex = 0;

        private readonly StatBar[] _bars = new StatBar[StatSet.StatNames.Length];

        public MemberStatsField()
        {
            for (int i = 0; i < StatSet.StatNames.Length; i++)
            {
                var nameLabel = new Label(StatSet.StatNames[i]);
                nameLabel.AddToClassList("stat-name");
                
                _bars[i] = new StatBar();

                var row = new VisualElement();
                row.AddToClassList("stat-row");
                row.Add(nameLabel);
                row.Add(_bars[i]);
                this.Add(row);
            }
        }

        public void SetStats(StatsData stats)
        {
            var finalStats = stats.Stats.Values;
            var baseStats = stats.BaseStats.Values;
            var evs = stats.EVs.Values;
            var ivs = stats.IVs.Values;

            for (int i = 0; i < _bars.Length; i++)
            {
                var max = i == HpIndex ? MaxHP : MaxStat;
                _bars[i].SetValue(finalStats[i], max);
                _bars[i].tooltip = $"{baseStats[i]}/{evs[i]}/{ivs[i]}/{finalStats[i]}";
            }
        }
    }
}