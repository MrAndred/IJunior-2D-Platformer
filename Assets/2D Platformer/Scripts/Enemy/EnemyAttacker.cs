using UnityEngine;
public class EnemyAttacker
{
    private Transform _enemy;
    private Animator _animator;
    private float _attackRate;
    private float _damage;

    private float _lastAttackTime = 0f;
    private Player _player = null;
    private string _attackTriggerName = "Attack";

    public EnemyAttacker(Transform enemy, Animator animator, float attackRate, float damage)
    {
        _enemy = enemy;
        _animator = animator;
        _attackRate = attackRate;
        _damage = damage;
    }

    public void CheckPlayer(float attackDistance, LayerMask playerMask)
    {
        RaycastHit2D hit = Physics2D.Raycast(_enemy.position, _enemy.right, attackDistance, playerMask);

        if (hit.collider == true && hit.collider.TryGetComponent(out Player player))
        {
            if (player.IsAlive == true)
            {
                Attack(player);
            }
        }
    }

    private void Attack(Player player)
    {
        _lastAttackTime += Time.deltaTime;

        if (_lastAttackTime < _attackRate)
        {
            return;
        }

        if (player.Health <= 0)
        {
            return;
        }

        _animator.SetTrigger(_attackTriggerName);
        player.TakeDamage(_damage);
        _lastAttackTime = 0f;
    }
}
