using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;

namespace SmaugCS.MudProgs.Mobile
{
    public static class EntryProg
    {
        public static bool Execute(object[] args)
        {
            var mob = (MobileInstance)args[0];
            if (mob.IsNpc() && mob.MobIndex.HasProg(MudProgTypes.Entry))
                CheckFunctions.CheckIfExecute(mob, MudProgTypes.Entry);
            return true;
        }
    }
}
