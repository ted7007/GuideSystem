// Создание экземпляра репозитория
using GuideSystemApp.Student;

var repository = new StudentRepository();

// Чтение данных из файла
repository.ReadFromFile("input_student.txt");

// Получение всех студентов
var allStudents = repository.GetAll();
foreach (var student in allStudents)
{
    Console.WriteLine($"ФИО: {student.FIO}, Группа: {student.Group}, Паспорт: {student.Passport}, Дата поступления: {student.AdmissionDate}");
}

// Добавление нового студента
var newStudent = new Student("Иванов Иван Иванович", "Б9122-09.03.04прогин", "1234 567890", "01.07.2023");
repository.Add(newStudent);

// Поиск студента по ФИО
var studentsByFIO = repository.GetStudentFIO();
foreach (var node in studentsByFIO)
{
    Console.WriteLine($"Студент: {node.Key}, Индекс: {node.value}");
}

// Удаление студента
repository.Delete(newStudent);

// Поиск студента по номеру паспорта
var studentByPassport = repository.Find(new Student("", "", "1234 567890", ""));
if (studentByPassport != -1 && studentByPassport < repository.GetAll().Count)
{
    var foundStudent = repository.GetAll()[studentByPassport];
    Console.WriteLine($"Найденный студент: ФИО: {foundStudent.FIO}, Группа: {foundStudent.Group}, Паспорт: {foundStudent.Passport}, Дата поступления: {foundStudent.AdmissionDate}");
}

// Запись данных в файл
repository.WriteToFile("students_updated.txt");

// Вывод всех студентов в хеш-таблице
var hashTableItems = repository.GetHashTable();
foreach (var item in hashTableItems)
{
    Console.WriteLine($"Ключ: {item.Key}, Значение: {item.Value}");
}