using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Указатели_для_курсовой
{
    internal class LinkedList
    {

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
                    while (curr != null && curr.next != null)
                    {
                        if (curr.next.data == a && head == curr)
                        {
                            while (curr.next.data == a && head == curr)
                            {
                                head = curr.next;
                                Node deleting = curr;
                                curr = head;
                                deleting = null;
                                if (curr.next == null) break;
                            }
                        }
                        if (curr.next.next == null) break;

                        if (curr.next.next.data == a && curr.next.next != null)
                        {
                            Node deleting = curr.next;
                            curr.next = deleting.next;
                            deleting = null;
                            if (curr.next.next == null) break;
                        }
                        else curr = curr.next;
                    }
                }
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
        }
}
