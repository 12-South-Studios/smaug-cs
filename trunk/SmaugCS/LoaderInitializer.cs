using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realm.Library.Common.Extensions;
using SmaugCS.Loaders;

namespace SmaugCS
{
    public static class LoaderInitializer
    {
        private static readonly List<ListLoader> _loaders = new List<ListLoader>();

        public static void Initialize()
        {
            _loaders.Add(new ClassLoader());
            _loaders.Add(new RaceLoader());
            _loaders.Add(new AreaListLoader());
            _loaders.Add(new ClanLoader());
            _loaders.Add(new CouncilLoader());
            _loaders.Add(new DeityListLoader());
            _loaders.Add(new WatchListLoader());

        }

        public static ListLoader GetLoader(string name)
        {
            return _loaders.FirstOrDefault(x => x.Filename.EqualsIgnoreCase(name));
        }

        public static IEnumerable<ListLoader> GetLoaders()
        {
            return _loaders;
        } 

        public static void Load()
        {
            _loaders.ForEach(x => x.Load());
        }
    }
}
