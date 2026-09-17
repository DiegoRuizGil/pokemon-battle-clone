using System;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool
{
    [UxmlElement]
    public partial class PokemonListEntry : VisualElement
    {
        public event Action<PokemonConfig> OnDeleteRequested; 
        
        private readonly Label _label;

        private PokemonConfig _boundPokemon;
        
        public PokemonListEntry()
        {
            _label = new Label();
            var deleteButton = new Button(OnDeleteClicked);
            deleteButton.iconImage = EditorGUIUtility.IconContent("d_TreeEditor.Trash").image as Texture2D;
            
            this.AddToClassList("list-entry");
            deleteButton.AddToClassList("delete-button");
            _label.AddToClassList("entry-label");
            
            this.Add(_label);
            this.Add(deleteButton);
        }

        public void Bind(PokemonConfig pokemonConfig)
        {
            _boundPokemon = pokemonConfig;
            _label.text = $"{pokemonConfig.ID:D3} - {pokemonConfig.pokemonName}";
        }

        private void OnDeleteClicked() => OnDeleteRequested?.Invoke(_boundPokemon);
    }
}