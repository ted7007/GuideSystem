using System.Security.AccessControl;
using GuideSystemApp.Marks;

namespace Lab2_3;

public class Node
{
    public Mark Value { get; set; }

    public NodeStatus Status { get; set; }

    public int? Hash1 { get; set; }

    public int? Hash2 { get; set; }

    public Node()
    {
        Status = NodeStatus.Free;
    }

    public Node(Mark value, int? hash1, int? hash2)
    {
        Value = value;
        Status = NodeStatus.Taken;
        Hash1 = hash1;
        Hash2 = hash2;
    }

    public void Clear()
    {
        Status = NodeStatus.Free;
    }

    public void Set(Node value)
    {
        Value = value.Value;
        Status = value.Status;
        Hash1 = value.Hash1;
        Hash2 = value.Hash2;
    }
}