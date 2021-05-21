using SmaugCS.Data.Instances;

namespace SmaugCS.MudProgs.MobileProgs
{
    public static class GiveProg
    {
        public static bool Execute(object[] args)
        {
            var mob = (MobileInstance)args[0];
            var ch = (CharacterInstance)args[1];
            var obj = (ObjectInstance)args[2];

            return false;
        }
    }
}
