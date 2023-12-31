﻿namespace GuideSystemApp.Marks.List;

public class Node<T> where T : IComparable<T>
{
    public T Data { get; set; }
    public Node<T> Next { get; set; }

    public Node(T data)
    {
        Data = data;
        Next = null;
    }
}