using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuideSystemApp.Student.HashTable
{
    internal class Item

    {


        public SuperKey key;
        public int status;
        public int hash;


        public Item(SuperKey key, int hash)
        {
            this.hash = hash;
            this.key = new SuperKey(key.Key, key.Index);
            status = 1;
        }
        public Item()
        {
            key = new SuperKey("", -1);
            status = 0;
        }
    }
}
