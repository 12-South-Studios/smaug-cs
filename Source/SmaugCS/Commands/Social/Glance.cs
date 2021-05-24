using Realm.Library.Common.Extensions;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;

namespace SmaugCS.Commands
{
    public static class Glance
    {
        public static void do_glance(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfNullObject(ch, ((PlayerInstance)ch).Descriptor)) return;
            if (CheckFunctions.CheckIfTrue(ch, (int)ch.CurrentPosition < (int)PositionTypes.Sleeping,
                "You can't see anything but stars!")) return;
            if (CheckFunctions.CheckIfTrue(ch, ch.CurrentPosition == PositionTypes.Sleeping,
                "You can't see anything, you're sleeping!")) return;
            if (CheckFunctions.CheckIfBlind(ch, "You can't see a thing!")) return;

            ch.SetColor(ATTypes.AT_ACTION);

            var firstWord = argument.FirstWord();
            if (string.IsNullOrEmpty(firstWord))
            {
                GlanceAtRoom(ch);
                return;
            }

            var victim = ch.GetCharacterInRoom(firstWord);
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
            var brief = ch.Act.IsSet((int)PlayerFlags.Brief);

            ch.Act.SetBit((int)PlayerFlags.Brief);
            Look.do_look(ch, "auto");
            if (!brief)
                ch.Act.RemoveBit((int)PlayerFlags.Brief);
        }

        private static void GlanceFromImmortal(CharacterInstance ch, CharacterInstance victim)
        {
            if (victim.IsNpc())
                ch.Printf("Mobile #{0} '{1}'", ((MobileInstance)victim).MobIndex.ID, victim.Name);
            else
            {
                ch.Printf("{0}", victim.Name);
                ch.Printf("is a level {0} {1} {2}.", victim.Level, victim.CurrentRace.GetShortName(),
                    victim.CurrentClass.GetShortName());
            }

            ch.ShowConditionTo(victim);
        }
    }
}
