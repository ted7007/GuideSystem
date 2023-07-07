    public class Item
    {
        public Key key;
        public Discipline value;
        public int status;

        public int hash1;

        public int k;

        public Item(Key key, Discipline value, int hash)
        {
            this.k = 0;
            this.hash1 = hash;
            this.key = key;
            this.value = value;
            this.status = 1;
        }
        public Item(Key key)
        {
            this.key = null;
            this.value = null;
            this.status = 2;
        }
        public void print(){
            key.print();
            Console.Write(" => ");
            value.print();
        }
    };