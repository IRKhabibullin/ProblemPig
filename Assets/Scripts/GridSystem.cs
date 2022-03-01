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
    [SerializeField] private GridObjects gridObjects;
    [SerializeField] private Transform gridStartPoint;
    [SerializeField] private int defaultDepth; // greater the depth, closer the object to the screen

    private static FieldGrid grid;
    private static IMoveable[,] objectsOnGrid;

    void Awake()
    {
        if (_instance != null)
            Destroy(this);
        DontDestroyOnLoad(this);

        grid = new FieldGrid(gridWidth, gridHeight, gridStartPoint.position);
        objectsOnGrid = new IMoveable[gridWidth, gridHeight];

    }

    void Start()
    {
        Instance.PlaceStones();
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
        if (objectsOnGrid[coords.x, coords.y] != null) return;

        var obj = Instantiate(objectPrefab, Coords2WorldPosition(coords, defaultDepth), objectPrefab.transform.rotation);
        objectsOnGrid[coords.x, coords.y] = obj.GetComponent<IMoveable>();
    }

    public void PlaceObject(GameObject obj, Vector2Int coords, int depth)
    {
        var moveable = obj.GetComponent<IMoveable>();
        if (moveable == null) return;

        if (IsOccupied(coords)) return;

        obj.transform.position = Coords2WorldPosition(coords, depth);
        moveable.PositionOnGrid = coords;
        objectsOnGrid[coords.x, coords.y] = moveable;
    }

    public bool MoveOnGrid(IMoveable moveable, Vector2Int direction)
    {
        var newCoords = moveable.PositionOnGrid + direction;
        if (!grid.IsValid(newCoords) || IsOccupied(newCoords)) return false;

        objectsOnGrid[moveable.PositionOnGrid.x, moveable.PositionOnGrid.y] = null;
        objectsOnGrid[newCoords.x, newCoords.y] = moveable;
        moveable.PositionOnGrid = newCoords;
        return true;
    }

    public Vector3 Coords2WorldPosition(Vector2Int coords, int depth)
    {
        var position = grid.Coords2WorldPosition(coords);
        return new Vector3(position.x, position.y, -depth / 100f);
    }

    public bool IsOccupied(Vector2Int coords)
    {
        return objectsOnGrid[coords.x, coords.y] != null;
    }
}
