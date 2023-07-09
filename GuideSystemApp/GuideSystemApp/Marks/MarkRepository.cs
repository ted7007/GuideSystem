using System.Text;
using GuideSystemApp.Marks.AvlTree;
using GuideSystemApp.Marks.Hashtable;

namespace GuideSystemApp.Marks;
/// <summary>
/// Репозиторий для работы с сущностью Оценки
/// </summary>
public class MarkRepository
{
    public Mark[] MarkArray { get; set; }

    public HashTable HashTable { get; set; }

    public AVLTree<KeyValue> MarkIndexByPassport { get; set; }

    public AVLTree<KeyValue> MarkIndexByDiscipline { get; set; }

    public AVLTree<KeyValue> MarkIndexByValue { get; set; }

    public AVLTree<KeyValue> MarkIndexByDate { get; set; }

    public MarkRepository(string path)
    {
        
    }

    public MarkRepository()
    {
        MarkArray = new Mark[0];
        HashTable = new HashTable(100);
        MarkIndexByPassport = new AVLTree<KeyValue>();
        MarkIndexByDiscipline = new AVLTree<KeyValue>();
        MarkIndexByValue = new AVLTree<KeyValue>();
        MarkIndexByDate = new AVLTree<KeyValue>();
    }

    public void WriteToFile(string path)
    {

        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine(MarkArray.Length);

            foreach (var mark in MarkArray)
            {
                writer.WriteLine($"{mark.PassportSerialNumber}\\{mark.Discipline}\\{mark.Date}\\{(int)mark.Value}");
            }
        }
    }
    
    public void ReadFromFile(string path)
    {
        using (StreamReader reader = new StreamReader(path))
        {

            int count = int.Parse(reader.ReadLine()); // Преобразование строку в число

            MarkArray = new Mark[count]; // Создаём массив с размером count

            // Записываем данные в массив
            for (int i = 0; i < count; i++)
            {
                string[] markStr = reader.ReadLine().Split('\\');
                Mark mark = new Mark()
                {
                    PassportSerialNumber = markStr[0],
                    Discipline = markStr[1],
                    Date = markStr[2],
                    Value = (MarkEnum)int.Parse(markStr[3])
                };
                MarkArray[i] = mark;
            }
        }

        CreateIndexes();
    }

    private void CreateIndexes()
    {
        MarkIndexByPassport = new AVLTree<KeyValue>();
        MarkIndexByDiscipline = new AVLTree<KeyValue>();
        MarkIndexByValue = new AVLTree<KeyValue>();
        MarkIndexByDate = new AVLTree<KeyValue>();
        for (int i = 0; i < MarkArray.Length; i++)
        {
            MarkIndexByPassport.Add(new KeyValue() {Key = MarkArray[i].PassportSerialNumber, Value = i});
            MarkIndexByDiscipline.Add(new KeyValue() {Key = MarkArray[i].Discipline, Value = i});
            MarkIndexByDate.Add(new KeyValue() {Key = MarkArray[i].Date, Value = i});
            MarkIndexByValue.Add(new KeyValue() {Key = ((int)MarkArray[i].Value).ToString(), Value = i});
            HashTable.Insert(
                MarkArray[i].PassportSerialNumber + MarkArray[i].Discipline + MarkArray[i].Date +
                ((int)MarkArray[i].Value), i);
        }
    }

    private void AddToIndexes(int i)
    {
        MarkIndexByPassport.Add(new KeyValue() {Key = MarkArray[i].PassportSerialNumber, Value = i});
        MarkIndexByDiscipline.Add(new KeyValue() {Key = MarkArray[i].Discipline, Value = i});
        MarkIndexByDate.Add(new KeyValue() {Key = MarkArray[i].Date, Value = i});
        MarkIndexByValue.Add(new KeyValue() {Key = ((int)MarkArray[i].Value).ToString(), Value = i});
        HashTable.Insert(
            MarkArray[i].PassportSerialNumber + MarkArray[i].Discipline + MarkArray[i].Date +
            ((int)MarkArray[i].Value), i);
    }

    private void RemoveFromIndexes(int i)
    {
        MarkIndexByPassport.Remove(new KeyValue() {Key = MarkArray[i].PassportSerialNumber, Value = i});
        MarkIndexByDiscipline.Remove(new KeyValue() {Key = MarkArray[i].Discipline, Value = i});
        MarkIndexByDate.Remove(new KeyValue() {Key = MarkArray[i].Date, Value = i});
        MarkIndexByValue.Remove(new KeyValue() {Key = ((int)MarkArray[i].Value).ToString(), Value = i});
        HashTable.Remove(
            MarkArray[i].PassportSerialNumber + MarkArray[i].Discipline + MarkArray[i].Date +
            ((int)MarkArray[i].Value));
    }
    private int? Find(Mark mark)
    {
        var res = HashTable.Find(mark.PassportSerialNumber + mark.Discipline + mark.Date +
                              ((int)mark.Value));
        return res;
    }

    public Mark FindUnique(Mark mark)
    {
        var res = HashTable.Find(mark.PassportSerialNumber + mark.Discipline + mark.Date +
                                 ((int)mark.Value));
        return MarkArray[(int)res];
    }
    
    public void Add(Mark mark)
    {
        Mark[] tmp = MarkArray;
        MarkArray = new Mark[MarkArray.Length+1];
        for (int i = 0; i < tmp.Length; i++)
        {
            MarkArray[i] = tmp[i];
        }

        int newNum = tmp.Length;
        MarkArray[newNum] = mark;
        AddToIndexes(newNum);
    }

    public void Delete(Mark mark)
    {
        var res = Find(mark);
        if(res is null)
            return;
        var removeItem = MarkArray[(int)res];
        RemoveFromIndexes((int)res);
        var tmp = MarkArray;
        MarkArray = new Mark[tmp.Length-1];
        int count = 0;
        foreach (var i in MarkArray)
        {
            if (i == removeItem)
                continue;
            MarkArray[count] = i;
            count++;
        }
    }

    public List<Mark> FindByKey(string key, IndexType type)
    {
        AVLNode<KeyValue> res = null;
        switch (type)
        {
            case IndexType.Passport:
                res = MarkIndexByPassport.Find(new KeyValue() { Key = key });
                break;
            case IndexType.Discipline:
                res = MarkIndexByDiscipline.Find(new KeyValue() { Key = key });
                break;
            case IndexType.Date:
                res = MarkIndexByDate.Find(new KeyValue() { Key = key });
                break;
            case IndexType.Value:
                res = MarkIndexByValue.Find(new KeyValue() { Key = key });
                break;
            default:
                return null;
        }
        
        var marks = new List<Mark>();
        var head = res.List.head;
        if (head == null)
            return marks;
        marks.Add(MarkArray[head.Data.Value]);
        head = head.Next;
        while (head != head)
        {
            marks.Add(MarkArray[head.Data.Value]);
            head = head.Next;
        }

        return marks;
    }

    public string GetIndexView(IndexType type)
    {
        switch (type)
        {
            case IndexType.Passport:
                return MarkIndexByPassport.GetView();
            case IndexType.Discipline:
                return MarkIndexByDiscipline.GetView();
            case IndexType.Date:
                return MarkIndexByDate.GetView();
            case IndexType.Value:
                return MarkIndexByValue.GetView();
        }

        return "Ошибка";
    }

    public void Edit(Mark oldValue, Mark newValue)
    {
        var num = Find(oldValue);
        if(num == null)
            return;
        RemoveFromIndexes((int)num);
        MarkArray[(int)num] = newValue;
        AddToIndexes((int)num);
    }

    public Mark[] GetAll()
    {
        return MarkArray;
    }
    
    public string GetUniqueView()
    {
        return HashTable.GetView();
    }
}