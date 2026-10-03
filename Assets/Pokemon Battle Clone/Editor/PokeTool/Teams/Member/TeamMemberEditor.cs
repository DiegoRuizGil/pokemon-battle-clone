using System;
using System.Collections.Generic;
using Pokemon_Battle_Clone.Editor.Database;
using Pokemon_Battle_Clone.Editor.PokeTool.Icons;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool.Teams.Member
{
    public class TeamMemberEditor : VisualElement
    {
        private static readonly string[] StatNames = { "HP", "Atk", "Def", "SpA", "SpD", "Spe" };

        public event Action OnRemoveRequested;
        public event Action<PokemonConfig> OnPokemonPicked;
        public event Action<int, MoveConfig> OnMovePicked; 
        
        private readonly PokemonSpritesRepository _spritesRepository;

        private readonly Image _sprite = new();
        private readonly VisualElement _typesRow = new();
        private readonly MemberPokemonField _pokemonField;
        private readonly IntegerField _levelField = new();
        private readonly MemberMovesField _movesField;
        private readonly VisualElement _statsColumn = new();
        private readonly List<Label[]> _statRows = new(); // for each stat: [base, ev, iv]
        private readonly Label _natureLabel = new();
        
        private readonly PanelHost _detailPanel = new();
        
        private readonly Button _removeButton;
        
        private readonly VisualElement[] _memberOnly; // elements to hide when selecting a new pokemon
        
        public TeamMemberEditor(
            PokemonSpritesRepository spritesRepository,
            Func<List<PokemonConfig>> getPokemons,
            Func<List<MoveConfig>> getMoves)
        {
            _spritesRepository = spritesRepository;
            this.AddToClassList("member-editor");
            
            _sprite.AddToClassList("member-sprite");
            _typesRow.AddToClassList("types-row");

            _pokemonField = new MemberPokemonField(
                _detailPanel,
                getPokemons,
                getIcon: p => spritesRepository.LoadOrDefault(p.ID, SpriteType.Icon));
            _pokemonField.OnPicked += pokemon => OnPokemonPicked?.Invoke(pokemon);
            
            // _levelField.label = "Lv";
            _levelField.AddToClassList("level-field");
            _levelField.RegisterCallback<FocusOutEvent>(
                _ => _levelField.value = Mathf.Clamp(_levelField.value, 1, 100));

            var nameRow = new VisualElement();
            nameRow.AddToClassList("name-row");
            nameRow.Add(_pokemonField);
            nameRow.Add(_levelField);
            
            var infoColumn = new VisualElement();
            infoColumn.AddToClassList("info-column");
            infoColumn.Add(_sprite);
            infoColumn.Add(_typesRow);
            infoColumn.Add(nameRow);

            _movesField = new MemberMovesField(_detailPanel, getMoves);
            _movesField.AddToClassList("moves-column");
            _movesField.OnPicked += (slot, move) => OnMovePicked?.Invoke(slot, move);

            _statsColumn.AddToClassList("stats-column");
            BuildStatsColumn();

            _removeButton = new Button(() => OnRemoveRequested?.Invoke())
            {
                iconImage = EditorGUIUtility.IconContent("d_TreeEditor.Trash").image as Texture2D
            };

            var row = new VisualElement();
            row.AddToClassList("member-row");
            row.Add(infoColumn);
            row.Add(_movesField);
            row.Add(_statsColumn);

            this.Add(row);
            this.Add(_detailPanel);
            // this.Add(_removeButton);

            _memberOnly = new[] { _typesRow, _levelField, _movesField, _statsColumn, _removeButton };
        }

        public void CloseDetail() => _detailPanel.Close();
        public void FocusPokemonField() => _pokemonField.FocusField();
        
        public void Bind(SerializedObject serializedObject, int index)
        {
            var team = (TeamConfig)serializedObject.targetObject;
            var member = team.pokemonList[index];
            var config = member.pokemonConfig;

            SetMemberElementsVisible(true);

            _sprite.sprite = config != null
                ? _spritesRepository.LoadOrDefault(config.ID, SpriteType.Front)
                : _spritesRepository.LoadDefault(SpriteType.Front);
            _pokemonField.SetPokemon(config);

            _levelField.BindProperty(serializedObject.FindProperty("pokemonList")
                .GetArrayElementAtIndex(index).FindPropertyRelative("level"));

            ShowTypes(config);
            _movesField.SetMoves(member.moves);
            ShowStats(config, member);
        }
        
        public void ShowNewMember()
        {
            _levelField.Unbind();
            SetMemberElementsVisible(false);
            _sprite.sprite = _spritesRepository.LoadDefault(SpriteType.Front);
            _pokemonField.SetPokemon(null);
        }
        
        private void SetMemberElementsVisible(bool isVisible)
        {
            foreach (var element in _memberOnly)
                element.style.display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;
        }
        
        private void ShowTypes(PokemonConfig config)
        {
            _typesRow.Clear();
            if (config == null) return;

            AddTypeIcon(config.type1);
            if (config.type2 != ElementalType.None) AddTypeIcon(config.type2);
        }
        
        private void AddTypeIcon(ElementalType type)
        {
            var iconId = PokeToolIcons.TypeToIconId(type);
            var icon = PokeToolIcons.GetImage(iconId, 32);
            icon.AddToClassList("type-icon");
            _typesRow.Add(icon);
        }
        
        private void ShowStats(PokemonConfig config, TeamMember member)
        {
            var baseValues = config != null ? config.baseStats.Values : new int[StatNames.Length];
            var evs = member.evs.Values;
            var ivs = member.ivs.Values;

            for (var i = 0; i < _statRows.Count; i++)
            {
                _statRows[i][0].text = baseValues[i].ToString();
                _statRows[i][1].text = evs[i].ToString();
                _statRows[i][2].text = ivs[i].ToString();
            }
            _natureLabel.text = $"Nature: {member.nature}";
        }
        
        private void BuildStatsColumn()
        {
            AddStatRow("", "Base", "EV", "IV"); // header
            foreach (var statName in StatNames)
                _statRows.Add(AddStatRow(statName, "", "", ""));
            _statsColumn.Add(_natureLabel);
        }
        
        private Label[] AddStatRow(string statName, string baseText, string evText, string ivText)
        {
            var row = new VisualElement();
            row.AddToClassList("stat-row");

            var nameLabel = new Label(statName);
            nameLabel.AddToClassList("stat-name");
            row.Add(nameLabel);

            var values = new[] { new Label(baseText), new Label(evText), new Label(ivText) };
            foreach (var value in values)
            {
                value.AddToClassList("stat-value");
                row.Add(value);
            }

            _statsColumn.Add(row);
            return values;
        }
    }
}