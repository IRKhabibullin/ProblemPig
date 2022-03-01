using System;
using UnityEngine;

[Serializable]
public class FieldGrid
{
    public readonly int width;
    public readonly int height;
    public readonly Vector2 gridStartPoint;
    private float cellWidth = 1.1f;
    private float cellHeight = 1.01f;
    private float rowShift = 0.12f;  // cell angle 12/100

    public FieldGrid(int width, int height, Vector2 gridStartPoint)
    {
        this.width = width;
        this.height = height;
        this.gridStartPoint = gridStartPoint;
    }

    public Vector2 Coords2WorldPosition(Vector2Int coords)
    {
        if (!IsValid(coords))
            return Vector2.zero;
        return new Vector2(gridStartPoint.x + coords.x * cellWidth + rowShift * coords.y, gridStartPoint.y + coords.y * cellHeight);
    }

    public bool IsValid(Vector2Int coords)
    {
        if (coords.x < 0 || coords.x >= width) return false;
        if (coords.y < 0 || coords.y >= height) return false;
        return true;
    }
}
