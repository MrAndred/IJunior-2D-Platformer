using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _jumpForce = 0.2f;
    [SerializeField] private float _groundCheckDistance = 0.1f;

    private bool _isGrounded;
    private bool _isJumping;
    private float _minSpeed;
    private float _flipAngle;
    private float _defaultRotationY;

    private string _horizontalAxisName = "Horizontal";
    private string _jumpAxisName = "Jump";
    private string _jumpTriggerName = "Jump";
    private string _speedFloatName = "Speed";

    private void Start()
    {
        _minSpeed = 0f;
        _defaultRotationY = transform.rotation.y;
        _flipAngle = 180f;
    }

    private void Update()
    {
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
            Debug.Log(coin.Value);
            coin.gameObject.SetActive(false);
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
        float jump = Input.GetAxis(_jumpAxisName) * _jumpForce * Time.deltaTime;
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
}
