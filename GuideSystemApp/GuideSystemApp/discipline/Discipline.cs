namespace GuideSystemApp.Disciplines;

public class Discipline
{
    public string discipline;
    public string department;
    public string teacher;
    public string institute;
    public Discipline(string discipline, string department, string teacher, string institute)
    {
        this.discipline = discipline;
        this.department = department;
        this.teacher = teacher;
        this.institute = institute;
    }
    public void print()
    {
        Console.Write($"{this.discipline} {this.department} {this.teacher} {this.institute}");
    }

    public override string ToString()
    {
        return $"Дисциплина: {discipline}, Департамент: {department}, Преподаватель: {teacher}, Институт: {institute}";
    }
}
