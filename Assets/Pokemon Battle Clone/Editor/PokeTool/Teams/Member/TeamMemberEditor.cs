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
        public event Action OnRemoveRequested;
        public event Action<PokemonConfig> OnPokemonPicked;
        public event Action<int, MoveConfig> OnMovePicked; 
        
        private readonly PokemonSpritesRepository _spritesRepository;

        private readonly Image _sprite = new();
        private readonly VisualElement _typesRow = new();
        private readonly MemberPokemonField _pokemonField;
        private readonly MemberMovesField _movesField;
        private readonly MemberStatsField _statsField = new();
        
        private readonly PanelHost _detailPanel = new();
        
        private readonly Button _removeButton;
        
        private readonly VisualElement[] _memberOnly; // elements to hide when selecting a new pokemon

        private readonly MemberStatsEditor _statsEditor = new();
        
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
            
            var infoColumn = new VisualElement();
            infoColumn.AddToClassList("info-column");
            infoColumn.Add(_sprite);
            infoColumn.Add(_typesRow);
            infoColumn.Add(_pokemonField);

            _movesField = new MemberMovesField(_detailPanel, getMoves);
            _movesField.AddToClassList("moves-column");
            _movesField.OnPicked += (slot, move) => OnMovePicked?.Invoke(slot, move);
            
            _statsField.AddToClassList("stats-column");

            _removeButton = new Button(() => OnRemoveRequested?.Invoke())
            {
                iconImage = EditorGUIUtility.IconContent("d_TreeEditor.Trash").image as Texture2D
            };

            var row = new VisualElement();
            row.AddToClassList("member-row");
            row.Add(infoColumn);
            row.Add(_movesField);
            row.Add(_statsField);

            this.Add(row);
            this.Add(_removeButton);
            this.Add(_statsEditor);
            // this.Add(_detailPanel);

            _memberOnly = new[] { _typesRow, _movesField, _statsField, _removeButton, _statsEditor };
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
            
            SetTypes(config);
            _movesField.SetMoves(member.moves);
            _statsField.SetStats(member.BuildStatsData());
            _statsEditor.Bind(member);
        }
        
        public void ShowNewMember()
        {
            SetMemberElementsVisible(false);
            _sprite.sprite = _spritesRepository.LoadDefault(SpriteType.Front);
            _pokemonField.SetPokemon(null);
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