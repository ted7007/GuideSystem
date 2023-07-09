using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Переделанные_хэш_таблицы__под_курсач_
{
    internal class HashTable
    {
        private Item[] items;
        private int size;
        private int count;
        private int initialSize;
        private double loadFactorThreshold = 0.75; // Пороговое значение заполненности для увеличения и уменьшения размера

        // private int hash;

        public HashTable(int size) //1.Конструктор
        {

            items = new Item[size];
            this.size = size;
            count = 0;
            initialSize = size;

            for (int i = 0; i < size; i++)
            {
                items[i] = new Item();
            }

        }

        public int FirstHash(Student student) //2.Первая хэш функция
        {
            string mass = "";
            int result = 0;
            string temp = "";
            mass = student.Passport;



            for (int i = 0; i < mass.Length; i++)
            {
                temp += $"{(int)mass[i]}";
            }

            if (temp.Length % 2 != 0)
            {
                temp += 0;
            }

            for (int i = 0; i < temp.Length; i += 2)
            {
                String temp2 = $"{Char.GetNumericValue(temp[i])}{Char.GetNumericValue(temp[i + 1])}";
                result += int.Parse(temp2);
            }

            // hash = result % size;
            return result % size;

        }

        public int SecondHash(int hash, int j) //3.вторая хэш функция (int j)номер попытки вставки
        {
            int k = 1;
            int m = (hash + j * k) % size;
            return m;

        }

        private int ResolveCollisionLinearForAdd(Student student, int index)
        {
            int j = 1;
            int startIndex = index;
            bool flag = false;
            int current_index = index;
            int cur;
            while ((items[index].status != 0 && index != startIndex) || j < size)
            {

                if (items[index].status == 2 && flag == false)
                {
                    current_index = index;
                    flag = true;
                }//запомним первую попавшуюся ячейку со статусом 2

                cur = SecondHash(index, j);
                if (Check(student, cur) == true) return -1;
                if (items[cur].status == 0) return cur;
                if (cur == startIndex) return current_index;
                j++;
            }

            return index;
        }
        public void Add(Student student) // 4. Добавление
        {
            int index = FirstHash(student);
            int current;
            if (items[index].status == 0) // Проверяем, что ячейка пустая 
            {
                Item item = new Item(student, index);
                items[index] = item;
                count++;
                items[index].status = 1; // Устанавливаем статус как занятую ячейку                                   
            }

            else
            {
                if (Check(student, index) == true) return;
                current = ResolveCollisionLinearForAdd(student, index);
                if (current == -1) return;
                if (items[current].status == 0 || items[current].status == 2) // Проверяем, что ячейка пустая или удаленная
                {
                    Item item = new Item(student, index);
                    items[current] = item;
                    count++;
                    items[current].status = 1; // Устанавливаем статус как занятую ячейку
                }
            }
            CheckLoadFactor();
        }

        public void Delete(Student student)
        {
            int index = FirstHash(student);

            if (Check(student, index) == true)
            {

                items[index] = new Item(); // Обновляем элемент
                items[index].status = 2;// Устанавливаем статус как удаленную ячейку
                count--; // Обновляем значение count
            }
            else
            {
                int cur = (Search(student));
                if (cur != -1)
                {
                    items[cur] = new Item(); // Обновляем элемент
                    items[cur].status = 2;// Устанавливаем статус как удаленную ячейку
                    count--; // Обновляем значение count
                }

            }

            CheckLoadFactor();
        }

        public int Search(Student student3)
        {
            int index = FirstHash(student3);
            int cur = index;
            int j = 1;
            while (j <= size)
            {
                if (items[cur].status == 1 &&
                    items[cur].student.FIO == student3.FIO &&
                    items[cur].student.Group == student3.Group &&
                    items[cur].student.Passport == student3.Passport &&
                    items[cur].student.AdmissionDate == student3.AdmissionDate)
                {
                    return cur;
                }
                cur = SecondHash(index, j);
                j++;
            }

            return -1;
        }

        public bool Check(Student student3, int index)
        {
            if (items[index].status == 1 &&
                    items[index].student.FIO == student3.FIO &&
                    items[index].student.Group == student3.Group &&
                    items[index].student.Passport == student3.Passport &&
                    items[index].student.AdmissionDate == student3.AdmissionDate)

            {
                return true;
            }
            else return false;
        }
        public void Print() //7.печать
        {
            for (int j = 0; j < size; j++)
            {
                if (items[j].status == 1 || items[j].status == 2 || items[j].status == 0)
                {
                    Console.WriteLine
                        ($"{j}|{items[j].student.FIO}" +
                        $" {items[j].student.Group}" +
                        $" {items[j].student.Passport}" +
                        $" {items[j].student.AdmissionDate}" +
                        $" " +
                        $"status = {items[j].status}" +
                        $" |hash = {items[j].hash}");
                }

            }
            Console.WriteLine(size);
        }

        private int ResolveCollisionLinearForAddNew(Student student, int index, Item[] newItems)
        {
            int j = 1;
            int startIndex = index;
            bool flag = false;
            int current_index = index;
            int cur;
            while ((newItems[index].status != 0 && index != startIndex) || j < size)
            {

                if (newItems[index].status == 2 && flag == false)
                {
                    current_index = index;
                    flag = true;
                }//запомним первую попавшуюся ячейку со статусом 2

                cur = SecondHash(index, j);
                if (newItems[cur].status == 0) return cur;
                if (cur == startIndex) return current_index;
                j++;
            }

            return index;
        }
        private void IncreaseSize() // Метод для увеличения размера хеш-таблицы
        {
            int newSize = size * 2; // Увеличиваем размер в два раза
            Item[] newItems = new Item[newSize];
            int k = size;
            size = newSize;

            for (int i = 0; i < newSize; i++)
            {
                newItems[i] = new Item();
            }

            int newCount = 0;

            for (int i = 0; i < k; i++)
            {
                if (items[i].status == 1)
                {

                    int index = FirstHash(items[i].student);

                    if (newItems[index].status == 0)
                    {
                        newItems[index] = items[i];
                        newItems[index].hash = index;
                        newCount++;
                    }
                    else
                    {

                        int cur = ResolveCollisionLinearForAddNew(items[i].student, index, newItems);
                        newItems[cur] = items[i];
                        newItems[cur].hash = index;
                        newCount++;

                    }


                }
            }

            items = newItems;
            count = newCount;
        }

        private void DecreaseSize() // Метод для уменьшения размера хеш-таблицы
        {
            int newSize = Math.Max(size / 2, initialSize); // Уменьшаем размер до половины текущего размера, но не меньше исходного размера
            Item[] newItems = new Item[newSize];
            int k = size;
            size = newSize;

            for (int i = 0; i < newSize; i++)
            {
                newItems[i] = new Item();
            }

            int newCount = 0;

            for (int i = 0; i < k; i++)
            {
                if (items[i].status == 1)
                {

                    int index = FirstHash(items[i].student);

                    if (newItems[index].status == 0)
                    {
                        newItems[index] = items[i];
                        newItems[index].hash = index;
                        newCount++;
                    }
                    else
                    {

                        int cur = ResolveCollisionLinearForAddNew(items[i].student, index, newItems);
                        newItems[cur] = items[i];
                        newItems[cur].hash = index;
                        newCount++;

                    }


                }
            }

            items = newItems;
            count = newCount;
        }

        public void CheckLoadFactor() // Метод для проверки и выполнения увеличения или уменьшения размера хеш-таблицы
        {
            double loadFactor = (double)count / size;

            if (loadFactor >= loadFactorThreshold) // Заполнение более или равно пороговому значению
            {
                IncreaseSize();
            }
            else if (loadFactor < loadFactorThreshold / 4 && size > initialSize) // Заполнение менее четверти и размер больше исходного размера
            {
                DecreaseSize();
            }
        }
    }
}
