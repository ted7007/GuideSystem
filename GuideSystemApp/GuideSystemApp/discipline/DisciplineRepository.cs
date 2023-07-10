namespace GuideSystemApp.Disciplines;


public class DisciplineRepository
{
    private string path;

    private HachTable table;
    public DisciplineRepository()
    {
        this.path = "input.txt";
        Discipline[] data = Input.read(path);
        int size = data.Length;
        this.table = new HachTable(10);

        for (int i = 0; i < size; i++)
        {
            Key key = new Key(data[i].discipline, data[i].department);
            table.Add(key, i);
            Console.WriteLine("");
        }
    }
    public void Print()
    {
        table.Print();
    }
}
