
public class HachTable
{
    private Item[] items;
    private int size;
    private int count;

    private int initial_size;

    public HachTable(int size)
    {
        items = new Item[size];
        this.size = size;
        this.count = 0;
        this.initial_size = size;

    }
    public void Remove(Key key, Discipline value)
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
            Increase(item,"remove");
        }
    }
    public void Add(Key key, Discipline value)
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
            Collision collision = new Collision(hash, size);
            collision.CollisionAdd(item, ref items, ref count);
        }
        float capacity = (float)count / size;
        if (capacity > 0.75)
        {
            Increase(item,"add");
        }
    }
    public Discipline Search(Key key)
    {
        int hash = HachOne(key);
        if (key == items[hash].key)
        {
            return items[hash].value;
        }
        else
        {
            Collision collision = new Collision(hash, size);
            return collision.CollisionSearch(key, items);
        }
    }
    public void Print()
    {
        for (int j = 0; j < size; j++)
        {
            if (items[j].status == 1){
                Console.Write($"{j}|");
                items[j].print();
                Console.WriteLine($"|{items[j].status}|hash={items[j].hash1}|шаг={items[j].k} ");
            }
            else
            {
                Console.WriteLine($"{j}|{items[j].status}");
            }

        }

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
        for (int j = 0; j < p; j++)
        {
            if (items[j].status == 1)
            {
                int hash = HachOne(items[j].key);
                if (arr[hash].status == 0)
                {
                    item = new Item(items[j].key, items[j].value, hash);
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
