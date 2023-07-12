using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuideSystemApp.Student.RB_Tree
{
    internal class RB_Tree
    {
        public TreeNode T_NULL;

        public bool IsComp(SuperKey b, SuperKey a)
        {
            if (String.Compare(a.Key, b.Key, StringComparison.Ordinal) > 0)
            {
                return true;
            }
            else if (String.Compare(a.Key, b.Key, StringComparison.Ordinal) == 0)
            {
                if (a.Index > b.Index)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsEqual(SuperKey b, SuperKey a)
        {
            return (a.Key == b.Key);
        }

        public bool Poisk_RB(TreeNode root, SuperKey k)
        {
            bool a = false;
            while (root != T_NULL && !IsEqual(root.Key, k))
            {
                if (IsEqual(root.Key, k))
                    a = true;
                if (!IsComp(root.Key, k))
                    root = root.Left;
                else
                    root = root.Right;
                if (IsEqual(root.Key, k))
                    a = true;
            }
            if (IsEqual(root.Key, k))
                a = true;
            return a;
        }

        public void Print_RB(TreeNode root, int l)
        {
            if (root != T_NULL)
            {
                Print_RB(root.Right, l + 1);

                for (int i = 1; i <= l; i++)
                {
                    Console.Write("   ");
                }

                Console.WriteLine(root.Key.Key + ", " + root.Key.Index + " " + root.Color);

                Print_RB(root.Left, l + 1);
            }
        }

        public int Count_RB(TreeNode root)
        {
            if (root == T_NULL)
            {
                return 0;
            }

            int l = Count_RB(root.Left);
            int r = Count_RB(root.Right);

            return 1 + l + r;
        }

        public void Print_Count(TreeNode root)
        {
            int count = Count_RB(root);
            Console.WriteLine("count = " + count);
        }

        public void Left_Rotate(ref TreeNode root, TreeNode x)
        {
            TreeNode y;
            y = x.Right;
            x.Right = y.Left;

            if (y.Left != T_NULL)
                y.Left.Parent = x;

            y.Parent = x.Parent;

            if (x.Parent == T_NULL)
                root = y;
            else if (x == x.Parent.Left)
                x.Parent.Left = y;
            else
                x.Parent.Right = y;

            y.Left = x;
            x.Parent = y;
        }

        public void Right_Rotate(ref TreeNode root, TreeNode x)
        {
            TreeNode y;
            y = x.Left;
            x.Left = y.Right;

            if (y.Right != T_NULL)
                y.Right.Parent = x;

            y.Parent = x.Parent;

            if (x.Parent == T_NULL)
                root = y;
            else if (x == x.Parent.Right)
                x.Parent.Right = y;
            else
                x.Parent.Left = y;

            y.Right = x;
            x.Parent = y;
        }

        public void Inicializaciya(ref TreeNode root)
        {
            T_NULL = new TreeNode();
            T_NULL.Color = Colors.Black;
            T_NULL.Key = new SuperKey();
            T_NULL.Right = null;
            T_NULL.Left = null;
            T_NULL.Parent = null;
            root = T_NULL;
        }

        public TreeNode Tree_Minimum(TreeNode x)
        {
            if (x != T_NULL)
            {
                while (x.Left != T_NULL)
                {
                    x = x.Left;
                }
                return x;
            }
            return null;
        }

        public TreeNode Tree_Max_Left(TreeNode y)
        {
            if (y != T_NULL)
            {
                TreeNode x = y.Left;
                while (x.Right != T_NULL)
                {
                    x = x.Right;
                }
                return x;
            }
            return null;
        }

        public TreeNode Tree_Maximum(TreeNode x)
        {
            if (x != T_NULL)
            {
                while (x.Right != T_NULL)
                {
                    x = x.Right;
                }
                return x;
            }
            return null;
        }

        public void RB_Transplant(ref TreeNode root, TreeNode u, TreeNode v)
        {
            if (u.Parent == T_NULL)
                root = v;
            else if (u == u.Parent.Left)
                u.Parent.Left = v;
            else
                u.Parent.Right = v;

            v.Parent = u.Parent;
        }

        public void RB_Insert_Fixup(ref TreeNode root, TreeNode z)
        {
            TreeNode y;
            while (z.Parent.Color == Colors.Red)
            {
                if (z.Parent == z.Parent.Parent.Left)
                {
                    y = z.Parent.Parent.Right;
                    if (y.Color == Colors.Red)            //Случай 1
                    {
                        z.Parent.Color = Colors.Black;
                        y.Color = Colors.Black;
                        z.Parent.Parent.Color = Colors.Red;
                        z = z.Parent.Parent;
                    }
                    else
                    {
                        if (z == z.Parent.Right) //Случай 2
                        {
                            z = z.Parent;
                            Left_Rotate(ref root, z);
                        }
                        z.Parent.Color = Colors.Black;       //Случай 3
                        z.Parent.Parent.Color = Colors.Red;
                        Right_Rotate(ref root, z.Parent.Parent);
                    }
                }
                else
                {
                    y = z.Parent.Parent.Left;
                    if (y.Color == Colors.Red)            //Случай 1
                    {
                        z.Parent.Color = Colors.Black;
                        y.Color = Colors.Black;
                        z.Parent.Parent.Color = Colors.Red;
                        z = z.Parent.Parent;
                    }
                    else
                    {
                        if (z == z.Parent.Left)  //Случай 2
                        {
                            z = z.Parent;
                            Right_Rotate(ref root, z);
                        }
                        z.Parent.Color = Colors.Black;       //Случай 3
                        z.Parent.Parent.Color = Colors.Red;
                        Left_Rotate(ref root, z.Parent.Parent);
                    }
                }
            }
            root.Color = Colors.Black;
        }

        public void RB_Delete_Fixup(ref TreeNode root, TreeNode x)
        {
            while (x != root && x.Color == Colors.Black)
            {
                if (x == x.Parent.Left)//??????????
                {
                    TreeNode w = x.Parent.Right;

                    if (w.Color == Colors.Red) // Случай 1
                    {
                        w.Color = Colors.Black;
                        x.Parent.Color = Colors.Red;
                        Left_Rotate(ref root, x.Parent);
                        w = x.Parent.Right;
                    }
                    if (w.Left.Color == Colors.Black && w.Right.Color == Colors.Black) // Случай 2
                    {
                        w.Color = Colors.Red;
                        x = x.Parent;
                    }
                    else
                    {

                        if (w.Right.Color == Colors.Black) // Случай 3
                        {
                            w.Left.Color = Colors.Black;
                            w.Color = Colors.Red;
                            Right_Rotate(ref root, w);
                            w = x.Parent.Right;
                        }
                        w.Color = x.Parent.Color; // Случай 4
                        x.Parent.Color = Colors.Black;
                        w.Right.Color = Colors.Black;
                        Left_Rotate(ref root, x.Parent);
                        x = root;
                    }
                }
                else
                {
                    TreeNode w = x.Parent.Left;

                    if (w.Color == Colors.Red) // Случай 1
                    {
                        w.Color = Colors.Black;
                        x.Parent.Color = Colors.Red;
                        Right_Rotate(ref root, x.Parent);
                        w = x.Parent.Left;
                    }
                    if (w.Right.Color == Colors.Black && w.Left.Color == Colors.Black) // Случай 2
                    {
                        w.Color = Colors.Red;
                        x = x.Parent;
                    }
                    else
                    {
                        if (w.Left.Color == Colors.Black) // Случай 3
                        {
                            w.Right.Color = Colors.Black;
                            w.Color = Colors.Red;
                            Left_Rotate(ref root, w);
                            w = x.Parent.Left;
                        }
                        w.Color = x.Parent.Color; // Случай 4
                        x.Parent.Color = Colors.Black;
                        w.Left.Color = Colors.Black;
                        Right_Rotate(ref root, x.Parent);
                        x = root;
                    }
                }
            }
            x.Color = Colors.Black;
        }

        public void DeletePamyat(ref TreeNode root)
        {
            if (root != T_NULL)
            {
                if (root.Left != T_NULL)
                    DeletePamyat(ref root.Left);
                if (root.Right != T_NULL)
                    DeletePamyat(ref root.Right);
                root = T_NULL;
            }
        }

        public void InsertNew(ref TreeNode root, TreeNode node, SuperKey k)
        {
            node.Right = T_NULL;
            node.Left = T_NULL;
            node.Parent = T_NULL;
            node.Key = k;
            RB_Insert(ref root, node);
        }

        public void RB_Insert(ref TreeNode root, TreeNode z)
        {
            if (Poisk_RB(root, z.Key) != true)
            {
                TreeNode y = T_NULL;
                TreeNode x = root;
                while (x != T_NULL)
                {
                    y = x;
                    if (IsComp(z.Key, x.Key) == true)
                        x = x.Left;
                    else
                        x = x.Right;
                }
                z.Parent = y;
                if (y == T_NULL)
                    root = z;
                else if (IsComp(z.Key, y.Key) == true)
                    y.Left = z;
                else
                    y.Right = z;
                z.Left = T_NULL;
                z.Right = T_NULL;
                z.Color = Colors.Red;
                RB_Insert_Fixup(ref root, z);
            }
        }

        public void RB_Delete(ref TreeNode root, TreeNode z)
        {
            TreeNode x;
            TreeNode y = z;
            Colors y_original_color = y.Color;
            if (root != T_NULL)
            {
                if (z.Left == T_NULL)
                {
                    x = z.Right;
                    RB_Transplant(ref root, z, z.Right);
                }
                else if (z.Right == T_NULL)
                {
                    x = z.Left;
                    RB_Transplant(ref root, z, z.Left);
                }
                else
                {
                    y = Tree_Max_Left(z);
                    y_original_color = y.Color;
                    x = y.Left;
                    if (y.Parent == z)
                        x.Parent = y;
                    else
                    {
                        RB_Transplant(ref root, y, y.Left);
                        y.Left = z.Left;
                        y.Left.Parent = y;
                    }
                    RB_Transplant(ref root, z, y);
                    y.Right = z.Right;
                    y.Right.Parent = y;
                    y.Color = z.Color;
                }

                if (y_original_color == Colors.Black)
                    RB_Delete_Fixup(ref root, x);
            }
        }

        public Random random = new Random();

        public string[] surnames = { "Антон", "Бухалихин Богдан Владиславович", "Бухалихин Богдан Владиславович", "Антошкин Богдан Антонович", "Brown", "Davis", "Miller", "Wilson", "Moore", "Taylor" };

        public SuperKey GetRandomData()
        {
            string surname = surnames[random.Next(surnames.Length)];
            int index = random.Next(1000, 9999);

            return new SuperKey { Key = surname, Index = index };
        }

        public void CreateTree(ref TreeNode root, TreeNode[] rootArray, int count)
        {
            for (int i = 0; i < count; i++)
            {
                SuperKey data = GetRandomData();
                TreeNode node = new TreeNode();
                InsertNew(ref root, node, data);
                rootArray[i] = node;
            }
        }
    }
}
