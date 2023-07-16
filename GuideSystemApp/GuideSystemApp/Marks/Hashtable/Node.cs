namespace GuideSystemApp.Marks.Hashtable;

public class Node
{
    public string Key { get; set; }
    
    public int Value { get; set; }

    public NodeStatus Status { get; set; }

    public int? Hash1 { get; set; }

    public int? Hash2 { get; set; }

    public Node()
    {
        Status = NodeStatus.Free;
    }

    public Node(string key, int value, int? hash1, int? hash2)
    {
        Key = key;
        Value = value;
        Status = NodeStatus.Taken;
        Hash1 = hash1;
        Hash2 = hash2;
    }

    public void Clear()
    {
        Status = NodeStatus.Free;
        Key = null;
    }

    public void Set(Node value)
    {
        Key = value.Key;
        Value = value.Value;
        Status = value.Status;
        Hash1 = value.Hash1;
        Hash2 = value.Hash2;
    }
}