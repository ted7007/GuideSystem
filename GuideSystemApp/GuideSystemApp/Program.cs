
using GuideSystemApp.Disciplines;

DisciplineRepository rep = new DisciplineRepository();
rep.ReadFromFile("input_discipline.txt");
Discipline one = new Discipline("Фундаментальные структуры данных и алгоритмы", "Департамент программной инженерии и искусственного интеллекта", "Крестникова Ольга Александровна", "ИМКТ");
rep.Delete(one);
// rep.FindUnique("Дифференциальные уравнения", "Департамент математики");
IndexType type = IndexType.department;
// rep.FindByKey("Департамент математики", type);

Console.WriteLine(rep.GetIndexView(type));
Console.WriteLine(rep.GetIndexView(IndexType.discipline));
Console.WriteLine(rep.GetIndexView(IndexType.institute));
Console.WriteLine(rep.GetIndexView(IndexType.teacher));
Console.WriteLine(rep.GetUniqueView());
Console.WriteLine(1);


