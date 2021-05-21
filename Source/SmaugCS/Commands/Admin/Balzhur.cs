using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using Realm.Library.Common.Extensions;
using SmaugCS.Helpers;
using System.Linq;

namespace SmaugCS.Commands.Admin
{
    public static class Balzhur
    {
        public static void do_balzhur(CharacterInstance ch, string argument)
        {
            ch.SetColor(ATTypes.AT_BLOOD);

            var firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Who is deserving of such a fate?")) return;

            var victim = ch.GetCharacterInWorld(firstArg);
            if (CheckFunctions.CheckIfNullObject(ch, victim, "They aren't currently playing.")) return;
            if (CheckFunctions.CheckIfNpc(ch, victim, "This will do little good on mobiles.")) return;
            if (CheckFunctions.CheckIf(ch, () => ch.Trust <= victim.Level || ch == victim, "I wouldn't even think of that if I were you..."))
                return;

            victim.Level = 2;
            victim.Trust = 0;
            // TODO check_switch(victim, true)
            
            ch.SetColor(ATTypes.AT_WHITE);
            ch.SendTo("You summon the demon Balzhur to wreak your wrath!");
            ch.SendTo("Balzhur sneers at you evilly, then vanishes in a puff of smoke");

            victim.SetColor(ATTypes.AT_IMMORT);
            victim.SendTo("You hear an ungodly sound in the distance that makes your blood run cold!");
            victim.SendTo($"Balzhur screams, 'You are MINE {victim.Name}!!!");

            // echo_to_all(AT_IMMORT, ECHOTAR_ALL)

            victim.Experience = 2000;
            victim.MaximumHealth = 10;
            victim.MaximumMana = 100;
            victim.MaximumMovement = 100;
            ((PlayerInstance)victim).PlayerData.ClearLearnedSkills();
            victim.Practice = 0;
            victim.CurrentHealth = victim.MaximumHealth;
            victim.CurrentMana = victim.MaximumMana;
            victim.CurrentMovement = victim.MaximumMovement;

            //snprintf(buf, MAX_STRING_LENGTH, "%s%s", GOD_DIR, capitalize(victim->name));

            ch.SetColor(ATTypes.AT_RED);

            // TODO act_wiz.c lines 3527 to 3556 (writing it out to data file)

            ((PlayerInstance)victim).AdvanceLevel();
            Help.do_help(victim, "M_BALZHUR_");
            victim.SetColor(ATTypes.AT_WHITE);
            victim.SendTo("You awake after a long period of time...");
            if (victim.Carrying.Any())
            {
                //extract_obj(victim.Carrying.First());
            }    
        }
    }
}
