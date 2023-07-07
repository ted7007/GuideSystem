using GuideSystemApp.Marks;
using Microsoft.VisualBasic.FileIO;

namespace Lab2_3;
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
    private  int _curCount;
    /// <summary>
    /// максимальный размер таблицы
    /// </summary>
    private int _maxCount;

    /// <summary>
    /// начальное кол-во элементов
    /// </summary>
    private int _startCount;
    
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

    public bool Insert(Mark value)
    {
        int hash1 = hashFunc(value);
        int startHash = hash1;
        if (_table[hash1].Status == NodeStatus.Taken)
        {
            (bool, int?) res = GetOutCollision(hash1, value);
            if (!res.Item1)
                return res.Item1;
            hash1 = (int)res.Item2;
        }
        
        
        _table[hash1] = new Node(value, startHash, startHash != hash1 ? hash1 : null);

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
            if (newCount < _startCount)
                return;
            Resize(newCount);
        }

        if (curPercents > 75f)
        {
            int newCount = _maxCount * 2;
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
                int hash1 = hashFunc(item);
                int startHash = hash1;
                if (_table[hash1].Status == NodeStatus.Taken)
                {
                    (bool, int?) res = GetOutCollision(hash1, item.Value);
                    if (!res.Item1)
                    {
                        Console.WriteLine("что то не так");
                        continue;
                    }

                    hash1 = (int)res.Item2;
                }

                _table[hash1] = new Node(item.Value, startHash, startHash != hash1 ? hash1 : null); 
                Insert(item.Value);
            }
        }
    }
    
    public bool Find(Mark value)
    {
        int hash1 = hashFunc(value);
        int hash2 = hash1;
        int j = 0;
        while (_table[hash2].Status == NodeStatus.Taken || _table[hash2].Status == NodeStatus.Free && _table[hash2].Value != null)
        {
            j++;
            string curFio = _table[hash2].Value.FirstName + _table[hash2].Value.LastName + _table[hash2].Value.MiddleName;
            if (curFio == fio && _table[hash2].Value.GroupNumber == groupNumber)
                return true;
            hash2 = GetHash2(hash1, j);
        }

        return false;
    }
    // вынести отдельно разрешение коллизий, MoveBack так чтоб последний элемент вставлялся на место удаляемого, также проверять значение первой хф
    public bool Remove(Student value)
    {
        int hash1 = hashFunc(value.FirstName+value.LastName+value.MiddleName, value.GroupNumber);
        int j = 0;
        if (_table[hash1].Value != value)
        {
            (bool, int?, int) res = GetOutCollisionForRemove(hash1, value);
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

    private (bool, int?, int) GetOutCollisionForRemove(int hash1, Student value)
    {
        int hash2 = hash1;
        int j = 0;
        while (_table[hash2].Status != NodeStatus.Free || _table[hash2].Status == NodeStatus.Free && _table[hash2].Value != null)
        {
            j++;
            if (_table[hash2].Value.Equals(value))
                return (true, hash2, j);
            hash2 = GetHash2(hash1, j);
        }

        return (false, 0, -1);
    }
    
    private void MoveBack(int hash, int cur, int j)
    {
        j++;
        int nextKey = GetHash2(hash, j);
        int realCurHash = hashFunc(_table[nextKey].Value.FirstName + _table[nextKey].Value.LastName + _table[nextKey].Value.MiddleName, _table[nextKey].Value.GroupNumber);
        
        if (_table[nextKey].Status != NodeStatus.Taken || realCurHash != hash)
        {
            _table[cur].Clear();
            return;
        }
        
        _table[cur] = _table[nextKey];
        MoveBack(hash, nextKey, j);
    }

    private void MoveBack2(int hash, int cur, int j)
    {
        //int nextKey = GetHash2(hash, j);
        //int realCurHash = hashFunc(_table[nextKey].Value.FirstName + _table[nextKey].Value.LastName + _table[nextKey].Value.MiddleName, _table[nextKey].Value.GroupNumber);
        int nextKey = cur;
        int realCurHash = hash;
        int prevKey = cur;
        while ( realCurHash == hash)
        { 
             j++;
             prevKey = nextKey;
             nextKey = GetHash2(hash, j);
             if(_table[nextKey].Status == NodeStatus.Free && _table[nextKey].Value == null)
                 break;
             realCurHash = hashFunc(_table[nextKey].Value.FirstName + _table[nextKey].Value.LastName + _table[nextKey].Value.MiddleName, _table[nextKey].Value.GroupNumber);
        }
        
        _table[cur].Set(_table[prevKey]);
        _table[prevKey].Clear();
    }
    
    private (bool, int?) GetOutCollision(int hash1, Mark value)
    {
        int hash2 = hash1;
        int j = 0;
        while (_table[hash2].Status == NodeStatus.Taken || _table[hash2].Status == NodeStatus.Free && _table[hash2].Value != null)
        {
            j++;
            if (_table[hash2].Value.Equals(value))
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

    public void Print()
    {
        Console.WriteLine("-----------------");
        for (int i = 0; i < _maxCount; i++)
        {
            var lineToStr = _table[i].Value is null ? "" : (_table[i].Value.GetString()); 
            Console.WriteLine($"[{i}:{_table[i].Status}| h1:{_table[i].Hash1} h2: {_table[i].Hash2}] {lineToStr}");
        }

        Console.WriteLine("-----------------");
    }

    private int GetHash2(int hash1, int j)
    {
        int k1 = 3;
        int k2 = 19;
        return (hash1 + j * k1 + j * j * k2) % _maxCount;
    }

    int hashFunc(Mark mark)
    {

        int v = getIntFromStr(fio) + groupNumber;
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