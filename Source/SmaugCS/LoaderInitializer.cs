using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Loaders;

namespace SmaugCS
{
    public static class LoaderInitializer
    {
        private static readonly List<ListLoader> Loaders = new List<ListLoader>();

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

        public static ListLoader GetLoader(string name)
        {
            return Loaders.FirstOrDefault(x => x.Filename.EqualsIgnoreCase(name));
        }

        public static IEnumerable<ListLoader> GetLoaders()
        {
            return Loaders;
        } 

        public static void Load()
        {
            Loaders.ForEach(x => x.Load());
        }
    }
}
