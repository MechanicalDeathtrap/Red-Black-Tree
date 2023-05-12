using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Red_Black_Tree
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RedBlackTree<int> tree = new RedBlackTree<int>();
            tree.Insert(10);
            tree.Insert(20);
            tree.Insert(5);
            tree.Insert(6);
            tree.Insert(7);
            tree.Insert(23);
            tree.Insert(24);
            //tree.Delete(10);

            tree.Show();
        }
    }
}
