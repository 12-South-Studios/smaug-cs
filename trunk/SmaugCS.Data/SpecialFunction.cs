using System;
using Realm.Library.Common;
using SmaugCS.Data;

namespace SmaugCS.Data
{
    public class SpecialFunction : Entity
    {
        public SpecialFunction(long id, string name) : base(id, name)
        {
        }

        public Func<CharacterInstance, bool> Value { get; set; }
    }
}
