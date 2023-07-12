

class NodeAvl
{
    public string key;

    public int value;
    public int balance;

    public CircularLinkedList listAvl;
    public int count;
    public NodeAvl left, right;

    public NodeAvl(string key, int value)
    {
        this.listAvl = new CircularLinkedList();
        this.key = key;
        this.value = value;
        balance = 0;
        count = 1;
    }
}

class AVLTree
{
    private NodeAvl root;

    private int Count(NodeAvl node)
    {
        if (node == null)
        {
            return 0;
        }
        return node.count;
    }


    private int BalanceFactor(NodeAvl node)
    {
        if (node == null)
        {
            return 0;
        }
        return Height(node.left) - Height(node.right);
    }

    private int Height(NodeAvl node)
    {
        if (node == null)
        {
            return 0;
        }
        return Math.Max(Height(node.left), Height(node.right)) + 1;
    }

    private NodeAvl RotateRight(NodeAvl y)
    {
        NodeAvl x = y.left;
        NodeAvl T2 = x.right;

        x.right = y;
        y.left = T2;

        y.balance = BalanceFactor(y);
        x.balance = BalanceFactor(x);

        return x;
    }

    private NodeAvl RotateLeft(NodeAvl x)
    {
        NodeAvl y = x.right;
        NodeAvl T2 = y.left;

        y.left = x;
        x.right = T2;

        x.balance = BalanceFactor(x);
        y.balance = BalanceFactor(y);

        return y;
    }

    private NodeAvl InsertNode(NodeAvl node, string key, int value)
    {
        if (node == null)
        {
            return new NodeAvl(key, value);
        }

        if (key.CompareTo(node.key) < 0)
        {
            node.left = InsertNode(node.left, key, value);
        }
        else if (key.CompareTo(node.key) > 0)
        {
            node.right = InsertNode(node.right, key, value);
        }
        else
        {
            node.listAvl.AddNode(value);
            node.count += 1;// Добавляем повторяющийся элемент в список
            return node;
        }

        node.balance = BalanceFactor(node);

        if (node.balance > 1 && key.CompareTo(node.left.key) < 0)
        {
            return RotateRight(node);
        }

        if (node.balance < -1 && key.CompareTo(node.right.key) > 0)
        {
            return RotateLeft(node);
        }

        if (node.balance > 1 && key.CompareTo(node.left.key) > 0)
        {
            node.left = RotateLeft(node.left);
            return RotateRight(node);
        }

        if (node.balance < -1 && key.CompareTo(node.right.key) < 0)
        {
            node.right = RotateRight(node.right);
            return RotateLeft(node);
        }

        return node;
    }

    public void Insert(string key, int value)
    {
        root = InsertNode(root, key, value);
    }

    public void Edit(string key, int value, int index)
    {
        root = EditeNode(root, key, value, index);
    }
    public NodeAvl Find(string key)
    {
        return FindNode(root, key);
    }
    private NodeAvl FindNode(NodeAvl node, string key)
    {
        if (node == null)
        {
            return null;
        }
        while (key.CompareTo(node.key) != 0)
        {
            if (key.CompareTo(node.key) < 0)
            {
                node = node.left;
            }
            if (key.CompareTo(node.key) > 0)
            {
                node = node.right;
            }
        }


        return node;

    }
    public NodeAvl EditeNode(NodeAvl node, string key, int value, int index)
    {
        if (node == null)
        {
            return node;
        }
        if (key.CompareTo(node.key) < 0)
        {
            node.left = EditeNode(node.left, key, value, index);
        }
        else if (key.CompareTo(node.key) > 0)
        {
            node.right = EditeNode(node.right, key, value, index);
        }
        else
        {
            if (node.value == index)
            {
                node.value = value;
            }
            else
            {
                node.listAvl.Edit(index, value);
            }
        }
        return node;
    }


    private NodeAvl FindMaxValueNode(NodeAvl node)
    {
        if (node == null)
        {
            return null;
        }

        while (node.right != null)
        {
            node = node.right;
        }

        return node;
    }

    private NodeAvl DeleteNode(NodeAvl node, string key, int value, ref bool deleted)
    {
        if (node == null)
        {
            return node;
        }

        if (key.CompareTo(node.key) < 0)
        {
            node.left = DeleteNode(node.left, key, value, ref deleted);
        }
        else if (key.CompareTo(node.key) > 0)
        {
            node.right = DeleteNode(node.right, key, value, ref deleted);
        }
        else
        {
            if (node.count > 1 && node.value != value)
            {

                node.listAvl.RemoveNode(value);
                return node;

            }
            if (node.value == value && node.count == 1)
            {
                if ((node.left == null) || (node.right == null))
                {
                    NodeAvl temp = null;
                    if (temp == node.left)
                    {
                        temp = node.right;
                    }
                    else
                    {
                        temp = node.left;
                    }

                    if (temp == null)
                    {
                        temp = node;
                        node = null;
                    }
                    else
                    {
                        node = temp;
                    }
                }
                else
                {
                    NodeAvl temp = FindMaxValueNode(node.left);
                    node.key = temp.key;
                    node.value = temp.value;
                    node.count = temp.count;
                    node.listAvl = temp.listAvl;
                    node.left = DeleteNode(node.left, temp.key, temp.value, ref deleted);
                }
                return node;
            }
            if (node.count > 1 && node.value == value)
            {
                node.count -= 1;
                node.value = node.listAvl.head.Data;
                node.listAvl.RemoveNode(node.value);
                return node;
            }

        }

        if (node == null)
        {
            return node;
        }

        node.balance = BalanceFactor(node);

        if (node.balance > 1 && BalanceFactor(node.left) >= 0)
        {
            return RotateRight(node);
        }

        if (node.balance > 1 && BalanceFactor(node.left) < 0)
        {
            node.left = RotateLeft(node.left);
            return RotateRight(node);
        }

        if (node.balance < -1 && BalanceFactor(node.right) <= 0)
        {
            return RotateLeft(node);
        }

        if (node.balance < -1 && BalanceFactor(node.right) > 0)
        {
            node.right = RotateRight(node.right);
            return RotateLeft(node);
        }

        return node;
    }

    public void Delete(string key, int value)
    {
        bool deleted = false;
        root = DeleteNode(root, key, value, ref deleted);

    }

    private void InorderTraversal(NodeAvl root)
    {
        if (root != null)
        {
            InorderTraversal(root.left);
            Console.Write(root.key + " ");
            InorderTraversal(root.right);
        }
    }

    public void PrintTree()
    {
        InorderTraversal(root);
        Console.WriteLine();
    }
}



