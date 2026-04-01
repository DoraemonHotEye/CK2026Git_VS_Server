#pragma once
#include <atomic>
#include <iostream>

template <typename T>
class LockFreeStack
{
public:
    struct Node
    {
        T data;
        Node* next;

        Node(const T& d) : data(d), next(nullptr) {}
    };

public:
    std::atomic<Node*> _head;

public:
    LockFreeStack() : _head(nullptr) {}

    ~LockFreeStack()
    {
        while (Node* node = Pop())
        {
            delete node;
        }
    }
    void Push(const T& data)
    {
        Node* newNode = new Node(data);

        newNode->next = _head.load();

        while (!_head.compare_exchange_weak(newNode->next, newNode))
        {

        }
    }

    Node* Pop()
    {
        Node* oldHead = _head.load();

        while (oldHead && !_head.compare_exchange_weak(oldHead, oldHead->next))
        {

        }

        return oldHead;
    }

    // 4. 비었는지 확인
    bool IsEmpty() const
    {
        return _head.load() == nullptr;
    }
};