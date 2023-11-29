using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HarshHealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private Health _health;
    private float _maxHealth;

    private Action _onInitialized;

    private void OnEnable()
    {
        _onInitialized += OnInitialized;
    }

    private void OnDisable()
    {
        _onInitialized -= OnInitialized;
        _health.OnHealthChanged -= HealthChanged;
    }

    public void Init(Health health)
    {
        _health = health;
        _maxHealth = health.Value;
        _slider.maxValue = _maxHealth;
        _slider.value = _health.Value;

        _onInitialized?.Invoke();
    }

    private void OnInitialized()
    {
        _health.OnHealthChanged += HealthChanged;
    }

    private void HealthChanged()
    {
        _slider.value = _health.Value;
    }
}
