namespace GuideSystemApp.Disciplines;


public class DisciplineRepository
{
    public List<Discipline> DisciplineArray;

    private HachTable table;

    private AVLTree treeDiscipline;
    private AVLTree treeInstitute;
    private AVLTree treeDepartment;
    private AVLTree treeTeacher;
    public DisciplineRepository()
    {
        this.DisciplineArray = new List<Discipline>();
        this.table = new HachTable(20);
        this.treeDiscipline = new AVLTree();
        this.treeInstitute = new AVLTree();
        this.treeTeacher = new AVLTree();
        this.treeDepartment = new AVLTree();

    }
    public void ReadFromFile(string path)
    {
        using (StreamReader reader = new StreamReader(path))
        {

            int count = int.Parse(reader.ReadLine()); // Преобразование строку в число

            // Записываем данные в массив
            for (int i = 0; i < count; i++)
            {
                string[] DisciplineStr = reader.ReadLine().Split('/');
                Discipline discipline = new Discipline(DisciplineStr[0], DisciplineStr[1], DisciplineStr[2], DisciplineStr[3]);
                DisciplineArray.Add(discipline);
            }
        }

        CreateIndexes();
    }

    private void CreateIndexes()
    {
        for (int i = 0; i < DisciplineArray.Count; i++)
        {
            Key key = new Key(DisciplineArray[i].discipline, DisciplineArray[i].department);
            treeDiscipline.Insert(DisciplineArray[i].discipline, i);
            treeInstitute.Insert(DisciplineArray[i].institute, i);
            treeTeacher.Insert(DisciplineArray[i].teacher, i);
            treeDepartment.Insert(DisciplineArray[i].department, i);
            table.Add(key, i);
            // Console.WriteLine(treeDiscipline.DisplayTree(treeDiscipline.root));
        }
    }

    private int Find(Discipline discipline)
    {
        Key key = new Key(discipline.discipline, discipline.department);
        var res = table.Search(key);
        return res;
    }
    public void Add(Discipline discipline)
    {
        DisciplineArray.Add(discipline);
        AddToIndexes(DisciplineArray.Count - 1);
    }
    private void AddToIndexes(int i)
    {
        Key key = new Key(DisciplineArray[i].discipline, DisciplineArray[i].department);
        treeDiscipline.Insert(DisciplineArray[i].discipline, i);
        treeInstitute.Insert(DisciplineArray[i].institute, i);
        treeTeacher.Insert(DisciplineArray[i].teacher, i);
        treeDepartment.Insert(DisciplineArray[i].department, i);
        table.Add(key, i);
    }

    public void Delete(Discipline discipline)
    {
        int res = Find(discipline);
        if (res == -1)
            return;
        var removeItem = DisciplineArray[res];
        int index = DisciplineArray.Count - 1;

        DisciplineArray[res] = DisciplineArray[index];
        DisciplineArray[index] = removeItem;
        DisciplineArray.Remove(removeItem);

        //удаляем
        Key key = new Key(removeItem.discipline, removeItem.department);
        treeDiscipline.Delete(removeItem.discipline, res);
        treeInstitute.Delete(removeItem.institute, res);
        treeTeacher.Delete(removeItem.teacher, res);
        treeDepartment.Delete(removeItem.department, res);
        table.Remove(key, res);
        // Console.WriteLine(treeDiscipline.DisplayTree(treeDiscipline.root));
        //заменяем
        removeItem = DisciplineArray[res];
        key = new Key(removeItem.discipline, removeItem.department);
        treeDiscipline.Edit(removeItem.discipline, res, index);
        treeInstitute.Edit(removeItem.institute, res, index);
        treeTeacher.Edit(removeItem.teacher, res, index);
        treeDepartment.Edit(removeItem.department, res, index);
        table.Edit(key, res);

    }
    public List<Discipline> GetAll()
    {
        return DisciplineArray;
    }

