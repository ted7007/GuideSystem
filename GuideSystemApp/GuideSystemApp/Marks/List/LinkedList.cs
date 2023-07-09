using System.Text;

namespace GuideSystemApp.Marks.List;

public class LinkedList<T> where T : IComparable<T>
{
    public Node<T> head;

    public Node<T> tail;

    public void Add(T data)
    {
        Node<T> newNode = new Node<T>(data);

        if (head == null)
        {
            head = newNode;
            tail = newNode;
        }
        else
        {
            tail.Next = newNode;
            tail = newNode;
        }

        tail.Next = head;
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

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        if (head == null)
            return "Ошибка";
        Node<T> currentNode = head;
        sb.Append(head.Data + "->");
        currentNode = head.Next;
        while (currentNode != head)
        {
            sb.Append(currentNode.Data + "->");
            currentNode = currentNode.Next;
        }

        sb.Append("head");
        return sb.ToString();
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
