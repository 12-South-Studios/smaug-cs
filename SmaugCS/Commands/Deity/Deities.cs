using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Deity
{
    public static class Deities
    {
        public static void do_deities(CharacterInstance ch, string argument)
        {
            if (argument.IsNullOrEmpty())
            {
                DisplayDeityList(ch);
                return;
            }

            DeityData deity = DatabaseManager.Instance.GetEntity<DeityData>(argument);
            if (CheckFunctions.CheckIfNullObject(ch, deity, "&gThat deity does not exist.")) return;

            color.pager_printf_color(ch, "&gDeity:        &G%s", deity.Name);
            color.pager_printf_color(ch, "&gDescription:\n\r&G%s", deity.Description);
        }

        private static void DisplayDeityList(CharacterInstance ch)
        {
            color.send_to_pager_color("&gFor detailed information on a deity, try 'deities <deity>' or 'help deities'", ch);
            color.send_to_pager_color("Deity			Worshippers", ch);

            List<DeityData> deities = DatabaseManager.Instance.DEITIES.Values.ToList();
            if (CheckFunctions.CheckIfTrue(ch, !deities.Any(), "&gThere are no deities on this world.")) return;

            foreach (DeityData deity in deities)
                color.pager_printf_color(ch, string.Format("&G{0}	&g{1}", deity.Name.PadRight(14), deity.Worshippers));
        }
    }
}
