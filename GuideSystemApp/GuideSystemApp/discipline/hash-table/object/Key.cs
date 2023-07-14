public class Key
{
    public string key1;
    public string key2;

    public Key(string key1, string key2)
    {
        this.key1 = key1;
        this.key2 = key2;
    }
    public static bool operator ==(Key key1, Key key2)
    {
        if (ReferenceEquals(key1, null) || ReferenceEquals(key2, null))
        {
            return false;
        }
        if (key1.key1 == key2.key1 && key1.key2 == key2.key2)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    public static bool operator !=(Key key1, Key key2)
    {

        if (key1.key1 != key2.key1 || key1.key2 != key2.key2)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    public string print()
    {
        return $"{key1} {key2}";
    }
}