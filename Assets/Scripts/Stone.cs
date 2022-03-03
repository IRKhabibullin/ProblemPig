using UnityEngine;

public class Stone : MonoBehaviour, IPlaceable
{
    public Vector2Int PositionOnGrid { get; set; }

    public GameObject Object { get; private set; }
    public bool IsPassable { get; set; } = false;

    private void Start()
    {
        Object = gameObject;
    }
}
