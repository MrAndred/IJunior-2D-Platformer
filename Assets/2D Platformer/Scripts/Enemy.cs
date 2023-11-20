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

    private Transform _target;
    private int _currentPointIndex;
    private float _flipAngle;
    private float _defaultRotationY;

    private bool _isAlive = true;

    private string _speedFloatName = "Speed";
    private string _attackTriggerName = "Attack";
    private string _takeDamageTriggerName = "TakeDamage";
    private string _dieTriggerName = "Die";

    private Player _player;
    private float _lastAttackTime;

    public float Health => _health;
    public bool IsAlive => _isAlive;

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
        _target = _points[0];
        _defaultRotationY = 0;
        _flipAngle = 180f;
    }

    private void Update()
    {
        if (_isAlive == false)
        {
            return;
        }

        CheckAgro();

        if (_player == null)
        {
            MoveAuto();
            return;
        }

        if (Vector2.Distance(transform.position, _player.transform.position) <= _attackDistance)
        {
            _animator.SetFloat(_speedFloatName, 0f);
            Attack();
        }
        else
        {
            MoveToPlayer();
        }
    }

    private void CheckAgro()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, _agroDistance, _playerMask.value);

        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent(out Player player))
            {
                if (player.IsAlive == true)
                {
                    _player = player;
                }
            }
        }
    }

    private void MoveToPlayer()
    {
        if (_player == null)
        {
            return;
        }

        Move(_player.transform.position);
    }

    private void MoveAuto()
    {
        if (Vector2.Distance(transform.position, _target.position) < _stopDistance)
        {
            _currentPointIndex++;

            if (_currentPointIndex >= _points.Length)
            {
                _currentPointIndex = 0;
            }

            _target = _points[_currentPointIndex];
        }

        Move(_target.position);
    }

    private void Move(Vector3 target)
    {
        Vector2 direction = (target - transform.position).normalized;
        float horizontal = direction.x * _speed * Time.deltaTime;

        if (horizontal < _defaultRotationY)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, _flipAngle, transform.rotation.z);
        }
        else if (horizontal > _defaultRotationY)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, _defaultRotationY, transform.rotation.z);
        }

        float horizontalAbs = Mathf.Abs(horizontal);
        _animator.SetFloat(_speedFloatName, horizontalAbs);
        _rigidbody2D.velocity = new Vector2(horizontal, 0f);
    }

    private void Attack()
    {
        _lastAttackTime += Time.deltaTime;

        if (_lastAttackTime < _attackRate)
        {
            return;
        }

        if (_player.Health <= 0)
        {
            _player = null;
            return;
        }

        _animator.SetTrigger(_attackTriggerName);
        _player.TakeDamage(_damage);
        _lastAttackTime = 0f;
    }
}
