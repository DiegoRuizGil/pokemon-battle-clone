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
        private readonly TeamMemberEditor _memberEditor;
        
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

            _memberEditor = new TeamMemberEditor(spritesRepository);
            _memberEditor.OnRemoveRequested += RemoveSelectedMember;
            
            this.AddToClassList("data-editor");
            this.Add(header);
            this.Add(_selector);
            this.Add(_memberEditor);

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
            if (_selectedIndex >= MembersProperty.arraySize)
                _memberEditor.ShowNewMember();
            else
                _memberEditor.Bind(_serializedObject, _selectedIndex);
        }
        
        private void RemoveSelectedMember()
        {
            var members = MembersProperty;
            members.DeleteArrayElementAtIndex(_selectedIndex);
            _serializedObject.ApplyModifiedProperties();

            // si borras el último, pasas al anterior; si el equipo queda vacío, 0 == estado "+"
            _selectedIndex = Mathf.Max(0, Mathf.Min(_selectedIndex, members.arraySize - 1));
            Refresh();
        }
    }
}