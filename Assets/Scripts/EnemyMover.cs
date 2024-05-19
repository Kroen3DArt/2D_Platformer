using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float _speed = 3;
    [SerializeField] private Transform[] _wayPoints;

    private int _currentTargetIndex = 0;
    private bool _isMoving = true;
    private bool _isPursing = false;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Pursuit(Transform targetPosition)
    {
        float speedMultiply = 2.5f;
        float pursingSpeed = _speed * speedMultiply;

        _isMoving = false;
        _animator.SetBool("isPursing", !_isPursing);
        Move(targetPosition, pursingSpeed);
    }

    public void ReturnToPatrol()
    {
        _animator.SetBool("isPursing", _isPursing);
        _isMoving = true;
    }

    private void Update()
    {
        if (_wayPoints.Length == 0)
        {
            Debug.LogError("Ќе установлены точки дл€ патрулировани€");
            return;
        }

        if (!_isPursing)
            Patrol();
    }

    private void Patrol()
    {
        if (_isMoving)
        {
            Transform targetPosition = _wayPoints[_currentTargetIndex];
            Move(targetPosition, _speed);

            if (Vector2.Distance(transform.position, targetPosition.position) < 0.1f)
                _currentTargetIndex = (_currentTargetIndex + 1) % _wayPoints.Length;
            else
                _isMoving = true;
        }
    }

    private void Move(Transform target, float speed)
    {
        Vector2 targetPosition = new(target.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        SetSpriteDirection(targetPosition);
    }

    private void SetSpriteDirection(Vector2 target)
    {
        Vector2 movementDirection = (target - (Vector2)transform.position).normalized;

        if (movementDirection.x > 0)
            transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
        else if (movementDirection.x < 0)
            transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
    }
}
