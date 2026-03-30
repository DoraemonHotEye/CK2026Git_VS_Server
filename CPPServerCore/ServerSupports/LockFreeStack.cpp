#include "LockFreeStack.h"

LockFreeStack::~LockFreeStack()
{
	while (Pop() != nullptr);
}

void LockFreeStack::Push(const std::string& data)
{
    Node* newNode = new Node(data);

    newNode->next = _head.load(std::memory_order_relaxed);

    while (!_head.compare_exchange_weak(
        newNode->next,
        newNode,
        std::memory_order_release,
        std::memory_order_relaxed))
    {
    }
}

LockFreeStack::Node* LockFreeStack::Pop()
{
    Node* oldHead = _head.load(std::memory_order_relaxed);

    while (oldHead &&
        !_head.compare_exchange_weak(
            oldHead,
            oldHead->next,
            std::memory_order_acquire,
            std::memory_order_relaxed))
    {
    }

    return oldHead;
}
