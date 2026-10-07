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
        public event Action<int> OnMoveRequested; 
        public event Action OnRemoveRequested;
        public event Action<PokemonConfig> OnPokemonPicked;
        public event Action<int, MoveConfig> OnMovePicked; 
        
        private readonly PokemonSpritesRepository _spritesRepository;

        private readonly VisualElement _toolbar = new();
        private readonly Button _moveLeftButton;
        private readonly Button _moveRightButton;
        private readonly Button _removeButton;
        
        private readonly Image _sprite = new();
        private readonly VisualElement _typesRow = new();
        private readonly MemberPokemonField _pokemonField;
        private readonly MemberMovesField _movesField;
        private readonly MemberStatsField _statsField;
        
        private readonly PanelHost _detailPanel = new();
        
        
        private readonly VisualElement[] _memberOnly; // elements to hide when selecting a new pokemon

        private SerializedObject _serializedObject;
        private int _index;
        
        public TeamMemberEditor(
            PokemonSpritesRepository spritesRepository,
            Func<List<PokemonConfig>> getPokemons,
            Func<List<MoveConfig>> getMoves)
        {
            _spritesRepository = spritesRepository;
            this.AddToClassList("member-editor");
            
            _sprite.AddToClassList("member-sprite");
            _typesRow.AddToClassList("pokemon-types");
            var topRow = new VisualElement();
            topRow.AddToClassList("info-top-row");
            topRow.Add(_sprite);
            topRow.Add(_typesRow);
            
            _pokemonField = new MemberPokemonField(
                _detailPanel,
                getPokemons,
                getIcon: p => spritesRepository.LoadOrDefault(p.ID, SpriteType.Icon));
            _pokemonField.OnPicked += pokemon => OnPokemonPicked?.Invoke(pokemon);
            _pokemonField.AddToClassList("pokemon-field");
            
            var infoColumn = new VisualElement();
            infoColumn.AddToClassList("info-section");
            infoColumn.Add(topRow);
            infoColumn.Add(_pokemonField);

            _movesField = new MemberMovesField(_detailPanel, getMoves);
            _movesField.AddToClassList("moves-column");
            _movesField.OnPicked += (slot, move) => OnMovePicked?.Invoke(slot, move);

            _moveLeftButton = new Button(() => OnMoveRequested?.Invoke(-1)) { text = "◀" };
            _moveRightButton = new Button(() => OnMoveRequested?.Invoke(1)) { text = "▶" };
            var arrows = new VisualElement();
            arrows.AddToClassList("member-arrows");
            arrows.Add(_moveLeftButton);
            arrows.Add(_moveRightButton);
            
            _removeButton = new Button(() => OnRemoveRequested?.Invoke())
            {
                text = "Delete",
                iconImage = EditorGUIUtility.IconContent("d_TreeEditor.Trash").image as Texture2D
            };

            _toolbar.AddToClassList("member-toolbar");
            _toolbar.Add(arrows);
            _toolbar.Add(_removeButton);
            
            _statsField = new MemberStatsField(_detailPanel);
            _statsField.AddToClassList("stats-column");
            _statsField.OnChanged += OnStatsChanged;

            var row = new VisualElement();
            row.AddToClassList("member-row");
            row.Add(infoColumn);
            row.Add(_movesField);
            row.Add(_statsField);

            this.Add(_toolbar);
            this.Add(row);
            this.Add(_detailPanel);
            
            _memberOnly = new[] { _typesRow, _movesField, _statsField, _toolbar };
        }

        public void CloseDetail() => _detailPanel.Close();
        public void FocusPokemonField() => _pokemonField.FocusField();
        
        public void Bind(SerializedObject serializedObject, int index)
        {
            _serializedObject = serializedObject;
            _index = index;
            
            var team = (TeamConfig)serializedObject.targetObject;
            var member = team.pokemonList[index];
            var config = member.pokemonConfig;
            _moveLeftButton.SetEnabled(index > 0);
            _moveRightButton.SetEnabled(index < team.pokemonList.Count - 1);

            SetMemberElementsVisible(true);

            _sprite.sprite = config != null
                ? _spritesRepository.LoadOrDefault(config.ID, SpriteType.Front)
                : _spritesRepository.LoadDefault(SpriteType.Front);
            _pokemonField.SetPokemon(config);
            
            SetTypes(config);
            _movesField.SetMoves(member.moves);
            
            var memberProperty = serializedObject
                .FindProperty(nameof(TeamConfig.pokemonList))
                .GetArrayElementAtIndex(index);
            _statsField.SetMember(memberProperty);
            _statsField.SetStats(member.BuildStatsData());
        }
        
        public void ShowNewMember()
        {
            SetMemberElementsVisible(false);
            _sprite.sprite = _spritesRepository.LoadDefault(SpriteType.Front);
            _pokemonField.SetPokemon(null);
        }
        
        private void OnStatsChanged()
        {
            var team = (TeamConfig)_serializedObject.targetObject;
            var stats = team.pokemonList[_index].BuildStatsData();
            _statsField.SetStats(stats);
        }
        
        private void SetMemberElementsVisible(bool isVisible)
        {
            foreach (var element in _memberOnly)
                element.style.display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;
        }
        
        private void SetTypes(PokemonConfig config)
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
    }
}