using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Data.Templates
{
    public abstract class Template : Entity, IHasMudProgs
    {
        public string Description { get; set; }

        public long Vnum
        {
            get { return ID; }
        }

        private readonly List<MudProgData> _mudProgs;

        public IEnumerable<MudProgData> MudProgs
        {
            get { return _mudProgs; }
        }

        protected Template(long id, string name)
            : base(id, name)
        {
            _mudProgs = new List<MudProgData>();
        }

        #region Implementation of IHasMudProgs

        public bool HasProg(int prog)
        {
            return MudProgs.Any(x => (int)x.Type == prog);
        }

        public bool HasProg(MudProgTypes type)
        {
            return HasProg((int)type);
        }

        public void AddMudProg(MudProgData mprog)
        {
            if (!MudProgs.Contains(mprog))
                _mudProgs.Add(mprog);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != GetType()) return false;

            var objToCheck = (Template)obj;
            return ((objToCheck.ID == ID) && (objToCheck.Name.Equals(Name)));
        }

        public override int GetHashCode()
        {
            var hash = 13;
            hash = (hash * 7) + ID.GetHashCode();
            hash = (hash * 7) + Name.GetHashCode();
            return hash;
        }

        public static bool operator ==(Template a, Template b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (((object)a == null) || ((object)b == null)) return false;
            return a.ID == b.ID && a.Name.Equals(b.Name);
        }

        public static bool operator !=(Template a, Template b)
        {
            return !(a == b);
        }
    }
}
