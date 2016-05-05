using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Data.Templates;

namespace SmaugCS.Data.Instances
{
    public class MobileInstance : CharacterInstance
    {
        public HuntHateFearData CurrentHunting { get; set; }
        public HuntHateFearData CurrentFearing { get; set; }
        public HuntHateFearData CurrentHating { get; set; }
        public SpecialFunction SpecialFunction { get; set; }
        public string SpecialFunctionName { get; set; }
        public ExtendedBitvector Attacks { get; set; }

        public override int Trust
        {
            get { return Level; }
            set { Trust = value; }
        }

        public MobileInstance(int id, string name) : base(id, name)
        {
        }

        public MobileTemplate MobIndex => Parent.CastAs<MobileTemplate>();
    }
}
