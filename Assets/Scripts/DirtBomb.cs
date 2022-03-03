using UnityEngine;

public class DirtBomb : MonoBehaviour, IPlaceable
{
    public GameObject Object { get; set; }

    public Vector2Int PositionOnGrid { get; set; }

    public bool IsPassable { get; set; } = true;

    private void Start()
    {
        Object = gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var interactable = collision.GetComponent<IBombInteractable>();
        if (interactable != null)
        {
            interactable.TriggerBomb();
            Destroy(gameObject);
        }
    }
}
