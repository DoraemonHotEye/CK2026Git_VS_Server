#pragma once
#include <atomic>
#include <string>

class LockFreeStack
{
public:
    struct Node
    {
        std::string data;
        Node* next;

        Node(const std::string& d) : data(d), next(nullptr) {}
    };

private:
    std::atomic<Node*> _head;

public:
    LockFreeStack() : _head(nullptr) {}

    ~LockFreeStack();

    void Push(const std::string& data);

    Node* Pop();
};