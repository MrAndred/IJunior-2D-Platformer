using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HarshHealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private Health _health;
    private float _maxHealth;
    private bool _isInitialized;

    private void OnEnable()
    {
        if (_isInitialized == true)
        {
            _health.OnHealthChanged += HealthChanged;
        }
    }

    private void OnDisable()
    {
        _health.OnHealthChanged -= HealthChanged;
    }

    public void Init(Health health)
    {
        _health = health;
        _maxHealth = health.Value;
        _slider.maxValue = _maxHealth;
        _slider.value = _health.Value;
        _health.OnHealthChanged += HealthChanged;
    }

    private void HealthChanged()
    {
        _slider.value = _health.Value;
    }
}
