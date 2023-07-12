using System.Windows;
using GuideSystemAppClient.Command;

namespace GuideSystemAppClient.ViewModel;

public class TextBoxViewModel
{
    public string Text { get; set; }
    
    public string TextOutput { get; set; }

    public RelayCommand AcceptCommand => new RelayCommand(Accept);

    private void Accept(object sender)
    {
        ((Window)sender).DialogResult = true;
    }
}