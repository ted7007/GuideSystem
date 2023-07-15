

using GuideSystemApp.Student;
using GuideSystemApp.Disciplines;
var repository = new StudentRepository();

// Чтение данных из файла
repository.ReadFromFile("input_student.txt");

// Получение всех студентов
var allStudents = repository.GetAll();


foreach (var student in allStudents)
{
    Console.WriteLine($"ФИО: {student.FIO}, Группа: {student.Group}, Паспорт: {student.Passport}, Дата поступления: {student.AdmissionDate}");
}

for (int i = 0; i <= 6; i++)
{
    var studentToDelete = allStudents[0];
    repository.Delete(studentToDelete);
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
// Вывод всех студентов до удаления
Console.WriteLine("Список студентов до удаления:");
foreach (var student in repository.GetAll())
{
    Console.WriteLine(student);
}
Console.WriteLine();

// Удаление нескольких студентов
var student1 = new Student("Антонов Борис Владимирович", "Б9121-09.03.04прогин", "1111 444444", "30.06.2020");
var student2 = new Student("Курицын Антон Антонович", "Б9121-09.03.04прогин", "5346 019287", "18.06.2020");
var student3 = new Student("Рудь Владимир Владиславович", "Б9121-09.03.04прогин", "4321 086453", "17.06.2020");

repository.Delete(student1);
repository.Delete(student2);
repository.Delete(student3);

// Вывод всех студентов после удаления
Console.WriteLine("Список студентов после удаления:");
foreach (var student in repository.GetAll())
{
    Console.WriteLine(student);
}
Console.WriteLine();
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


/*DisciplineRepository rep = new DisciplineRepository(1);
rep.ReadFromFile("dataDisciplines.txt");



Discipline discipline1 = new Discipline("Русский язык", "Кафедра Русского языка", "Скурихин Евгений Евгеньевич", "ШРМИ");
Discipline discipline2 = new Discipline("Философия", "Кафедра Философии", "Cмагин Сергей Владимирович", "ПДД");
Discipline discipline3 = new Discipline("Проекты в информационных технологиях", "Департамент программной инженерии и искусственного интеллекта", "Сглыпа Сергей Владимирович", "ИМКТ");
Discipline discipline4 = new Discipline("Геометрия и топология", "Департамент математики", "Скурихин Евгений Евгеньевич", "ШРМИ");
Discipline discipline5 = new Discipline("Алгебра и теория чисел", "Департамент математики", "Cмагин Сергей Владимирович", "ИМКТ");
Discipline discipline6 = new Discipline("Математический анализ", "Департамент математики", "Скурихин Евгений Евгеньевич" ,"ИМКТ");
Discipline discipline7 = new Discipline("Иностранный язык", "Кафедра профессионально-ориентированного перевода", "Лазарева Ирина Николаевна", "ШРМИ");
Discipline discipline8 = new Discipline("Психология в коллективе", "Кафедра психологии и науки", "Cмагин Сергей Владимирович", "ПДД");

Console.WriteLine(rep.GetIndexView(IndexType.department));
Console.WriteLine(rep.GetIndexView(IndexType.discipline));
Console.WriteLine(rep.GetIndexView(IndexType.institute));
Console.WriteLine(rep.GetIndexView(IndexType.teacher));
Console.WriteLine(rep.GetUniqueView());
rep.Delete(discipline1);
rep.Delete(discipline2);
rep.Delete(discipline3);
rep.Delete(discipline4);
rep.Delete(discipline5);
rep.Delete(discipline6);
rep.Delete(discipline7);

Console.WriteLine(rep.GetUniqueView());
Console.WriteLine(rep.GetIndexView(IndexType.department));
Console.WriteLine(rep.GetIndexView(IndexType.discipline));
Console.WriteLine(rep.GetIndexView(IndexType.institute));
Console.WriteLine(rep.GetIndexView(IndexType.teacher));

rep.Delete(discipline8);
// Console.WriteLine(rep.GetIndexView(IndexType.department));
// Console.WriteLine(rep.GetIndexView(IndexType.discipline));
// Console.WriteLine(rep.GetIndexView(IndexType.institute));
// Console.WriteLine(rep.GetIndexView(IndexType.teacher));
Console.WriteLine(rep.GetUniqueView());
// rep.Delete(discipline2);
// Console.WriteLine(rep.GetIndexView(IndexType.department));
// Console.WriteLine(rep.GetIndexView(IndexType.discipline));
// Console.WriteLine(rep.GetIndexView(IndexType.institute));
// Console.WriteLine(rep.GetIndexView(IndexType.teacher));
// Console.WriteLine(rep.GetUniqueView());
rep.FindUnique("Геометрия и топология", "Департамент математик");
// IndexType type = IndexType.institute;
var one = rep.FindByKey("ИМКТ", IndexType.teacher);

Console.WriteLine(1);

*/