using UnityEngine;
using UnityEngine.UI;
using ThomasDev.HealthDamageSystem;

namespace ThomasDev.HealthSystem
{
    [DisallowMultipleComponent]
    public class HealthBarUI : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Health health;

        private void Start()
        {
            health.OnDamaged.AddListener(OnHealthChanged);
            health.OnHealed.AddListener(OnHealthChanged);
        }

        private void OnHealthChanged(float healthCurr, float healthMax)
        {
            image.fillAmount = healthCurr / healthMax;
        }
    }
}