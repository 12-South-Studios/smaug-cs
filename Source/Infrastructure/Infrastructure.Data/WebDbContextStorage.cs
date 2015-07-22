using System.Collections.Generic;
using System.Data.Entity;
using System.Web;

namespace Infrastructure.Data
{
    public class WebDbContextStorage : IDbContextStorage
    {
        public WebDbContextStorage(HttpApplication app)
        {
            app.EndRequest += (sender, args) =>
            {
                DbContextManager.CloseAllDbContexts();
                HttpContext.Current.Items.Remove(StorageKey);
            };
        }

        public DbContext GetDbContextForKey(string key)
        {
            SimpleDbContextStorage storage = GetSimpleDbContextStorage();
            return storage.GetDbContextForKey(key);
        }

        public void SetDbContextForKey(string factoryKey, DbContext context)
        {
            SimpleDbContextStorage storage = GetSimpleDbContextStorage();
            storage.SetDbContextForKey(factoryKey, context);
        }

        public IEnumerable<DbContext> GetAllDbContexts()
        {
            SimpleDbContextStorage storage = GetSimpleDbContextStorage();
            return storage.GetAllDbContexts();
        }

        private SimpleDbContextStorage GetSimpleDbContextStorage()
        {
            HttpContext context = HttpContext.Current;
            SimpleDbContextStorage storage = context.Items[StorageKey] as SimpleDbContextStorage;
            if (storage != null) return storage;

            storage = new SimpleDbContextStorage();
            context.Items[StorageKey] = storage;
            return storage;
        }

        private const string StorageKey = "HttpContextObjectContextStorageKey";
    }
}
