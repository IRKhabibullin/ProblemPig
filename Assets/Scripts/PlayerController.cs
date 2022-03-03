using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IMoveable moveable;
    private Vector2Int lastDirection;
    [SerializeField] private GameObject bombPrefab;

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
        CheckPlantInput();
    }

    private void UpdateMovement()
    {
        lastDirection = CheckMovement();
        if (lastDirection != Vector2Int.zero)
        {
            moveable.Move(lastDirection);
        }
    }

    private Vector2Int CheckMovement()
    {
        var horizontal = (int)Input.GetAxisRaw("Horizontal");
        return new Vector2Int(horizontal, horizontal != 0 ? 0 : (int)Input.GetAxisRaw("Vertical"));
    }

    private void CheckPlantInput()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            PlantBomb();
        }

    }

    private void PlantBomb()
    {
        var bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
        var pigBackside = lastDirection == Vector2Int.zero ? Vector2Int.right : -lastDirection;
        var placed = GridSystem.Instance.PlaceObject(bomb, moveable.PositionOnGrid + pigBackside);
        if (placed)
            bomb.transform.position = GridSystem.Instance.Coords2WorldPosition(bomb.GetComponent<IPlaceable>().PositionOnGrid);
        else
            Destroy(bomb);
    }
}
