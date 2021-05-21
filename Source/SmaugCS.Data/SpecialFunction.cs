using Realm.Library.Common.Objects;
using SmaugCS.Common;
using SmaugCS.Data.Instances;
using System;

namespace SmaugCS.Data
{
    public class SpecialFunction : Entity
    {
        public SpecialFunction(long id, string name) : base(id, name)
        {
        }

        public Func<MobileInstance, IManager, bool> Value { get; set; }
    }
}
