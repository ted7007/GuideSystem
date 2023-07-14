using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using GuideSystemApp.Disciplines;
using GuideSystemApp.Marks;
using GuideSystemAppClient.Command;
using GuideSystemAppClient.Dto;
using GuideSystemAppClient.View;
using IndexType = GuideSystemApp.Marks.IndexType;

namespace GuideSystemAppClient.ViewModel;

public class GuideSystemVM : INotifyPropertyChanged
{
    private MarkRepository _markRepository;
    
    private readonly DisciplineRepository _disciplineRepository;

    
    public GuideSystemVM()
    {
        var vm = new TextBoxViewModel() { Text = "Введите стартовое количество элементов в хеш таблицах"};
        bool res = false;
        int count = -1;
        while (!res || !Int32.TryParse(vm.TextOutput, out count) || count <=0)
        {
            var askCountWindow = new TextBoxView();
            askCountWindow.DataContext = vm;
            res = (bool)askCountWindow.ShowDialog();
        }
         
        _markRepository = new MarkRepository(count);
        _disciplineRepository = new DisciplineRepository(count);
    }

    public ObservableCollection<object> CurrentList { get; set; }

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
                    CurrentList = new ObservableCollection<object>(_markRepository.GetAll());
                    break;
                case "Дисциплины":
                    CurrentList = new ObservableCollection<object>(_disciplineRepository.GetAll());
                    break;
                default:
                    MessageBox.Show("Функционал не реализован! Дальше могут быть ошибки.");
                    break;
            }
            OnPropertyChanged("CurrentList");
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
        get => new RelayCommand((o) => SaveList());
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
                    if(_disciplineRepository.FindByKey(vm.Discipline, GuideSystemApp.Disciplines.IndexType.discipline) == null)
                        ShowError();
                    
                    if (!Mark.Validate(vm.PassportSerialNumber, vm.Discipline, vm.Date, vm.Value))
                    {
                        ShowError();
                        return;
                    }
                    
                    _markRepository.Add(new Mark()
                    {
                        PassportSerialNumber = vm.PassportSerialNumber,
                        Discipline = vm.Discipline,
                        Date = vm.Date,
                        Value = (MarkEnum)int.Parse(vm.Value)
                    });
                    CurrentList = new ObservableCollection<object>(_markRepository.GetAll());
                    break;
                case "Дисциплины":
                    var inputWindow = new DynamicTextBoxesView();
                    var disciplineVm = new DynamicTextBoxVM(new[]
                    {
                        new DynamicTextBoxVM.TextBoxData() { Question = "Дисцплина:" },
                        new DynamicTextBoxVM.TextBoxData() { Question = "Департамент:" },
                        new DynamicTextBoxVM.TextBoxData() { Question = "Преподаватель:" },
                        new DynamicTextBoxVM.TextBoxData() { Question = "Институт:" }
                    });
                    inputWindow.DataContext = disciplineVm;
                    var resDiscipline = inputWindow.ShowDialog();
                    if(resDiscipline is null || !(bool)resDiscipline)
                        return;
                    var newDiscipline = new Discipline(disciplineVm.TextBoxDatas[0].Answer,
                        disciplineVm.TextBoxDatas[1].Answer,
                        disciplineVm.TextBoxDatas[2].Answer,
                        disciplineVm.TextBoxDatas[3].Answer);
                    if (!_disciplineRepository.isCorected(newDiscipline))
                    {
                        ShowError();
                        return;
                    }
                    
                    _disciplineRepository.Add(newDiscipline);
                    CurrentList = new ObservableCollection<object>(_disciplineRepository.GetAll());
                    break;

                default:
                    throw new NotImplementedException();
            }
            
            OnPropertyChanged("CurrentList");
        });
    }
    
    public RelayCommand RemoveCommand
    {
        get => new RelayCommand(obj =>
        {
            switch (SelectedList.Content.ToString())
            {
                case "Оценки":
                    if (CurrentListSelectedItem == null || !(CurrentListSelectedItem is Mark mark))
                    {
                        ShowError();
                        return;
                    }
                    _markRepository.Delete(new Mark
                    {
                        PassportSerialNumber = mark.PassportSerialNumber,
                        Discipline = mark.Discipline,
                        Date = mark.Date,
                        Value = mark.Value
                    });
                    CurrentList = new ObservableCollection<object>(_markRepository.GetAll());
                    OnPropertyChanged("CurrentList");
                    break;
                case "Дисциплины":
                    if (CurrentListSelectedItem == null || !(CurrentListSelectedItem is Discipline discipline) || _markRepository.FindByKey(discipline.discipline, IndexType.Discipline) != null)
                    {
                        ShowError();
                        return;
                    }
                    _disciplineRepository.Delete(new Discipline(discipline.discipline, discipline.department, discipline.teacher, discipline.institute));
                    CurrentList = new ObservableCollection<object>(_disciplineRepository.GetAll());
                    OnPropertyChanged("CurrentList");
                    break;
                default:
                    throw new NotImplementedException();
            }

        });
    }

    public RelayCommand FindCommand
    {
        get => new RelayCommand(obj =>
        {
            int countChecks = 0;
            switch (SelectedList.Content.ToString())
            {
                case "Оценки":
                    var w = new FindWindow();
                    var vm = new FindWindowVM(new List<SearchModel>()
                    {
                        new SearchModel() { SearchName = "По паспорту", SearchFields = new[] { "Паспорт" } },
                        new SearchModel() { SearchName = "По дисциплине", SearchFields = new[] { "Дисциплина" } },
                        new SearchModel() { SearchName = "По дате сдачи", SearchFields = new[] { "Дата сдачи" } },
                        new SearchModel() { SearchName = "По оценке", SearchFields = new[] { "Оценка" } },
                        new SearchModel()
                        {
                            SearchName = "Поиск конкретной оценки",
                            SearchFields = new[] { "Паспорт", "Дисциплина", "Дата сдачи", "Оценка" }
                        }
                    });
                    w.DataContext = vm;
                    var resDialog = w.ShowDialog();
                    if (!(bool)resDialog)
                        return;
                    Comparisons<List<Mark>> resMarks = null;
                    switch (vm.ComboSelectedItem)
                    {
                        case "По паспорту":
                            if (!Mark.ValidatePassport(vm.FieldInputList.First().FieldValue))
                            {
                                ShowError();
                                return;
                            }
                            resMarks = 
                                _markRepository.FindByKey(vm.FieldInputList.First().FieldValue, IndexType.Passport);
                            if (resMarks == null)
                            {
                                CurrentList = new ObservableCollection<object>();
                                OnPropertyChanged("CurrentList");
                            }
                            CurrentList = new ObservableCollection<object>(resMarks.node);
                            countChecks = resMarks.k;
                            break;
                        case "По дисциплине":
                            if (!Mark.ValidateDiscipline(vm.FieldInputList.First().FieldValue))
                            {
                                ShowError();
                                return;
                            }
                            resMarks = 
                                _markRepository.FindByKey(vm.FieldInputList.First().FieldValue, IndexType.Discipline);
                            if (resMarks == null)
                            {
                                CurrentList = new ObservableCollection<object>();
                                OnPropertyChanged("CurrentList");
                            }
                            CurrentList = new ObservableCollection<object>(resMarks.node);
                            countChecks = resMarks.k;
                            break;
                        case "По дате сдачи":
                            if (!Mark.ValidateDate(vm.FieldInputList.First().FieldValue))
                            {
                                ShowError();
                                return;
                            }
                            resMarks = 
                                _markRepository.FindByKey(vm.FieldInputList.First().FieldValue, IndexType.Date);
                            if (resMarks == null)
                            {
                                CurrentList = new ObservableCollection<object>();
                                OnPropertyChanged("CurrentList");
                            }
                            CurrentList = new ObservableCollection<object>(resMarks.node);
                            countChecks = resMarks.k;
                            break;
                        case "По оценке":
                            if (!Mark.ValidateValue(vm.FieldInputList.First().FieldValue))
                            {
                                ShowError();
                                return;
                            }
                            resMarks = 
                                _markRepository.FindByKey(vm.FieldInputList.First().FieldValue, IndexType.Value);
                            if (resMarks == null)
                            {
                                CurrentList = new ObservableCollection<object>();
                                OnPropertyChanged("CurrentList");
                            }
                            CurrentList = new ObservableCollection<object>(resMarks.node);
                            countChecks = resMarks.k;
                            break;
                        case "Поиск конкретной оценки":
                            
                            var fields = vm.FieldInputList.Select(f => f.FieldValue).ToList();
                            if (!Mark.Validate(fields[0], fields[1], fields[2], fields[3]))
                            {
                                ShowError();
                                return;
                            }
                            var resMarksOne = 
                                _markRepository.FindUnique(new Mark() { PassportSerialNumber = fields[0], Date = fields[2], Discipline = fields[1], Value = (MarkEnum)Int32.Parse(fields[3])});
                            
                            if (resMarksOne == null)
                            {
                                CurrentList = new ObservableCollection<object>();
                                OnPropertyChanged("CurrentList");
                            }
                            CurrentList = new ObservableCollection<object>(new []{resMarksOne.node});
                            countChecks = resMarksOne.k;
                            break;
                    }
                    break;
                case "Дисциплины":
                    var findDisciplines = new FindWindow();
                    var findDisciplinesVm = new FindWindowVM(new List<SearchModel>()
                    {
                        new SearchModel() { SearchName = "По Дисциплине", SearchFields = new[] { "Дисциплина" } },
                        new SearchModel() { SearchName = "По Департаменту", SearchFields = new[] { "Кафедра" } },
                        new SearchModel() { SearchName = "По Преподавателю", SearchFields = new[] { "Преподаватель" } },
                        new SearchModel() { SearchName = "По Институту", SearchFields = new[] { "Институт" } },
                        new SearchModel()
                        {
                            SearchName = "Поиск конкретной дисциплины",
                            SearchFields = new[] { "Дисциплина", "Департамент"}
                        }
                    });
                    findDisciplines.DataContext = findDisciplinesVm;
                    var resDiscipline = findDisciplines.ShowDialog();
                    if (!(bool)resDiscipline)
                        return;
                    Comparisons<List<Discipline>> resDisciplines = null;
                    switch (findDisciplinesVm.ComboSelectedItem)
                    {
                        case "По Дисциплине":
                            resDisciplines = 
                                _disciplineRepository.FindByKey(findDisciplinesVm.FieldInputList.First().FieldValue, GuideSystemApp.Disciplines.IndexType.discipline);
                            if (resDisciplines == null)
                            {
                                CurrentList = new ObservableCollection<object>();
                                OnPropertyChanged("CurrentList");
                            }
                            CurrentList = new ObservableCollection<object>(resDisciplines.node);
                            countChecks = resDisciplines.k;
                            break;
                        case "По Департаменту":
                            resDisciplines = 
                                _disciplineRepository.FindByKey(findDisciplinesVm.FieldInputList.First().FieldValue, GuideSystemApp.Disciplines.IndexType.department);
                            if (resDisciplines == null)
                            {
                                CurrentList = new ObservableCollection<object>();
                                OnPropertyChanged("CurrentList");
                            }
                            CurrentList = new ObservableCollection<object>(resDisciplines.node);
                            countChecks = resDisciplines.k;
                            break;
                        case "По Преподавателю":
                            resDisciplines = 
                                _disciplineRepository.FindByKey(findDisciplinesVm.FieldInputList.First().FieldValue, GuideSystemApp.Disciplines.IndexType.teacher);
                            if (resDisciplines == null)
                            {
                                CurrentList = new ObservableCollection<object>();
                                OnPropertyChanged("CurrentList");
                            }
                            CurrentList = new ObservableCollection<object>(resDisciplines.node);
                            countChecks = resDisciplines.k;
                            break;
                        case "По Институту":
                            resDisciplines = 
                                _disciplineRepository.FindByKey(findDisciplinesVm.FieldInputList.First().FieldValue, GuideSystemApp.Disciplines.IndexType.institute);
                            CurrentList = new ObservableCollection<object>(resDisciplines.node);
                            countChecks = resDisciplines.k;
                            break;
                        case "Поиск конкретной дисциплины":
                            var fields = findDisciplinesVm.FieldInputList.Select(f => f.FieldValue).ToList();
                            var resDisciplinesOne = 
                                _disciplineRepository.FindUnique(fields[0], fields[1]);
                            if (resDisciplinesOne == null)
                            {
                                CurrentList = new ObservableCollection<object>();
                                OnPropertyChanged("CurrentList");
                            }
                            CurrentList = new ObservableCollection<object>(new []{resDisciplinesOne.node});
                            countChecks = resDisciplinesOne.k;
                            break;
                    }
                    
                    
                    break;
                default:
                    throw new NotImplementedException();
            }
            OnPropertyChanged("CurrentList");
            MessageBox.Show($"Количество проверок - {countChecks}");
        });
    }
    
    public RelayCommand LogCommand
    {
        get => new RelayCommand(obj =>
        {
            switch (SelectedList.Content.ToString())
            {
                case "Оценки":
                    var comboBoxWindow = new ComboBoxWindow();
                    var vm = new ComboBoxViewModel()
                    {
                        ComboItems = new[]
                        {
                            "Дерево по полю Паспорта",
                            "Дерево по полю Дисциплины",
                            "Дерево по полю Даты сдачи",
                            "Дерево по полю Оценки",
                            "Хеш таблица"
                        }
                    };
                    comboBoxWindow.DataContext = vm;
                    var resDialog = comboBoxWindow.ShowDialog();
                    if(!(bool)resDialog)
                        return;
                    switch (vm.ComboSelectedItem)
                    {
                          case  "Дерево по полю Паспорта":
                              MessageBox.Show(_markRepository.GetIndexView(IndexType.Passport));
                              break;
                          case  "Дерево по полю Дисциплины":
                              MessageBox.Show(_markRepository.GetIndexView(IndexType.Discipline));
                              break;
                          case  "Дерево по полю Даты сдачи":
                              MessageBox.Show(_markRepository.GetIndexView(IndexType.Date));
                              break;
                          case  "Дерево по полю Оценки":
                              MessageBox.Show(_markRepository.GetIndexView(IndexType.Value));
                              break;
                          case  "Хеш таблица":
                              MessageBox.Show(_markRepository.GetUniqueView());
                              break;
                          
                          default:
                              MessageBox.Show("incorrect data");
                              break;
                    }

                    break;

                case "Дисциплины":
                {
                    var comboBoxWindowDiscipline = new ComboBoxWindow();
                    var vmDiscipline = new ComboBoxViewModel()
                    {
                        ComboItems = new[]
                        {
                            "Дерево по полю Дисциплины",
                            "Дерево по полю Департамента",
                            "Дерево по полю Преподавателя",
                            "Дерево по полю Института",
                            "Хеш таблица"
                        }
                    };
                    comboBoxWindowDiscipline.DataContext = vmDiscipline;
                    var resDialogDiscipline = comboBoxWindowDiscipline.ShowDialog();
                    if(!(bool)resDialogDiscipline)
                        return;
                    switch (vmDiscipline.ComboSelectedItem)
                    {
                        case  "Дерево по полю Дисциплины":
                            MessageBox.Show(_disciplineRepository.GetIndexView(GuideSystemApp.Disciplines.IndexType.discipline));
                            break;
                        case  "Дерево по полю Департамента":
                            MessageBox.Show(_disciplineRepository.GetIndexView(GuideSystemApp.Disciplines.IndexType.department));
                            break;
                        case  "Дерево по полю Преподавателя":
                            MessageBox.Show(_disciplineRepository.GetIndexView(GuideSystemApp.Disciplines.IndexType.teacher));
                            break;
                        case  "Дерево по полю Института":
                            MessageBox.Show(_disciplineRepository.GetIndexView(GuideSystemApp.Disciplines.IndexType.institute));
                            break;
                        case  "Хеш таблица":
                            MessageBox.Show(_disciplineRepository.GetUniqueView());
                            break;
                          
                        default:
                            MessageBox.Show("incorrect data");
                            break;
                    }
                }
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
            switch (SelectedList.Content.ToString())
            {
                case "Оценки":
                    _markRepository.ReadFromFile("dataMarks.txt");
                    CurrentList = new ObservableCollection<object>(_markRepository.GetAll());
                    break;
                case "Дисциплины":
                    _disciplineRepository.ReadFromFile("dataDisciplines.txt");
                    CurrentList = new ObservableCollection<object>(_disciplineRepository.GetAll());
                    break;
                case "Студенты":
                    //_studentRepository.ReadFromFile("dataStudents.txt");
                    //CurrentList = _studentRepository.GetAll();
                    break;
                default:
                    MessageBox.Show("Выберите нужный список!");
                    break;
            }
            OnPropertyChanged("CurrentList");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error mazafaka");//todo: error
        }
    }
    
    private void SaveList()
    {
        try
        {
            switch (SelectedList.Content.ToString())
            {
                case "Оценки":
                    _markRepository.WriteToFile("dataMarks.txt");
                    break;
                case "Дисциплины":
                    _disciplineRepository.WriteToFile("dataDisciplines.txt");
                    break;
                case "Студенты":
                  //  _markRepository.WriteToFile("dataStudents.txt");
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

    private void ShowError()
    {
        MessageBox.Show("incorrect data");
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