using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;
    private float _raycastDistance = 15f;
    private EnemyMover _enemyMover;

    public int Damage { get; private set; }

    private void Start()
    {
        _enemyMover = GetComponent<EnemyMover>();
        Damage = 10;
    }

    private void Update()
    {
        Vector2 movementDirection = transform.right * Mathf.Sign(transform.localScale.x);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, movementDirection, _raycastDistance, _playerLayer);

        if (raycastHit2D.collider != null && raycastHit2D.collider.TryGetComponent(out Player _player))
        {
            _enemyMover.Pursuit(_player.transform);
        }
        else
        {
            _enemyMover.ReturnToPatrol();
        }
    }
}