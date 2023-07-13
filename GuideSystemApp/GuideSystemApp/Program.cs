
using GuideSystemApp.Disciplines;

DisciplineRepository rep = new DisciplineRepository();
rep.ReadFromFile("input_discipline.txt");
// Колизии
// Discipline one = new Discipline("Элективные курсы по физической культуре и спорту", "Департамент физического воспитания", "Сухомлинов Максим Дмитриевич", "ШИГН");
// Discipline tree = new Discipline("Дифференциальные уравнения", "Департамент математики" , "Кузнецов Кирилл Сергеевич", "ИМКТ");

// Discipline treen = new Discipline("Геометрия и топология", "Департамент математики", "Скурихин Евгений Евгеньевич", "ИМКТ");
// Discipline two2  = new Discipline("Алгоритмы и теория игр", "Департамент математики", "Рогулин Родион Сергеевич", "ИМКТ");

// Философия/Кафедра Философии/Cмагин Сергей Владимирович/ПДД
// Психология в коллективе/Кафедра психологии и науки/Cмагин Сергей Владимирович/ПДД

Console.WriteLine("eee");
// rep.Add(one);
// rep.Add(two2);
// rep.Add(treen);
// rep.Add(tree);
// rep.FindUnique("Дифференциальные уравнения", "Департамент математики");
IndexType type = IndexType.department;
// rep.FindByKey("Департамент математики", type);

Console.WriteLine(rep.GetIndexView(type));
Console.WriteLine(rep.GetIndexView(IndexType.discipline));
Console.WriteLine(rep.GetIndexView(IndexType.institute));
Console.WriteLine(rep.GetIndexView(IndexType.teacher));
Console.WriteLine(rep.GetUniqueView());
// Console.WriteLine(1);


