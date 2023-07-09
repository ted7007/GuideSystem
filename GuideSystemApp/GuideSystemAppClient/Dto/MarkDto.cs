using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GuideSystemApp.Marks;

namespace GuideSystemAppClient.Dto;

public class MarkDto : INotifyPropertyChanged
{
    private string passportSerialNumber;

    private string discipline;

    private MarkEnum markValue;

    private string date;
    
    /// <summary>
    /// Серия и номер паспорта разделенные пробелом
    /// </summary>
    public string PassportSerialNumber { get => passportSerialNumber; set => SetField(ref passportSerialNumber, value);}

    /// <summary>
    /// Название дисциплины
    /// </summary>
    public string Discipline { get => discipline; set => SetField(ref discipline, value); }

    /// <summary>
    /// Оценка
    /// </summary>
    public MarkEnum Value { get => markValue; set => SetField(ref markValue, value); }

    /// <summary>
    /// Дата сдачи
    /// </summary>
    public string Date { get => date; set => SetField(ref date, value); }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    public override string ToString()
    {
        return $"Паспорт: {passportSerialNumber}, Дисциплина: {discipline}, Дата: {date}, Оценка: {markValue}";
    }
}