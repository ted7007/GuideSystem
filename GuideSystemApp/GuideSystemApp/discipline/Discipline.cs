namespace GuideSystemApp.Disciplines;

public class Discipline
{
    public string discipline;
    public string department;
    public string teacher;
    public string institute;

    public int Index { get; set; }
    
    public Discipline(string discipline, string department, string teacher, string institute)
    {
        this.discipline = discipline;
        this.department = department;
        this.teacher = teacher;
        this.institute = institute;
    }
    public void print()
    {
        Console.Write($"[{Index}]: {this.discipline} {this.department} {this.teacher} {this.institute}");
    }

    public override string ToString()
    {
        return $"[{Index}]: Дисциплина: {discipline}, Департамент: {department}, Преподаватель: {teacher}, Институт: {institute}";
    }
    public bool Validate()
    {
        return ValidateTeacher(teacher) && ValidateDiscipline(discipline) && ValiInstitute(institute) && ValiDepartment(department);
    }
    public static bool Validate(string teacher, string discipline, string institute, string department)
    {

        return ValidateTeacher(teacher) && ValidateDiscipline(discipline) && ValiInstitute(institute) && ValiDepartment(department);
    }
    public static bool ValidateTeacher(string teacher)
    {
        return IsRussianWord(teacher) && MyIsUpper(teacher);
    }
    public static bool ValidateDiscipline(string discipline)
    {
        return IsRussianWord(discipline) && char.IsUpper(discipline[0]);
    }
    public static bool ValiInstitute(string institute)
    {
        return IsRussianWord(institute) && MyIsUpperAll(institute);
    }
    public static bool ValiDepartment(string department)
    {
        return IsRussianWord(department) && MyIsUpper(department);
    }
    private static bool MyIsUpperAll(string word)
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
    private static bool MyIsUpper(string word)
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
    private static bool IsRussianWord(string word)
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
}
