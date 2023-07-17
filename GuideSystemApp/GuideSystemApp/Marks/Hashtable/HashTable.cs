﻿using System.Text;

namespace GuideSystemApp.Marks.Hashtable;
/*
Косилов Александр - ХТ - динамическая (статус 0/1), 0/1, 0 - свободно, 1 - занято
 разрешение коллизий - открытая адресация (квадратичный),
  ХФ - середина квадрата
  
Реализовать класс для работы с хеш-таблицей (предметная область -
соответствует заданию 1.3; ключ - составной, вид хеш-таблицы, метод разрешения коллизий, хеш-функция -
см. свой вариант задания).
Методы:
1. Конструктор
2. Первичная хеш-функция
3. Вторичная хеш-функция
4. Добавление
5. Удаление
6. Поиск
7. Печать
8. Деструктор

1. Должна быть возможность первоначального задания размера хеш-таблицы
2. Функционал для разрешения коллизий при добавлении/удалении должен быть отдельно
3. Типы полей должны соответствовать типу данных в предметной области 
(все что является числом, должно храниться в виде числа, а не строки)
 */
public class HashTable
{
 
    /// <summary>
    /// текущие элементы
    /// </summary>
    private int _curCount;
    /// <summary>
    /// максимальный размер таблицы
    /// </summary>
    private int _maxCount;

    /// <summary>
    /// начальное кол-во элементов
    /// </summary>
    private int _startCount;

    private int _k1;

    private int _k2;
    
    /// <summary>
    /// таблица 
    /// </summary>
    private Node[] _table;

    
    public HashTable(int startCount)
    {
        _curCount = 0;
        _maxCount = startCount;
        _table = InitNewTable(_maxCount);
        _startCount = startCount;
    }

    public bool Insert(string key, int value)
    {
        int hash1 = hashFunc(key);
        int startHash = hash1;
        if (_table[hash1].Status == NodeStatus.Taken)
        {
            (bool, int?) res = GetOutCollision(hash1, key);
            if (!res.Item1)
                return res.Item1;
            hash1 = (int)res.Item2;
        }
        
        
        _table[hash1] = new Node(key, value, startHash, startHash != hash1 ? hash1 : null);

        _curCount++;
        ResizeIfNeed();
        return true;
    }

    private void ResizeIfNeed()
    {
        float onePercent = _maxCount / 100f;
        float curPercents = _curCount / onePercent;
        if (curPercents < 25f)
        {
            int newCount = _maxCount / 2;
            if (newCount % 3 == 0)
                newCount--;
            if (newCount < _startCount)
                return;
            Resize(newCount);
        }

        if (curPercents > 75f)
        {
            int newCount = _maxCount * 2;
            if (newCount % 3 == 0)
                newCount++;
            Resize(newCount);
        }
    }

    private void Resize(int newCount)
    {
        

        Node[] tmp = _table;
        _table = InitNewTable(newCount);
        _maxCount = newCount;
        foreach (var item in tmp)
        {
            if (item.Status == NodeStatus.Taken)
            {
                int hash1 = hashFunc(item.Key);
                int startHash = hash1;
                if (_table[hash1].Status == NodeStatus.Taken)
                {
                    (bool, int?) res = GetOutCollision(hash1, item.Key);
                    if (!res.Item1)
                    {
                        Console.WriteLine("что то не так");
                        continue;
                    }

                    hash1 = (int)res.Item2;
                }

                _table[hash1] = new Node(item.Key, item.Value, startHash, startHash != hash1 ? hash1 : null); 
                //Insert(item.Value);
            }
        }
    }
    
    public Comparisons<Node> Find(string key)
    {
        int hash1 = hashFunc(key);
        int hash2 = hash1;
        int j = 0;
        int k = 0;
        while (_table[hash2].Status == NodeStatus.Taken || _table[hash2].Status == NodeStatus.Free && !String.IsNullOrEmpty(_table[hash2].Key))
        {
            k++;
            j++;
            if (_table[hash2].Key == key && _table[hash2].Status == NodeStatus.Taken)
                return new Comparisons<Node>(_table[hash2], k);
            hash2 = GetHash2(hash1, j);
        }

        return null;
    }

    public void Edit(string key, int newValue)
    {
        var node = Find(key).node;
        node.Value = newValue;
    }
    
    // вынести отдельно разрешение коллизий, MoveBack так чтоб последний элемент вставлялся на место удаляемого, также проверять значение первой хф
    public bool Remove(string key)
    {
        int hash1 = hashFunc(key);
        int j = 0;
        if (_table[hash1].Key != key)
        {
            (bool, int?, int) res = GetOutCollisionForRemove(hash1, key);
            if (!res.Item1)
                return res.Item1;
            hash1 = (int)res.Item2;
            j = res.Item3;
        }
        int hash2 = hash1;
        //RemoveCell(hash1);
        MoveBack2(hash1, hash2, j);
        /*while (_table[hash2].Status == NodeStatus.Taken)
        {
            if (_table[hash2].Value.Equals(value))
            {
                MoveBack(hash1, hash2, j);
                _curCount--;
                ResizeIfNeed();
                return true;
                
            }
                
            hash2 = GetHash2(hash1, j);
            j++;
        }*/
                
                

        return false;
    }

