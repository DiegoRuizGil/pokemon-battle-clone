using Pokemon_Battle_Clone.Editor.Database;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool
{
    public class PokemonDataEditor : VisualElement
    {
        private readonly PokemonSpritesRepository _spritesRepository;
        
        private readonly IntegerField _idField;
        private readonly TextField _nameField;
        private readonly EnumField _type1Field;
        private readonly EnumField _type2Field;
        private readonly PropertyField _baseStatsField;
        private readonly PokemonSpritesPreview _spritesPreview;

        public PokemonDataEditor(PokemonSpritesRepository spritesRepository)
        {
            _spritesRepository = spritesRepository;
            
            _idField = new IntegerField("ID");
            _idField.SetEnabled(false);
            _nameField = new TextField("Name");
            _nameField.SetEnabled(false);
            _type1Field = new EnumField("Type 1", ElementalType.None);
            _type2Field = new EnumField("Type 2", ElementalType.None);
            _baseStatsField = new PropertyField();
            _spritesPreview = new PokemonSpritesPreview();
            
            var scrollView = new ScrollView();
            scrollView.Add(_idField);
            scrollView.Add(_nameField);
            scrollView.Add(_type1Field);
            scrollView.Add(_type2Field);
            scrollView.Add(_baseStatsField);
            scrollView.Add(_spritesPreview);
            
            this.Add(scrollView);
            
            Bind(null);
        }

        public void Bind(PokemonConfig pokemon)
        {
            if (pokemon == null)
            {
                this.Unbind();
                style.display = DisplayStyle.None;
            }
            else
            {
                style.display = DisplayStyle.Flex;

                var serializedObject = new SerializedObject(pokemon);
                
                _idField.BindProperty(serializedObject.FindProperty("ID"));
                _nameField.BindProperty(serializedObject.FindProperty("pokemonName"));
                _type1Field.BindProperty(serializedObject.FindProperty("type1"));
                _type2Field.BindProperty(serializedObject.FindProperty("type2"));
                _baseStatsField.BindProperty(serializedObject.FindProperty("baseStats"));
                BindSprites(pokemon.ID);
            }
        }

        private void BindSprites(int id)
        {
            var front = _spritesRepository.Load(id, SpriteType.Front);
            var back = _spritesRepository.Load(id, SpriteType.Back);
            var icon = _spritesRepository.Load(id, SpriteType.Icon);
            
            _spritesPreview.SetSprites(front, back, icon);
        }
    }
}