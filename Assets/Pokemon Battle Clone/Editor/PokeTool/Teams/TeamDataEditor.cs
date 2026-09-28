using System.Collections.Generic;
using Pokemon_Battle_Clone.Editor.Database;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool.Teams
{
    public class TeamDataEditor : VisualElement
    {
        private readonly PokemonSpritesRepository _spritesRepository;

        private readonly Label _nameLabel;
        private readonly Label _countLabel;
        private readonly TeamMemberSelector _selector;
        private readonly Label _memberPlaceHolder;

        private SerializedObject _serializedObject;
        private int _selectedIndex;
        
        private SerializedProperty MembersProperty => _serializedObject.FindProperty("pokemonList");

        public TeamDataEditor(PokemonSpritesRepository spritesRepository)
        {
            _spritesRepository = spritesRepository;

            _nameLabel = new Label();
            _nameLabel.AddToClassList("team-name");
            _countLabel = new Label();
            var header = new VisualElement();
            header.AddToClassList("team-header");
            header.Add(_nameLabel);
            header.Add(_countLabel);

            _selector = new TeamMemberSelector();
            _selector.OnSlotSelected += Select;

            _memberPlaceHolder = new Label();
            
            this.AddToClassList("data-editor");
            this.Add(header);
            this.Add(_selector);
            this.Add(_memberPlaceHolder);

            Bind(null);
        }

        public void Bind(TeamConfig team)
        {
            if (team == null)
            {
                _serializedObject = null;
                style.display = DisplayStyle.None;
                return;
            }
            
            style.display = DisplayStyle.Flex;
            _serializedObject = new SerializedObject(team);
            _nameLabel.text = team.name;
            _selectedIndex = 0;
            Refresh();
        }

        private void Refresh()
        {
            _serializedObject.Update();
            var members = MembersProperty;

            _countLabel.text = $"{members.arraySize}/{TeamMemberSelector.MaxMembers}";
            _selector.SetMembers(GetIcons(members), _selectedIndex);
            UpdateMemberArea();
        }

        private List<Sprite> GetIcons(SerializedProperty members)
        {
            var icons = new List<Sprite>();
            for (int i = 0; i < members.arraySize; i++)
            {
                var config = members.GetArrayElementAtIndex(i)
                    .FindPropertyRelative("pokemonConfig").objectReferenceValue as PokemonConfig;
                
                icons.Add(config != null
                    ? _spritesRepository.LoadOrDefault(config.ID, SpriteType.Icon)
                    : _spritesRepository.LoadDefault(SpriteType.Icon));
            }
            return icons;
        }

        private void Select(int index)
        {
            _selectedIndex = index;
            _selector.SetSelected(index);
            UpdateMemberArea();
        }

        private void UpdateMemberArea()
        {
            var adding = _selectedIndex >= MembersProperty.arraySize;
            _memberPlaceHolder.text = adding ? "Adding new member" : $"Member {_selectedIndex}";
        }
    }
}