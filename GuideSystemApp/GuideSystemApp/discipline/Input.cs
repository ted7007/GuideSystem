
public class Input
{

    public static Discipline[] read(string path)
    {

        StreamReader reader = new StreamReader(path);

        int count = int.Parse(reader.ReadLine()); // Преобразование строку в число

        Discipline[] data = new Discipline[count]; // Создаём массив с размером count

        // Записываем данные в массив
        for (int i = 0; i < count; i++)
        {
            string[] person = reader.ReadLine().Split('/');
            Discipline discipline = new Discipline(person[0], person[1], person[2], person[3]);
            data[i] = discipline;
        }
        return data;
    }

}
