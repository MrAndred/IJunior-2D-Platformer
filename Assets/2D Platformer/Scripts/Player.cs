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

    private bool _isGrounded;
    private bool _isJumping;
    private bool _isAlive = true;

    private float _minSpeed;
    private float _flipAngle;
    private float _defaultRotationY;

    private string _horizontalAxisName = "Horizontal";
    private string _jumpTriggerName = "Jump";
    private string _speedFloatName = "Speed";
    private string _attackTriggerName = "Attack";
    private string _takeDamageTriggerName = "TakeDamage";
    private string _dieTriggerName = "Die";

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

    private void Start()
    {
        _minSpeed = 0f;
        _defaultRotationY = transform.rotation.y;
        _flipAngle = 180f;
    }

    private void Update()
    {
        if (_isAlive == false)
        {
            return;
        }

        CheckEnemy();
        HandleInput();
        HandleJump();
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
            _health += aidKit.Health;
            aidKit.gameObject.SetActive(false);
        }
    }

    private void CheckEnemy()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + _centerOffset, transform.right, _attackDistance, _enemyMask);

        if (hit.collider != null && hit.collider.TryGetComponent(out Enemy enemy))
        {
            if (enemy.IsAlive == true)
            {
                Attack(enemy);
            }
        }
    }

    private void HandleInput()
    {
        if (_isGrounded == false)
        {
            _animator.SetFloat(_speedFloatName, _minSpeed);
            return;
        }

        float horizontal = Input.GetAxis(_horizontalAxisName) * _speed * Time.deltaTime;

        if (horizontal < _defaultRotationY)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, _flipAngle, transform.rotation.z);
        }
        else if (horizontal > _defaultRotationY)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, _defaultRotationY, transform.rotation.z);
        }

        _animator.SetFloat(_speedFloatName, Mathf.Abs(horizontal));
        _rigidbody2D.velocity = new Vector2(horizontal, _rigidbody2D.velocity.y);
    }

    private void HandleJump()
    {
        float jump = Input.GetAxis(_jumpTriggerName) * _jumpForce * Time.deltaTime;
        _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, _groundCheckDistance);

        if (jump > 0 && _isGrounded == true)
        {
            _rigidbody2D.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
            _isJumping = true;
        }

        if (_isJumping == true)
        {
            _animator.SetTrigger(_jumpTriggerName);
            _isJumping = false;
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
