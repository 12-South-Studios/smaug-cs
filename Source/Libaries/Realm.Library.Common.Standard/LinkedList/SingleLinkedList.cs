using System.Collections.Generic;

namespace Realm.Library.Common.LinkedList

{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingleLinkedList<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        public void Push(T p)
        {
            SingleLinkedNode<T> node = new SingleLinkedNode<T> {Data = p};
            if (Head == null)
                Head = node;
            else
            {
                SingleLinkedNode<T> current = Last;
                current.Next = node;
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public SingleLinkedNode<T> Head { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                int total = 0;

                SingleLinkedNode<T> current = Head;
                while (current != null)
                {
                    total++;
                    current = current.Next;
                }
                return total;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public SingleLinkedNode<T> First => Head;

        /// <summary>
        /// 
        /// </summary>
        public SingleLinkedNode<T> Last
        {
            get
            {
                SingleLinkedNode<T> current = Head;
                while (current.Next != null)
                    current = current.Next;
                return current;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEmpty => Count == 0;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SingleLinkedNode<T> Pop()
        {
            SingleLinkedNode<T> current = Head;
            Head = current.Next;
            current.Next = null;
            return current;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reverse()
        {
            SingleLinkedNode<T> current = Head;
            SingleLinkedNode<T> previous = null;

            while (current != null)
            {
                SingleLinkedNode<T> temp = current.Next;
                current.Next = previous;
                previous = current;
                current = temp;
            }

            Head = previous;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> ToEnumerable()
        {
            var list = new List<T>();
            SingleLinkedNode<T> current = Head;

            while (current != null)
            {
                list.Add(current.Data);
                current = current.Next;
            }
            return list;
        }
    }
}
