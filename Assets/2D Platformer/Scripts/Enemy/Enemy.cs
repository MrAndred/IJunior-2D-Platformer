using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private LayerMask _playerMask;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform[] _points;
    [SerializeField] private Rigidbody2D _rigidbody2D;

    [SerializeField] private float _agroDistance = 5f;
    [SerializeField] private float _stopDistance = 1f;
    [SerializeField] private float _attackDistance = 2f;
    [SerializeField] private float _attackRate = 1f;
    [SerializeField] private Transform _path;
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;
    [SerializeField] private float _health = 50f;

    private EnemyMover _enemyMover;
    private EnemyAttacker _enemyAttacker;
    private EnemyHealth _enemyHealth;

    public float Health => _enemyHealth.Health;
    public bool IsAlive => _enemyHealth.IsAlive;

    public void TakeDamage(float damage)
    {
        _enemyHealth.TakeDamage(damage);
    }

    private void OnValidate()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _points = new Transform[_path.childCount];

        for (int i = 0; i < _path.childCount; i++)
        {
            _points[i] = _path.GetChild(i);
        }
    }

    private void Start()
    {
        _enemyHealth = new EnemyHealth(_animator, _health);
        _enemyMover = new EnemyMover(transform, _rigidbody2D, _animator, _points, _speed, _stopDistance, _agroDistance, _playerMask);
        _enemyAttacker = new EnemyAttacker(transform, _animator, _attackRate, _damage);
    }

    private void Update()
    {
        if (_enemyHealth.IsAlive == false)
        {
            return;
        }

        _enemyMover.Move();
        _enemyAttacker.CheckPlayer(_attackDistance, _playerMask);
    }
}
