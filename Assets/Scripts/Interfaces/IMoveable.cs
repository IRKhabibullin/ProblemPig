using UnityEngine;

public interface IMoveable: IPlaceable
{
    public bool IsMoving { get; set; }
    public void Move(Vector2Int direction);
}
