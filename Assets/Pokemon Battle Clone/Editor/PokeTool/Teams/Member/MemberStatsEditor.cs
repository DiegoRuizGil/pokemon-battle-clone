using System;
using System.Linq;
using Pokemon_Battle_Clone.Runtime.Database;
using Pokemon_Battle_Clone.Runtime.Stats.Domain;
using UnityEditor;
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

        public event Action OnChanged;

        private const int EvStep = 4;
        
        private static readonly NatureEnum[] Natures = (NatureEnum[])Enum.GetValues(typeof(NatureEnum));
        
        private readonly StatRow[] _rows = new StatRow[StatInfo.All.Length];
        private readonly IntegerField _levelField = new("Level");
        private readonly DropdownField _natureField;
        private readonly Label _remainingEvsLabel = new();

        private SerializedProperty _member;
        private SerializedProperty _levelProp;
        private SerializedProperty _natureProp;
        private readonly SerializedProperty[] _evProps = new SerializedProperty[StatInfo.All.Length];
        private readonly SerializedProperty[] _ivProps = new SerializedProperty[StatInfo.All.Length];

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

            _levelField.isDelayed = true;
            foreach (var row in _rows)
            {
                row.Ev.isDelayed = true;
                row.Iv.isDelayed = true;
            }

            RegisterCallbacks();
        }
        
        public void Bind(SerializedProperty member)
        {
            _member = member;
            _levelProp = member.FindPropertyRelative(nameof(TeamMember.level));
            _natureProp = member.FindPropertyRelative(nameof(TeamMember.nature));
            
            var evs = member.FindPropertyRelative(nameof(TeamMember.evs));
            var ivs = member.FindPropertyRelative(nameof(TeamMember.ivs));
            foreach (var stat in StatInfo.All)
            {
                // [field: SerializeField] --> "<Name>k__BackingField"
                var fieldName = $"<{stat}>k__BackingField";
                _evProps[(int)stat] = evs.FindPropertyRelative(fieldName);
                _ivProps[(int)stat] = ivs.FindPropertyRelative(fieldName);
            }
            
            var natureEnum = Natures[_natureProp.enumValueIndex];
            _levelField.SetValueWithoutNotify(_levelProp.intValue);
            _natureField.SetValueWithoutNotify(NatureText(natureEnum));
            MarkNature(natureEnum);
        }

        public void ShowStats(StatsData stats)
        {
            foreach (var stat in StatInfo.All)
            {
                var row = _rows[(int)stat];
                var final = stats.Stats[stat];
                var ev = stats.EVs[stat];

                row.Base.text = stats.BaseStats[stat].ToString();
                row.Bar.SetValue(final, StatBarScale.MaxFor(stat));
                row.Ev.SetValueWithoutNotify(ev);
                row.EvSlider.SetValueWithoutNotify(ev / EvStep * EvStep);
                row.Iv.SetValueWithoutNotify(stats.IVs[stat]);
                row.Final.text = final.ToString();
            }

            _remainingEvsLabel.text = $"Left: {StatsData.MaxTotalEVs - stats.EVs.Sum}";
        }
        
        private void RegisterCallbacks()
        {
            _levelField.RegisterValueChangedCallback(evt => OnLevelChanged(evt.newValue));
            _natureField.RegisterValueChangedCallback(evt => 
                OnNatureChanged(_natureField.choices.IndexOf(evt.newValue)));

            foreach (var stat in StatInfo.All)
            {
                var s = stat;
                var row = _rows[(int)stat];
                row.Ev.RegisterValueChangedCallback(evt => OnEvChanged(s, evt.newValue, snapToStep: false));
                row.EvSlider.RegisterValueChangedCallback(evt => OnEvChanged(s, evt.newValue, snapToStep: true));
                row.Iv.RegisterValueChangedCallback(evt => OnIvChanged(s, evt.newValue));
            }
        }
        
        private void OnLevelChanged(int requested)
        {
            if (_member == null) return;

            var level = Mathf.Clamp(requested, StatsData.MinLevel, StatsData.MaxLevel);
            _levelField.SetValueWithoutNotify(level);
            Commit(WriteInt(_levelProp, level));
        }
        
        private void OnNatureChanged(int index)
        {
            if (_member == null || index < 0) return;

            MarkNature(Natures[index]);
            if (_natureProp.enumValueIndex == index) return;

            _natureProp.enumValueIndex = index;
            Commit(true);
        }
        
        private void OnIvChanged(Stat stat, int requested)
        {
            if (_member == null) return;

            var iv = Mathf.Clamp(requested, 0, StatsData.MaxIV);
            _rows[(int)stat].Iv.SetValueWithoutNotify(iv);
            Commit(WriteInt(_ivProps[(int)stat], iv));
        }
        
        private void OnEvChanged(Stat stat, int requested, bool snapToStep)
        {
            if (_member == null) return;

            var index = (int)stat;
            var others = _evProps.Sum(p => p.intValue) - _evProps[index].intValue;
            var budget = Mathf.Max(0, StatsData.MaxTotalEVs - others);

            var ev = Mathf.Clamp(requested, 0, Mathf.Min(StatsData.MaxEVPerStat, budget));
            if (snapToStep) ev = ev / EvStep * EvStep;

            _rows[index].Ev.SetValueWithoutNotify(ev);
            _rows[index].EvSlider.SetValueWithoutNotify(ev / EvStep * EvStep);
            Commit(WriteInt(_evProps[index], ev));
        }
        
        private static bool WriteInt(SerializedProperty property, int value)
        {
            if (property.intValue == value) return false;
            property.intValue = value;
            return true;
        }

        private void Commit(bool changed)
        {
            if (!changed) return;
            _member.serializedObject.ApplyModifiedProperties();
            OnChanged?.Invoke();
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