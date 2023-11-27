using UnityEngine;

public class EnemyMover
{
    private Transform _enemy;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    private Transform[] _points;
    private Transform _targetPoint;

    private float _stopDistance;
    private float _speed;
    private float _agroDistance;
    private LayerMask _playerMask;

    private float _defaultRotationY = 0f;
    private float _flipAngle = 180f;
    private int _currentPointIndex = 0;
    private float _minSpeed = 0f;
    private Player _player = null;

    private string _speedFloatName = "Speed";

    public EnemyMover(Transform enemy, Rigidbody2D rigidbody2D, Animator animator, Transform[] points, float speed, float stopDistance, float agroDistance, LayerMask playerMask)
    {
        _enemy = enemy;
        _rigidbody2D = rigidbody2D;
        _animator = animator;
        _points = points;
        _speed = speed;
        _stopDistance = stopDistance;
        _defaultRotationY = 0;
        _flipAngle = 180f;
        _agroDistance = agroDistance;
        _playerMask = playerMask;

        _targetPoint = _points[_currentPointIndex];
    }

    public void Move()
    {
        if (_player == null)
        {
            GetPlayer();
            MoveAuto();
        }
        else
        {
            MoveToPlayer();
        }
    }

    private void GetPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(_enemy.position, _enemy.right, _agroDistance, _playerMask);

        if (hit == true && hit.collider.TryGetComponent(out Player player))
        {
            if (player.IsAlive == true)
            {
                _player = player;
            }
        }
    }

    private void MoveAuto()
    {
        if (Vector2.Distance(_enemy.position, _targetPoint.position) < _stopDistance)
        {
            _currentPointIndex++;

            if (_currentPointIndex >= _points.Length)
            {
                _currentPointIndex = 0;
            }

            _targetPoint = _points[_currentPointIndex];
        }

        MoveToPosition(_targetPoint.position);
    }

    private void MoveToPlayer()
    {
        if (Vector3.Distance(_enemy.position, _player.transform.position) < _stopDistance)
        {
            _animator.SetFloat(_speedFloatName, _minSpeed);

            if (_player.IsAlive == false)
            {
                _player = null;
            }

            return;
        }

        MoveToPosition(_player.transform.position);
    }

    private void MoveToPosition(Vector3 target)
    {
        Vector2 direction = (target - _enemy.position).normalized;
        float horizontal = direction.x * _speed;

        if (horizontal < _defaultRotationY)
        {
            _enemy.rotation = Quaternion.Euler(_enemy.rotation.x, _flipAngle, _enemy.rotation.z);
        }
        else if (horizontal > _defaultRotationY)
        {
            _enemy.rotation = Quaternion.Euler(_enemy.rotation.x, _defaultRotationY, _enemy.rotation.z);
        }

        float horizontalAbs = Mathf.Abs(horizontal);
        _animator.SetFloat(_speedFloatName, horizontalAbs);
        _rigidbody2D.velocity = new Vector2(horizontal, 0f);
    }
}
