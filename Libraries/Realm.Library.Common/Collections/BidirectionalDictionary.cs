using System.Collections.Generic;
using System.Linq;

namespace Realm.Library.Common.Collections
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Used without permission from http://timbar.blogspot.com/2011/01/c-bidirectional-lookup.html</remarks>
    /// <typeparam name="TFirst"></typeparam>
    /// <typeparam name="TSecond"></typeparam>
    public class BidirectionalDictionary<TFirst, TSecond>
    {
        private IDictionary<TFirst, IEnumerable<TSecond>> Forward { get; set; }
        private IDictionary<TSecond, IEnumerable<TFirst>> Backward { get; set; }

        private static readonly IEnumerable<TFirst> EmptyFirstList = new List<TFirst>();
        private static readonly IEnumerable<TSecond> EmptySecondList = new List<TSecond>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="forward"></param>
        /// <param name="backward"></param>
        public BidirectionalDictionary(IDictionary<TFirst, IEnumerable<TSecond>> forward, IDictionary<TSecond, IEnumerable<TFirst>> backward)
        {
            Forward = forward;
            Backward = backward;
        }

        /// <summary>
        /// 
        /// </summary>
        public BidirectionalDictionary()
        {
            Forward = new Dictionary<TFirst, IEnumerable<TSecond>>();
            Backward = new Dictionary<TSecond, IEnumerable<TFirst>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public void Add(TFirst first, TSecond second)
        {
            IEnumerable<TSecond> secondList;
            if (!Forward.TryGetValue(first, out secondList))
            {
                secondList = new List<TSecond>();
                Forward[first] = secondList;
            }

            IEnumerable<TFirst> firstList;
            if (!Backward.TryGetValue(second, out firstList))
            {
                firstList = new List<TFirst>();
                Backward[second] = firstList;
            }

            Forward[first] = secondList.Append(second);
            Backward[second] = firstList.Append(first);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="first"></param>
        /// <returns></returns>
        public IEnumerable<TSecond> GetByFirst(TFirst first)
        {
            return Forward.ContainsKey(first) ? Forward[first] : EmptySecondList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        public IEnumerable<TFirst> GetBySecond(TSecond second)
        {
            return Backward.ContainsKey(second) ? Backward[second] : EmptyFirstList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public void Remove(TFirst first, TSecond second)
        {
            IEnumerable<TSecond> secondList;
            if (Forward.TryGetValue(first, out secondList))
            {
                if (secondList.Contains(second))
                {
                    secondList.ToList().Remove(second);
                    if (secondList.Any())
                        Forward[first] = secondList;
                    else
                        Forward.Remove(first);
                }
                else
                    Forward.Remove(first);
            }

            IEnumerable<TFirst> firstList;
            if (Backward.TryGetValue(second, out firstList))
            {
                if (firstList.Contains(first))
                {
                    firstList.ToList().Remove(first);
                    if (firstList.Any())
                        Backward[second] = firstList;
                    else
                        Backward.Remove(second);
                }
                else
                    Backward.Remove(second);
            }
        }
    }
}
