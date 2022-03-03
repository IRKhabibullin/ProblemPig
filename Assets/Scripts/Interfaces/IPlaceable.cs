using UnityEngine;

public interface IPlaceable
{
    public GameObject Object { get; }
    public Vector2Int PositionOnGrid { get; set; }
}
