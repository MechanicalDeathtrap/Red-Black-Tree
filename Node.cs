using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Red_Black_Tree
{

    internal class Node<T> where T : IComparable<T>
    {
        public T Value { get; set; }
        public Node<T> Parent { get; set; }
        public Node<T> Left { get; set; }
        public Node<T> Right { get; set; }
        public bool IsBlack { get; set; }

        public Node(T value)
        {
            Value = value;
            Parent = null;
            Left = null;
            Right = null;
            IsBlack = false; // Новые ноды всегда красные
        }

        public override string ToString()
        {
            return $"{Value} IsBlack:({IsBlack})";
        }
    }
}
