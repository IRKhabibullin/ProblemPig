using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IMoveable moveable;

    void Start()
    {
        moveable = GetComponent<IMoveable>();
        if (moveable == null)
        {
            Debug.LogError("Can't find required component of type <IMoveable>");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        var direction = CheckMovement();
        if (direction != Vector2Int.zero)
        {
            moveable.Move(direction);
        }
    }

    private Vector2Int CheckMovement()
    {
        var horizontal = (int)Input.GetAxisRaw("Horizontal");
        return new Vector2Int(horizontal, horizontal != 0 ? 0 : (int)Input.GetAxisRaw("Vertical"));
    }
}
