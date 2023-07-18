using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using GuideSystemApp.Disciplines;
using GuideSystemApp.Marks;
using GuideSystemApp.Student;
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
        _studentRepository = new StudentRepository(count);
    }

    public ObservableCollection<object> CurrentList { get; set; }

    public object CurrentListSelectedItem { get; set; }
    
    private ListBoxItem selectedList;
    private readonly StudentRepository _studentRepository;

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
                case "Студенты":
                    CurrentList = new ObservableCollection<object>(_studentRepository.GetAll());
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
                    if (!Mark.Validate(vm.PassportSerialNumber, vm.Discipline, vm.Date, vm.Value))
                    {
                        ShowError();
                        return;
                    }
                    
                    if(_disciplineRepository.FindByKey(vm.Discipline, GuideSystemApp.Disciplines.IndexType.discipline) == null
                       || _studentRepository.SearchByPassport(vm.PassportSerialNumber, out int some) == null)
                        ShowError("Ошибка валидации");
                    
                    
                    
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
                case "Студенты":
                    var inputWindowStudent = new DynamicTextBoxesView();
                    var studentVm = new DynamicTextBoxVM(new[]
                    {
                        new DynamicTextBoxVM.TextBoxData() { Question = "ФИО:" },
                        new DynamicTextBoxVM.TextBoxData() { Question = "Группа:" },
                        new DynamicTextBoxVM.TextBoxData() { Question = "Паспорт:" },
                        new DynamicTextBoxVM.TextBoxData() { Question = "Дата поступления:" }
                    });
                    inputWindowStudent.DataContext = studentVm;
                    var resStudent = inputWindowStudent.ShowDialog();
                    if(resStudent is null || !(bool)resStudent)
                        return;
                    var newStudent = new Student(studentVm.TextBoxDatas[0].Answer,
                        studentVm.TextBoxDatas[1].Answer,
                        studentVm.TextBoxDatas[2].Answer,
                        studentVm.TextBoxDatas[3].Answer);
                    // if (!_studentRepository.newStudent))
                    // {
                    //     ShowError();
                    //     return;
                    // }
                    
                    _studentRepository.Add(newStudent);
                    CurrentList = new ObservableCollection<object>(_studentRepository.GetAll());
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
                        ShowError("Ошибка целостности данных");
                        return;
                    }
                    _disciplineRepository.Delete(new Discipline(discipline.discipline, discipline.department, discipline.teacher, discipline.institute));
                    CurrentList = new ObservableCollection<object>(_disciplineRepository.GetAll());
                    OnPropertyChanged("CurrentList");
                    break;
                case "Студенты":
                    if (CurrentListSelectedItem == null || !(CurrentListSelectedItem is Student student) || _markRepository.FindByKey(student.Passport, IndexType.Passport) != null)
                    {
                        ShowError("Ошибка целостности данных");
                        return;
                    }
                    _studentRepository.Delete(new Student(student.FIO, student.Group, student.Passport, student.AdmissionDate));
                    CurrentList = new ObservableCollection<object>(_studentRepository.GetAll());
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
                                ShowError("Не найдено.");
                                return;
                            }

                            MessageBox.Show(GetViewMarks(resMarks));
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
                                ShowError("Не найдено.");
                                return;
                            }
                            MessageBox.Show(GetViewMarks(resMarks));
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
                                ShowError("Не найдено.");
                                return;
                            }
                            MessageBox.Show(GetViewMarks(resMarks));
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
                                ShowError("Не найдено.");
                                return;
                            }
                            MessageBox.Show(GetViewMarks(resMarks));
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
                                ShowError("Не найдено.");
                                
                                return;
                            }
                            
                            MessageBox.Show(GetViewMarks(new Comparisons<List<Mark>>(node:new List<Mark>{resMarksOne.node}, resMarksOne.k)));
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
                            if (!Discipline.ValidateDiscipline(findDisciplinesVm.FieldInputList.First().FieldValue))
                            {
                                ShowError();
                                return;
                            }
                            
                            resDisciplines = 
                                _disciplineRepository.FindByKey(findDisciplinesVm.FieldInputList.First().FieldValue, GuideSystemApp.Disciplines.IndexType.discipline);
                            if (resDisciplines == null)
                            {
                                ShowError("Не найдено.");
                                return;
                            }
                           
                            MessageBox.Show(GetViewDisciplines(resDisciplines));
                            break;
                        case "По Департаменту":
                            if (!Discipline.ValiDepartment(findDisciplinesVm.FieldInputList.First().FieldValue))
                            {
                                ShowError();
                                return;
                            }
                            resDisciplines = 
                                _disciplineRepository.FindByKey(findDisciplinesVm.FieldInputList.First().FieldValue, GuideSystemApp.Disciplines.IndexType.department);
                            if (resDisciplines == null)
                            {
                                ShowError("Не найдено.");
                                return;
                            }
                            MessageBox.Show(GetViewDisciplines(resDisciplines));
                            break;
                        case "По Преподавателю":
                            if (!Discipline.ValidateTeacher(findDisciplinesVm.FieldInputList.First().FieldValue))
                            {
                                ShowError();
                                return;
                            }
                            resDisciplines = 
                                _disciplineRepository.FindByKey(findDisciplinesVm.FieldInputList.First().FieldValue, GuideSystemApp.Disciplines.IndexType.teacher);
                            if (resDisciplines == null)
                            {
                                ShowError("Не найдено.");
                                return;
                            }
                            MessageBox.Show(GetViewDisciplines(resDisciplines));
                            break;
                        case "По Институту":
                            if (!Discipline.ValiInstitute(findDisciplinesVm.FieldInputList.First().FieldValue))
                            {
                                ShowError();
                                return;
                            }
                            resDisciplines = 
                                _disciplineRepository.FindByKey(findDisciplinesVm.FieldInputList.First().FieldValue, GuideSystemApp.Disciplines.IndexType.institute);
                            if (resDisciplines == null)
                            {
                                ShowError("Не найдено.");
                                return;
                            }
                            
                            MessageBox.Show(GetViewDisciplines(resDisciplines));
                            break;
                        case "Поиск конкретной дисциплины":
                            var fields = findDisciplinesVm.FieldInputList.Select(f => f.FieldValue).ToList();
                            if (!Discipline.ValidateDiscipline(fields[0]) && !Discipline.ValiDepartment(fields[1]))
                            {
                                ShowError();
                                return;
                            }
                            var resDisciplinesOne = 
                                _disciplineRepository.FindUnique(fields[0], fields[1]);
                            if (resDisciplinesOne == null)
                            {
                                ShowError("Не найдено.");
                                return;
                            }
                            ShowMessage(GetViewDisciplines(new Comparisons<List<Discipline>>(node:new List<Discipline>{resDisciplinesOne.node}, resDisciplinesOne.k)));
                            break;
                    }
                    break;
                case "Студенты":
                    var findStudents = new FindWindow();
                    var findStudentsVm = new FindWindowVM(new List<SearchModel>()
                    {
                        new SearchModel() { SearchName = "По ФИО", SearchFields = new[] { "ФИО" } },
                        new SearchModel() { SearchName = "По Группе", SearchFields = new[] { "Группа" } },
                        new SearchModel() { SearchName = "По Дате поступления", SearchFields = new[] { "Дата поступления" } },
                        new SearchModel()
                        {
                            SearchName = "Поиск конкретного студента",
                            SearchFields = new[] { "Паспорт"}
                        }
                    });
                    findStudents.DataContext = findStudentsVm;
                    var resStudent = findStudents.ShowDialog();
                    if (!(bool)resStudent)
                        return;
                    List<Student> resStudents = null;
                    switch (findStudentsVm.ComboSelectedItem)
                    {
                        case "По ФИО":
                            resStudents = 
                                _studentRepository.SearchByFIO(findStudentsVm.FieldInputList.First().FieldValue, out int countChecks);
                            if (resStudents == null)
                            {
                                CurrentList = new ObservableCollection<object>();
                                OnPropertyChanged("CurrentList");
                                return;
                            }

                            ShowMessage(GetViewStudents(resStudents, countChecks));
                            break;
                        case "По группе":
                            resStudents = 
                                _studentRepository.SearchByGroup(findStudentsVm.FieldInputList.First().FieldValue, out int countChecks2);
                            if (resStudents == null)
                            {
                                CurrentList = new ObservableCollection<object>();
                                OnPropertyChanged("CurrentList");
                                return;
                            }
                            
                            ShowMessage(GetViewStudents(resStudents, countChecks2));
                            break;
                        case "По Дате поступления":
                            resStudents = 
                                _studentRepository.SearchByAdmissionDate(findStudentsVm.FieldInputList.First().FieldValue, out int countChecks3);
                            if (resStudents == null)
                            {
                                CurrentList = new ObservableCollection<object>();
                                OnPropertyChanged("CurrentList");
                                return;
                            }
                            
                            ShowMessage(GetViewStudents(resStudents, countChecks3));
                            break;
                        case "Поиск конкретного студента":
                            resStudents = new List<Student>()
                            {
                                _studentRepository.SearchByPassport(findStudentsVm.FieldInputList.First().FieldValue,
                                    out int countChecks4)
                            };
                            if (resStudents == null)
                            {
                                CurrentList = new ObservableCollection<object>();
                                OnPropertyChanged("CurrentList");
                                return;
                            }
                            
                            ShowMessage(GetViewStudents(resStudents, countChecks4));
                            break;
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        });
    }
    
    public RelayCommand LogCommand
    {
        get => new RelayCommand(obj =>
        {
            if (SelectedList == null)
            {
                ShowError();
                return;
            }
            
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
                              ShowMessage(_markRepository.GetIndexView(IndexType.Passport));
                              break;
                          case  "Дерево по полю Дисциплины":
                              ShowMessage(_markRepository.GetIndexView(IndexType.Discipline));
                              break;
                          case  "Дерево по полю Даты сдачи":
                              ShowMessage(_markRepository.GetIndexView(IndexType.Date));
                              break;
                          case  "Дерево по полю Оценки":
                              ShowMessage(_markRepository.GetIndexView(IndexType.Value));
                              break;
                          case  "Хеш таблица":
                              ShowMessage(_markRepository.GetUniqueView());
                              break;
                          
                          default:
                              MessageBox.Show("incorrect data");
                              break;
                    }

                    break;

                case "Дисциплины":
                
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
                            ShowMessage(_disciplineRepository.GetIndexView(GuideSystemApp.Disciplines.IndexType.discipline));
                            break;
                        case  "Дерево по полю Департамента":
                            ShowMessage(_disciplineRepository.GetIndexView(GuideSystemApp.Disciplines.IndexType.department));
                            break;
                        case  "Дерево по полю Преподавателя":
                            ShowMessage(_disciplineRepository.GetIndexView(GuideSystemApp.Disciplines.IndexType.teacher));
                            break;
                        case  "Дерево по полю Института":
                            ShowMessage(_disciplineRepository.GetIndexView(GuideSystemApp.Disciplines.IndexType.institute));
                            break;
                        case  "Хеш таблица":
                            ShowMessage(_disciplineRepository.GetUniqueView());
                            break;
                          
                        default:
                            MessageBox.Show("incorrect data");
                            break;
                    }
                    break;
                
                case "Студенты":
                    var comboBoxWindowStudent = new ComboBoxWindow();
                    var vmStudent = new ComboBoxViewModel()
                    {
                        ComboItems = new[]
                        {
                            "Дерево по полю ФИО",
                            "Дерево по полю Группе",
                            "Дерево по полю Дате поступления",
                            "Хеш таблица"
                        }
                    };
                    comboBoxWindowStudent.DataContext = vmStudent;
                    var resDialogStudent = comboBoxWindowStudent.ShowDialog();
                    if(!(bool)resDialogStudent)
                        return;
                    switch (vmStudent.ComboSelectedItem)
                    {
                        case  "Дерево по полю ФИО":
                            MessageBox.Show(_studentRepository.GetStudentFIOString());
                            break;
                        case  "Дерево по полю Группе":
                            MessageBox.Show(_studentRepository.GetStudentGroupString());
                            break;
                        case  "Дерево по полю Дате поступления":
                            MessageBox.Show(_studentRepository.GetStudentAdmissionDateString());
                            break;
                        case  "Хеш таблица":
                            MessageBox.Show(_studentRepository.GetPassportHashTableString());
                            break;
                          
                        default:
                            MessageBox.Show("incorrect data");
                            break;
                    }
                    break;
                default:
                    ShowError();
                    break;
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
                    _studentRepository.ReadFromFile("dataStudents.txt");
                    CurrentList = new ObservableCollection<object>(_studentRepository.GetAll());
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
                    _studentRepository.WriteToFile("dataStudents.txt");
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
    
    private void ShowError(string text)
    {
        MessageBox.Show(text);
    }

    private string GetViewMarks(Comparisons<List<Mark>> marks)
    {
        StringBuilder sb = new StringBuilder();
        int i = 0;
        
        sb.Append($"Поиск занял {marks.k} итераций.");
        sb.Append("Оценки: \n");
        foreach (var mark in marks.node)
        {
            var newMark = mark;
            newMark.Index = i;
            sb.Append(newMark + "\n");
            i++;
        }

        return sb.ToString();
    }
    
    private string GetViewDisciplines(Comparisons<List<Discipline>> disciplines)
    {
        StringBuilder sb = new StringBuilder();
        int i = 0;
        sb.Append($"Поиск занял {disciplines.k} итераций.");
        sb.Append("Дисциплины: \n");
        foreach (var mark in disciplines.node)
        {
            var newMark = mark;
            newMark.Index = i;
            sb.Append(newMark + "\n");
            i++;
        }

        return sb.ToString();
    }
    
    private string GetViewStudents(List<Student> students, int comparisons)
    {
        StringBuilder sb = new StringBuilder();
        int i = 0;
        sb.Append($"Поиск занял {comparisons} итераций.");
        sb.Append("Дисциплины: \n");
        foreach (var mark in students)
        {
            var newMark = mark;
            newMark.Index = i;
            sb.Append(newMark + "\n");
            i++;
        }

        return sb.ToString();
    }

    private void ShowMessage(string message)
    {
        TextBoxView window = new TextBoxView();
        window.DataContext = new TextBoxViewModel()
            { Visibility = Visibility.Collapsed, IsReadOnly = true, TextOutput = message, Height = 300, Width = 300};
        window.ShowDialog();
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