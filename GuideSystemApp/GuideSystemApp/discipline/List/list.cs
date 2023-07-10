public class Node
{
    public int Data { get; set; }
    public Node Next { get; set; }
}

public class CircularLinkedList
{
    private Node head;

    public void AddNode(int data)
    {
        Node newNode = new Node();
        newNode.Data = data;

        if (head == null)
        {
            head = newNode;
            newNode.Next = head;
        }
        else
        {
            Node current = head;
            while (current.Next != head)
            {
                current = current.Next;
            }

            current.Next = newNode;
            newNode.Next = head;
        }
    }

    public bool RemoveNode(int data)
    {
        if (head == null)
        {
            return false;
        }

        Node current = head;
        Node previous = null;

        do
        {
            if (current.Data == data)
            {
                if (previous != null)
                {
                    previous.Next = current.Next;

                    if (current == head)
                    {
                        head = current.Next;
                    }
                }
                else // Удаление головного узла
                {
                    if (current.Next == head) // Список содержал только один элемент
                    {
                        head = null;
                    }
                    else
                    {
                        Node temp = head;
                        while (temp.Next != head)
                        {
                            temp = temp.Next;
                        }

                        head = head.Next;
                        temp.Next = head;
                    }
                }

                return true;
            }

            previous = current;
            current = current.Next;
        } while (current != head);

        return false; // Элемент не найден
    }

    public bool Search(int data)
    {
        if (head == null)
        {
            return false;
        }

        Node current = head;

        do
        {
            if (current.Data == data)
            {
                return true;
            }

            current = current.Next;
        } while (current != head);

        return false; // Элемент не найден
    }

    public void PrintList()
    {
        if (head == null)
        {
            Console.WriteLine("Список пуст");
            return;
        }

        Node current = head;
        do
        {
            Console.Write(current.Data + " ");
            current = current.Next;
        } while (current != head);

        Console.WriteLine();
    }
}
