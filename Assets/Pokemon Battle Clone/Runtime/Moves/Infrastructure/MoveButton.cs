using System;
using Pokemon_Battle_Clone.Runtime.Core.Infrastructure.Types;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Pokemon_Battle_Clone.Runtime.Moves.Infrastructure
{
    [RequireComponent(typeof(Button))]
    public class MoveButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Images")]
        [SerializeField] private Image typeIcon;
        [SerializeField] private Image background;
        [SerializeField] private Image border;
        [SerializeField] private Image ppBackground;
        
        [Header("Text")]
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI ppText;
        
        [Header("Colors")]
        [SerializeField] private Color enabledColor = Color.black;
        [SerializeField] private Color disabledColor = Color.red;
        
        [Space(10)]
        [SerializeField] private ElementalTypesConfig elementalTypesConfig;

        [HideInInspector] public int index;
        
        private Button _button;
        
        public event Action<int> OnClick = delegate { };

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => OnClick.Invoke(index));
        }
        
        public void SetData(MoveDTO move)
        {
            nameText.text = move.Name;
            ppText.text = $"{move.CurrentPP} / {move.MaxPP}";

            SetInteraction(move.CurrentPP > 0);
            
            typeIcon.sprite = elementalTypesConfig.GetIcon(move.Type);
            background.color = elementalTypesConfig.GetColor(move.Type);
        }
        
        private void OnDisable()
        {
            border.gameObject.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            border.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            border.gameObject.SetActive(false);
        }

        private void SetInteraction(bool interactable)
        {
            _button.interactable = interactable;
            ppBackground.color = interactable ? enabledColor : disabledColor;
            border.color = interactable ? enabledColor : disabledColor;
        }
    }
}