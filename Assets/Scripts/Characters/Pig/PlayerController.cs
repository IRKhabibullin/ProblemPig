using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IBombInteractable
{
    private IMoveable moveable;
    private Vector2Int lastDirection;
    private bool isPlanting = false;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private float plantingTime;
    [SerializeField] private float plantingCooldown;
    [SerializeField] private BombPlantButtonController bombPlantButton;

    void Start()
    {
        moveable = GetComponent<IMoveable>();
        if (moveable == null)
        {
            Debug.LogError("Can't find required component of type <IMoveable>");
            Destroy(gameObject);
        }

        ResetCharacter();
    }

    public void ResetCharacter()
    {
        isPlanting = false;
        lastDirection = Vector2Int.left;
        ((MovementController)moveable).ResetPosition();
    }

    void Update()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        if (isPlanting) return;

        lastDirection = CheckMovementAlternative();
        if (lastDirection != Vector2Int.zero)
        {
            moveable.Move(lastDirection);
        }
    }

    private Vector2Int CheckMovementAlternative()
    {
        var direction = Vector2Int.zero;

        var gamepad = Gamepad.current;
        if (gamepad == null) return direction;

        Vector2 leftStickValue = gamepad.leftStick.ReadValue();

        if (leftStickValue.magnitude >= 0.1f)
        {
            if (Mathf.Abs(leftStickValue.x) > Mathf.Abs(leftStickValue.y))
            {
                direction = leftStickValue.x > 0 ? Vector2Int.right : Vector2Int.left;
            }
            else
            {
                direction = leftStickValue.y > 0 ? Vector2Int.up : Vector2Int.down;
            }
        }

        return direction;
    }

    public void PlantBombPressed()
    {
        StartCoroutine(PlantBomb());
    }

    private IEnumerator PlantBomb()
    {
        isPlanting = true;
        yield return StartCoroutine(bombPlantButton.ActivateCoroutine(plantingTime));
        isPlanting = false;
        var bomb = Instantiate(bombPrefab, GridSystem.Instance.Coords2WorldPosition(moveable.PositionOnGrid), Quaternion.identity);
        yield return StartCoroutine(bombPlantButton.StartCooldownCoroutine(plantingCooldown));
    }

    public void TriggerBomb()
    {
        StartCoroutine(Haste());
    }

    private IEnumerator Haste()
    {
        ((MovementController)moveable).SpeedUp(1.5f);
        yield return new WaitForSeconds(5);
        ((MovementController)moveable).SpeedUp(1);
    }
}
