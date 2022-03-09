using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Jarmallnick.Miscellaneous.DataStructures
{
    public class Grid<T>
    {
        public readonly int width;
        public readonly int height;
        private T[,] _objects;

        public Grid(int width, int height)
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
            if (!IsValid(coords)) return;

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

        /// <summary>
        /// Note! Returns true if coordinates are out of grid bounds
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        public virtual bool IsOccupied(Vector2Int coords)
        {
            return !IsValid(coords) || _objects[coords.x, coords.y] != null;
        }

        public virtual List<(Vector2Int coords, float weight)> Neigbours(Vector2Int currentCell, Func<Vector2Int, bool> filter)
        {
            Vector2Int[] straightDirections = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
            List<(Vector2Int, float)> neighbours = new List<(Vector2Int, float)>();
            foreach (var direction in straightDirections)
            {
                var neighbour = currentCell + direction;
                if (filter(neighbour))
                    neighbours.Add((neighbour, 1f));
            }

            return neighbours;
        }

        /// <summary>
        /// A* pathfinding
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public virtual List<Vector2Int> FindPath(Vector2Int source, Vector2Int destination)
        {

            List<Vector2Int> path = new List<Vector2Int>();
            PriorityQueue<Vector2Int> queue = new PriorityQueue<Vector2Int>();
            Dictionary<Vector2Int, Vector2Int> came_from = new Dictionary<Vector2Int, Vector2Int>();
            Dictionary<Vector2Int, float> costs = new Dictionary<Vector2Int, float>();
            Func<Vector2Int, bool> filter = (Vector2Int item) => !IsOccupied(item) || item == destination;

            queue.Enqueue(source, 0);
            costs[source] = 0;

            while (queue.Count != 0)
            {
                var current = queue.Dequeue();
                if (current == destination)
                    break;
                foreach (var neighbour in Neigbours(current, filter))
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

        public virtual float Distance(Vector2Int x, Vector2Int y)
        {
            return Vector2.Distance(x, y);
        }
    }
}
