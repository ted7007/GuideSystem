using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using GuideSystemApp.Marks;
using GuideSystemAppClient.Command;
using GuideSystemAppClient.Dto;
using GuideSystemAppClient.View;

namespace GuideSystemAppClient.ViewModel;

public class GuideSystemVM : INotifyPropertyChanged
{
    private MarkRepository _markRepository;
    
    public GuideSystemVM()
    {
        _markRepository = new MarkRepository();
    }

    public IEnumerable<object> CurrentList { get; set; }

    public object CurrentListSelectedItem { get; set; }
    
    private ListBoxItem selectedList;
    public ListBoxItem SelectedList
    {
        get => selectedList;
        set
        {
            selectedList = value;
            switch (SelectedList.Content.ToString())
            {
                case "Оценки":
                    CurrentList = GetMarks();
                    OnPropertyChanged("CurrentList");
                    break;
                default:
                    MessageBox.Show("Функционал не реализован! Дальше могут быть ошибки.");
                    break;
            }
        }
    }
    
    public RelayCommand DownloadCommand
    {
        get => new RelayCommand(obj =>
        {
            DownloadList();
        });
    }
    
    public RelayCommand SaveCommand
    {
        get => new RelayCommand(obj =>
        {
            var textWindow = new TextBoxView();
            var vm = new TextBoxViewModel { Text = "Введите абсолютный путь до справочника" };
            textWindow.DataContext = vm;
            var res = textWindow.ShowDialog();
            if(res == null || !(bool)res)
                return;
            SaveList(vm.TextOutput);
        });
    }
    
    public RelayCommand AddCommand
    {
        get => new RelayCommand(obj =>
        {
            switch (SelectedList.Content.ToString())
            {
                case "Оценки":
                    var markFieldsInpitWindow = new MarkFieldsInput();
                    var vm = new MarkFieldsInputVM();
                    markFieldsInpitWindow.DataContext = vm;
                    var res = markFieldsInpitWindow.ShowDialog();
                    if(res == null || !(bool)res)
                        return;
                    //todo: validation
                    _markRepository.Add(new Mark()
                    {
                        PassportSerialNumber = vm.PassportSerialNumber,
                        Discipline = vm.Discipline,
                        Date = vm.Date,
                        Value = (MarkEnum)int.Parse(vm.Value)
                    });
                    CurrentList = GetMarks().ToList();
                    OnPropertyChanged("CurrentList");
                    break;
                default:
                    throw new NotImplementedException();
            }
            
        });
    }
    
    public RelayCommand RemoveCommand
    {
        get => new RelayCommand(obj =>
        {
            switch (SelectedList.Content.ToString())
            {
                case "Оценки":
                    if (CurrentListSelectedItem == null || !(CurrentListSelectedItem is MarkDto mark))
                    {
                        MessageBox.Show("Для удаления нужно выбрать элемент из списка!");
                        return;
                    }
                    _markRepository.Delete(new Mark
                    {
                        PassportSerialNumber = mark.PassportSerialNumber,
                        Discipline = mark.Discipline,
                        Date = mark.Date,
                        Value = mark.Value
                    });
                    CurrentList = GetMarks();
                    OnPropertyChanged("CurrentList");
                    break;
                default:
                    throw new NotImplementedException();
            }
            
        });
    }

    public RelayCommand EditCommand
    {
        get => new RelayCommand(obj =>
        {
            switch (SelectedList.Content.ToString())
            {
                case "Оценки":
                    if (CurrentListSelectedItem == null || !(CurrentListSelectedItem is MarkDto oldMark))
                    {   
                        MessageBox.Show("Для редактирования нужно выбрать элемент из списка!");
                        return;
                    }
                    
                    var markFieldsInputWindow = new MarkFieldsInput();
                    var vm = new MarkFieldsInputVM();
                    markFieldsInputWindow.DataContext = vm;
                    var res = markFieldsInputWindow.ShowDialog();
                    if(res == null || !(bool)res)
                        return;
                    
                    _markRepository.Edit(new Mark
                    {
                        PassportSerialNumber = oldMark.PassportSerialNumber,
                        Discipline = oldMark.Discipline,
                        Date = oldMark.Date,
                        Value = oldMark.Value
                    },
                    new Mark()
                    {
                        PassportSerialNumber = vm.PassportSerialNumber,
                        Discipline = vm.Discipline,
                        Date = vm.Date,
                        Value = (MarkEnum)int.Parse(vm.Value)
                    });
                    CurrentList = GetMarks();
                    OnPropertyChanged("CurrentList");
                    break;
                default:
                    throw new NotImplementedException();
            }
            
        });
    }
    
    private void DownloadList()
    {
        try
        {
            var textWindow = new TextBoxView();
            var vm = new TextBoxViewModel { Text = "Введите абсолютный путь до справочника" };
            textWindow.DataContext = vm;
            var res = textWindow.ShowDialog();
            if(res == null || !(bool)res)
                return;
            switch (SelectedList.Content.ToString())
            {
                case "Оценки":
                    _markRepository.ReadFromFile(vm.TextOutput);
                    break;
                default:
                    MessageBox.Show("Выберите нужный список!");
                    break;
            }

            CurrentList = GetMarks();
            OnPropertyChanged("CurrentList");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error mazafaka");//todo: error
        }
    }
    
    private void SaveList(string vmTextOutput)
    {
        try
        {
            switch (SelectedList.Content.ToString())
            {
                case "Оценки":
                    _markRepository.WriteToFile(vmTextOutput);
                    break;
                default:
                    MessageBox.Show("Выберите нужный список!");
                    break;
            }
        }
        catch (Exception ex)
        {
            //todo: error
        }
    }
    

    private IEnumerable<object> GetMarks()
    {
        return _markRepository.GetAll().Select(m => (object)new MarkDto()
        {
            PassportSerialNumber = m.PassportSerialNumber,
            Date = m.Date,
            Discipline = m.Discipline,
            Value = m.Value
        });
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