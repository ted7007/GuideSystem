
using GuideSystemApp.Disciplines;

DisciplineRepository rep = new DisciplineRepository(20);
rep.ReadFromFile("input_discipline.txt");
rep.FindUnique("Геометрия и топология", "Департамент математики");
IndexType type = IndexType.department;
var one = rep.FindByKey("Департамент математики", type);
var two = rep.FindByKey("Кафедра Русского языка", type);

Console.WriteLine(rep.GetIndexView(type));
Console.WriteLine(rep.GetIndexView(IndexType.discipline));
Console.WriteLine(rep.GetIndexView(IndexType.institute));
Console.WriteLine(rep.GetIndexView(IndexType.teacher));
Console.WriteLine(rep.GetUniqueView());
// Console.WriteLine(1);


