using System;
using System.Collections.Generic;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon_Battle_Clone.Runtime.TeamBuilder.Selector
{
    public class TeamSelector : MonoBehaviour
    {
        [SerializeField] private PokemonAssetDatabase assetDatabase;
        [SerializeField] private List<Image> icons;
        
        [Header("Buttons")]
        [SerializeField] private Button playerButton;
        [SerializeField] private Button rivalButton;
        [SerializeField] private Button infoButton;
        
        private TeamConfig _teamConfig;
        
        public event Action<TeamConfig> OnPlayerSelected = delegate { };
        public event Action<TeamConfig> OnRivalSelected = delegate { };
        public event Action<TeamConfig> OnInfoSelected = delegate { };

        private void Awake()
        {
            BindActions();
        }

        public void Init(TeamConfig teamConfig)
        {
            _teamConfig = teamConfig;
            for (var i = 0; i < icons.Count; i++)
            {
                if (_teamConfig.pokemonList.Count > i)
                {
                    icons[i].sprite = assetDatabase.GetIcon((uint)_teamConfig.pokemonList[i].pokemonConfig.ID);
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
            playerButton.onClick.AddListener(() => OnPlayerSelected.Invoke(_teamConfig));
            rivalButton.onClick.AddListener(() => OnRivalSelected.Invoke(_teamConfig));
            infoButton.onClick.AddListener(() => OnInfoSelected.Invoke(_teamConfig));
        }
    }
}