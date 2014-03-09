using System.Collections.Generic;
using Realm.Library.Common;
using SmaugCS.Data;

// ReSharper disable CheckNamespace
namespace SmaugCS.Data
// ReSharper restore CheckNamespace
{
    public abstract class Instance : Entity
    {
        public string ShortDescription { get; set; }
        public string Description { get; set; }

        public Template Parent { get; set; }
        public List<AffectData> Affects { get; set; }

        public int Timer { get; set; }

        protected Instance(long id, string name)
            : base(id, name)
        {
            Affects = new List<AffectData>();
        }
    }
}
