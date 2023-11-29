using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    [SerializeField] private Button _takeDamageButton;
    [SerializeField] private Button _healButton;

    [SerializeField] private float _maxHealth;

    [SerializeField] private TextHealthBar _textHealthBar;
    [SerializeField] private HarshHealthBar _harshHealthBar;
    [SerializeField] private SmoothHealthBar _smoothHealthBar;

    [SerializeField] private float _healValue = 10f;
    [SerializeField] private float _damageValue = 10f;

    private Health _health;

    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        _takeDamageButton.onClick.AddListener(() => _health.TakeDamage(_damageValue));
        _healButton.onClick.AddListener(() => _health.Heal(_healValue));
    }

    private void OnDisable()
    {
        _takeDamageButton.onClick.RemoveListener(() => _health.TakeDamage(_damageValue));
        _healButton.onClick.RemoveListener(() => _health.Heal(_healValue));
    }

    public void Init()
    {
        _health = new Health(_maxHealth);

        _textHealthBar.Init(_health);
        _harshHealthBar.Init(_health);
        _smoothHealthBar.Init(_health);
    }
}
