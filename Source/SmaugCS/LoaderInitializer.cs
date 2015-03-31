using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Loaders;

namespace SmaugCS
{
    /// <summary>
    /// 
    /// </summary>
    public static class LoaderInitializer
    {
        private static readonly List<ListLoader> Loaders = new List<ListLoader>();

        /// <summary>
        /// 
        /// </summary>
        public static void Initialize()
        {
            Loaders.Add(new ClassLoader());
            Loaders.Add(new RaceLoader());
            Loaders.Add(new AreaListLoader());
            Loaders.Add(new ClanLoader());
            Loaders.Add(new CouncilLoader());
            Loaders.Add(new DeityListLoader());
            //Loaders.Add(new WatchListLoader());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ListLoader GetLoader(string name)
        {
            return Loaders.FirstOrDefault(x => x.Filename.EqualsIgnoreCase(name));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ListLoader> GetLoaders()
        {
            return Loaders;
        } 

        /// <summary>
        /// 
        /// </summary>
        public static void Load()
        {
            Loaders.ForEach(x => x.Load());
        }
    }
}
