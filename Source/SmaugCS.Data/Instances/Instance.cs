using System.Collections.Generic;
using Realm.Library.Common;
using SmaugCS.Data.Templates;

namespace SmaugCS.Data.Instances
{
    public abstract class Instance : Entity
    {
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public Template Parent { get; set; }
        public ICollection<AffectData> Affects { get; private set; }
        public int Level { get; set; }
        public int Timer { get; set; }

        protected Instance(long id, string name)
            : base(id, name)
        {
            Affects = new List<AffectData>();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != GetType()) return false;

            var objToCheck = (Instance) obj;
            return (objToCheck.ID == ID) && objToCheck.Name.Equals(Name);
        }

        public override int GetHashCode()
        {
            var hash = 13;
            hash = hash * 7 + ID.GetHashCode();
            hash = hash * 7 + Name.GetHashCode();
            return hash;
        }

        public static bool operator ==(Instance a, Instance b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (((object)a == null) || ((object)b == null)) return false;
            return a.ID == b.ID && a.Name.Equals(b.Name);
        }

        public static bool operator !=(Instance a, Instance b) => !(a == b);
    }
}
