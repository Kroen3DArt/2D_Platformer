using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Renderer), typeof(Rigidbody2D), typeof(Animator))]
public class Player : MonoBehaviour
{
    public int _health;
    private int _maxHealth = 100;
    private int _minHealth = 0;
    private float _blinkDuration = 0.3f;
    private int _blinkCount = 5;
    private bool _isDamaged = false;
    private float _kickBackForce = 5f;
    private bool _isInvulnerable = false;
    private Animator _animator;
    private Coroutine _blinkCoroutine;
    private Rigidbody2D _rigidbody;
    private Renderer _renderer;
    private Enemy _enemy;
    private Traps _traps;
    private AidHeart _aidHeart;

    public event Action ResourcePickUp;

    public int Colectable { get; private set; }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
        _traps = GetComponent<Traps>();
        _aidHeart = GetComponent<AidHeart>();
    }

    private void Start()
    {
        Colectable = 0;
        _health = _maxHealth;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Cherry cherry))
        {
            Colectable++;
            ResourcePickUp?.Invoke();
        }
        else if (collision.TryGetComponent(out _traps))
        {
            TakeDamage(_traps.Damage);
        }
        else if (collision.TryGetComponent(out _enemy))
        {
            TakeDamage(_enemy.Damage);
        }
        else if (collision.TryGetComponent(out _aidHeart))
        {
            if (_health < _maxHealth)
            {
                AddHealth(_aidHeart.Aid);
                _aidHeart.Destroy();
            }
        }
    }

    private void TakeDamage(int damage)
    {
        if (_blinkCoroutine != null)
            StopCoroutine(_blinkCoroutine);

        if (!_isInvulnerable)
        {
            _animator.SetBool("isDamaged", !_isDamaged);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _kickBackForce);

            if (damage > 0)
                _health -= damage;

            if (_health <= _minHealth)
                Die();
        }

        _blinkCoroutine = StartCoroutine(BlinkCorutine());
    }

    private IEnumerator BlinkCorutine()
    {
        WaitForSeconds wait = new(_blinkDuration);
        _isInvulnerable = true;

        for (int i = 0; i < _blinkCount; i++)
        {
            _renderer.enabled = !_renderer.enabled;
            yield return wait;
        }

        _animator.SetBool("isDamaged", _isDamaged);
        _renderer.enabled = true;
        _isInvulnerable = false;
    }

    private void Die()
    {
        Debug.Log("Ћ€гух погиб");
    }

    private void AddHealth(int aid)
    {
        if (aid > 0)
        {
            _health += aid;

            if (_health > _maxHealth)
                _health = _maxHealth;
        }
    }
}
