using System;
using Unity.Collections;

public struct PriorityQueue<T,TK> where TK : unmanaged, IComparable<TK> where T : unmanaged
{
    private NativeList<PriorityQueueNode> _nodes;

    public PriorityQueue(Allocator allocator)
    {
        _nodes = new NativeList<PriorityQueueNode>(allocator);
    }

    public int Count => _nodes.Length;
    
    public void Enqueue(T obj, TK key)
    {
        var node = new PriorityQueueNode(obj, key);
        _nodes.Add(node);
        var currentIndex = _nodes.Length - 1;

        while (currentIndex > 0)
        {
            var parentIndex = (currentIndex - 1) / 2;
            if (_nodes[parentIndex].Key.CompareTo(_nodes[currentIndex].Key) <= 0)
                break;

            Swap(parentIndex, currentIndex);
            currentIndex = parentIndex;
        }
    }

    public T Dequeue()
    {
        if (_nodes.Length == 0) return default;

        T node = _nodes[0].Obj;
        _nodes[0] = _nodes[^1];
        _nodes.RemoveAt(_nodes.Length - 1);

        var currentIndex = 0;

        while (true)
        {
            var leftChildIndex = 2 * currentIndex + 1;
            var rightChildIndex = 2 * currentIndex + 2;
            var minIndex = currentIndex;

            if (leftChildIndex < _nodes.Length && _nodes[leftChildIndex].Key.CompareTo(_nodes[minIndex].Key) == -1)
            {
                minIndex = leftChildIndex;
            }

            if (rightChildIndex < _nodes.Length && _nodes[rightChildIndex].Key.CompareTo(_nodes[minIndex].Key) == -1)
            {
                minIndex = rightChildIndex;
            }

            if (minIndex == currentIndex) break;

            Swap(minIndex, currentIndex);
            currentIndex = minIndex;
        }

        return node;
    }

    private void Swap(int indexA, int indexB)
    {
        (_nodes[indexA], _nodes[indexB]) = (_nodes[indexB], _nodes[indexA]);
    }

    private struct PriorityQueueNode
    {
        public readonly T Obj;
        public readonly TK Key;

        public PriorityQueueNode(T obj, TK key)
        {
            Obj = obj;
            Key = key;
        }
    }
}