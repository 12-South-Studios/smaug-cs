using SmaugCS.Common;
using SmaugCS.Data.Instances;

namespace SmaugCS.MudProgs.Mobile
{
    public static class CommandProg
    {
        public static bool Execute(object[] args)
        {
            var actor = args.GetValue<CharacterInstance>(0);
            var txt = args.GetValue<string>(1);

            return false;
        }
    }
}
