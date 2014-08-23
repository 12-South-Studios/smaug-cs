using Realm.Library.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Deity
{
    public static class Devote
    {
        public static void do_devote(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfNpc(ch, ch, "Huh?")) return;
            if (CheckFunctions.CheckIfTrue(ch, ch.Level < 5, "You are not yet prepared for such devotion.")) return;

            string firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Devote yourself to which deity?")) return;

            if (firstArg.EqualsIgnoreCase("none"))
            {
                RemoveDevotion(ch);
                return;
            }

            DeityData deity = DatabaseManager.Instance.GetEntity<DeityData>(firstArg);
            if (CheckFunctions.CheckIfNullObject(ch, deity, "No such deity holds weight on this world.")) return;
            if (CheckFunctions.CheckIfNotNullObject(ch, ch.PlayerData.CurrentDeity,
                "You are already devoted to a deity.")) return;
            if (CheckFunctions.CheckIfTrue(ch, WillDeityDenyPlayerClass(ch, deity),
                "That deity will not accept your worship due to your class.")) return;
            if (CheckFunctions.CheckIfTrue(ch, WillDeityDenyPlayerGender(ch, deity),
                "That deity will not accept worshippers of your gender.")) return;
            if (CheckFunctions.CheckIfTrue(ch, WillDeityDenyPlayerRace(ch, deity),
                "That deity will not accept worshippers of your race.")) return;

            ch.PlayerData.CurrentDeity = deity;

            if (ch.PlayerData.Favor > deity.AffectedNum)
            {
                // TODO Transfer Deity affecteds to player
            }
            if (ch.PlayerData.Favor > deity.ElementNum)
            {
                // TODO Transfer Deity elements to player resistance
            }
            if (ch.PlayerData.Favor > deity.SusceptNum)
            {
                // TODO Transfer Deity suscept to player susceptible
            }

            comm.act(ATTypes.AT_MAGIC, "Body and soul, you devote yourself to $t!", ch, deity.Name, null, ToTypes.Character);
            deity.Worshippers++;

            // TODO: Save deity worshipper data to database

            save.save_char_obj(ch);
        }

        private static bool WillDeityDenyPlayerRace(CharacterInstance ch, DeityData deity)
        {
            return deity.Race != -1 && deity.Race2 != -1
                   && (EnumerationExtensions.GetEnum<RaceTypes>(deity.Race) != ch.CurrentRace)
                   && (EnumerationExtensions.GetEnum<RaceTypes>(deity.Race2) != ch.CurrentRace);
        }

        private static bool WillDeityDenyPlayerGender(CharacterInstance ch, DeityData deity)
        {
            return deity.Gender != -1 && (EnumerationExtensions.GetEnum<GenderTypes>(deity.Gender) != ch.Gender);
        }

        private static bool WillDeityDenyPlayerClass(CharacterInstance ch, DeityData deity)
        {
            return deity.Class != -1 && (EnumerationExtensions.GetEnum<ClassTypes>(deity.Class) != ch.CurrentClass);
        }

        private static void RemoveDevotion(CharacterInstance ch)
        {
            if (CheckFunctions.CheckIfNullObject(ch, ch.PlayerData.CurrentDeity,
                "You have already chosen to worship no deities.")) return;

            DeityData deity = ch.PlayerData.CurrentDeity;

            --deity.Worshippers;
            if (deity.Worshippers < 0)
                deity.Worshippers = 0;

            ch.PlayerData.Favor = -2500;
            ch.MentalState = -80;
            color.send_to_char("A terrible curse afflicts you as you forsake a deity!", ch);

            // TODO Remove deity affects from player
            // TODO Remove deity resistances from player
            // TODO Remove deity susceptibles from player

            AffectData af = AffectData.Create();
            af.Type = AffectedByTypes.Blind;
            af.Location = ApplyTypes.HitRoll;
            af.Modifier = -4;
            af.Duration = 50*GameConstants.GetConstant<int>("AffectDurationConversionValue");
            // TODO af.bitvecotr = meb(AFF_BLIND);
            ch.AddAffect(af);

            // TODO Save the deity data to the database

            color.send_to_char("You cease to worship any deity.", ch);
            ch.PlayerData.CurrentDeity = null;
            save.save_char_obj(ch);
        }
    }
}
