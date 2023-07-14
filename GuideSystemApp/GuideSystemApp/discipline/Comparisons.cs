

public class Comparisons<T>
{
    public T node;
    public int k;

    public Comparisons(T node, int k)
    {
        this.node = node;
        this.k = k;

    }

    public override string ToString()
    {
        return $"[{k}]: {node}";
    }
}