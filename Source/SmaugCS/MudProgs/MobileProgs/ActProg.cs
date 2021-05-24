using SmaugCS.Common;
using SmaugCS.Data.Instances;

namespace SmaugCS.MudProgs.Mobile
{
    public static class ActProg
    {
        public static bool Execute(object[] args)
        {
            var buffer = args.GetValue<string>(0);
            var actor = args.GetValue<CharacterInstance>(1);
            var mob = args.GetValue<MobileInstance>(2);
            var obj = args.GetValue<ObjectInstance>(3);
            // var vo = args[4]

            return false;
        }
    }
}
