// See https://aka.ms/new-console-template for more information

using GuideSystemApp.Marks;

Console.WriteLine("Hello, World!");
GuideSystemApp.Marks.List.LinkedList<int> list = new GuideSystemApp.Marks.List.LinkedList<int>();
for(int i = 0; i < 5; i++)
    list.Add(i);
Console.WriteLine(
list.ToString());
MarkRepository repo = new MarkRepository();
int count = 10;
for (int i = 0; i < 10; i++)
{
    var mark = new Mark()
    {
        PassportSerialNumber = $"051{i} 421214",
        Discipline = $"ПРОИН",
        Date = $"2{i}.12.23",
        Value = (MarkEnum)Random.Shared.Next(2, 5)
    };
    repo.Add(mark);
}

Console.WriteLine(repo.GetIndexView(IndexType.Value));

Console.WriteLine(repo.GetUniqueView());