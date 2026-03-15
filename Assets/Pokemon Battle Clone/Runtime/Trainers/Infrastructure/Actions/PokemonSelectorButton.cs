using System;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Core.Infrastructure.Status;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon_Battle_Clone.Runtime.Trainers.Infrastructure.Actions
{
    [RequireComponent(typeof(Button))]
    public class PokemonSelectorButton : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private HealthView healthView;

        [Space(10)]
        [SerializeField] private Button infoButton;
        
        [HideInInspector] public int index;
        
        private HUDButton _hudButton;
        
        public event Action<int> OnClick = delegate { };
        public event Action<int> OnInfoSelected = delegate { };

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => OnClick.Invoke(index));
            infoButton.onClick.AddListener(() => OnInfoSelected.Invoke(index));
            
            _hudButton = GetComponent<HUDButton>();
        }

        public void SetData(Pokemon pokemon, Sprite icon)
        {
            iconImage.sprite = icon;
            nameText.text = pokemon.Name;
            levelText.text = $"Lv. {pokemon.Stats.Level}";
            healthView.SetHealth(pokemon.Health.Max, pokemon.Health.Current, animated: false);
            
            _hudButton.SetInteraction(!pokemon.Defeated);
        }
    }
}