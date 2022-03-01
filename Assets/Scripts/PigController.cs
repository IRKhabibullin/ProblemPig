using System.Collections;
using UnityEngine;

public class PigController : MonoBehaviour, IMoveable
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator animator;
    [SerializeField] private Vector2Int startCoords;
    [SerializeField] private float maxSpeed;
    [SerializeField] private int depth;

    public Vector2Int PositionOnGrid { get; set; }

    private bool isMoving = false;
    private IEnumerator movementCoroutine;

    void Start()
    {
        GridSystem.Instance.PlaceObject(gameObject, startCoords, depth);
    }

    public void Move(Vector2Int direction)
    {
        if (isMoving) return;

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
        isMoving = true;
        while (_rb.position != (Vector2)destination)
        {
            var newPosition = Vector3.MoveTowards(_rb.position, destination, maxSpeed * Time.fixedDeltaTime);
            _rb.MovePosition(newPosition);
            yield return new WaitForFixedUpdate();
        }
        isMoving = false;
        UpdateAnimator(Vector2.zero);
    }

    private void UpdateAnimator(Vector2 direction)
    {
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
    }
}
