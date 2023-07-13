using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using GuideSystemAppClient.Command;

namespace GuideSystemAppClient.ViewModel;

public class DynamicTextBoxVM : INotifyPropertyChanged
{

    public DynamicTextBoxVM(IEnumerable<TextBoxData> textBoxDatas )
    {
        TextBoxDatas = new ObservableCollection<TextBoxData>(textBoxDatas);
        OnPropertyChanged("TextBoxDatas");
    }

    public ObservableCollection<TextBoxData> TextBoxDatas { get; set; }

    public RelayCommand AcceptCommand => new RelayCommand(Accept);

    private void Accept(object sender)
    {
        ((Window)sender).DialogResult = true;
    }
    
    
    public class TextBoxData
    {
        public string Question { get; set; }

        public string Answer { get; set; }
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