using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _horizontalInput;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpPower = 9;

    private bool _isGrounded = true;
    private bool _isLeft = false;
    private float _defaultGravityScale;
    private float _gravityBigScale = 3f;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _defaultGravityScale = _rigidbody2D.gravityScale;
    }

    private void FixedUpdate()
    {
        Jump();
        Move();
        FlipSprite();
    }

    private void FlipSprite()
    {
        if (_isLeft && _horizontalInput > 0f || !_isLeft && _horizontalInput < 0f)
        {
            _isLeft = !_isLeft;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void Move()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _rigidbody2D.velocity = new Vector2(_horizontalInput * _speed, _rigidbody2D.velocity.y);
        _animator.SetFloat("xVelocity", Mathf.Abs(_rigidbody2D.velocity.x));
    }

    private void Jump()
    {
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W)) && _isGrounded)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpPower);
            _isGrounded = false;
            FallAnimation();
        }

        _animator.SetFloat("yVelocity", _rigidbody2D.velocity.y);

        if (_rigidbody2D.velocity.y < 0 && !_isGrounded)
            _rigidbody2D.gravityScale = _gravityBigScale;
        else
            _rigidbody2D.gravityScale = _defaultGravityScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _isGrounded = true;
        FallAnimation();
        _rigidbody2D.gravityScale = _defaultGravityScale;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _isGrounded = false;
        FallAnimation();
    }

    private void FallAnimation() => _animator.SetBool("isJumping", !_isGrounded);
}
