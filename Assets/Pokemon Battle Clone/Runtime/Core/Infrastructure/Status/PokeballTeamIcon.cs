using UnityEngine;
using UnityEngine.UI;

namespace Pokemon_Battle_Clone.Runtime.Core.Infrastructure.Status
{
    public class PokeballTeamIcon : MonoBehaviour
    {
        [SerializeField] private Image disableImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private Sprite activePokeball;
        [SerializeField] private Sprite defeatedPokeball;
        
        private Image _pokeballIcon;
        
        public uint PokemonID { get; private set; }

        private void Awake()
        {
            _pokeballIcon = GetComponent<Image>();
        }

        public void Init(uint pokemonID)
        {
            PokemonID = pokemonID;
            SetAsAlive();
        }

        public void SetAsDefeated()
        {
            disableImage.gameObject.SetActive(false);
            iconImage.gameObject.SetActive(true);
            
            iconImage.sprite = defeatedPokeball;
        }

        public void SetAsAlive()
        {
            disableImage.gameObject.SetActive(false);
            iconImage.gameObject.SetActive(true);
            
            iconImage.sprite = activePokeball;
        }

        public void SetAsDisabled()
        {
            disableImage.gameObject.SetActive(true);
            iconImage.gameObject.SetActive(false);
        }
    }
}