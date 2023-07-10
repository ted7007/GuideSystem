

class NodeAvl
{
    public int value;
    public int height;
    public NodeAvl left;
    public NodeAvl right;

    public NodeAvl(int value)
    {
        this.value = value;
        this.height = 1;
    }
}

class AVLTree
{
    private NodeAvl root;

    private int GetHeight(NodeAvl node)
    {
        if (node == null)
            return 0;
        
        return node.height;
    }

    private int GetBalanceFactor(NodeAvl node)
    {
        if (node == null)
            return 0;
        
        return GetHeight(node.left) - GetHeight(node.right);
    }

    private NodeAvl RotateRight(NodeAvl y)
    {
        NodeAvl x = y.left;
        NodeAvl T2 = x.right;

        x.right = y;
        y.left = T2;

        y.height = Math.Max(GetHeight(y.left), GetHeight(y.right)) + 1;
        x.height = Math.Max(GetHeight(x.left), GetHeight(x.right)) + 1;

        return x;
    }

    private NodeAvl RotateLeft(NodeAvl x)
    {
        NodeAvl y = x.right;
        NodeAvl T2 = y.left;

        y.left = x;
        x.right = T2;

        x.height = Math.Max(GetHeight(x.left), GetHeight(x.right)) + 1;
        y.height = Math.Max(GetHeight(y.left), GetHeight(y.right)) + 1;

        return y;
    }

    public void Insert(int value)
    {
        root = InsertNode(root, value);
    }

    private NodeAvl InsertNode(NodeAvl node, int value)
    {
        if (node == null)
            return new NodeAvl(value);

        if (value < node.value)
            node.left = InsertNode(node.left, value);
        else if (value > node.value)
            node.right = InsertNode(node.right, value);
        else
            return node;

        node.height = 1 + Math.Max(GetHeight(node.left), GetHeight(node.right));

        int balanceFactor = GetBalanceFactor(node);

        // Left Left Case
        if (balanceFactor > 1 && value < node.left.value)
            return RotateRight(node);

        // Right Right Case
        if (balanceFactor < -1 && value > node.right.value)
            return RotateLeft(node);

        // Left Right Case
        if (balanceFactor > 1 && value > node.left.value)
        {
            node.left = RotateLeft(node.left);
            return RotateRight(node);
        }

        // Right Left Case
        if (balanceFactor < -1 && value < node.right.value)
        {
            node.right = RotateRight(node.right);
            return RotateLeft(node);
        }

        return node;
    }

    public void Delete(int value)
    {
        root = DeleteNode(root, value);
    }

    private NodeAvl DeleteNode(NodeAvl root, int value)
    {
        if (root == null)
            return root;

        if (value < root.value)
            root.left = DeleteNode(root.left, value);
        else if (value > root.value)
            root.right = DeleteNode(root.right, value);
        else
        {
            if ((root.left == null) || (root.right == null))
            {
                NodeAvl temp = null;
                if (temp == root.left)
                    temp = root.right;
                else
                    temp = root.left;

                if (temp == null)
                {
                    temp = root;
                    root = null;
                }
                else
                    root = temp;
            }
            else
            {
                NodeAvl temp = FindMax(root.left);
                root.value = temp.value;
                root.left = DeleteNode(root.left, temp.value);
            }
        }

        if (root == null)
            return root;

        root.height = 1 + Math.Max(GetHeight(root.left), GetHeight(root.right));

        int balanceFactor = GetBalanceFactor(root);

        // Left Left Case
        if (balanceFactor > 1 && GetBalanceFactor(root.left) >= 0)
            return RotateRight(root);

        // Left Right Case
        if (balanceFactor > 1 && GetBalanceFactor(root.left) < 0)
        {
            root.left = RotateLeft(root.left);
            return RotateRight(root);
        }

        // Right Right Case
        if (balanceFactor < -1 && GetBalanceFactor(root.right) <= 0)
            return RotateLeft(root);

        // Right Left Case
        if (balanceFactor < -1 && GetBalanceFactor(root.right) > 0)
        {
            root.right = RotateRight(root.right);
            return RotateLeft(root);
        }

        return root;
    }

    private NodeAvl FindMax(NodeAvl node)
    {
        while (node.right != null)
            node = node.right;

        return node;
    }

    public void Print()
    {
        InOrderTraversal(root);
        Console.WriteLine();
    }

    private void InOrderTraversal(NodeAvl node)
    {
        if (node != null)
        {
            InOrderTraversal(node.left);
            Console.Write(node.value + " ");
            InOrderTraversal(node.right);
        }
    }
}

// public class Program
// {
//     public static void Main(string[] args)
//     {
//         AVLTree tree = new AVLTree();

//         tree.Insert(9);
//         tree.Insert(5);
//         tree.Insert(10);
//         tree.Insert(0);
//         tree.Insert(6);
//         tree.Insert(11);
//         tree.Insert(-1);
//         tree.Insert(1);
//         tree.Insert(2);

//         Console.WriteLine("AVL Tree:");
//         tree.Print();

//         tree.Delete(10);

//         Console.WriteLine("AVL Tree after deleting 10:");
//         tree.Print();
//     }
// }

