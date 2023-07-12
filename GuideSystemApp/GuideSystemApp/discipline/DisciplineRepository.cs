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


}
