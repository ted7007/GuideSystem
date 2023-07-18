using System.Text;
using GuideSystemApp.Marks.AvlTree;
using GuideSystemApp.Marks.Hashtable;

namespace GuideSystemApp.Marks;
/// <summary>
/// Репозиторий для работы с сущностью Оценки
/// </summary>
public class MarkRepository
{
    public List<Mark> MarkArray { get; set; }

    public HashTable HashTable { get; set; }

    public AVLTree<KeyValue> MarkIndexByPassport { get; set; }

    public AVLTree<KeyValue> MarkIndexByDiscipline { get; set; }

    public AVLTree<KeyValue> MarkIndexByValue { get; set; }

    public AVLTree<KeyValue> MarkIndexByDate { get; set; }

    public AVLTree<KeyValue> MarkindexByKafedra { get; set; }

    public MarkRepository(int startCount)
    {
        MarkArray = new List<Mark>();
        HashTable = new HashTable(startCount);
        MarkIndexByPassport = new AVLTree<KeyValue>();
        MarkIndexByDiscipline = new AVLTree<KeyValue>();
        MarkIndexByValue = new AVLTree<KeyValue>();
        MarkIndexByDate = new AVLTree<KeyValue>();
        MarkindexByKafedra = new AVLTree<KeyValue>();
    }

