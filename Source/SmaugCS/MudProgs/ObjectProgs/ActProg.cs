using SmaugCS.Common;
using SmaugCS.Data.Instances;

namespace SmaugCS.MudProgs.ObjectProgs
{
    public static class ActProg
    {
        public static bool Execute(object[] args)
        {
            var buffer = args.GetValue<string>(0);
            var actObj = args.GetValue<ObjectInstance>(1);
            var mob = args.GetValue<MobileInstance>(2);
            var obj = args.GetValue<ObjectInstance>(3);
            // var vo = args[4]

            return false;
        }
    }
}
