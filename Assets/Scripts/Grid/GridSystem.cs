using System;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public static GridSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GridSystem>();
                if (_instance == null)
                {
                    _instance = new GameObject().AddComponent<GridSystem>();
                }
            }
            return _instance;
        }
    }
    private static GridSystem _instance;

    [SerializeField] private int gridWidth = 17;
    [SerializeField] private int gridHeight = 9;

    [SerializeField] private float cellWidth = 1.1f;
    [SerializeField] private float cellHeight = 1.01f;
    [SerializeField] private float rowShift = 0.12f;  // cell angle 12/100
    [SerializeField] private Transform gridStartPoint;
    [SerializeField] private Transform gridObjectsStorage;

    [SerializeField] private GridObjects gridObjects;
    [SerializeField] private int defaultDepth = 1; // greater the depth, closer the object to the screen

    private static FieldGrid<IPlaceable> grid;

    void Awake()
    {
        if (_instance != null)
            Destroy(this);
        DontDestroyOnLoad(this);

        grid = new FieldGrid<IPlaceable>(gridWidth, gridHeight);
    }

    void Start()
    {
        Instance.PlaceStones();
    }

    public (int width, int height) GridSize
    {
        get { return (gridWidth, gridHeight); }
    }

    private void PlaceStones()
    {
        for (int i = 1; i < grid.width; i += 2)
        {
            for (int j = 1; j < grid.height; j += 2)
            {
                AddObject(gridObjects.stonePrefab, new Vector2Int(i, j));
            }
        }
    }

    public void AddObject(GameObject objectPrefab, Vector2Int coords)
    {
        if (grid.IsOccupied(coords)) return;

        var obj = Instantiate(objectPrefab, Coords2WorldPosition(coords, defaultDepth), objectPrefab.transform.rotation);
        var placeable = obj.GetComponent<IPlaceable>();
        grid.Set(placeable, coords);
        placeable.PositionOnGrid = coords;
        obj.transform.parent = gridObjectsStorage;
    }

    public bool PlaceObject(GameObject obj, Vector2Int coords)
    {
        var placeable = obj.GetComponent<IPlaceable>();
        if (placeable == null) return false;

        if (grid.IsOccupied(coords)) return false;

        grid.Set(placeable, coords);
        placeable.PositionOnGrid = coords;
        return true;
    }

    public bool MoveOnGrid(IMoveable moveable, Vector2Int direction)
    {
        var newCoords = moveable.PositionOnGrid + direction;
        if (!grid.IsValid(newCoords) || grid.IsOccupied(newCoords)) return false;

        grid.Remove(moveable.PositionOnGrid);
        grid.Set(moveable, newCoords);
        moveable.PositionOnGrid = newCoords;
        return true;
    }

    public Vector3 Coords2WorldPosition(Vector2Int coords)
    {
        return Coords2WorldPosition(coords, defaultDepth);
    }

    public Vector3 Coords2WorldPosition(Vector2Int coords, int depth)
    {
        if (!grid.IsValid(coords)) return Vector3.zero;

        return new Vector3(
            gridStartPoint.position.x + coords.x * cellWidth + rowShift * coords.y,
            gridStartPoint.position.y + coords.y * cellHeight,
            -depth / 100f);
    }

    public List<Vector2Int> FindPath(IMoveable moveable, Vector2Int destination, bool checkDestinationUnoccupied = true)
    {
        if (checkDestinationUnoccupied && grid.IsOccupied(destination)) return new List<Vector2Int>();

        return grid.FindPath(moveable.PositionOnGrid, destination);
    }

    public IPlaceable FindByTag(Vector2Int searchPosition, int searchRange, string tag)
    {
        for (var i = 0; i < grid.width; i++)
        {
            for (var j = 0; j < grid.height; j++)
            {
                var cellCoords = new Vector2Int(i, j);
                if (grid.Distance(searchPosition, cellCoords) > searchRange)
                    continue;

                var cell = grid.Get(cellCoords);
                if (cell != null && cell.Object.CompareTag(tag))
                    return cell;
            }
        }
        return null;
    }
}
