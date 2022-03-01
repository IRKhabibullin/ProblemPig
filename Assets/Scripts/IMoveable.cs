using UnityEngine;

public interface IMoveable: IPlaceable
{
    public void Move(Vector2Int direction);
}
