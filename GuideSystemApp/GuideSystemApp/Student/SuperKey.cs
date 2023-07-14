using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuideSystemApp.Student
{
    public class SuperKey
    {
        public string Key;
        public int Index;
        public SuperKey(string key, int index)
        {
            Key = key;
            Index = index;
        }
        public SuperKey()
        {
            // Пустой конструктор без параметров
        }
    }
}