    public Discipline FindUnique(string discipline, string department)
    {
        Key key = new Key(discipline, department);
        int res = table.Search(key);
        return DisciplineArray[res];
    }
    public void WriteToFile(string path)
    {

        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine(DisciplineArray.Count);

            foreach (var discipline in DisciplineArray)
            {
                writer.WriteLine($"{discipline.discipline}\\{discipline.department}\\{discipline.teacher}\\{discipline.institute}");
            }
        }
    }

    public List<Discipline> FindByKey(string key, IndexType type)
    {
        NodeAvl res = null;
        switch (type)
        {
            case IndexType.discipline:
                res = treeDiscipline.Find(key);
                break;
            case IndexType.department:
                res = treeDepartment.Find(key);
                break;
            case IndexType.teacher:
                res = treeTeacher.Find(key);
                break;
            case IndexType.institute:
                res = treeInstitute.Find(key);
                break;
            default:
                return null;
        }

        var Disciplins = new List<Discipline>();
        var head = res.listAvl.head;
        Disciplins.Add(DisciplineArray[res.value]);
        if (head == null)
        {
            return Disciplins;
        }
        Disciplins.Add(DisciplineArray[head.Data]);
        var temp = head.Next;
        while (head != temp)
        {
            Disciplins.Add(DisciplineArray[temp.Data]);
            temp = temp.Next;
        }

        return Disciplins;
    }
    public bool isCorected(Discipline discipline)
    {
        bool isRussianWord1 = IsRussianWord(discipline.discipline);
        bool isRussianWord2 = IsRussianWord(discipline.discipline);
        bool isRussianWord3 = IsRussianWord(discipline.discipline);
        bool isRussianWord4 = IsRussianWord(discipline.discipline);
        bool flagWord = isRussianWord1 && isRussianWord2 && isRussianWord3 && isRussianWord4;
        bool startsWithUppercaseLetter1 = char.IsUpper(discipline.discipline[0]);
        bool startsWithUppercaseLetter2 = MyIsUpper(discipline.department);
        bool startsWithUppercaseLetter3 = MyIsUpper(discipline.teacher);
        bool startsWithUppercaseLetter4 = MyIsUpperAll(discipline.teacher);

        bool flagUpper = startsWithUppercaseLetter1 && startsWithUppercaseLetter2 && startsWithUppercaseLetter3 && startsWithUppercaseLetter4;
        if (flagWord && flagUpper)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsRussianWord(string word)
    {
        foreach (char letter in word)
        {
            // Проверяем, что символ относится к диапазону русских букв по таблице Unicode.
            if (!(letter >= 'А' && letter <= 'я'))
            {
                return false;
            }
        }
        return true;

    }
    private bool MyIsUpper(string word)
    {
        bool startsWithUppercaseLetter = false;
        string[] wordArray = word.Split(' ');
        for (int i = 0; i < wordArray.Length; i++)
        {
            if (char.IsUpper(wordArray[i][0]))
            {
                startsWithUppercaseLetter = true;
                break;
            }
        }
        if (startsWithUppercaseLetter)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool MyIsUpperAll(string word)
    {
        foreach (char letter in word)
        {
            // Проверяем, что символ относится к диапазону русских букв по таблице Unicode.
            if (!char.IsUpper(letter))
            {
                return false;
            }
        }
        return true;
    }
    public string GetIndexView(IndexType type)
    {
        switch (type)
        {
            case IndexType.discipline:
                return treeDiscipline.DisplayTree(treeDiscipline.root);
                break;
            case IndexType.department:
                return treeDepartment.DisplayTree(treeDepartment.root);
                break;
            case IndexType.teacher:
                return treeTeacher.DisplayTree(treeTeacher.root);
                break;
            case IndexType.institute:
                return treeInstitute.DisplayTree(treeInstitute.root);
                break;
            default:
                return null;
        }
    }
    public string GetUniqueView()
    {
        return table.Print();
    }
};

