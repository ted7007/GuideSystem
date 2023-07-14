// Создание экземпляра класса StudentRepository
using GuideSystemApp.Student;

StudentRepository repository = new StudentRepository();

// Чтение данных из файла и добавление студентов
repository.ReadFromFile("input_student.txt");

// Получение всех студентов
List<Student> allStudents = repository.GetAll();
foreach (var student in allStudents)
{
    Console.WriteLine($"Student: {student.FIO}, Group: {student.Group}, Admission Date: {student.AdmissionDate}, Passport: {student.Passport}");
}

// Добавление нового студента
Student newStudent = new Student("John Doe", "Group A", "AB1234567", "2022-01-01");
repository.Add(newStudent);

// Удаление студента
repository.Delete(newStudent);

// Получение списка студентов по ФИО
List<TreeNode> studentFIOList = repository.GetStudentFIO();
foreach (var node in studentFIOList)
{
    Console.WriteLine($"FIO: {node.Key}, Index: {node.value}");
}

// Получение списка студентов по группе
List<TreeNode> studentGroupList = repository.GetStudentGroup();
foreach (var node in studentGroupList)
{
    Console.WriteLine($"Group: {node.Key}, Index: {node.value}");
}

// Получение списка студентов по дате поступления
List<TreeNode> studentAdmissionDateList = repository.GetStudentAdmissionDate();
foreach (var node in studentAdmissionDateList)
{
    Console.WriteLine($"Admission Date: {node.Key}, Index: {node.value}");
}

// Получение таблицы хешей
List<KeyValuePair<string, int>> hashTable = repository.GetHashTable();
foreach (var entry in hashTable)
{
    Console.WriteLine($"Passport: {entry.Key}, Index: {entry.Value}");
}

// Запись данных в файл
repository.WriteToFile("students_updated.txt");
