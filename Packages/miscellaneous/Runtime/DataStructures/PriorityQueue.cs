using System.Collections.Generic;
using System.Linq;

namespace Jarmallnick.Miscellaneous.DataStructures
{
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
}
