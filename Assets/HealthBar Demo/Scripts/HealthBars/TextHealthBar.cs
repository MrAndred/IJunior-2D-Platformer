using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextHealthBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private Health _health;
    private float _maxHealth;

    private const string _textFormat = "{0}/{1}";

    public void Init(Health health)
    {
        _health = health;
        _maxHealth = health.Value;
        _health.OnHealthChanged += HealthChanged;
        _text.text = string.Format(_textFormat, _health.Value, _maxHealth);
    }

    private void HealthChanged()
    {
        _text.text = string.Format(_textFormat, _health.Value, _maxHealth);
    }
}
