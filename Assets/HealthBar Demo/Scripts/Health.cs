using System;
using UnityEngine;

public class Health
{
    public Action OnHealthChanged;

    private float _health;
    private bool _isAlive = true;

    public Health(float health)
    {
        _health = health;
    }

    public float Value => _health;

    public bool IsAlive => _isAlive;

    public void Heal(float health)
    {
        if (_isAlive == false || health < 0)
        {
            return;
        }

        _health += health;
        OnHealthChanged?.Invoke();
    }

    public void TakeDamage(float damage)
    {
        if (_isAlive == false || damage < 0)
        {
            return;
        }

        _health -= damage;

        if (_health <= 0)
        {
            _isAlive = false;
        }

        OnHealthChanged?.Invoke();
    }
}
