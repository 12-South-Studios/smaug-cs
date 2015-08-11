using System;
using System.Collections.Generic;
using Ninject;
using SmaugCS.Common;
using SmaugCS.Loaders.Loaders;
using SmaugCS.Logging;

namespace SmaugCS.Loaders
{
    public class LoaderInitializer : IInitializer
    {
        private readonly List<BaseLoader> _loaders;
        private readonly ILogManager _logManager;
        private readonly IKernel _kernel;

        public LoaderInitializer(ILogManager logManager, IKernel kernel)
        {
            _logManager = logManager;
            _kernel = kernel;
            _loaders = new List<BaseLoader>();
        }

        public void Initialize()
        {
            _loaders.Add(_kernel.Get<BaseLoader>("AreaLoader"));
            _loaders.Add(_kernel.Get<BaseLoader>("ClanLoader"));
            _loaders.Add(_kernel.Get<BaseLoader>("ClassLoader"));
            _loaders.Add(_kernel.Get<BaseLoader>("CouncilLoader"));
            _loaders.Add(_kernel.Get<BaseLoader>("DeityLoader"));
            _loaders.Add(_kernel.Get<BaseLoader>("LanguageLoader"));
            _loaders.Add(_kernel.Get<BaseLoader>("RaceLoader"));
        }

        public void Load()
        {
            foreach (var loader in _loaders)
            {
                try
                {
                    loader.Load();
                    LogManager.Instance.Boot("Loaded {0}", loader.GetType().Name);
                }
                catch (Exception ex)
                {
                    _logManager.Error(ex);
                }
            }
        }
    }
}
