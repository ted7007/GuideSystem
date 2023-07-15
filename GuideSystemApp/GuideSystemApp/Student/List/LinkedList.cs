using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuideSystemApp.Student.List
{
    public class LinkedList
    {
        public Node head;
        public Node tail;

        // 2. Инициализация списка
        public void Inicializaciya(ref Node head)
            {
                head = null;
            }

        // 3. Освобождение памяти (удаление всего списка)
        public void DeletePamyat(ref Node head)
            {
                if (head != null)
                {
                    Node p = head;
                    while (p.next != null)
                    {
                        Node q = p;
                        p = p.next;
                        q = null;
                    }
                    p = null;
                }
                head = null;
            }

        // 4. Метод добавляющий новый элемент в конец
        public void AddNode(ref Node head, int d)
            {
                Node p = new Node();
                p.data = d;
                p.next = null;

                if (head == null)
                    head = p;
                else
                {
                    Node current = head;
                    while (current.next != null)
                        current = current.next;
                    current.next = p;
                    p.next = null;
                }
            }

        // 5. Метод удаления элемента перед заданным
        public void DeletePeredElementom(ref Node head, int a)
        {
            if (head != null)
            {
                Node curr = head;

                // Удаление узлов в начале списка
                while (curr != null && curr.data == a)
                {
                    Node deleting = curr;
                    curr = curr.next;
                    head = curr;
                    deleting = null;
                }

                // Удаление узлов в середине и конце списка
                Node prev = null;
                while (curr != null)
                {
                    if (curr.data == a)
                    {
                        Node deleting = curr;
                        curr = curr.next;
                        prev.next = curr;
                        deleting = null;
                    }
                    else
                    {
                        prev = curr;
                        curr = curr.next;
                    }
                }
            }
        }
        public void DeleteNode(ref Node head, int index)
        {
            if (head == null)
            {
                return;
            }

            if (head.data == index)
            {
                head = head.next;
                return;
            }

            Node current = head;
            Node previous = null;

            while (current != null)
            {
                if (current.data == index)
                {
                    previous.next = current.next;
                    return;
                }

                previous = current;
                current = current.next;
            }
        }
        public bool Contains(int index)
        {
            Node current = head;
            while (current != null)
            {
                if (current.data == index)
                {
                    return true;
                }
                current = current.next;
            }
            return false;
        }

        // 6. Поиск заданного элемента
        public bool PoiskChisla(Node head, int chislo)
            {
                bool f = false;
                Node current = head;
                if (head != null)
                {
                    while (current.next != null && current.data != chislo)
                    {
                        current = current.next;
                    }
                    if (current.data == chislo) f = true;
                }
                return f;
            }
        public Node PoiskUzla(Node head, int chislo)
        {
            bool f = false;
            Node current = head;
            if (head != null)
            {
                while (current.next != null && current.data != chislo)
                {
                    current = current.next;
                }
                if (current.data == chislo) return current;
            }
            return null;
        }
        // 7. Разность списков
        public void Raznost(Node a, Node b, ref Node c)
            {
                if (a != null)
                {
                    Node curr = a;
                    while (curr != null)
                    {
                        if (PoiskChisla(b, curr.data) == false) AddNode(ref c, curr.data);
                        curr = curr.next;
                    }
                }
            }

        // 8. Печать списка
        public void PrintList(Node head)
            {
                Node current = head;
                while (current != null)
                {
                    Console.Write(current.data + " ");
                    current = current.next;
                }
                Console.WriteLine();
            }

        // 9. Получение всех индексов элементов списка
        public List<int> GetAllIndices()
        {
            List<int> indices = new List<int>();

            Node current = head;
            while (current != null)
            {
                indices.Add(current.data);
                current = current.next;
            }

            return indices;
        }

    }
    }

