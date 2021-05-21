using System.Collections.Concurrent;
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
        private IDictionary<TFirst, IEnumerable<TSecond>> Forward { get; }
        private IDictionary<TSecond, IEnumerable<TFirst>> Backward { get; }

        private static readonly ConcurrentBag<TFirst> EmptyFirstList = new ConcurrentBag<TFirst>();
        private static readonly ConcurrentBag<TSecond> EmptySecondList = new ConcurrentBag<TSecond>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="forward"></param>
        /// <param name="backward"></param>
        public BidirectionalDictionary(IDictionary<TFirst, IEnumerable<TSecond>> forward,
            IDictionary<TSecond, IEnumerable<TFirst>> backward)
        {
            Forward = forward;
            Backward = backward;
        }

        /// <summary>
        /// 
        /// </summary>
        public BidirectionalDictionary()
        {
            Forward = new ConcurrentDictionary<TFirst, IEnumerable<TSecond>>();
            Backward = new ConcurrentDictionary<TSecond, IEnumerable<TFirst>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public void Add(TFirst first, TSecond second)
        {
            if (!Forward.TryGetValue(first, out IEnumerable<TSecond> secondList))
            {
                secondList = new List<TSecond>();
                Forward[first] = secondList;
            }

            if (!Backward.TryGetValue(second, out IEnumerable<TFirst> firstList))
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
            if (Forward.TryGetValue(first, out IEnumerable<TSecond> secondList))
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

            if (!Backward.TryGetValue(second, out IEnumerable<TFirst> firstList)) return;
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
