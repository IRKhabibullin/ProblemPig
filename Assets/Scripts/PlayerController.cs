using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour, IBombInteractable
{
    private IMoveable moveable;
    private Vector2Int lastDirection;
    private bool isPlanting = false;
    private bool isPlantingOnCooldown = false;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private float plantingTime;
    [SerializeField] private float plantingCooldown;

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
        if (isPlanting) return;

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
        if (isPlantingOnCooldown) return;

        if (Input.GetKeyUp(KeyCode.Space))
        {
            StartCoroutine(PlantBomb());
        }
    }

    private IEnumerator PlantBomb()
    {
        isPlanting = true;
        isPlantingOnCooldown = true;
        yield return new WaitForSeconds(plantingTime);
        var bomb = Instantiate(bombPrefab, GridSystem.Instance.Coords2WorldPosition(moveable.PositionOnGrid), Quaternion.identity);
        isPlanting = false;
        yield return new WaitForSeconds(plantingCooldown);
        isPlantingOnCooldown = false;
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