    private void RemoveCell(int hash1)
    {
        _table[hash1].Clear();
    }

    private (bool, int?, int) GetOutCollisionForRemove(int hash1, string key)
    {
        int hash2 = hash1;
        int j = 0;
        while (_table[hash2].Status != NodeStatus.Free || _table[hash2].Status == NodeStatus.Free && !String.IsNullOrEmpty(_table[hash2].Key))
        {
            j++;
            if (_table[hash2].Key.Equals(key))
                return (true, hash2, j);
            hash2 = GetHash2(hash1, j);
        }

        return (false, 0, -1);
    }

    private void MoveBack2(int hash, int cur, int j)
    {
        //int nextKey = GetHash2(hash, j);
        //int realCurHash = hashFunc(_table[nextKey].Value.FirstName + _table[nextKey].Value.LastName + _table[nextKey].Value.MiddleName, _table[nextKey].Value.GroupNumber);
        int nextKey = cur;
        int realCurHash = hash;
        int prevKey = cur;
        int lastCollision = hash;
        while (true)
        { 
             j++;
             prevKey = nextKey;
                 Console.WriteLine("tyt");
             nextKey = GetHash2(hash, j);
             if(_table[nextKey].Status == NodeStatus.Free && String.IsNullOrEmpty(_table[nextKey].Key))
                break;
             realCurHash = hashFunc(_table[nextKey].Key);
             if (realCurHash == hash && _table[nextKey].Status == NodeStatus.Taken)
                lastCollision = nextKey;
             
        }


        var tmp = new Node()
            { Hash1 = _table[cur].Hash1, Hash2 = _table[cur].Hash2, Key = _table[cur].Key, Value = _table[cur].Value };
        _table[cur].Set(_table[lastCollision]);
        _table[lastCollision].Set(tmp);
        _table[lastCollision].Clear();
        _curCount--;
    }
    
    private (bool, int?) GetOutCollision(int hash1, string key)
    {
        int hash2 = hash1;
        int j = 0;
        while (_table[hash2].Status == NodeStatus.Taken || _table[hash2].Status == NodeStatus.Free && !String.IsNullOrEmpty(_table[hash2].Key))
        {
            j++;
            if (_table[hash2].Key.Equals(key))
                return (false, 0);
            hash2 = GetHash2(hash1, j);
        }
        
        Console.WriteLine($"Коллизия! Элементы {hash1} и {hash2}.");
        return (true, hash2);
    }
    
    private Node[] InitNewTable(int n)
    {
        var table = new Node[n];
        for (int i = 0; i < n; i++)
        {
            table[i] = new Node();
        }
        return table;
    }

    public string GetView()
    {
        var sb = new StringBuilder();
        sb.Append("-----------------\n");
        Console.WriteLine("");
        for (int i = 0; i < _maxCount; i++)
        {
            var lineToStr = _table[i].Key +" " +_table[i].Value; 
            sb.Append($"[{i}:{_table[i].Status}| h1:{_table[i].Hash1} h2: {_table[i].Hash2}] {lineToStr}\n");
        }
        sb.Append("-----------------\n");
        return sb.ToString();
    }

    private int GetHash2(int hash1, int j)
    {
        return (int)((hash1 + 3 * j) % _maxCount);
    }

    
    private void CalculateCoefficients()
    {
        int prime = FindNextPrime(_maxCount-1); // Находим следующее простое число больше maxCount
        int k = FindCoprime(_maxCount-1, prime); // Ищем число, которое является простым, взаимно простым с maxCount и prime
        // Используем найденное значение k для дальнейшего вычисления хеша
        _k1 = k;
        _k2 = FindNextPrime(k); // Для k2 используем следующее простое число после k1
    }

    private int FindCoprime(int num, int prime)
    {
        int coprime = 2; // Начинаем с 2, проверяем на взаимно простые числа
        while (!IsCoprime(coprime, num) || !IsCoprime(coprime, prime))
        {
            coprime = FindNextPrime(coprime); // Ищем следующее простое число
        }
        return coprime;
    }


    private int FindNextPrime(int num)
    {
        while (true)
        {
            num++;
            if (IsPrime(num))
                return num;
        }
    }

    private bool IsPrime(int num)
    {
        if (num <= 1)
            return false;
        for (int i = 2; i * i <= num; i++)
        {
            if (num % i == 0)
                return false;
        }
        return true;
    }
    
    private bool IsCoprime(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a == 1;
    }

    
    int hashFunc(string key)
    {

        int v = getIntFromStr(key);
        int hash = 0;
        int newNumber = v * v;
        string newNumberStr = newNumber.ToString();
        if (newNumberStr.Length <= 2)
        {
            newNumberStr.Select(number => hash += Int32.Parse(number.ToString()));
            hash = hash % _maxCount;
        }
        else
        {
            int start = newNumberStr.Length / 2;
            hash = (Int32.Parse(newNumberStr[start].ToString()) + Int32.Parse(newNumberStr[start - 1].ToString())) % _maxCount;
        }

        return hash;

    }
    
    int getIntFromStr(string str)
    {
        int res = 0;
        foreach (var c in str)
        {
            res += c;
        }

        return res;
    }

}