using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realm.Library.Patterns.Repository;

namespace SmaugCS.Repositories
{
    public class GenericRepository<T> : Repository<long, T> where T : class
    {
    }
}
