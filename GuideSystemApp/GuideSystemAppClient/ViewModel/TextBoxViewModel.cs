using System.Windows;
using GuideSystemAppClient.Command;

namespace GuideSystemAppClient.ViewModel;

public class TextBoxViewModel
{

    public int Width { get; set; } = 250;

    public int Height { get; set; } = 450;
    
    public string Text { get; set; }

    public Visibility Visibility { get; set; } = Visibility.Visible;

    public bool IsReadOnly { get; set; } = false;
    
    public string TextOutput { get; set; }

    public RelayCommand AcceptCommand => new RelayCommand(Accept);

    private void Accept(object sender)
    {
        ((Window)sender).DialogResult = true;
    }
}