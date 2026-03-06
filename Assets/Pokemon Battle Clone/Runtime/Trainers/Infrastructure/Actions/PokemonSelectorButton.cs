using System;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon_Battle_Clone.Runtime.Trainers.Infrastructure.Actions
{
    [RequireComponent(typeof(Button))]
    public class PokemonSelectorButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI pokemonNameText;
        
        [HideInInspector] public int index;
        
        private Button _button;
        
        public event Action<int> OnClick = delegate { };

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => OnClick.Invoke(index));
        }

        public void SetData(Pokemon pokemon)
        {
            pokemonNameText.text = pokemon.Name;
            _button.interactable = !pokemon.Defeated;
        }
    }
}