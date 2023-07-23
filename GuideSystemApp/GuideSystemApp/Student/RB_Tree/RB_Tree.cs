using GuideSystemApp.Student.List;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GuideSystemApp.Student.RB
{
    public class RB_Tree
    {
        public TreeNode root;

        public RB_Tree()
        {
            root = null;
        }
        private void RotateLeft(TreeNode node)
        {
            TreeNode newNode = node.Right;
            node.Right = newNode.Left;

            if (newNode.Left != null)
            {
                newNode.Left.Parent = node;
            }

            if (newNode != null)
            {
                newNode.Parent = node.Parent;
            }

            if (node.Parent == null)
            {
                root = newNode;
            }
            else if (node == node.Parent.Left)
            {
                node.Parent.Left = newNode;
            }
            else
            {
                node.Parent.Right = newNode;
            }

            newNode.Left = node;

            if (node != null)
            {
                node.Parent = newNode;
            }
        }

        private void RotateRight(TreeNode node)
        {
            TreeNode newNode = node.Left;
            node.Left = newNode.Right;

            if (newNode.Right != null)
            {
                newNode.Right.Parent = node;
            }

            if (newNode != null)
            {
                newNode.Parent = node.Parent;
            }

            if (node.Parent == null)
            {
                root = newNode;
            }
            else if (node == node.Parent.Right)
            {
                node.Parent.Right = newNode;
            }
            else
            {
                node.Parent.Left = newNode;
            }

            newNode.Right = node;

            if (node != null)
            {
                node.Parent = newNode;
            }
        }

        private void FixViolation(TreeNode node)
        {
            TreeNode parent = null;
            TreeNode grandparent = null;

            while (node != null && node != root && node.Color != Colors.Black && node.Parent != null && node.Parent.Color == Colors.Red)
            {
                parent = node.Parent;
                grandparent = node.Parent.Parent;

                if (parent == grandparent.Left)
                {
                    TreeNode uncle = grandparent.Right;

                    if (uncle != null && uncle.Color == Colors.Red)
                    {
                        grandparent.Color = Colors.Red;
                        parent.Color = Colors.Black;
                        uncle.Color = Colors.Black;
                        node = grandparent;
                    }
                    else
                    {
                        if (node == parent.Right)
                        {
                            RotateLeft(parent);
                            node = parent;
                            parent = node.Parent;
                        }

                        RotateRight(grandparent);
                        Colors temp = parent.Color;
                        parent.Color = grandparent.Color;
                        grandparent.Color = temp;
                        node = parent;
                    }
                }
                else
                {
                    TreeNode uncle = grandparent.Left;

                    if (uncle != null && uncle.Color == Colors.Red)
                    {
                        grandparent.Color = Colors.Red;
                        parent.Color = Colors.Black;
                        uncle.Color = Colors.Black;
                        node = grandparent;
                    }
                    else
                    {
                        if (node == parent.Left)
                        {
                            RotateRight(parent);
                            node = parent;
                            parent = node.Parent;
                        }

                        RotateLeft(grandparent);
                        Colors temp = parent.Color;
                        parent.Color = grandparent.Color;
                        grandparent.Color = temp;
                        node = parent;
                    }
                }
            }

            root.Color = Colors.Black;
        }

        private TreeNode InsertNode(TreeNode root, TreeNode node)
        {
            if (root == null)
            {
                return node;
            }

            if (String.Compare(root.Key, node.Key, StringComparison.Ordinal) > 0)
            {
                root.Left = InsertNode(root.Left, node);
                root.Left.Parent = root;
            }
            else if (String.Compare(root.Key, node.Key, StringComparison.Ordinal) < 0)
            {
                root.Right = InsertNode(root.Right, node);
                root.Right.Parent = root;
            }
            else if (String.Compare(root.Key, node.Key, StringComparison.Ordinal) == 0)
            {
                if (root.List == null)
                {
                    root.List = new LinkedList();
                }

                root.List.AddNode(ref root.List.head, node.value);
            }

            return root;
        }

        public void Insert(string Key, int value)
        {
            TreeNode node = new TreeNode(Key, value);
            if (root == null)
            {
                root = node; // Инициализация root
            }
            else
            {
                root = InsertNode(root, node);
            }
            FixViolation(node);
        }
        public void Delete(string key, int index)
        {

            var node = Find(key);
            if (node != null)
            {
                {
                    if(node.value == index && node.List.head == null)
                    {
                        DeleteTreeNode(root, node.Key, index);
                    }
                    else if (node.List.head != null&&node.value==index)                  
                    {
                            var a = node.List.head.data;
                            node.List.head.data = node.value;
                            node.value = a;
                            node.List.DeleteNode(ref node.List.head, index);                       
                    }
                    else
                    {
                        node.List.DeleteNode(ref node.List.head, index);
                    }

                }
            }
        }

        private TreeNode DeleteTreeNode(TreeNode root, string key, int i)
        {
            if (root == null)
            {
                return null;
            }

            int comparisonResult = String.Compare(key, root.Key, StringComparison.Ordinal);

            if (comparisonResult < 0)
            {
                root.Left = DeleteTreeNode(root.Left, key, i);
            }
            else if (comparisonResult > 0)
            {
                root.Right = DeleteTreeNode(root.Right, key, i);
            }
            else
            {
                if (root.Left == null || root.Right == null)
                {
                    TreeNode replacement = root.Left ?? root.Right;

                    if (replacement == null)
                    {
                        return null;
                    }
                    else
                    {
                        replacement.Parent = root.Parent;
                        if (root.Parent == null)
                        {
                            this.root = replacement;
                        }
                        else if (root == root.Parent.Left)
                        {
                            root.Parent.Left = replacement;
                        }
                        else
                        {
                            root.Parent.Right = replacement;
                        }

                        if (root.Color == Colors.Black)
                        {
                            if (replacement.Color == Colors.Red)
                            {
                                replacement.Color = Colors.Black;
                            }
                            else
                            {
                                FixDoubleBlack(replacement);
                            }
                        }
                    }
                }
                else
                {
                    if (root.List != null && root.List.Contains(i))
                    {
                        root.List.DeleteNode(ref root.List.head, i);
                    }

                    TreeNode successor = FindMinimum(root.Right);
                    root.Key = successor.Key;
                    root.Right = DeleteTreeNode(root.Right, successor.Key, i);
                }
            }

            return root;
        }

        /*   public void Edit(string key, int value, int index)
           {
               var node = FindNode(_root, value);
               var resItem = node.List.Find(expression);
               resItem.Data = newValue;
           }*/
        public TreeNode Find(string value)
        {
            return FindNode(root, value);
        }
        public void EditValue(string key, int value, int newValue)
        {
            var node = FindNode(root, key);
            if (node != null)
            {
                var resItem = node.List.PoiskUzla(node.List.head, value);
                if (resItem != null)
                {
                    resItem.data = newValue;
                }
            }
        }

        private TreeNode FindNode(TreeNode root, string key)
        {
            if (root == null)
            {
                return null;
            }

            int compareResult = key.CompareTo(root.Key);

            if (compareResult == 0)
            {
                return root;
            }
            else if (compareResult < 0)
            {
                return FindNode(root.Left, key);
            }
            else
            {
                return FindNode(root.Right, key);
            }
        }
        private TreeNode FindMinimum(TreeNode node)
        {
            while (node.Left != null)
            {
                node = node.Left;
            }

            return node;
        }

        private void FixDoubleBlack(TreeNode node)
        {
            if (node == root)
            {
                return;
            }

            TreeNode sibling = GetSibling(node);
            TreeNode parent = node.Parent;

            if (sibling == null)
            {
                FixDoubleBlack(parent);
            }
            else
            {
                if (sibling.Color == Colors.Red)
                {
                    parent.Color = Colors.Red;
                    sibling.Color = Colors.Black;
                    if (node == parent.Left)
                    {
                        RotateLeft(parent);
                    }
                    else
                    {
                        RotateRight(parent);
                    }
                    FixDoubleBlack(node);
                }
                else
                {
                    if ((sibling.Left == null || sibling.Left.Color == Colors.Black) && (sibling.Right == null || sibling.Right.Color == Colors.Black))
                    {
                        sibling.Color = Colors.Red;
                        if (parent.Color == Colors.Black)
                        {
                            FixDoubleBlack(parent);
                        }
                        else
                        {
                            parent.Color = Colors.Black;
                        }
                    }
                    else
                    {
                        if (node == parent.Left && (sibling.Right == null || sibling.Right.Color == Colors.Black))
                        {
                            sibling.Color = Colors.Red;
                            sibling.Left.Color = Colors.Black;
                            RotateRight(sibling);
                        }
                        else if (node == parent.Right && (sibling.Left == null || sibling.Left.Color == Colors.Black))
                        {
                            sibling.Color = Colors.Red;
                            sibling.Right.Color = Colors.Black;
                            RotateLeft(sibling);
                        }
                        sibling = GetSibling(node);
                        sibling.Color = parent.Color;
                        parent.Color = Colors.Black;
                        if (node == parent.Left)
                        {
                            sibling.Right.Color = Colors.Black;
                            RotateLeft(parent);
                        }
                        else
                        {
                            sibling.Left.Color = Colors.Black;
                            RotateRight(parent);
                        }
                    }
                }
            }
        }

        public List<TreeNode> GetNodes()
        {
            List<TreeNode> nodes = new List<TreeNode>();
            InOrderTraversal(root, nodes);
            return nodes;
        }

        public void InOrderTraversal(TreeNode node, List<TreeNode> nodes)
        {
            if (node == null)
            {
                return;
            }

            InOrderTraversal(node.Left, nodes);
            nodes.Add(node);
            InOrderTraversal(node.Right, nodes);
        }
        public TreeNode GetSibling(TreeNode node)
        {
            if (node == node.Parent.Left)
            {
                return node.Parent.Right;
            }
            else
            {
                return node.Parent.Left;
            }
        }
        public void PrintTree()
        {
            PrintTree(root, 0);
        }

        public int Search(string key, out int comparisons)
        {
            TreeNode node = Find(root, key, out comparisons);
            return node != null ? node.value : -1;
        }

        public TreeNode Find(TreeNode node, string key, out int comparisons)
        {
            comparisons = 0;

            while (node != null)
            {
                comparisons++;
                int comparisonResult = key.CompareTo(node.Key);

                if (comparisonResult == 0)
                {
                    return node;
                }
                else if (comparisonResult < 0)
                {
                    node = node.Left;
                }
                else
                {
                    node = node.Right;
                }
            }

            return null;
        }
        public void PrintTree(TreeNode node, int level)
        {
            if (node == null)
            {
                return;
            }

            PrintTree(node.Right, level + 1);

            string indent = GetIndent(level);
            string nodeColor = node.Color == Colors.Red ? "Red" : "Black"; // Получаем символ цвета вершины
            Console.WriteLine($"{indent}{nodeColor} {node.Key} ({node.value})");

            if (node.List != null)
            {
                Node current = node.List.head;
                while (current != null)
                {
                    string listIndent = GetIndent(level + 1);
                    Console.WriteLine($"{listIndent}Index: {current.data}");
                    current = current.next;
                }
            }

            PrintTree(node.Left, level + 1);
        }
        public string GetTreeString(TreeNode node, int level = 0)
        {
            StringBuilder treeString = new StringBuilder();

            if (node != null)
            {
                treeString.Append(GetTreeString(node.Right, level + 1));

                string indent = GetIndent(level);
                string nodeColor = node.Color == Colors.Red ? "Red" : "Black"; // Получаем символ цвета вершины
                treeString.AppendLine($"{indent}{nodeColor} {node.Key} ({node.value})");

                if (node.List != null)
                {
                    Node current = node.List.head;
                    while (current != null)
                    {
                        string listIndent = GetIndent(level + 1);
                        treeString.AppendLine($"{listIndent}Index: {current.data}");
                        current = current.next;
                    }
                }

                treeString.Append(GetTreeString(node.Left, level + 1));
            }

            return treeString.ToString();
        }

        public string GetIndent(int level)
        {
            const int SpacesPerLevel = 12;
            return new string(' ', level * SpacesPerLevel);
        }
    }
}