using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;

namespace SmaugCS.Commands
{
    public static class Rent
    {
        public static void do_rent(CharacterInstance ch, string argument)
        {
            ch.SetColor(ATTypes.AT_WHITE);
            ch.SendTo("There is no rent here. Just save and quit.");
        }
    }
}
