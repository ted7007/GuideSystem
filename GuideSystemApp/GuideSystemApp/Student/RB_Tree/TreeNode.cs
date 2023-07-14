using GuideSystemApp.Student.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuideSystemApp.Student
{
   /* enum Colors { Black, Red };
    class TreeNode
    {
        public LinkedList List;
        public SuperKey Key;
        public Colors Color;
        public TreeNode Parent, Left, Right;
        public TreeNode()
        {
            Color = Colors.Red;
        }
    }*/
    enum Colors { Black, Red };

    class TreeNode
    {
        public LinkedList List;
        public string Key;
        public int value;
        public Colors Color;
        public TreeNode Parent, Left, Right;

        public TreeNode(string key,int value)
        {
            Key = key;
            this.value = value;
            List = new LinkedList();
            Color = Colors.Red;
        }

    }
}
