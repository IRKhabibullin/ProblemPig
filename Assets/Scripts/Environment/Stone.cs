using UnityEngine;

public class Stone : MonoBehaviour, IPlaceable
{
    public Vector2Int PositionOnGrid { get; set; }

    public GameObject Object { get; private set; }

    private void Awake()
    {
        Object = gameObject;
    }
}
