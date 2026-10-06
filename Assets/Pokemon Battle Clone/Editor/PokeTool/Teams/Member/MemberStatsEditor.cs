using System;
using System.Linq;
using Pokemon_Battle_Clone.Runtime.Database;
using Pokemon_Battle_Clone.Runtime.Stats.Domain;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool.Teams.Member
{
    public class MemberStatsEditor : VisualElement
    {
        private sealed class StatRow
        {
            public Label Name;
            public Label Base;
            public StatBar Bar;
            public IntegerField Ev;
            public SliderInt EvSlider;
            public IntegerField Iv;
            public Label Final;
        }
        
        private static readonly NatureEnum[] Natures = (NatureEnum[])Enum.GetValues(typeof(NatureEnum));
        
        private readonly StatRow[] _rows = new StatRow[StatInfo.All.Length];
        private readonly IntegerField _levelField = new("Level");
        private readonly DropdownField _natureField;
        private readonly Label _remainingEvsLabel = new();

        public MemberStatsEditor()
        {
            AddToClassList("member-stats-editor");
            
            _natureField = new DropdownField("Nature", Natures.Select(NatureText).ToList(), 0);
            _natureField.AddToClassList("stats-nature-field");
            _levelField.AddToClassList("stats-level-field");
            var paramsRow = new VisualElement();
            paramsRow.AddToClassList("stats-params-row");
            paramsRow.Add(_levelField);
            paramsRow.Add(_natureField);
            Add(paramsRow);
            
            // headers
            var header = TableRow("stats-header");
            header.Add(Cell("stats-col-name"));
            header.Add(Cell("stats-col-base", "Base"));
            header.Add(Cell("stats-col-bar"));
            header.Add(Cell("stats-col-ev", "EVs"));
            header.Add(Cell("stats-col-ev-slider"));
            header.Add(Cell("stats-col-iv", "IVs"));
            header.Add(Cell("stats-col-final"));
            Add(header);
            
            // stats rows
            foreach (var stat in StatInfo.All)
            {
                var row = new StatRow
                {
                    Name = Cell("stats-col-name", stat.FullName()),
                    Base = Cell("stats-col-base"),
                    Bar = new StatBar(),
                    Ev = new IntegerField(),
                    EvSlider = new SliderInt(0, StatsData.MaxEVPerStat),
                    Iv = new IntegerField(),
                    Final = Cell("stats-col-final")
                };
                row.Bar.AddToClassList("stats-col-bar");
                row.Ev.AddToClassList("stats-col-ev");
                row.EvSlider.AddToClassList("stats-col-ev-slider");
                row.Iv.AddToClassList("stats-col-iv");
                _rows[(int)stat] = row;

                var tableRow = TableRow();
                tableRow.Add(row.Name);
                tableRow.Add(row.Base);
                tableRow.Add(row.Bar);
                tableRow.Add(row.Ev);
                tableRow.Add(row.EvSlider);
                tableRow.Add(row.Iv);
                tableRow.Add(row.Final);
                Add(tableRow);
            }
            
            // footer
            _remainingEvsLabel.AddToClassList("stats-col-ev");
            _remainingEvsLabel.AddToClassList("stats-remaining");
            var footer = TableRow("stats-footer");
            footer.Add(Cell("stats-col-name"));
            footer.Add(Cell("stats-col-base"));
            footer.Add(Cell("stats-col-bar"));
            footer.Add(_remainingEvsLabel);
            footer.Add(Cell("stats-col-ev-slider"));
            footer.Add(Cell("stats-col-iv"));
            footer.Add(Cell("stats-col-final"));
            Add(footer);
            
            // SetEnabled(false);
        }
        
        public void Bind(TeamMember member)
        {
            _levelField.SetValueWithoutNotify(member.level);
            _natureField.SetValueWithoutNotify(NatureText(member.nature));
            MarkNature(member.nature);
            ShowStats(member.BuildStatsData());
        }

        private void ShowStats(StatsData stats)
        {
            foreach (var stat in StatInfo.All)
            {
                var row = _rows[(int)stat];
                var final = stats.Stats[stat];

                row.Base.text = stats.BaseStats[stat].ToString();
                row.Bar.SetValue(final, StatBarScale.MaxFor(stat));
                row.Ev.SetValueWithoutNotify(stats.EVs[stat]);
                row.EvSlider.SetValueWithoutNotify(stats.EVs[stat]);
                row.Iv.SetValueWithoutNotify(stats.IVs[stat]);
                row.Final.text = final.ToString();
            }

            _remainingEvsLabel.text = $"Left: {StatsData.MaxTotalEVs - stats.EVs.Sum}";
        }
        
        private void MarkNature(NatureEnum natureEnum)
        {
            var nature = Nature.FromEnum(natureEnum);
            foreach (var stat in StatInfo.All)
            {
                var label = _rows[(int)stat].Name;
                var multiplier = nature[stat];

                label.EnableInClassList("stat-up", multiplier > 1f);
                label.EnableInClassList("stat-down", multiplier < 1f);

                var percent = Mathf.RoundToInt((multiplier - 1f) * 100f);
                label.tooltip = percent == 0 ? string.Empty : percent > 0 ? $"+{percent} %" : $"−{-percent} %";
            }
        }
        
        private static string NatureText(NatureEnum natureEnum)
        {
            var nature = Nature.FromEnum(natureEnum);
            var up = StatInfo.Battle.FirstOrDefault(s => nature[s] > 1f);
            var down = StatInfo.Battle.FirstOrDefault(s => nature[s] < 1f);

            var isNeutral = nature[up] <= 1f;
            return isNeutral
                ? $"{natureEnum} (neutral)"
                : $"{natureEnum} (+{up.ShortName()} −{down.ShortName()})";
        }
        
        private static VisualElement TableRow(string roleClass = null)
        {
            var row = new VisualElement();
            row.AddToClassList("stats-table-row");
            if (roleClass != null) row.AddToClassList(roleClass);
            return row;
        }
        
        private static Label Cell(string className, string text = "")
        {
            var label = new Label(text);
            label.AddToClassList(className);
            return label;
        }
    }
}