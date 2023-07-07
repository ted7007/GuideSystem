
public class CreateTable
{

    public static HachTable create(Discipline[] data)
    {


        int size = data.Length;
        var table = new HachTable(10);

        for (int i = 0; i < size; i++)
        {
            Key key = new Key(data[i].discipline, data[i].department);
            table.Add(key, data[i]);
        }
        
        return table;

    }

}
