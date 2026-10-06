using Pokemon_Battle_Clone.Runtime.Stats.Domain;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool.Teams.Member
{
    public class MemberStatsField : VisualElement
    {
        private readonly Label _levelValueLabel = new();
        private readonly StatBar[] _bars = new StatBar[StatInfo.All.Length];

        public MemberStatsField()
        {
            var levelLabel = new Label("Lv");
            levelLabel.AddToClassList("stat-name");
            var levelRow = new VisualElement();
            levelRow.AddToClassList("stat-row");
            levelRow.Add(levelLabel);
            levelRow.Add(_levelValueLabel);
            this.Add(levelRow);
            
            foreach (var stat in StatInfo.All)
            {
                var nameLabel = new Label(stat.ShortName());
                nameLabel.AddToClassList("stat-name");
                
                var bar = new StatBar();
                _bars[(int)stat] = bar;

                var statRow = new VisualElement();
                statRow.AddToClassList("stat-row");
                statRow.Add(nameLabel);
                statRow.Add(bar);
                this.Add(statRow);
            }
        }

        public void SetStats(StatsData stats)
        {
            _levelValueLabel.text = stats.Level.ToString();
            foreach (var stat in StatInfo.All)
            {
                var final = stats.Stats[stat];
                var baseStat = stats.BaseStats[stat];
                var ev = stats.EVs[stat];
                var iv = stats.IVs[stat];
                
                var max = StatBarScale.MaxFor(stat);
                var bar = _bars[(int)stat];
                
                bar.SetValue(final, max);
                bar.tooltip = $"{baseStat}/{ev}/{iv}/{final}";
            }
        }
    }
}