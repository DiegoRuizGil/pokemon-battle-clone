using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool
{
    [UxmlElement]
    public partial class PokemonDataEditor : VisualElement
    {
        private readonly ScrollView _scrollView;
        
        private readonly IntegerField _idField;
        private readonly TextField _nameField;
        private readonly EnumField _type1Field;
        private readonly EnumField _type2Field;
        private readonly PropertyField _baseStatsField;
        private readonly SpriteField _frontSpriteField;
        private readonly SpriteField _backSpriteField;
        private readonly SpriteField _iconSpriteField;
        
        public PokemonDataEditor()
        {
            _scrollView = new ScrollView();
            
            _idField = new IntegerField("ID");
            _nameField = new TextField("Name");
            _type1Field = new EnumField("Type 1", ElementalType.None);
            _type2Field = new EnumField("Type 2", ElementalType.None);
            _baseStatsField = new PropertyField();
            _frontSpriteField = new SpriteField("Front Sprite");
            _backSpriteField = new SpriteField("Back Sprite");
            _iconSpriteField = new SpriteField("Icon Sprite");
            
            _scrollView.Add(_idField);
            _scrollView.Add(_nameField);
            _scrollView.Add(_type1Field);
            _scrollView.Add(_type2Field);
            _scrollView.Add(_baseStatsField);
            _scrollView.Add(_frontSpriteField);
            _scrollView.Add(_backSpriteField);
            _scrollView.Add(_iconSpriteField);
            
            this.Add(_scrollView);
            
            BindPokemon(null);
        }

        public void BindPokemon(PokemonConfig pokemon)
        {
            if (pokemon == null)
            {
                this.Unbind();
                style.display = DisplayStyle.None;
            }
            else
            {
                Debug.Log($"Binding: {pokemon.pokemonName}");
                
                style.display = DisplayStyle.Flex;

                var serializedObject = new SerializedObject(pokemon);
                _idField.BindProperty(serializedObject.FindProperty("ID"));
                _nameField.BindProperty(serializedObject.FindProperty("pokemonName"));
                _type1Field.BindProperty(serializedObject.FindProperty("type1"));
                _type2Field.BindProperty(serializedObject.FindProperty("type2"));
                _baseStatsField.BindProperty(serializedObject.FindProperty("baseStats"));
                _frontSpriteField.BindProperty(serializedObject.FindProperty("frontSprite"));
                _backSpriteField.BindProperty(serializedObject.FindProperty("backSprite"));
                _iconSpriteField.BindProperty(serializedObject.FindProperty("iconSprite"));
            }
        }
    }

    [UxmlElement]
    public partial class SpriteField : VisualElement
    {
        private readonly ObjectField _spriteField;
        private readonly Image _preview;

        public SpriteField() { }
        
        public SpriteField(string label)
        {
            _spriteField = new ObjectField(label) { objectType = typeof(Sprite) };
            _preview = new Image();
            _preview.AddToClassList("sprite-preview");

            _spriteField.RegisterValueChangedCallback(UpdatePreview);
            
            this.Add(_spriteField);
            this.Add(_preview);
        }

        private void UpdatePreview(ChangeEvent<Object> evt)
        {
            var sprite = evt.newValue as Sprite;
            _preview.sprite = sprite;
        }

        public void BindProperty(SerializedProperty property) => _spriteField.BindProperty(property);
    }
}