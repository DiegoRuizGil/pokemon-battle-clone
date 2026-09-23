using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool
{
    public class PokemonDataEditor : VisualElement
    {
        private readonly IntegerField _idField;
        private readonly TextField _nameField;
        private readonly EnumField _type1Field;
        private readonly EnumField _type2Field;
        private readonly PropertyField _baseStatsField;

        public PokemonDataEditor()
        {
            _idField = new IntegerField("ID");
            _nameField = new TextField("Name");
            _type1Field = new EnumField("Type 1", ElementalType.None);
            _type2Field = new EnumField("Type 2", ElementalType.None);
            _baseStatsField = new PropertyField();
            
            var spritesContainer = new VisualElement();
            spritesContainer.AddToClassList("pokemon-sprites-container");
            
            var scrollView = new ScrollView();
            scrollView.Add(_idField);
            scrollView.Add(_nameField);
            scrollView.Add(_type1Field);
            scrollView.Add(_type2Field);
            scrollView.Add(_baseStatsField);
            scrollView.Add(spritesContainer);
            
            this.Add(scrollView);
            
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
                style.display = DisplayStyle.Flex;

                var serializedObject = new SerializedObject(pokemon);
                
                _idField.BindProperty(serializedObject.FindProperty("ID"));
                _nameField.BindProperty(serializedObject.FindProperty("pokemonName"));
                _type1Field.BindProperty(serializedObject.FindProperty("type1"));
                _type2Field.BindProperty(serializedObject.FindProperty("type2"));
                _baseStatsField.BindProperty(serializedObject.FindProperty("baseStats"));
            }
        }
    }
}