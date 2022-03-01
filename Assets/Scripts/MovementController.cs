using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator animator;
    [SerializeField] private IMoveable moveable;
    private Vector2Int currentMovement = Vector2Int.zero;

    private bool isMoving = false;
    private IEnumerator movementCoroutine;

    private void Start()
    {
        moveable = GetComponent<IMoveable>();
    }

    void Update()
    {
        currentMovement.x = (int)Input.GetAxisRaw("Horizontal");
        currentMovement.y = (int)Input.GetAxisRaw("Vertical");
        if (currentMovement != Vector2Int.zero && !isMoving)
        {
            Move(currentMovement);
        }
    }

    private void Move(Vector2Int direction)
    {
        /*var newPosition = moveable.Move(direction);
        if (newPosition != null)
        {
            animator.SetFloat("Horizontal", direction.x);
            animator.SetFloat("Vertical", direction.y);
            movementCoroutine = MoveCoroutine((Vector3)newPosition);
            StartCoroutine(movementCoroutine);
        }*/
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
        animator.SetFloat("Horizontal", 0f);
        animator.SetFloat("Vertical", 0f);
    }
}
