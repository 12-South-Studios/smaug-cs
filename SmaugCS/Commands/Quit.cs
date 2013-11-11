using SmaugCS.Commands.Combat;
using SmaugCS.Commands.Skills;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Managers;

namespace SmaugCS.Commands
{
    public static class Quit
    {
        public static void do_qui(CharacterInstance ch, string argument)
        {
            do_quit(ch, argument);
        }

        public static void do_quit(CharacterInstance ch, string argument)
        {
            if (ch.IsNpc())
                return;

            if (ch.CurrentPosition == PositionTypes.Fighting
                || ch.CurrentPosition == PositionTypes.Evasive
                || ch.CurrentPosition == PositionTypes.Defensive
                || ch.CurrentPosition == PositionTypes.Aggressive
                || ch.CurrentPosition == PositionTypes.Berserk)
            {
                color.set_char_color(ATTypes.AT_RED, ch);
                color.send_to_char("No way! You are fighting.\r\n", ch);
                return;
            }

            if (ch.CurrentPosition == PositionTypes.Stunned)
            {
                color.set_char_color(ATTypes.AT_BLOOD, ch);
                color.send_to_char("You're not DEAD yet.\r\n", ch);
                return;
            }

            if (handler.get_timer(ch, (short)TimerTypes.RecentFight) > 0
                && !ch.IsImmortal())
            {
                color.set_char_color(ATTypes.AT_RED, ch);
                color.send_to_char("Your adrenaline is pumping too hard to quit now!\r\n", ch);
                return;
            }

            // TODO: auction

            if (ch.IsPKill() && ch.wimpy > ((int)ch.MaximumHealth / 2.25f))
            {
                color.send_to_char("Your wimpy has been adjusted to the maximum level for daedlies.\r\n", ch);
                Wimpy.do_wimpy(ch, "max");
            }

            if (ch.CurrentPosition == PositionTypes.Mounted)
                Dismount.do_dismount(ch, "");

            color.set_char_color(ATTypes.AT_WHITE, ch);
            color.send_to_char("Your surroundings begin to fade as a mystical swirling vortex of colors\r\nenvelops your body... When you come to, things are not as they were.\r\n\r\n", ch);
            comm.act(ATTypes.AT_SAY, "A strange voice says, 'We await your return, $n...'", ch, null, null, ToTypes.Character);
            comm.act(ATTypes.AT_BYE, "$n has left the game.", ch, null, null, ToTypes.CanSee);
            color.set_char_color(ATTypes.AT_GREY, ch);

            // TODO quitting_char = ch;
            save.save_char_obj(ch);

            if (db.SystemData.SavePets && ch.PlayerData.Pet != null)
            {
                comm.act(ATTypes.AT_BYE, "$N follows $S master into the Void.", ch, null, ch.PlayerData.Pet, ToTypes.Room);
                handler.extract_char(ch.PlayerData.Pet, true);
            }

            //if (ch.PlayerData.Clan != null)
            //   ch.PlayerData.Clan.Save();

            // TODO saving_char = null;

            for (int x = 0; x < Program.MAX_WEAR; x++)
            {
                for (int y = 0; y < Program.MAX_LAYERS; y++)
                {
                    // TODO Save equipment
                }
            }

            LogManager.Log(
                string.Format("{0} has quit (Room {1}).", ch.Name, ch.CurrentRoom != null ? ch.CurrentRoom.Vnum : -1),
                LogTypes.Comm, ch.Trust);

            handler.extract_char(ch, true);
        }
    }
}
