using System;

namespace КЧ_дерево_для_курсовой
{
    internal class Program
    {
        static void Main()
        {
            TreeNode root = null; // Инициализируем переменную root значением null
            RB_Tree rbTree = new RB_Tree();
            rbTree.Inicializaciya(ref root);

            // Создание новой вершины вручную
            TreeNode newNode1 = new TreeNode();
            TreeNode newNode2 = new TreeNode();
            TreeNode newNode3 = new TreeNode();
            TreeNode newNode4 = new TreeNode();
            TreeNode newNode5 = new TreeNode();
            TreeNode newNode6 = new TreeNode();
            TreeNode newNode7 = new TreeNode();
            TreeNode newNode8 = new TreeNode();
            TreeNode newNode9 = new TreeNode();
            TreeNode newNode10 = new TreeNode();
            TreeNode newNode11 = new TreeNode();
            TreeNode newNode12 = new TreeNode();

            newNode1.Key = new SuperKey { Surname = "Бухалихин Богдан Владиславович", Index = 1 };
            newNode2.Key = new SuperKey { Surname = "Антошкин Богдан Владиславович", Index = 3 };
            newNode3.Key = new SuperKey { Surname = "Папичев Богдан Владиславович", Index = 4 };
            newNode4.Key = new SuperKey { Surname = "Инвалидов Богдан Владиславович", Index = 12 };
            newNode5.Key = new SuperKey { Surname = "Бухалихин Антон Владиславович", Index = 42 };
            newNode6.Key = new SuperKey { Surname = "Картошкин Богдан Владиславович", Index = 23 };
            newNode7.Key = new SuperKey { Surname = "Рудь Богдан Игоревич", Index = 55 };
            newNode8.Key = new SuperKey { Surname = "Келтузадов Богдан Владиславович", Index = 14 };
            newNode9.Key = new SuperKey { Surname = "Бухалихин Игорь Владиславович", Index = 32 };
            newNode10.Key = new SuperKey { Surname = "Антошкин Богдан Владиславович", Index = 56 };
            newNode11.Key = new SuperKey { Surname = "Василисков Богдан Владиславович", Index = 76 };
            newNode12.Key = new SuperKey { Surname = "Инвалидов Богдан Владиславович", Index = 123 };

            // Вставка новой вершины в дерево
            rbTree.RB_Insert(ref root, newNode1);
            rbTree.RB_Insert(ref root, newNode2);
            rbTree.RB_Insert(ref root, newNode3);
            rbTree.RB_Insert(ref root, newNode4);
            rbTree.RB_Insert(ref root, newNode5);
            rbTree.RB_Insert(ref root, newNode6);
            rbTree.RB_Insert(ref root, newNode7);
            rbTree.RB_Insert(ref root, newNode8);
            rbTree.RB_Insert(ref root, newNode9);
            rbTree.RB_Insert(ref root, newNode10);
            rbTree.RB_Insert(ref root, newNode11);
            rbTree.RB_Insert(ref root, newNode12);

            // Вывод дерева после вставки новой вершины
            rbTree.Print_RB(root, 0);
            Console.WriteLine();
            rbTree.Print_Count(root);
        }
    }
}