using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using GuideSystemApp.Marks;
using GuideSystemAppClient.Command;

namespace GuideSystemAppClient.ViewModel;

public class MarkFieldsInputVM: INotifyPropertyChanged
{
    private string passportSerialNumber;

    private string discipline;

    private string markValue;

    private string date;

    private string kafedra;
    
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
    public string Value { get => markValue; set => SetField(ref markValue, value); }

    /// <summary>
    /// Дата сдачи
    /// </summary>
    public string Date { get => date; set => SetField(ref date, value); }

    public string Kafedra { get => kafedra; set => SetField(ref kafedra, value); }
    
    public RelayCommand AcceptCommand => new RelayCommand(Accept);

    private void Accept(object sender)
    {
        ((Window)sender).DialogResult = true;
    }
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
}