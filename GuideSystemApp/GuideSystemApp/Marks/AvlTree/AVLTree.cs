namespace GuideSystemApp.Marks.AvlTree;


// Дерево АВЛ с повторами
public class AVLTree<T> where T : IComparable<T>
{
    private AVLNode<T> _root;

    // Добавление элемента в дерево
    public void Add(T value)
    {
        _root = AddNode(_root, value);
    }

    // Рекурсивный метод добавления узла в дерево
    private AVLNode<T> AddNode(AVLNode<T> node, T value)
    {
        if (node == null)
        {
            return new AVLNode<T>(value);
        }
        else if (value.CompareTo(node.Value) < 0)
        {
            node.Left = AddNode(node.Left, value);
        }
        else if (value.CompareTo(node.Value) > 0)
        {
            node.Right = AddNode(node.Right, value);
        }
        else
        {
            node.List.Add(value); // Добавляем элемент в список в узле
        }

        node.UpdateHeight(); // Обновляем высоту узла

        // Балансировка узла после добавления
        int balance = node.GetBalance();

        if (balance > 1) // Необходимо поворот вправо
        {
            if (value.CompareTo(node.Right.Value) < 0)
            {
                node.Right = RotateRight(node.Right);
            }
            node = RotateLeft(node);
        }
        else if (balance < -1) // Необходимо поворот влево
        {
            if (value.CompareTo(node.Left.Value) > 0)
            {
                node.Left = RotateLeft(node.Left);
            }
            node = RotateRight(node);
        }

        return node;
    }

    // Вспомогательный метод для правого поворота
    private AVLNode<T> RotateRight(AVLNode<T> node)
    {
        AVLNode<T> newRoot = node.Left;
        AVLNode<T> newNode = newRoot.Right;

        newRoot.Right = node;
        node.Left = newNode;

        node.UpdateHeight();
        newRoot.UpdateHeight();

        return newRoot;
    }

    // Вспомогательный метод для левого поворота
    private AVLNode<T> RotateLeft(AVLNode<T> node)
    {
        AVLNode<T> newRoot = node.Right;
        AVLNode<T> newNode = newRoot.Left;

        newRoot.Left = node;
        node.Right = newNode;

        node.UpdateHeight();
        newRoot.UpdateHeight();

        return newRoot;
    }

    // Поиск элемента в дереве
    public bool Contains(T value)
    {
        return FindNode(_root, value) != null;
    }

    // Рекурсивный метод поиска узла в дереве
    private AVLNode<T> FindNode(AVLNode<T> node, T value)
    {
        if (node == null)
        {
            return null;
        }

        int compareResult = value.CompareTo(node.Value);

        if (compareResult == 0)
        {
            return node;
        }
        else if (compareResult < 0)
        {
            return FindNode(node.Left, value);
        }
        else
        {
            return FindNode(node.Right, value);
        }
    }
    
    // Вывод элементов дерева в консоль
    public void Print()
    {
        PrintTree(_root);
    }

    private void PrintTree(AVLNode<T> node)
    {
        if (node != null)
        {PrintTree(node.Left);
            Console.WriteLine(node.Value + ": " + string.Join(", ", node.List));
            PrintTree(node.Right);
        }
    }

    // Удаление элемента из дерева
    public void Remove(T value)
    {
        _root = RemoveNode(_root, value);
    }

    // Рекурсивный метод удаления узла из дерева
    private AVLNode<T> RemoveNode(AVLNode<T> node, T value)
    {
        if (node == null)
        {
            return null;
        }

        int compareResult = value.CompareTo(node.Value);

        if (compareResult < 0)
        {
            node.Left = RemoveNode(node.Left, value);
        }
        else if (compareResult > 0)
        {
            node.Right = RemoveNode(node.Right, value);
        }
        else
        {
            // Удаляем элемент из списка в узле
            node.List.Remove(value);

            if (node.List.head == null)
            {
                // Узел больше не содержит элементов, удаляем его из дерева
                if (node.Left == null && node.Right == null)
                {
                    node = null;
                }
                else if (node.Left == null)
                {
                    node = node.Right;
                }
                else if (node.Right == null)
                {
                    node = node.Left;
                }
                else
                {
                    // Находим минимальный элемент в правом поддереве
                    AVLNode<T> minNode = FindMinNode(node.Right);

                    node.Value = minNode.Value;
                    node.List = minNode.List; // Заменяем список в удаляемом узле на список минимального узла
                    node.Right = RemoveNode(node.Right, minNode.Value);
                }
            }
        }

        if (node != null)
        {
            node.UpdateHeight();

            // Балансировка узла после удаления
            int balance = node.GetBalance();

            if (balance > 1) // Необходимо поворот вправо
            {
                if (node.Right.GetBalance() < 0)
                {
                    node.Right = RotateRight(node.Right);
                }
                node = RotateLeft(node);
            }
            else if (balance < -1) // Необходимо поворот влево
            {
                if (node.Left.GetBalance() > 0)
                {
                    node.Left = RotateLeft(node.Left);
                }
                node = RotateRight(node);
            }
        }

        return node;
    }

    // Находит узел с минимальным значением в дереве
    private AVLNode<T> FindMinNode(AVLNode<T> node)
    {
        if (node.Left != null)
        {
            return FindMinNode(node.Left);
        }

        return node;
    }

    // Возвращает высоту дерева
    public int GetHeight()
    {
        return GetTreeHeight(_root);
    }

    // Рекурсивный метод для определения высоты дерева
    private int GetTreeHeight(AVLNode<T> node)
    {
        if (node == null)
        {
            return 0;
        }

        int leftHeight = GetTreeHeight(node.Left);
        int rightHeight = GetTreeHeight(node.Right);

        return Math.Max(leftHeight, rightHeight) + 1;
    }
}