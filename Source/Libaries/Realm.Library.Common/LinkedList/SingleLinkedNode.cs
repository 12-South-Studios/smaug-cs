namespace Realm.Library.Common.LinkedList
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingleLinkedNode<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SingleLinkedNode<T> Next { get; set; } 
    }
}