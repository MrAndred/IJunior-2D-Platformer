using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private LayerMask _enemyMask;
    [SerializeField] private Vector3 _centerOffset;

    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _jumpForce = 0.2f;
    [SerializeField] private float _groundCheckDistance = 0.1f;

    [SerializeField] private float _health = 100f;

    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _attackDistance = 0.5f;
    [SerializeField] private float _attackRate = 1f;

    private PlayerHealth _playerHealth;
    private PlayerMover _playerMover;
    private PlayerAttacker _playerAttacker;

    public float Health => _playerHealth.Health;

    public bool IsAlive => _playerHealth.IsAlive;

    private void Start()
    {
        _playerHealth = new PlayerHealth(_animator, _health);
        _playerMover = new PlayerMover(transform, _rigidbody2D, _animator, _speed, _jumpForce, _groundCheckDistance);
        _playerAttacker = new PlayerAttacker(transform, _animator, _attackRate, _damage, _attackDistance);
    }

    public void TakeDamage(float damage)
    {
        _playerHealth.TakeDamage(damage);
    }

    private void Update()
    {
        if (_playerHealth.IsAlive == false)
        {
            return;
        }

        _playerAttacker.CheckEnemy(_centerOffset, _enemyMask);
        _playerMover.Move();
    }

    private void OnValidate()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Coin coin))
        {
            coin.gameObject.SetActive(false);
        }
        else if (collision.TryGetComponent(out AidKit aidKit))
        {
            _playerHealth.Heal(aidKit.Health);
            aidKit.gameObject.SetActive(false);
        }
    }
}
