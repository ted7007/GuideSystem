using GuideSystemApp.Student.RB_Tree;
using GuideSystemApp.Student.HashTable;
using GuideSystemApp.Student;
using GuideSystemApp.Student.List;

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

newNode1.Key = new SuperKey ("Бухалихин Богдан Владиславович", 1);
newNode2.Key = new SuperKey ("Антошкин Богдан Владиславович", 3) ;
newNode3.Key = new SuperKey ("Папичев Богдан Владиславович", 4);
newNode4.Key = new SuperKey ("Инвалидов Богдан Владиславович", 12);
newNode5.Key = new SuperKey ("Бухалихин Антон Владиславович", 42);
newNode6.Key = new SuperKey ("Картошкин Богдан Владиславович", 23);
newNode7.Key = new SuperKey ("Рудь Богдан Игоревич", 55);
newNode8.Key = new SuperKey ("Келтузадов Богдан Владиславович", 14);
newNode9.Key = new SuperKey ("Бухалихин Игорь Владиславович", 32);
newNode10.Key = new SuperKey ("Антошкин Богдан Владиславович", 56);
newNode11.Key = new SuperKey ("Василисков Богдан Владиславович", 76);
newNode12.Key = new SuperKey ("Инвалидов Богдан Владиславович", 123);

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


var table = new HashTable(10);

table.Add(new SuperKey("Антонов Курица Толстый", 5));
table.Add(new SuperKey("Инвалидов Богдан Владиславович", 123));
table.Add(new SuperKey("Бухалихин Игорь Владиславович", 32));
table.Add(new SuperKey("Бухалихин Антон Владиславович", 42));
table.Add(new SuperKey("Папичев Богдан Владиславович", 4));
table.Add(new SuperKey("Василисков Богдан Владиславович", 76));
table.Add(new SuperKey("Бухалихин Богдан Владиславович", 1));






table.Print();

LinkedList linkedList = new LinkedList();

Node head1 = new Node();
Node head2 = new Node();
Node head3 = new Node();

// Инициализация списков
linkedList.Inicializaciya(ref head1);
linkedList.Inicializaciya(ref head2);
linkedList.Inicializaciya(ref head3);

// Добавление элементов в список
linkedList.AddNode(ref head1, 1);
linkedList.AddNode(ref head1, 2);
linkedList.AddNode(ref head1, 3);
linkedList.AddNode(ref head1, 1);

linkedList.AddNode(ref head2, 2);
linkedList.AddNode(ref head2, 3);
linkedList.AddNode(ref head2, 4);
linkedList.AddNode(ref head2, 2);

// Удаление элементов перед заданным элементом
linkedList.DeletePeredElementom(ref head1, 4);

// Печать списков
Console.WriteLine("Список 1:");
linkedList.PrintList(head1);

Console.WriteLine("Список 2:");
linkedList.PrintList(head2);

Console.WriteLine("Список 3 (разность):");
// Разность списков
linkedList.Raznost(head1, head2, ref head3);
linkedList.PrintList(head3);

// Поиск элемента в списке
int searchNumber = 3;
bool found = linkedList.PoiskChisla(head1, searchNumber);
Console.WriteLine("Число {0} найдено в списке 1: {1}", searchNumber, found);

searchNumber = 5;
found = linkedList.PoiskChisla(head2, searchNumber);
Console.WriteLine("Число {0} найдено в списке 2: {1}", searchNumber, found);

// Освобождение памяти (удаление списков)
linkedList.DeletePamyat(ref head1);
linkedList.DeletePamyat(ref head2);
linkedList.DeletePamyat(ref head3);