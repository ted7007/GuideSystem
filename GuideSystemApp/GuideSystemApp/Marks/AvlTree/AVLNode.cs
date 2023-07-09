namespace GuideSystemApp.Marks.AvlTree;

// Узел дерева АВЛ
public class AVLNode<T> where T : IComparable<T>
{
    public T Value { get; set; }
    public List.LinkedList<T> List { get; set; }
    public AVLNode<T> Left { get; set; }
    public AVLNode<T> Right { get; set; }
    public int Height { get; set; }

    public AVLNode(T value)
    {
        Value = value;
        List = new List.LinkedList<T>();
        List.Add(value); // Добавляем элемент в список
        Height = 1;
    }

    // Возвращает балансировку узла
    public int GetBalance()
    {
        int leftHeight = (Left != null) ? Left.Height : 0;
        int rightHeight = (Right != null) ? Right.Height : 0;
        return rightHeight - leftHeight;
    }

    // Обновляет высоту узла на основе высоты его дочерних узлов
    public void UpdateHeight()
    {
        int leftHeight = (Left != null) ? Left.Height : 0;
        int rightHeight = (Right != null) ? Right.Height : 0;
        Height = Math.Max(leftHeight, rightHeight) + 1;
    }
}
