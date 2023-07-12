using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuideSystemApp.Student
{
    enum Colors { Black, Red };
    class TreeNode
    {
        public SuperKey Key;
        public Colors Color;
        public TreeNode Parent, Left, Right;
        public TreeNode()
        {
            Color = Colors.Red;
        }
    }
}
