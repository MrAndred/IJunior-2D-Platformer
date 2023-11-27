using UnityEngine;

public class PlayerAttacker
{
    private Transform _player;
    private Animator _animator;

    private float _lastAttackTime;
    private float _attackRate;
    private float _attackDistance;
    private float _damage;

    private string _attackTriggerName = "Attack";

    public PlayerAttacker(Transform player, Animator animator, float attackRate, float damage, float attackDistance)
    {
        _player = player;
        _attackDistance = attackDistance;
        _animator = animator;
        _attackRate = attackRate;
        _damage = damage;
    }
    public void CheckEnemy(Vector3 centerOffset, LayerMask enemyMask)
    {
        RaycastHit2D hit = Physics2D.Raycast(_player.position + centerOffset, _player.right, _attackDistance, enemyMask);

        if (hit.collider != null && hit.collider.TryGetComponent(out Enemy enemy))
        {
            if (enemy.IsAlive == true)
            {
                Attack(enemy);
            }
        }
    }

    private void Attack(Enemy enemy)
    {
        if (enemy.IsAlive == false)
        {
            return;
        }

        _lastAttackTime += Time.deltaTime;

        if (_lastAttackTime < _attackRate)
        {
            return;
        }

        _animator.SetTrigger(_attackTriggerName);
        enemy.TakeDamage(_damage);
        _lastAttackTime = 0f;
    }
}
