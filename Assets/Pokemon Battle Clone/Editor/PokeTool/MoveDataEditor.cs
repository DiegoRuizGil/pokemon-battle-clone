using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Database;
using Pokemon_Battle_Clone.Runtime.Moves.Domain;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool
{
    public class MoveDataEditor : VisualElement
    {
        private readonly IntegerField _idField;
        private readonly TextField _nameField;
        private readonly EnumField _typeField;
        private readonly EnumField _categoryField;
        private readonly IntegerField _ppField;
        private readonly IntegerField _accuracyField;
        private readonly IntegerField _powerField;
        private readonly IntegerField _priorityField;
        private readonly PropertyField _mainEffectField;
        private readonly PropertyField _additionalEffectsField;

        public MoveDataEditor()
        {
            _idField = new IntegerField("ID");
            _idField.SetEnabled(false);
            _nameField = new TextField("Name");
            _nameField.SetEnabled(false);
            _typeField = new EnumField("Type", ElementalType.Normal);
            _categoryField = new EnumField("Category", MoveCategory.Physical);
            _ppField = new IntegerField("PP");
            _accuracyField = new IntegerField("Accuracy");
            _powerField = new IntegerField("Power");
            _priorityField = new IntegerField("Priority");
            _mainEffectField = new PropertyField();
            _additionalEffectsField = new PropertyField();
            
            var scrollView = new ScrollView();
            scrollView.Add(_idField);
            scrollView.Add(_nameField);
            scrollView.Add(_typeField);
            scrollView.Add(_categoryField);
            scrollView.Add(_ppField);
            scrollView.Add(_accuracyField);
            scrollView.Add(_powerField);
            scrollView.Add(_priorityField);
            scrollView.Add(_mainEffectField);
            scrollView.Add(_additionalEffectsField);
            
            this.Add(scrollView);
            this.AddToClassList("data-editor");
            
            Bind(null);
        }

        public void Bind(MoveConfig move)
        {
            if (move == null)
            {
                this.Unbind();
                style.display = DisplayStyle.None;
            }
            else
            {
                style.display = DisplayStyle.Flex;

                var serializedObject = new SerializedObject(move);

                _idField.BindProperty(serializedObject.FindProperty("id"));
                _nameField.BindProperty(serializedObject.FindProperty("moveName"));
                _typeField.BindProperty(serializedObject.FindProperty("type"));
                _categoryField.BindProperty(serializedObject.FindProperty("category"));
                _ppField.BindProperty(serializedObject.FindProperty("pp"));
                _accuracyField.BindProperty(serializedObject.FindProperty("accuracy"));
                _powerField.BindProperty(serializedObject.FindProperty("power"));
                _priorityField.BindProperty(serializedObject.FindProperty("priority"));
                _mainEffectField.BindProperty(serializedObject.FindProperty("mainEffect"));
                _additionalEffectsField.BindProperty(serializedObject.FindProperty("additionalEffects"));
            }
        }
    }
}