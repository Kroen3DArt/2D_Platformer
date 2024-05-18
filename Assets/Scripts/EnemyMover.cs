using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float _speed = 3;
    [SerializeField] Transform[] _wayPoints;

    private int _currentTargetIndex = 0;
    private bool _isMoving = true;

    private void Update()
    {
        if (_wayPoints.Length == 0)
        {
            Debug.LogError("Ќе установлены точки дл€ патрулировани€");
            return;
        }

        Patrol();
    }

    private void Patrol()
    {
        if (_isMoving)
        {
            Vector3 targetPosition = _wayPoints[_currentTargetIndex].position;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
            {
                _currentTargetIndex = (_currentTargetIndex + 1) % _wayPoints.Length;
                FlipSprite();
            }
            else
            {
                _isMoving = true;
            }
        }
    }

    private void FlipSprite()
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }
}
