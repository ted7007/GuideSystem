
using GuideSystemApp.Disciplines;

DisciplineRepository rep = new DisciplineRepository();
rep.ReadFromFile("input_discipline.txt");
rep.FindUnique("Геометрия и топология", "Департамент математики");
IndexType type = IndexType.institute;
var one = rep.FindByKey("ИМКТ", type);

Console.WriteLine(rep.GetIndexView(type));
Console.WriteLine(rep.GetIndexView(IndexType.discipline));
Console.WriteLine(rep.GetIndexView(IndexType.institute));
Console.WriteLine(rep.GetIndexView(IndexType.teacher));
Console.WriteLine(rep.GetUniqueView());
// Console.WriteLine(1);


