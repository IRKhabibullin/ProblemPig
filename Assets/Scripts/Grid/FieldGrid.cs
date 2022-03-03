using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class FieldGrid<T>
{
    public readonly int width;
    public readonly int height;
    private T[,] _objects;

    public FieldGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
        _objects = new T[width, height];
    }

    public T Get(Vector2Int coords)
    {
        if (!IsValid(coords)) return default;

        return _objects[coords.x, coords.y];
    }

    public void Set(T obj, Vector2Int coords)
    {
        _objects[coords.x, coords.y] = obj;
    }

    public void Remove(Vector2Int coords)
    {
        if (!IsValid(coords)) return;

        _objects[coords.x, coords.y] = default;
    }

    public bool IsValid(Vector2Int coords)
    {
        if (coords.x < 0 || coords.x >= width) return false;
        if (coords.y < 0 || coords.y >= height) return false;
        return true;
    }

    public bool IsOccupied(Vector2Int coords)
    {
        return !IsValid(coords) || _objects[coords.x, coords.y] != null;
    }

    private List<(Vector2Int coords, float weight)> WeightedNeigbours(Vector2Int currentCell, Vector2Int destination)
    {
        Vector2Int[] straightDirections = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        List<(Vector2Int, float)> neighbours = new List<(Vector2Int, float)>();
        foreach (var direction in straightDirections)
        {
            var neighbour = currentCell + direction;
            if (!IsOccupied(neighbour) || neighbour == destination)
                neighbours.Add((neighbour, 1f));
        }

        return neighbours;
    }

    public List<Vector2Int> FindPath(Vector2Int source, Vector2Int destination)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        PriorityQueue<Vector2Int> queue = new PriorityQueue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> came_from = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, float> costs = new Dictionary<Vector2Int, float>();

        queue.Enqueue(source, 0);
        costs[source] = 0;

        while (queue.Count != 0)
        {
            var current = queue.Dequeue();
            if (current == destination)
                break;
            foreach (var neighbour in WeightedNeigbours(current, destination))
            {
                float point_cost = costs[current] + neighbour.weight;
                if (!costs.ContainsKey(neighbour.coords) || point_cost < costs[neighbour.coords])
                {
                    costs[neighbour.coords] = point_cost;
                    queue.Enqueue(neighbour.coords, point_cost + Distance(destination, neighbour.coords));
                    came_from[neighbour.coords] = current;
                }
            }
        }

        if (came_from.Count() == 0)
            return path;
        var x = destination;
        while (x != source)
        {
            path.Add(x);
            x = came_from[x];
        }

        path.Reverse();
        return path;
    }

    public float Distance(Vector2Int x, Vector2Int y)
    {
        // TODO change distance measurement. Absolute distance is wrong, need somehow measure distance by way.
        return Vector2.Distance(x, y);
    }
}

public sealed class PriorityQueue<T>
{
    public LinkedList<(T elem, float priority)> wayPoints { get; } = new LinkedList<(T elem, float priority)>();

    public int Count
    {
        get { return wayPoints.Count; }
    }

    public T Dequeue()
    {
        if (wayPoints.Any())
        {
            var itemTobeRemoved = wayPoints.First.Value.elem;
            wayPoints.RemoveFirst();
            return itemTobeRemoved;
        }

        return default;
    }

    public void Enqueue(T elem, float priority)
    {
        if (wayPoints.First == null)
        {
            wayPoints.AddFirst((elem, priority));
        }
        else
        {
            var ptr = wayPoints.First;
            while (ptr.Next != null && ptr.Value.priority < priority)
            {
                ptr = ptr.Next;
            }

            var value = new LinkedListNode<(T, float)>((elem, priority));
            if (ptr.Value.priority <= priority)
            {
                wayPoints.AddAfter(ptr, value);
            }
            else
            {
                wayPoints.AddBefore(ptr, value);
            }
        }
    }
}
