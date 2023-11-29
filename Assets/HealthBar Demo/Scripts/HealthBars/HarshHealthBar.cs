using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Slider))]
public class HarshHealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private Health _health;

    private float _maxHealth;

    public void Init(Health health)
    {
        _health = health;
        _maxHealth = health.Value;
        _health.OnHealthChanged += HealthChanged;
        _slider.maxValue = _maxHealth;
        _slider.value = _health.Value;
    }

    private void HealthChanged()
    {
        _slider.value = _health.Value;
    }
}
