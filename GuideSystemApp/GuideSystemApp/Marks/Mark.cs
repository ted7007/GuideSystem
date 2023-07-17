using System.Text.RegularExpressions;

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

    public int Index { get; set; }

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
        return $"[{Index}]: Паспорт: {PassportSerialNumber}, Дисциплина: {Discipline}, Дата: {Date}, Оценка: {Value}";
    }
    
    public static bool ValidatePassport(string passport)
    {
        if (passport == null)
            return false;
        
        // Паттерн для валидации паспорта
        string pattern = @"^\d{4} \d{6}$";

        // Проверка на соответствие паттерну
        Match match = Regex.Match(passport, pattern);

        // Возвращаем результат валидации
        return match.Success;
    }
    
    public static bool ValidateDiscipline(string variable)
    {
        if (variable == null)
            return false;
        
        // Паттерн для валидации переменной
        string pattern = @"^[а-яА-ЯёЁ\s]+$";

        // Проверка на соответствие паттерну
        Match match = Regex.Match(variable, pattern);

        // Возвращаем результат валидации
        return match.Success;
    }
    
    public static bool ValidateDate(string date)
    {
        if (date == null)
            return false;
        
        // Проверка на пустую строку
        if (string.IsNullOrEmpty(date))
        {
            return false;
        }

        // Разделение даты на отдельные компоненты
        string[] parts = date.Split('.');

        // Проверка на корректное количество компонентов
        if (parts.Length != 3)
        {
            return false;
        }

        // Парсинг компонентов и проверка на число
        int day, month, year;
        if (!int.TryParse(parts[0], out day) || !int.TryParse(parts[1], out month) || !int.TryParse(parts[2], out year))
        {
            return false;
        }

        // Проверка на корректные значения дня, месяца и года
        if (day < 1 || day > 31 || month < 1 || month > 12 || year < 0 || year > 99)
        {
            return false;
        }

        return true;
    }

    public static bool ValidateValue(string value)
    {
        if (value == null)
            return false;
        if (!Int32.TryParse(value, out int res))
            return false;
        if (res >= 2 && res <= 5)
            return true;
        return false;
    }

    public static bool Validate(string passport, string discipline, string date, string value)
    {
        return ValidatePassport(passport) && ValidateDiscipline(discipline) && ValidateDate(date) && ValidateValue(value);
    }
    
    public bool Validate()
    {
        return ValidatePassport(PassportSerialNumber) && ValidateDiscipline(Discipline) && ValidateDate(Date);
    }
}