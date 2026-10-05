using Pokemon_Battle_Clone.Runtime.Stats.Domain;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool.Teams.Member
{
    public class MemberStatsField : VisualElement
    {
        private const int MaxStat = 500;
        private const int MaxHP = 605;

        private readonly StatBar[] _bars = new StatBar[StatInfo.All.Length];

        public MemberStatsField()
        {
            foreach (var stat in StatInfo.All)
            {
                var nameLabel = new Label(stat.ShortName());
                nameLabel.AddToClassList("stat-name");
                
                var bar = new StatBar();
                _bars[(int)stat] = bar;

                var row = new VisualElement();
                row.AddToClassList("stat-row");
                row.Add(nameLabel);
                row.Add(bar);
                this.Add(row);
            }
        }

        public void SetStats(StatsData stats)
        {
            foreach (var stat in StatInfo.All)
            {
                var final = stats.Stats[stat];
                var baseStat = stats.BaseStats[stat];
                var ev = stats.EVs[stat];
                var iv = stats.IVs[stat];
                
                var max = stat == Stat.HP ? MaxHP : MaxStat;
                var bar = _bars[(int)stat];
                
                bar.SetValue(final, max);
                bar.tooltip = $"{baseStat}/{ev}/{iv}/{final}";
            }
        }
    }
}