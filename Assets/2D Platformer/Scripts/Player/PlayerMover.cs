using UnityEngine;

public class PlayerMover
{
    private Transform _player;
    private Rigidbody2D _rigidbody2D;

    private Animator _animator;
    private string _horizontalAxisName = "Horizontal";
    private string _jumpTriggerName = "Jump";
    private string _speedFloatName = "Speed";

    private float _speed;
    private float _jumpForce;
    private float _groundCheckDistance;

    private float _defaultRotationY;
    private bool _isGrounded;
    private bool _isJumping;

    private float _minSpeed = 0f;
    private float _flipAngle = 180f;

    public PlayerMover(Transform player, Rigidbody2D rigidbody2D, Animator animator, float speed, float jumpForce, float checkGroundDistance)
    {
        _player = player;
        _rigidbody2D = rigidbody2D;
        _animator = animator;
        _speed = speed;
        _jumpForce = jumpForce;
        _groundCheckDistance = checkGroundDistance;
        _defaultRotationY = _player.rotation.y;
    }

    public void Move()
    {
        HandleInput();
        HandleJump();
    }

    private void HandleInput()
    {
        if (_isGrounded == false)
        {
            _animator.SetFloat(_speedFloatName, _minSpeed);
            return;
        }

        float horizontal = Input.GetAxis(_horizontalAxisName) * _speed;

        if (horizontal < _defaultRotationY)
        {
            _player.rotation = Quaternion.Euler(_player.rotation.x, _flipAngle, _player.rotation.z);
        }
        else if (horizontal > _defaultRotationY)
        {
            _player.rotation = Quaternion.Euler(_player.rotation.x, _defaultRotationY, _player.rotation.z);
        }

        _animator.SetFloat(_speedFloatName, Mathf.Abs(horizontal));
        _rigidbody2D.velocity = new Vector2(horizontal, _rigidbody2D.velocity.y);
    }

    private void HandleJump()
    {
        float jump = Input.GetAxis(_jumpTriggerName) * _jumpForce;
        _isGrounded = Physics2D.Raycast(_player.position, Vector2.down, _groundCheckDistance);

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
