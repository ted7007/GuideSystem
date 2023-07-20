using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuideSystemApp.Student.Hash
{
    public class Item

    {
        public string key;
        public int value;
        public int status;
        public int hash;

        public Item(string key, int value, int hash)
        {
            this.hash = hash;
            this.key = key;
            this.value = value;
            status = 1;
        }
        public Item()
        {
            hash = -1;
            key = null;
            value = -1;
            status = 0;
        }
    }
}
