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
    }
}
