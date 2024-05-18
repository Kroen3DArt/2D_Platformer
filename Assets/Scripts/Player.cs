using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Renderer), typeof(Rigidbody2D), typeof(Animator))]
public class Player : MonoBehaviour
{
    private float _blinkDuration = 0.2f;
    private int _blinkCount = 5;
    private Animator _animator;
    private Coroutine _blinkCoroutine;
    private Rigidbody2D _rigidbody;
    private bool _isDamaged = false;
    private float _kickBackForce = 5f;
    private Renderer _renderer;

    public event Action ResourcePickUp;

    public int Colectable { get; private set; }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        Colectable = 0;
        _animator = GetComponent<Animator>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Cherry>())
        {
            Colectable++;
            ResourcePickUp?.Invoke();
        }

        if (collision.GetComponent<Traps>() || collision.GetComponent<EnemyMover>())
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        if (_blinkCoroutine != null)
            StopCoroutine(_blinkCoroutine);

        _animator.SetBool("isDamaged", !_isDamaged);
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _kickBackForce);

        _blinkCoroutine = StartCoroutine(BlinkCorutine());
    }

    private IEnumerator BlinkCorutine()
    {
        WaitForSeconds wait = new(_blinkDuration);

        for (int i = 0; i < _blinkCount; i++)
        {
            _renderer.enabled = !_renderer.enabled;
            yield return wait;
        }

        _animator.SetBool("isDamaged", _isDamaged);
        _renderer.enabled = true;
    }
}
