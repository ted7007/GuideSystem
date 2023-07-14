using GuideSystemApp.Student;

StudentRepository repository = new StudentRepository();

// Чтение данных из файла
repository.ReadFromFile("input_student.txt");

// Вывод списка студентов перед удалением
Console.WriteLine("Список студентов перед удалением:");
repository.PrintStudentFIO();
repository.PrintStudentGroup();
repository.PrintStudentAdmissionDate();
repository.PrintHashTable();

// Удаление студента по объекту
Student studentToDelete = new Student("Бухалихин Богдан Владиславович", "Б9121-09.03.04прогин", "2985 987421", "13.06.2020");
repository.Delete(studentToDelete);

// Вывод списка студентов после удаления
Console.WriteLine("Список студентов после удаления:");
repository.PrintStudentFIO();
repository.PrintStudentGroup();
repository.PrintStudentAdmissionDate();
repository.PrintHashTable();

// Добавление нового студента
Student newStudent = new Student("Иванов Иван Иванович", "Б9121-09.03.04прогин", "1234 567890", "01.07.2020");
repository.Add(newStudent);

// Вывод списка студентов после добавления
Console.WriteLine("Список студентов после добавления:");
repository.PrintStudentFIO();
repository.PrintStudentGroup();
repository.PrintStudentAdmissionDate();
repository.PrintHashTable();

// Запись данных в файл
repository.WriteToFile("output_student.txt");