using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Commands
{
    public static class Ide
    {
        public static void do_ide(CharacterInstance ch, string argument)
        {
            ch.SetColor(ATTypes.AT_PLAIN);
            ch.SendTo("If you want to send an idea, type 'idea <message>'.");
            ch.SendTo("If you want to identify an object, use the identify spell.");
        }
    }
}
