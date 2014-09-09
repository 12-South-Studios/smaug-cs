using System;
using Realm.Library.Common;
using SmaugCS.Data.Instances;

namespace SmaugCS.Data
{
    public class SpecialFunction : Entity
    {
        public SpecialFunction(long id, string name) : base(id, name)
        {
        }

        public Func<MobileInstance, bool> Value { get; set; }
    }
}
