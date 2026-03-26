using System.Threading;

public class LockFreeStack<T>
{
    private class Node
    {
        public T Value;
        public Node Next;
    }

    private Node _head;

    public void Push(T value)
    {
        Node newNode = new Node { Value = value };

        while (true)
        {
            Node oldHead = _head;
            newNode.Next = oldHead;

            // head 교체 시도
            if (Interlocked.CompareExchange(ref _head, newNode, oldHead) == oldHead)
                return;
        }
    }

    public bool TryPop(out T value)
    {
        while (true)
        {
            Node oldHead = _head;

            if (oldHead == null)
            {
                value = default;
                return false;
            }

            Node newHead = oldHead.Next;

            if (Interlocked.CompareExchange(ref _head, newHead, oldHead) == oldHead)
            {
                value = oldHead.Value;
                return true;
            }
        }
    }
}