    public void WriteToFile(string path)
    {

        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine(MarkArray.Count);

            foreach (var mark in MarkArray)
            {
                writer.WriteLine($"{mark.PassportSerialNumber}/{mark.Discipline}/{mark.Date}/{(int)mark.Value}/{mark.Kafedra}");
            }
        }
    }
    
    public void ReadFromFile(string path)
    {
        using (StreamReader reader = new StreamReader(path))
        {

            int count = int.Parse(reader.ReadLine()); // Преобразование строку в число

            MarkArray = new List<Mark>(); // Создаём массив с размером count

            // Записываем данные в массив
            for (int i = 0; i < count; i++)
            {
                string[] markStr = reader.ReadLine().Split('/');
                Mark mark = new Mark()
                {
                    PassportSerialNumber = markStr[0],
                    Discipline = markStr[1],
                    Date = markStr[2],
                    Value = (MarkEnum)int.Parse(markStr[3]),
                    Kafedra = markStr[4]
                };
                MarkArray.Add(mark);
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
        MarkindexByKafedra = new AVLTree<KeyValue>();
        for (int i = 0; i < MarkArray.Count; i++)
        {
            MarkIndexByPassport.Add(new KeyValue() {Key = MarkArray[i].PassportSerialNumber, Value = i});
            MarkIndexByDiscipline.Add(new KeyValue() {Key = MarkArray[i].Discipline, Value = i});
            MarkIndexByDate.Add(new KeyValue() {Key = MarkArray[i].Date, Value = i});
            MarkIndexByValue.Add(new KeyValue() {Key = ((int)MarkArray[i].Value).ToString(), Value = i});
            MarkindexByKafedra.Add(new KeyValue() {Key = MarkArray[i].Kafedra, Value = i});
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
        MarkindexByKafedra.Add(new KeyValue() {Key = MarkArray[i].Kafedra, Value = i });
        HashTable.Insert(
            MarkArray[i].PassportSerialNumber + MarkArray[i].Discipline + MarkArray[i].Date +
            ((int)MarkArray[i].Value), i);
    }

    private void RemoveFromIndexes(int i)
    {
        var deleteElem = MarkArray[i];
        MarkIndexByPassport.Remove(new KeyValue() {Key = MarkArray[i].PassportSerialNumber, Value = i},
            (elem => elem.Key == deleteElem.PassportSerialNumber && elem.Value == i));
        MarkIndexByDiscipline.Remove(new KeyValue() {Key = MarkArray[i].Discipline, Value = i},
            (elem => elem.Key == deleteElem.Discipline && elem.Value == i));
        MarkIndexByDate.Remove(new KeyValue() {Key = MarkArray[i].Date, Value = i},
            (elem => elem.Key == deleteElem.Date && elem.Value == i));
        MarkIndexByValue.Remove(new KeyValue() {Key = ((int)MarkArray[i].Value).ToString(), Value = i},
            (elem => elem.Key == ((int)deleteElem.Value).ToString() && elem.Value == i));
        MarkIndexByValue.Remove(new KeyValue() {Key = MarkArray[i].Kafedra, Value = i},
            (elem => elem.Key == deleteElem.Kafedra && elem.Value == i));
        HashTable.Remove(
            MarkArray[i].PassportSerialNumber + MarkArray[i].Discipline + MarkArray[i].Date +
            ((int)MarkArray[i].Value));
    }
    private int? Find(Mark mark)
    {
        var res = HashTable.Find(mark.PassportSerialNumber + mark.Discipline + mark.Date +
                              ((int)mark.Value));
        return res.node.Value;
    }

    public Comparisons<Mark> FindUnique(Mark mark)
    {
        var res = HashTable.Find(mark.PassportSerialNumber + mark.Discipline + mark.Date +
                                 ((int)mark.Value));
        return new Comparisons<Mark>(MarkArray[(int)res.node.Value], res.k);
    }
    
    public void Add(Mark mark)
    {
        MarkArray.Add(mark);
        // var tmp = MarkArray;
        // MarkArray = new List<Mark>(MarkArray.Count+1);
        // for (int i = 0; i < tmp.Count; i++)
        // {
        //     MarkArray[i] = tmp[i];
        // }
        //
        // int newNum = tmp.Count;
        // MarkArray[newNum] = mark;
        AddToIndexes(MarkArray.Count-1);
    }

    public void Delete(Mark mark)
    {
        var res = Find(mark);
        if(res is null)
            return;
        var num = (int)res;
        RemoveFromIndexes(num);
        if (num == MarkArray.Count - 1)
        {
            MarkArray.Remove(MarkArray[num]);
            return;
        }

        MarkArray[num] = new Mark()
        {
            Date = MarkArray[^1].Date, Discipline = MarkArray[^1].Discipline, Value = MarkArray[^1].Value,
            PassportSerialNumber = MarkArray[^1].PassportSerialNumber
        };
        MarkArray.Remove(MarkArray[^1]);
        ChangeIndexForMark(MarkArray[num], MarkArray.Count, num);
        // var removeItem = MarkArray[(int)res];
        // RemoveFromIndexes((int)res);
        // var tmp = MarkArray;
        // MarkArray = new Mark[tmp.Length-1];
        // int count = 0;
        // foreach (var i in MarkArray)
        // {
        //     if (i == removeItem)
        //         continue;
        //     MarkArray[count] = i;
        //     count++;
        // }
    }

    private void ChangeIndexForMark(Mark mark, int oldNum, int newNum)
    {
        MarkIndexByPassport.EditValue(new KeyValue() {Key = mark.PassportSerialNumber, Value = oldNum}, 
            key => key.Value == oldNum, 
            new KeyValue() { Key = mark.PassportSerialNumber, Value = newNum});
        MarkIndexByDiscipline.EditValue(new KeyValue() {Key = mark.Discipline, Value = oldNum}, 
            key => key.Value == oldNum, 
            new KeyValue() { Key = mark.Discipline, Value = newNum});
        MarkIndexByDate.EditValue(new KeyValue() {Key = mark.Date, Value = oldNum}, 
            key => key.Value == oldNum, 
            new KeyValue() { Key = mark.Date, Value = newNum});
        MarkIndexByValue.EditValue(new KeyValue() {Key = ((int)mark.Value).ToString(), Value = oldNum}, 
            key => key.Value == oldNum, 
            new KeyValue() { Key = ((int)mark.Value).ToString(), Value = newNum});
        HashTable.Edit(
            mark.PassportSerialNumber + mark.Discipline + mark.Date +
            ((int)mark.Value), newNum);
    }

    public Comparisons<List<Mark>> FindByKey(string key, IndexType type)
    {
        Comparisons<AVLNode<KeyValue>> res = null;
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
            case IndexType.Kafedra:
                res = MarkindexByKafedra.Find(new KeyValue() {Key=key});
                break;
            default:
                return null;
        }
        
        var marks = new List<Mark>();
        if (res == null)
            return null;
        var head = res.node.List.head;
        if (head == null)
            return null;
        marks.Add(MarkArray[head.Data.Value]);
        var curNode = head.Next;
        while (curNode != head)
        {
            res.k++;
            marks.Add(MarkArray[head.Data.Value]);
            curNode = curNode.Next;
        }

        return new Comparisons<List<Mark>>(marks, res.k);
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
            case IndexType.Kafedra:
                return MarkindexByKafedra.GetView();
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

    public IEnumerable<Mark> GetAll()
    {
        var marks = new List<Mark>();
        for (int i = 0; i < MarkArray.Count; i++)
        {
            marks.Add(MarkArray[i]);
            marks[i].Index = i;
        }

        return marks;
    }
    
    public string GetUniqueView()
    {
        return HashTable.GetView();
    }
}