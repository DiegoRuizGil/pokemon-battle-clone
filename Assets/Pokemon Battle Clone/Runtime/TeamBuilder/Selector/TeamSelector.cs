using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon_Battle_Clone.Runtime.TeamBuilder.Selector
{
    public class TeamSelector : MonoBehaviour
    {
        [SerializeField] private List<Image> icons;
        
        [Header("Buttons")]
        [SerializeField] private Button playerButton;
        [SerializeField] private Button rivalButton;
        [SerializeField] private Button infoButton;
        
        private TeamConfig _teamConfig;
        private IPokemonSpriteProvider _spriteProvider;
        
        public event Action<TeamConfig> OnPlayerSelected = delegate { };
        public event Action<TeamConfig> OnRivalSelected = delegate { };
        public event Action<TeamConfig> OnInfoSelected = delegate { };

        private void Awake()
        {
            BindActions();
        }

        public void Init(TeamConfig teamConfig, IPokemonSpriteProvider spriteProvider)
        {
            _teamConfig = teamConfig;
            _spriteProvider = spriteProvider;

            DisplayIcons();
        }

        private async void DisplayIcons()
        {
            var sprites = await Task.WhenAll(
                _teamConfig.pokemonList.Select(p => _spriteProvider.GetIconSprite((uint)p.pokemonConfig.ID))
            );
            for (var i = 0; i < icons.Count; i++)
            {
                if (_teamConfig.pokemonList.Count > i)
                {
                    icons[i].sprite = sprites[i];
                    icons[i].gameObject.SetActive(true);
                }
                else
                {
                    icons[i].gameObject.SetActive(false);
                }
            }
        }

        private void BindActions()
        {
            playerButton?.onClick.AddListener(() => OnPlayerSelected.Invoke(_teamConfig));
            rivalButton?.onClick.AddListener(() => OnRivalSelected.Invoke(_teamConfig));
            infoButton?.onClick.AddListener(() => OnInfoSelected.Invoke(_teamConfig));
        }
    }
}