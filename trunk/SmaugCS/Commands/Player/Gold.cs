using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Commands.Player
{
    public static class Gold
    {
        public static void do_gold(CharacterInstance ch, string argument)
        {
           ch.SetColor(ATTypes.AT_GOLD);
            ch.Printf("You have {0} gold pieces.", ch.CurrentCoin.ToPunctuation());
        }
    }
}
