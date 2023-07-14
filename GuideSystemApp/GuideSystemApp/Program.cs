
using GuideSystemApp.Disciplines;

DisciplineRepository rep = new DisciplineRepository(20);
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

// Console.WriteLine(1);


