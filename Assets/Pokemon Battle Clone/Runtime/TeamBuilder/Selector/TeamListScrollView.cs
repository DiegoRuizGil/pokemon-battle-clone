using System.Collections.Generic;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.TeamBuilder.Selector
{
    public class TeamListScrollView : MonoBehaviour
    {
        [SerializeField] private List<TeamConfig> teamConfigs;

        [Header("UI")]
        [SerializeField] private TeamSelector teamSelectorPrefab;
        [SerializeField] private GameObject content;
        
        private void Awake()
        {
            foreach (var teamConfig in teamConfigs)
            {
                var selector = Instantiate(teamSelectorPrefab, content.transform);
                selector.Init(teamConfig);
            }
        }
    }
}