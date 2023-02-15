using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{
    private class Node
    {
        public T Data { get; private set; }
        public int Priority { get; set; } = 0;

        public Node(T data, int priority)
        {
            this.Data = data;
            this.Priority = priority;
        }
    }

    private List<Node> nodes = new List<Node>();

    public int Count => nodes.Count;

    public void Enqueue(T data, int priority)
    {
        Node newNode = new Node(data, priority);
        if (nodes.Count == 0)
        {
            nodes.Add(newNode);
        }
        else
        {
            //////////////////////////////
            // 이진 탐색을 시작한다. 'O(logN)'
            int start = 0;
            int end = nodes.Count - 1;
            int harf = 0;
            while (start != end)
            {
                if (end - start == 1)
                {
                    if (nodes[start].Priority < priority)
                    {
                        harf = end;
                    }
                    else
                    {
                        harf = start;
                    }
                    break;
                }
                else
                {
                    harf = start + ((end - start) / 2);
                    if (nodes[harf].Priority > priority)
                    {
                        // Down
                        end = harf;
                    }
                    else
                    {
                        // Up
                        start = harf;
                    }
                }
            }
            //////////////////////////////

            if (nodes[harf].Priority > priority)
                nodes.Insert(harf, newNode);
            else
                nodes.Insert(harf + 1, newNode);
        }
    }

    public T Dequeue()
    {
        Node tail = null;
        
        if (Count > 0)
        {
            tail = nodes[nodes.Count - 1];
            nodes.RemoveAt(nodes.Count - 1);
        }

        if (tail != null)
            return tail.Data;
        return default(T);
    }

    public T Peek()
    {
        Node tail = null;
        
        if (Count > 0)
            tail = nodes[nodes.Count - 1];

        if (tail != null)
            return tail.Data;
        return default(T);
    }

    public object this[int iKey]
    {
        get
        {
            return this.nodes[iKey].Data;
        }
    }

    public bool Contains(T data)
    {
        bool isInQueue = false;

        foreach(Node node in nodes)
        {
            if(node.Data.Equals(data))
                isInQueue = true;
        }

        return isInQueue;
    }
}
