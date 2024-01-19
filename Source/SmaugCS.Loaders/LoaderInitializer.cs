using SmaugCS.Common;
using SmaugCS.Loaders.Loaders;
using SmaugCS.Logging;
using System;
using System.Collections.Generic;
using Autofac;

namespace SmaugCS.Loaders
{
    public class LoaderInitializer : IInitializer
    {
        private IEnumerable<IBaseLoader> _loaders;
        private readonly ILogManager _logManager;

        public LoaderInitializer(ILogManager logManager)
        {
            _logManager = logManager;
        }

        public void Initialize()
        {
            //_loaders.Add(_kernel.Get<BaseLoader>("AreaLoader"));
            //_loaders.Add(_kernel.Get<BaseLoader>("ClanLoader"));
            //_loaders.Add(_kernel.Get<BaseLoader>("ClassLoader"));
            //_loaders.Add(_kernel.Get<BaseLoader>("CouncilLoader"));
            //_loaders.Add(_kernel.Get<BaseLoader>("DeityLoader"));
            //_loaders.Add(_kernel.Get<BaseLoader>("LanguageLoader"));
            //_loaders.Add(_kernel.Get<BaseLoader>("RaceLoader"));
        }

        public void Load(ILogManager logManager)
        {
            //foreach (var loader in _loaders)
            //{
            //    try
            //    {
            //        loader.Load();
            //        logManager.Boot($"Loaded {loader.GetType().Name}");
            //    }
            //    catch (Exception ex)
            //    {
            //        _logManager.Error(ex);
            //    }
            //}
        }
    }
}
