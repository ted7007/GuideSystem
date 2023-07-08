using GuideSystemApp.Marks.AvlTree;
using GuideSystemApp.Marks.Hashtable;

namespace GuideSystemApp.Marks;
/// <summary>
/// Репозиторий для работы с сущностью Оценки
/// </summary>
public class MarkRepository
{
    public Mark[] MarkArray { get; set; }

    public HashTable HashTable { get; set; }

    public AVLTree<KeyValue> MarkIndexByPassport { get; set; }

    public AVLTree<KeyValue> MarkIndexByDiscipline { get; set; }

    public AVLTree<KeyValue> MarkIndexByValue { get; set; }

    public AVLTree<KeyValue> MarkIndexByDate { get; set; }

    public MarkRepository(string path)
    {
        
    }

    private void ReadFromFile(string path)
    {
        
    }
    
    public void Add(Mark mark)
    {
        
    }

    public void Delete(Mark mark)
    {
        
    }
    // и т.д.
}