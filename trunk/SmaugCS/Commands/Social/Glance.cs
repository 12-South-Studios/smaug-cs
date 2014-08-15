using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Social
{
   public static class Glance
    {
       public static void do_glance(CharacterInstance ch, string argument)
       {
           if (CheckFunctions.CheckIfNullObject(ch, ch.Descriptor)) return;
           if (CheckFunctions.CheckIfTrue(ch, (int)ch.CurrentPosition < (int) PositionTypes.Sleeping,
               "You can't see anything but stars!")) return;
           if (CheckFunctions.CheckIfTrue(ch, ch.CurrentPosition == PositionTypes.Sleeping,
               "You can't see anything, you're sleeping!")) return;
           if (CheckFunctions.CheckIfBlind(ch)) return;

           color.set_char_color(ATTypes.AT_ACTION, ch);

           string firstWord = argument.FirstWord();
           if (string.IsNullOrEmpty(firstWord))
           {
               GlanceAtRoom(ch);
               return;
           }

           CharacterInstance victim = CharacterInstanceExtensions.GetCharacterInRoom(ch, firstWord);
           if (CheckFunctions.CheckIfNullObject(ch, victim, "They're not here.")) return;

           if (victim.CanSee(ch))
           {
               comm.act(ATTypes.AT_ACTION, "$n glances at you.", ch, null, victim, ToTypes.Victim);
               comm.act(ATTypes.AT_ACTION, "$n glances at $N.", ch, null, victim, ToTypes.NotVictim);
           }

           if (ch.IsImmortal() && victim != ch)
               GlanceFromImmortal(ch, victim);
       }

       private static void GlanceAtRoom(CharacterInstance ch)
       {
           bool brief = ch.Act.IsSet(PlayerFlags.Brief);

           ch.Act.SetBit(PlayerFlags.Brief);
           Look.do_look(ch, "auto");
           if (!brief)
               ch.Act.RemoveBit(PlayerFlags.Brief);
       }

       private static void GlanceFromImmortal(CharacterInstance ch, CharacterInstance victim)
       {
           if (victim.IsNpc())
               color.ch_printf(ch, "Mobile #{0} '{1}'", victim.MobIndex.ID, victim.Name);
           else
           {
               color.ch_printf(ch, "{0}", victim.Name);
               color.ch_printf(ch, "is a level {0} {1} {2}.", victim.Level, victim.CurrentRace.GetShortName(),
                   victim.CurrentClass.GetShortName());
           }

           act_info.show_condition(ch, victim);
       }
    }
}
