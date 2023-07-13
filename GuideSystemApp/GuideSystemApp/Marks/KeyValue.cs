namespace GuideSystemApp.Marks;

public class KeyValue : IComparable<KeyValue>
{
    public string Key { get; set; }

    public int Value { get; set; }

    public int CompareTo(KeyValue? other)
    { 
        return Key.CompareTo(other.Key);
    }

    public override string ToString()
    {
        return Key;
    }
}