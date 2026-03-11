using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon_Battle_Clone.Runtime.Core.Infrastructure.Status
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private TextMeshProUGUI healthText;
        [Space(10)]
        [SerializeField] private float barSpeed = 5f;
        [SerializeField] private Color highHealthColor = Color.green;
        [SerializeField] private Color middleHealthColor = Color.yellow;
        [SerializeField] private Color lowHealthColor = Color.red;

        private float _target = 1f;

        private void Awake()
        {
            fillImage.fillAmount = 1;
        }

        private void Update()
        {
            UpdateBar();
        }

        public void SetHealth(int maxHealth, int currentHealth, bool animated)
        {
            _target = (float)currentHealth / maxHealth;
            if (!animated)
                fillImage.fillAmount = _target;
            healthText.text = $"{currentHealth}/{maxHealth}";
        }

        private void UpdateBar()
        {
            fillImage.fillAmount = Mathf.MoveTowards(fillImage.fillAmount, _target, Time.deltaTime * barSpeed);
            SetColor();
        }

        private void SetColor()
        {
            if (fillImage.fillAmount >= 0.5f)
                fillImage.color = highHealthColor;
            else if (fillImage.fillAmount >= 0.2f)
                fillImage.color = middleHealthColor;
            else
                fillImage.color = lowHealthColor;
        }
    }
}