

// using GuideSystemApp.Student;
using GuideSystemApp.Disciplines;
// // Создание экземпляра StudentRepository
// StudentRepository repository = new StudentRepository(20);

// // Чтение данных из файла
// repository.ReadFromFile("input_student.txt");
// string studentFIOTreeString = repository.GetStudentFIOString();
// Console.WriteLine(studentFIOTreeString);

// string studentGroupTreeString = repository.GetStudentGroupString();
// Console.WriteLine(studentGroupTreeString);

// string studentAdmissionDateTreeString = repository.GetStudentAdmissionDateString();
// Console.WriteLine(studentAdmissionDateTreeString);

// string passportHashTableString = repository.GetPassportHashTableString();
// Console.WriteLine(passportHashTableString);


// /*// Вывод всех студентов
// Console.WriteLine("Список всех студентов:");
// var allStudents = repository.GetAll();
// foreach (var student in allStudents)
// {
//     Console.WriteLine(student);
// }
// Console.WriteLine();

// // Удаление студентов
// Console.WriteLine("Удаление студентов:");

// // Удаление студента по индексу
// int indexToDelete = 3; // Пример удаления студента по индексу 3
// if (indexToDelete < allStudents.Count)
// {
//     Student studentToDelete = allStudents[indexToDelete];
//     repository.Delete(studentToDelete);
//     Console.WriteLine($"Студент с индексом {indexToDelete} удален.");
// }
// else
// {
//     Console.WriteLine($"Некорректный индекс: {indexToDelete}");
// }

// // Удаление студента по данным
// string fioToDelete = "Бухалихин Богдан Владиславович";
// string groupToDelete = "Б9121-09.03.04прогин";
// string passportToDelete = "0865 345143";
// string admissionDateToDelete = "13.06.2020";
// Student studentToDelete2 = new Student(fioToDelete, groupToDelete, passportToDelete, admissionDateToDelete);
// repository.Delete(studentToDelete2);
// Console.WriteLine($"Студент {fioToDelete} удален.");

// // Повторный вывод всех студентов после удаления
// Console.WriteLine("\nСписок всех студентов после удаления:");
// allStudents = repository.GetAll();
// foreach (var student in allStudents)
// {
//     Console.WriteLine(student);
// }*/

// // Запись данных в файл
// repository.WriteToFile("output_student.txt");
// /*
DisciplineRepository rep = new DisciplineRepository(20);
rep.ReadFromFile("dataDisciplines.txt");



Discipline discipline1 = new Discipline("Русский язык", "Кафедра Русского языка", "Скурихин Евгений Евгеньевич", "ШРМИ");
// Discipline discipline2 = new Discipline("Философия", "Кафедра Философии", "Cмагин Сергей Владимирович", "ПДД");
// Discipline discipline3 = new Discipline("Проекты в информационных технологиях", "Департамент программной инженерии и искусственного интеллекта", "Сглыпа Сергей Владимирович", "ИМКТ");
// Discipline discipline4 = new Discipline("Геометрия и топология", "Департамент математики", "Скурихин Евгений Евгеньевич", "ШРМИ");
// Discipline discipline5 = new Discipline("Алгебра и теория чисел", "Департамент математики", "Cмагин Сергей Владимирович", "ИМКТ");
// Discipline discipline6 = new Discipline("Математический анализ", "Департамент математики", "Скурихин Евгений Евгеньевич" ,"ИМКТ");
Discipline discipline7 = new Discipline("Иностранный язык", "Кафедра профессионально-ориентированного перевода", "Лазарева Ирина Николаевна", "ШРМИ");
Discipline discipline8 = new Discipline("Психология в коллективе", "Кафедра психологии и науки", "Cмагин Сергей Владимирович", "ПДД");
Discipline discipline9 = new Discipline("Элективные курсы по физической культуре и спорту","Департамент физического воспитания","Сухомлинов Максим Дмитриевич","ШИГН");

Console.WriteLine(rep.GetIndexView(IndexType.department));
Console.WriteLine(rep.GetIndexView(IndexType.discipline));
Console.WriteLine(rep.GetIndexView(IndexType.institute));
Console.WriteLine(rep.GetIndexView(IndexType.teacher));
Console.WriteLine(rep.GetUniqueView());
// rep.Delete(discipline1);
// rep.Delete(discipline2);
// rep.Delete(discipline3);
// rep.Delete(discipline4);
// rep.Delete(discipline5);
// rep.Delete(discipline6);
// rep.Delete(discipline7);

Console.WriteLine(rep.GetUniqueView());
Console.WriteLine(rep.GetIndexView(IndexType.department));
Console.WriteLine(rep.GetIndexView(IndexType.discipline));
Console.WriteLine(rep.GetIndexView(IndexType.institute));
Console.WriteLine(rep.GetIndexView(IndexType.teacher));

rep.Delete(discipline7);
rep.Delete(discipline9);
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

// */
// var a = Convert.ToInt32('А');
// Console.WriteLine(a);