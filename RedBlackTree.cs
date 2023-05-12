using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Red_Black_Tree
{
    internal class RedBlackTree<T> where T : IComparable<T>
    {
        private Node<T> root;

        public void Insert(T value)
        {
            var newNode = new Node<T>(value);

            // Если дерево пустое, создаем первую ноду - корень
            if (root == null)
            {
                root = newNode;
                root.IsBlack = true; //=> корень всегда черный
            }
            else
            {
                InsertNode(newNode);
                BalanceAfterInsertion(newNode);
            }
        }

        private void InsertNode(Node<T> newNode)
        {
            Node<T> current = root;
            Node<T> parent = null;

            // Находим место вставки
            while (current != null)
            {
                parent = current;
                if (newNode.Value.CompareTo(current.Value) < 0)
                    current = current.Left;
                else
                    current = current.Right;
            }

            // Родительский узел становится родительским для нового узла
            newNode.Parent = parent;

            // Вставка
            if (parent == null)
                root = newNode;
            else if (newNode.Value.CompareTo(parent.Value) < 0)
                parent.Left = newNode;
            else
                parent.Right = newNode;
        }
        private void BalanceAfterInsertion(Node<T> node)
        {
            while (node != root && node.Parent.IsBlack == false)
            {
                // Если родительский узел (node.Parent) является левым узлом для своего отца
                //L case
                if (node.Parent == node.Parent.Parent.Left) 
                {
                    Node<T> uncle = node.Parent.Parent.Right;

                    // LL case
                    if (uncle != null && uncle.IsBlack == false) 
                    {
                        node.Parent.IsBlack = true;
                        uncle.IsBlack = true;
                        node.Parent.Parent.IsBlack = false;
                        node = node.Parent.Parent;
                    }
                    else // LR case
                    {
                        if (node == node.Parent.Right)
                        {
                            node = node.Parent;
                            RotateLeft(node);
                        }

                        node.Parent.IsBlack = true;
                        node.Parent.Parent.IsBlack = false;
                        RotateRight(node.Parent.Parent);
                    }
                }
                else //R case
                {
                    Node<T> uncle = node.Parent.Parent.Left;

                    //RR case
                    if (uncle != null && uncle.IsBlack == false) 
                    {
                        node.Parent.IsBlack = true;
                        uncle.IsBlack = true;
                        node.Parent.Parent.IsBlack = false;
                        node = node.Parent.Parent;
                    }
                    //RL case
                    else 
                    {
                        if (node == node.Parent.Left)
                        {
                            node = node.Parent;
                            RotateRight(node);
                        }

                        node.Parent.IsBlack = true;
                        node.Parent.Parent.IsBlack = false;
                        RotateLeft(node.Parent.Parent);
                    }
                }
            }

            root.IsBlack = true;
        }
        private void RotateLeft(Node<T> node)
        {
            Node<T> rightChild = node.Right;
            node.Right = rightChild.Left;

            if (rightChild.Left != null)
                rightChild.Left.Parent = node;

            rightChild.Parent = node.Parent;

            if (node.Parent == null)
                root = rightChild;
            else if (node == node.Parent.Left)
                node.Parent.Left = rightChild;
            else
                node.Parent.Right = rightChild;

            rightChild.Left = node;
            node.Parent = rightChild;
        }

        private void RotateRight(Node<T> node)
        {
            Node<T> leftChild = node.Left;
            node.Left = leftChild.Right;

            if (leftChild.Right != null)
                leftChild.Right.Parent = node;

            leftChild.Parent = node.Parent;

            if (node.Parent == null)
                root = leftChild;
            else if (node == node.Parent.Left)
                node.Parent.Left = leftChild;
            else
                node.Parent.Right = leftChild;

            leftChild.Right = node;
            node.Parent = leftChild;
        }

        public bool Search(T value)
        {
            return SearchNode(value, root) != null;
        }

        private Node<T> SearchNode(T value, Node<T> node)
        {
            if (node == null || value.CompareTo(node.Value) == 0)
                return node;

            if (value.CompareTo(node.Value) < 0)
                return SearchNode(value, node.Left);
            else
                return SearchNode(value, node.Right);
        }

        public void Delete(T value)
        {
            Node<T> node = SearchNode(value, root);

            if (node == null)
                return;

            Node<T> replacementNode;

            // У ноды два ребёнка
            if (node.Left != null && node.Right != null)
            {
                // Находим преемника для замены (мин значение в правом поддереве)
                Node<T> successor = FindSuccessor(node);
                node.Value = successor.Value;
                node = successor;
            }

            // по одному ребёнку
            // Здесь замена удаляемого элемента - его дочерний
            if (node.Left != null)
                replacementNode = node.Left;
            else
                replacementNode = node.Right;

            if (replacementNode != null)
                replacementNode.Parent = node.Parent;

            if (node.Parent == null)
                root = replacementNode;
            else if (node == node.Parent.Left)
                node.Parent.Left = replacementNode;
            else
                node.Parent.Right = replacementNode;

            if (node.IsBlack == true)
                BalanceAfterDeletion(replacementNode, node.Parent);
        }

        private void BalanceAfterDeletion(Node<T> node, Node<T> parent)
        {
            while (node != root && (node == null || node.IsBlack == true))
            {
                if (node == parent.Left)
                {
                    Node<T> sibling = parent.Right;

                    if (sibling.IsBlack == false)
                    {
                        sibling.IsBlack = true;
                        parent.IsBlack = false;
                        RotateLeft(parent);
                        sibling = parent.Right;
                    }

                    if ((sibling.Left == null || sibling.Left.IsBlack == true) &&
                        (sibling.Right == null || sibling.Right.IsBlack == true))
                    {
                        sibling.IsBlack = false;
                        node = parent;
                        parent = node.Parent;
                    }
                    else
                    {
                        if (sibling.Right == null || sibling.Right.IsBlack == true)
                        {
                            sibling.Left.IsBlack = true;
                            sibling.IsBlack = false;
                            RotateRight(sibling);
                            sibling = parent.Right;
                        }

                        sibling.IsBlack = parent.IsBlack;
                        parent.IsBlack = true;
                        sibling.Right.IsBlack = true;
                        RotateLeft(parent);
                        node = root;
                        break;
                    }
                }
                else
                {
                    Node<T> sibling = parent.Left;

                    if (sibling.IsBlack == false)
                    {
                        sibling.IsBlack = true;
                        parent.IsBlack = false;
                        RotateRight(parent);
                        sibling = parent.Left;
                    }

                    if ((sibling.Left == null || sibling.Left.IsBlack == true) &&
                        (sibling.Right == null || sibling.Right.IsBlack == true))
                    {
                        sibling.IsBlack = false;
                        node = parent;
                        parent = node.Parent;
                    }
                    else
                    {
                        if (sibling.Left == null || sibling.Left.IsBlack == true)
                        {
                            sibling.Right.IsBlack = true;
                            sibling.IsBlack = false;
                            RotateLeft(sibling);
                            sibling = parent.Left;
                        }

                        sibling.IsBlack = parent.IsBlack;
                        parent.IsBlack = true;
                        sibling.Left.IsBlack = true;
                        RotateRight(parent);
                        node = root;
                        break;
                    }
                }
            }

            if (node != null)
                node.IsBlack = true;
        }

        // Поиск преемника для замены значения при удалении ноды с двумя дочерними элементами
        private Node<T> FindSuccessor(Node<T> node)
        {
            if (node.Right != null)
            {
                node = node.Right;
                while (node.Left != null)
                    node = node.Left;
                return node;
            }

            Node<T> parent = node.Parent;
            while (parent != null && node == parent.Right)
            {
                node = parent;
                parent = parent.Parent;
            }
            return parent;
        }
    }
}
