using UnityEngine;

public class PlayerHealth
{
    private Animator _animator;

    private float _health;
    private bool _isAlive = true;

    private string _takeDamageTriggerName = "TakeDamage";
    private string _dieTriggerName = "Die";

    public PlayerHealth(Animator animator, float health)
    {
        _animator = animator;
        _health = health;
    }

    public float Health => _health;

    public bool IsAlive => _isAlive;

    public void Heal(float health)
    {
        if (_isAlive == false || health < 0)
        {
            return;
        }

        _health += health;
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
            _animator.SetTrigger(_dieTriggerName);
        }
        else
        {
            _animator.SetTrigger(_takeDamageTriggerName);
        }
    }
}
