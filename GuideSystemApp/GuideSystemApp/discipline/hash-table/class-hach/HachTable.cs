
public class HachTable
{
    private Item[] items;
    private int size;

    public int k = 1;
    private int count;

    private int initial_size;

    public HachTable(int size)
    {
        items = new Item[size];
        for (int i = 0; i < size; i++)
        {
            items[i] = new Item();
        }
        this.size = size;
        this.count = 0;
        this.initial_size = size;

    }

    public void Edit(Key key, int res)
    {
        int hash = HachOne(key);
        if (key == items[hash].key)
        {
            items[hash].value = res;
        }
        else
        {
            Collision collision = new Collision(hash, size);
            collision.CollisionEdit(key, items, res);
        }
    }
    public void Remove(Key key, int value)
    {
        bool flag = true;
        int hash = HachOne(key);
        Item item = new Item(key);
        if (key == items[hash].key)
        {
            if (value == items[hash].value)
            {
                items[hash] = item;
                this.count--;
            }
            else
            {
                flag = false;
            }

        }
        else
        {
            if (flag)
            {
                Collision collision = new Collision(hash, size);
                collision.CollisionRemove(key, value, ref items, ref count);
            }

        }
        float capacity = (float)count / size;
        if (capacity < 0.25 && initial_size < size)
        {
            Increase(item, "remove");
        }
    }
    public void Add(Key key, int value)
    {
        int hash = HachOne(key);

        Item item = new Item(key, value, hash);

        if (items[hash].status == 0)
        {
            items[hash] = item;
            this.count++;
        }
        else
        {
            if (key != items[hash].key)
            {
                Collision collision = new Collision(hash, size);
                collision.CollisionAdd(item, ref items, ref count);
            }
        }
        float capacity = (float)count / size;
        if (capacity > 0.7)
        {
            Increase(item, "add");
        }

    }
    public int Search(Key key)
    {
        int hash = HachOne(key);
        if (key == items[hash].key)
        {
            return items[hash].value;
        }
        else
        {
            Collision collision = new Collision(hash, size);
            k = collision.j + 1;
            return collision.CollisionSearch(key, items);
        }
    }
    public string Print()
    {
        string res = "";
        for (int j = 0; j < size; j++)
        {
            if (items[j].status == 1)
            {
                res = res + $"{j}|" + items[j].print() + $"|{items[j].status}|hash={items[j].hash1}|шаг={items[j].k}";
                res = res + "\n";
            }
            else
            {
                res = res + $"{j}|{items[j].status}";
                res = res + "\n";
            }

        }
        return res;

    }
    private int HachOne(Key key)
    {
        string str = key.key1 + key.key2;
        int i = 0;
        for (int j = 0; j < str.Length; j++)
            i += str[j];

        return i % this.size;

    }
    private void Increase(Item item, string now)
    {
        int p;
        count = 0;
        if (now == "add")
        {
            this.size = this.size * 2;
            p = size / 2;
        }
        else
        {
            this.size = this.size / 2;
            p = size * 2;
        }
        Item[] arr = new Item[size];
        for (int i = 0; i < size; i++)
        {
            arr[i] = new Item();
        }
        for (int j = 0; j < p; j++)
        {
            if (items[j].status == 1)
            {
                int hash = HachOne(items[j].key);
                item = new Item(items[j].key, items[j].value, hash);
                if (arr[hash].status == 0)
                {
                    arr[hash] = item;
                    count++;
                }
                else
                {
                    Collision collision = new Collision(hash, size);
                    collision.CollisionAdd(item, ref arr, ref count);
                }
            }
        }
        items = new Item[size];
        Array.Copy(arr, items, items.Length);

    }
}
