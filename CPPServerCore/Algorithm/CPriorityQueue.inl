#pragma once
#include "CPriorityQueue.h"

template <typename T>
void PriorityQueue<T>::HeapifyUp(int index)
{
    int now = index;
    while (now > 0)
    {
        int parent = (now - 1) / 2;
        if (!(_heap[now] > _heap[parent]))
            break;

        std::swap(_heap[now], _heap[parent]);
        now = parent;
    }
}

template <typename T>
void PriorityQueue<T>::HeapifyDown(int index)
{
    int now = index;
    int size = static_cast<int>(_heap.size());

    while (true)
    {
        int left = 2 * now + 1;
        int right = 2 * now + 2;
        int next = now;

        if (left < size && _heap[left] > _heap[next])
            next = left;
        if (right < size && _heap[right] > _heap[next])
            next = right;

        if (next == now)
            break;

        std::swap(_heap[now], _heap[next]);
        now = next;
    }
}

template <typename T>
int PriorityQueue<T>::Count() const
{
    return static_cast<int>(_heap.size());
}

template <typename T>
void PriorityQueue<T>::Push(const T& data)
{
    _heap.push_back(data);
    HeapifyUp(static_cast<int>(_heap.size()) - 1);
}

template <typename T>
T PriorityQueue<T>::Pop()
{
    if (_heap.empty())
        throw std::runtime_error("PriorityQueue is empty");

    T ret = _heap[0];
    _heap[0] = _heap.back();
    _heap.pop_back();

    if (!_heap.empty())
        HeapifyDown(0);

    return ret;
}

template <typename T>
T PriorityQueue<T>::Peek() const
{
    if (_heap.empty())
        throw std::runtime_error("PriorityQueue is empty");

    return _heap[0];
}

template <typename T>
bool PriorityQueue<T>::Empty() const
{
    return _heap.empty();
}