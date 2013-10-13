using System.Linq;
using SmaugCS.Common;
using SmaugCS.Config;
using SmaugCS.Enums;
using SmaugCS.Objects;

namespace SmaugCS
{
    public static class comm
    {
        public static void cleanup_memory()
        {
            // TODO
        }

        public static bool chk_watch(short player_level, string player_name, string player_site)
        {
            if (db.WATCHES.Count == 0)
                return false;

            return db.WATCHES.Any(watch => (watch.TargetName.EqualsIgnoreCase(player_name)
                || watch.PlayerSite.EqualsIgnoreCase(player_site))
                && player_level < watch.ImmortalLevel);
        }

        public static void show_title(DescriptorData d)
        {
            CharacterInstance ch = d.Character;
            if (ch.PlayerData.Flags.IsSet((int)PCFlags.NoIntro))
                write_to_buffer(d, "Press enter...\r\n", 0);
            else
            {
                if (ch.Act.IsSet((int)PlayerFlags.Rip))
                    act_comm.send_rip_title(ch);
                else if (ch.Act.IsSet((int)PlayerFlags.Ansi))
                    act_comm.send_ansi_title(ch);
                else
                    act_comm.send_ascii_title(ch);
            }

            d.ConnectionStatus = ConnectionTypes.PressEnter;
        }

        public static bool is_reserved_name(string name)
        {
            return db.ReservedNames.Any(reservedName => reservedName.EqualsIgnoreCase(name)
                || (reservedName.StartsWithIgnoreCase("*")
                && reservedName.ContainsIgnoreCase(name)));
        }

        /// <summary>
        /// PArse a name for acceptability
        /// </summary>
        /// <param name="name"></param>
        /// <param name="newchar"></param>
        /// <returns></returns>
        /// <remarks> Name checking should really only be done on new characters, otherwise 
        /// we could end up with people who can't access their characters.
        /// </remarks>
        public static bool check_parse_name(string name, bool newchar)
        {
            if (name.Length < ConstantConfigurationSection.GetConfig().Constants["MinNameLength"].Value.ToInt32()
                || name.Length > ConstantConfigurationSection.GetConfig().Constants["MaxNameLength"].Value.ToInt32())
                return false;

            return name.IsAlphaNum();
        }

        public static void stop_idling(CharacterInstance ch)
        {
            if (ch == null || ch.Descriptor == null
                || ch.Descriptor.ConnectionStatus != ConnectionTypes.Playing)
                return;

            ch.Timer = 0;
            RoomTemplate wasInRoom = ch.PreviousRoom;
            ch.CurrentRoom.FromRoom(ch);
            wasInRoom.ToRoom(ch);
            ch.PreviousRoom = ch.CurrentRoom;

            ch.PlayerData.Flags.RemoveBit((int)PCFlags.Idle);
            act(ATTypes.AT_ACTION, "$n has returned from the world.", ch, null, null, ToTypes.Room);
        }

        /// <summary>
        /// Function to strip off the "a" or "an" or "the" or "some" from an object's
        /// short description for the purpose of using it in a sentence sent to
        /// the owner of the object.  (Ie: an object with the short description
        /// "a long dark blade" would return "long dark blade" for use in a sentence
        /// like "Your long dark blade".  The object name isn't always appropriate
        /// since it contains keywords that may not look proper.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string myobj(ObjectInstance obj)
        {
            if (obj.ShortDescription.StartsWithIgnoreCase("a "))
                return obj.ShortDescription.Remove(0, 2);
            if (obj.ShortDescription.StartsWithIgnoreCase("an "))
                return obj.ShortDescription.Remove(0, 3);
            if (obj.ShortDescription.StartsWithIgnoreCase("the "))
                return obj.ShortDescription.Remove(0, 4);
            if (obj.ShortDescription.StartsWithIgnoreCase("some "))
                return obj.ShortDescription.Remove(0, 5);
            return obj.ShortDescription;
        }

        public static string obj_short(ObjectInstance obj)
        {
            return obj.Count > 1
                ? string.Format("{0} ({1})", obj.ShortDescription, obj.Count)
                : obj.ShortDescription;
        }

        public static string MORPHNAME(CharacterInstance ch)
        {
            if (ch.CurrentMorph != null
                && ch.CurrentMorph.Morph != null
                && !string.IsNullOrEmpty(ch.CurrentMorph.Morph.ShortDescription))
                return ch.CurrentMorph.Morph.ShortDescription;
            return Macros.NAME(ch);
        }

        public static string act_string(string format, CharacterInstance to, CharacterInstance ch, object arg1, object arg2, int flags)
        {
            // TODO
            return string.Empty;
        }

        public static void act(ATTypes attype, string format, CharacterInstance to, object arg1, object arg2, ToTypes type)
        {
            // TODO
        }

        public static string default_fprompt(CharacterInstance ch)
        {
            // TODO
            return string.Empty;
        }

        public static string default_promp(CharacterInstance ch)
        {
            // tODO
            return string.Empty;
        }

        public static int getcolor(char clr)
        {
            // TODO
            return 0;
        }

        public static void display_prompt(DescriptorData d)
        {

        }

        public static void set_pager_input(DescriptorData d, string argument)
        {

        }

        public static bool pager_output(DescriptorData d)
        {
            // TODO
            return false;
        }

        public static void write_to_buffer(DescriptorData d, string txt, int length)
        {
            // TODO
        }

        public static void bailout()
        {
            // TODO
        }
    }
}
