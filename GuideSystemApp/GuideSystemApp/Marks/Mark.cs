namespace GuideSystemApp.Marks;

/// <summary>
/// Сущность оценки
/// </summary>
public class Mark : IComparable<Mark>
{

    /// <summary>
    /// Серия и номер паспорта разделенные пробелом
    /// </summary>
    public string PassportSerialNumber { get; set; }

    /// <summary>
    /// Название дисциплины
    /// </summary>
    public string Discipline { get; set; }

    /// <summary>
    /// Оценка
    /// </summary>
    public MarkEnum Value { get; set; }

    /// <summary>
    /// Дата сдачи
    /// </summary>
    public string Date { get; set; }

    public int CompareTo(Mark? other)
    {
        if (other.Date == Date && other.Discipline == Discipline && other.Value == Value &&
            other.PassportSerialNumber == PassportSerialNumber)
            return 0;
        if (other.Value > Value)
            return 1;

        return -1;
    }
    
    public override string ToString()
    {
        return $"Паспорт: {PassportSerialNumber}, Дисциплина: {Discipline}, Дата: {Date}, Оценка: {Value}";
    }
}