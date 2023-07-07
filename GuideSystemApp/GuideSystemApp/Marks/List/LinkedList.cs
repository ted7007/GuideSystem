using Lab2_4;

namespace GuideSystemApp.Marks.List;

public class LinkedList<T> where T : IComparable<T>
{
    public Node<T> head;

    public void Add(T data)
    {
        Node<T> newNode = new Node<T>(data);

        if (head == null)
        {
            head = newNode;
        }
        else
        {
            Node<T> currentNode = head;

            while (currentNode.Next != head)
            {
                currentNode = currentNode.Next;
            }

            newNode.Next = head;
            currentNode.Next = newNode;
        }
    }

    public void Remove(T data)
    {
        if (head == null)
        {
            return;
        }

        if (head.Data.Equals(data))
        {
            head = head.Next;
            return;
        }

        Node<T> currentNode = head;

        while (currentNode.Next != head)
        {
            if (currentNode.Next.Data.Equals(data))
            {
                currentNode.Next = currentNode.Next.Next;
                return;
            }

            currentNode = currentNode.Next;
        }
    }
    
    public void Remove(Func<T, bool> exp)
    {
        if (head == null)
        {
            return;
        }

        if (exp(head.Data))
        {
            head = head.Next;
            return;
        }

        Node<T> currentNode = head;

        while (currentNode.Next != head)
        {
            if (exp(head.Data))
            {
                currentNode.Next = currentNode.Next.Next;
                return;
            }

            currentNode = currentNode.Next;
        }
    }

    public void Print()
    {
        Node<T> currentNode = head;

        while (currentNode != null)
        {
            Console.WriteLine(currentNode.Data);
            currentNode = currentNode.Next;
        }
    }
    
    public Node<T>? Find(T data)
    {
        Node<T> currentNode = head;

        while (currentNode != head)
        {
            if (currentNode.Data.Equals(data))
            {
                return currentNode;
            }

            currentNode = currentNode.Next;
        }

        return null;
    }
    
    public Node<T>? Find(Func<T, bool> exp)
    {
        Node<T> currentNode = head;

        while (currentNode != head)
        {
            if (exp(currentNode.Data))
            {
                return currentNode;
            }

            currentNode = currentNode.Next;
        }

        return null;
    }
}
