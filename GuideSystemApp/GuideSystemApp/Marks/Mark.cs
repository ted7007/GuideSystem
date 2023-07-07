namespace GuideSystemApp.Marks;

/// <summary>
/// Сущность оценки
/// </summary>
public class Mark
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

    public override bool Equals(object? obj)
    {
        if (obj == this)
            return true;
        if (!(obj is Mark mark))
            return false;
        return Equals(mark);
    }

    public bool Equals(Mark mark)
    {
        if (mark.Date == Date && mark.Discipline == Discipline && mark.Value == Value &&
            mark.PassportSerialNumber == PassportSerialNumber)
            return true;
        return false
    }
}