using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using GuideSystemAppClient.Command;

namespace GuideSystemAppClient.ViewModel;

public class FindWindowVM : INotifyPropertyChanged
{
    private readonly IEnumerable<SearchModel> _searchModels;

    public FindWindowVM(IEnumerable<SearchModel> searchModels )
    {
        _searchModels = searchModels;
        ComboItems = searchModels.Select(s => s.SearchName);
        OnPropertyChanged("ComboItems");
        OnPropertyChanged("FieldInputList");
    }
    
    public IEnumerable<string> ComboItems { get; set; }

    private string comboBoxItem;
    
    public string ComboSelectedItem
    {
        get => comboBoxItem;
        set
        {
            comboBoxItem = value;
            var inputList = _searchModels.First(m => m.SearchName == (string)value)//.Content)
                .SearchFields
                .Select(f => new FieldInput()
                {
                    FieldName = f
                });
            FieldInputList = new ObservableCollection<FieldInput>(inputList);
            OnPropertyChanged("FieldInputList");
        }
    }

    public ObservableCollection<FieldInput> FieldInputList { get; set; }

    


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

public class SearchModel
{
    public string SearchName { get; set; }

    public IEnumerable<string> SearchFields { get; set; }
}

public class SearchResult
{
    public string SearchName { get; set; }

    public Dictionary<string, string> SearchResults { get; set; }
}

public class FieldInput
{
    public string FieldName { get; set; }
 
    public string FieldValue { get; set; }
}