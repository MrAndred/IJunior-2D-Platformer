using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextHealthBar : MonoBehaviour
{
    private const string _textFormat = "{0}/{1}";

    [SerializeField] private TextMeshProUGUI _text;

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
        _text.text = string.Format(_textFormat, _health.Value, _maxHealth);

        _onInitialized?.Invoke();
    }

    private void OnInitialized()
    {
        _health.OnHealthChanged += HealthChanged;
    }

    private void HealthChanged()
    {
        _text.text = string.Format(_textFormat, _health.Value, _maxHealth);
    }
}
