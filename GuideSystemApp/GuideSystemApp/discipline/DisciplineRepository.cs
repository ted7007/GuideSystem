namespace GuideSystemApp.Disciplines;


public class DisciplineRepository
{
    public List<Discipline> DisciplineArray;

    private HachTable table;

    private AVLTree treeDiscipline;
    private AVLTree treeInstitute;
    private AVLTree treeDepartment;
    private AVLTree treeTeacher;
    private readonly int _startCount;

    public DisciplineRepository(int count)
    {
        _startCount = count;
        this.DisciplineArray = new List<Discipline>();
        this.table = new HachTable(count);
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
            this.DisciplineArray = new List<Discipline>();
            this.table = new HachTable(_startCount);
            this.treeDiscipline = new AVLTree();
            this.treeInstitute = new AVLTree();
            this.treeTeacher = new AVLTree();
            this.treeDepartment = new AVLTree();
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
        Console.WriteLine(table.Print());
        //заменяем
        if (res != index)
        {
            removeItem = DisciplineArray[res];
            key = new Key(removeItem.discipline, removeItem.department);
            treeDiscipline.Edit(removeItem.discipline, res, index);
            treeInstitute.Edit(removeItem.institute, res, index);
            treeTeacher.Edit(removeItem.teacher, res, index);
            treeDepartment.Edit(removeItem.department, res, index);
            table.Edit(key, res);
        }

    }
    public IEnumerable<Discipline> GetAll()
    {
        List<Discipline> disciplines = new List<Discipline>();
        for (int i = 0; i < DisciplineArray.Count; i++)
        {
            disciplines.Add(DisciplineArray[i]);
            disciplines[i].Index = i;
        }

        return disciplines;
    }

    public Comparisons<Discipline> FindUnique(string discipline, string department)
    {
        Key key = new Key(discipline, department);
        int res = table.Search(key);
        if (res == -1)
        {
            return null;
        }
        else
        {
            Comparisons<Discipline> comparisons = new Comparisons<Discipline>(DisciplineArray[res], table.k);
            table.k = 1;
            return comparisons;
        }
    }
    public void WriteToFile(string path)
    {

        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine(DisciplineArray.Count);

            foreach (var discipline in DisciplineArray)
            {
                writer.WriteLine($"{discipline.discipline}/{discipline.department}/{discipline.teacher}/{discipline.institute}");
            }
        }
    }

    public Comparisons<List<Discipline>> FindByKey(string key, IndexType type)
    {
        NodeAvl res = null;
        int i = 0;
        switch (type)
        {
            case IndexType.discipline:
                treeDiscipline.i = 1;
                res = treeDiscipline.Find(key);
                i = treeDiscipline.i;
                break;
            case IndexType.department:
                treeDepartment.i = 1;
                res = treeDepartment.Find(key);
                i = treeDepartment.i;
                break;
            case IndexType.teacher:
                treeTeacher.i = 1;
                res = treeTeacher.Find(key);
                i = treeTeacher.i;
                break;
            case IndexType.institute:
                treeInstitute.i = 1;
                res = treeInstitute.Find(key);
                i = treeInstitute.i;
                break;
            default:
                return null;
        }
        if (res == null)
        {
            return null;
        }
        else
        {
            var Disciplins = new List<Discipline>();
            var head = res.listAvl.head;
            Disciplins.Add(DisciplineArray[res.value]);
            Comparisons<List<Discipline>> comparisons = new Comparisons<List<Discipline>>(Disciplins, i);
            if (head == null)
            {
                return comparisons;
            }
            Disciplins.Add(DisciplineArray[head.Data]);
            var temp = head.Next;
            while (head != temp)
            {
                Disciplins.Add(DisciplineArray[temp.Data]);
                temp = temp.Next;
            }
            comparisons.node = Disciplins;
            return comparisons;
        }
    }
    public bool isCorected(Discipline discipline)
    {

        if (discipline == null)
        {
            return false;
        }
        if (discipline.department == null || discipline.discipline == null || discipline.institute == null || discipline.teacher == null)
        {
            return false;
        }
        return discipline.Validate();
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

