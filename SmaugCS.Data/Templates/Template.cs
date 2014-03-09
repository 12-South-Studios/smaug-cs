using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Constants.Enums;


// ReSharper disable CheckNamespace
namespace SmaugCS.Data
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Template : Entity, IHasMudProgs
    {
        public string Description { get; set; }
        public long Vnum { get { return ID; } }
        public List<MudProgData> MudProgs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        protected Template(long id, string name)
            : base(id, name)
        {
            MudProgs = new List<MudProgData>();
        }

        #region Implementation of IHasMudProgs

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prog"></param>
        /// <returns></returns>
        public bool HasProg(int prog)
        {
            return MudProgs.Any(x => (int)x.Type == prog);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool HasProg(MudProgTypes type)
        {
            return HasProg((int)type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mprog"></param>
        public void AddMudProg(MudProgData mprog)
        {
            if (!MudProgs.Contains(mprog))
                MudProgs.Add(mprog);
        }

        #endregion
    }
}
