using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using GuideSystemAppClient.Command;

namespace GuideSystemAppClient.ViewModel;

public class ComboBoxViewModel
{
    public ComboBoxViewModel()
    {
        
    }

    public IEnumerable<string> ComboItems { get; set; }

    private string comboBoxItem;
    
    public string ComboSelectedItem { get; set; }
    
    
    public RelayCommand AcceptCommand => new RelayCommand(Accept);

    private void Accept(object sender)
    {
        ((Window)sender).DialogResult = true;
    }

}