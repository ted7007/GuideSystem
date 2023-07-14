public class Item
{
    public Key key;
    public int value;
    public int status;

    public int hash1;

    public int k;

    public Item(Key key, int value, int hash)
    {
        this.k = 0;
        this.hash1 = hash;
        this.key = key;
        this.value = value;
        this.status = 1;
    }
    public Item()
    {
        this.k = 0;
        this.hash1 = -1;
        this.key = null;
        this.value = -1;
        this.status = 0;
    }
    public Item(Key key)
    {
        this.key = null;
        this.value = -1;
        this.status = 2;
    }
    public string print()
    {
        string res;
        res = key.print() + " =>" + value;
        return res;
    }
};