using System.Collections;
using UnityEngine;

public class MovementController : MonoBehaviour, IMoveable
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator animator;
    [SerializeField] private Vector2Int startCoords;
    [SerializeField] private float maxSpeed;
    [SerializeField] private int depth;

    public GameObject Object { get; private set; }
    public Vector2Int PositionOnGrid { get; set; }

    public bool IsMoving { get; set; } = false;
    public bool IsPassable { get; set; } = true;

    private IEnumerator movementCoroutine;

    void Start()
    {
        Object = gameObject;
        GridSystem.Instance.PlaceObject(gameObject, startCoords);
        transform.position = GridSystem.Instance.Coords2WorldPosition(startCoords, depth);
    }

    public void Move(Vector2Int direction)
    {
        if (IsMoving) return;

        var hasMoved = GridSystem.Instance.MoveOnGrid(this, direction);
        if (hasMoved)
        {
            UpdateAnimator(direction);
            var newPosition = GridSystem.Instance.Coords2WorldPosition(PositionOnGrid, depth);
            movementCoroutine = MoveCoroutine(newPosition);
            StartCoroutine(movementCoroutine);
        }
    }

    private IEnumerator MoveCoroutine(Vector3 destination)
    {
        IsMoving = true;
        while (_rb.position != (Vector2)destination)
        {
            var newPosition = Vector3.MoveTowards(_rb.position, destination, maxSpeed * Time.fixedDeltaTime);
            _rb.MovePosition(newPosition);
            yield return new WaitForFixedUpdate();
        }
        IsMoving = false;
        UpdateAnimator(Vector2.zero);
    }

    private void UpdateAnimator(Vector2 direction)
    {
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
    }
}
