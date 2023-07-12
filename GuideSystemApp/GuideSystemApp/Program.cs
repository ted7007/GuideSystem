
using GuideSystemApp.Disciplines;

DisciplineRepository rep = new DisciplineRepository();
rep.ReadFromFile("input_discipline.txt");
Discipline one = new Discipline("Фундаментальные структуры данных и алгоритмы", "Департамент программной инженерии и искусственного интеллекта", "Крестникова Ольга Александровна", "ИМКТ");
rep.Delete(one);


