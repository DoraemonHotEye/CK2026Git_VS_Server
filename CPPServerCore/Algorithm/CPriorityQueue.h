#pragma once
#include <vector>
#include <stdexcept>
#include <utility> // std::swap

template <typename T>
class PriorityQueue
{
private:
    std::vector<T> _heap;

    void HeapifyUp(int index);
    void HeapifyDown(int index);

public:
    PriorityQueue() = default;

    int Count() const;
    void Push(const T& data);
    T Pop();
    T Peek() const;
    bool Empty() const;
};

// 템플릿 구현 분리용
#include "CPriorityQueue.inl"