using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmaugCS.Enums;
using SmaugCS.Objects;

namespace SmaugCS
{
    public static class mud_comm
    {
        public static Dictionary<MudProgTypes, string> mprog_types = new Dictionary<MudProgTypes, string>()
            {
                {MudProgTypes.InFile, "in_file_prog"},
                {MudProgTypes.Act, "act_prog"},
                {MudProgTypes.Speech, "speech_prog"},
                {MudProgTypes.Random, "rand_prog"},
                {MudProgTypes.Fight, "fight_prog"},
                {MudProgTypes.HitPercent, "hitprcnt_prog"},
                {MudProgTypes.Death, "death_prog"},
                {MudProgTypes.Entry, "entry_prog"},
                {MudProgTypes.Greet, "greet_prog"},
                {MudProgTypes.GreetAll, "all_greet_prog"},
                {MudProgTypes.Give, "give_prog"},
                {MudProgTypes.Bribe, "bribe_prog"},
                {MudProgTypes.Hour, "hour_prog"},
                {MudProgTypes.Time, "time_prog"},
                {MudProgTypes.Wear, "wear_prog"},
                {MudProgTypes.Remove, "remove_prog"},
                {MudProgTypes.Sacrifice, "sac_prog"},
                {MudProgTypes.Look, "look_prog"},
                {MudProgTypes.Examine, "exa_prog"},
                {MudProgTypes.Zap, "zap_prog"},
                {MudProgTypes.Get, "get_prog"},
                {MudProgTypes.Drop, "drop_prog"},
                {MudProgTypes.Repair, "repair_prog"},
                {MudProgTypes.Damage, "damage_prog"},
                {MudProgTypes.Pull, "pull_prog"},
                {MudProgTypes.Push, "push_prog"},
                {MudProgTypes.Script, "script_prog"},
                {MudProgTypes.Sleep, "sleep_prog"},
                {MudProgTypes.Rest, "rest_prog"},
                {MudProgTypes.Leave, "leave_prog"},
                {MudProgTypes.Use, "use_prog"}
            };

        public static string mprog_type_to_name(MudProgTypes type)
        {
            return mprog_types.ContainsKey(type) ? mprog_types[type] : "ERROR_PROG";
        }

        public static void do_mpstat(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_opstat(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_rpstat(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpsupress(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpkill(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpjunk(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static int get_color(string argument)
        {
            // TODO
            return 0;
        }

        public static void do_mpasound(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpechoaround(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpechoat(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpecho(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpsoundaround(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpsoundat(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpsound(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpmusicaround(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpmusic(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpmusicat(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpmload(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpoload(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mppardon(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mppurge(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpinvis(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpgoto(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpat(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpdavance(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mptransfer(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpforce(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpnuisance(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpunnuisance(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpbodybag(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpmorph(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpunmorph(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpechozone(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mp_practice(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpstrew(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpscatter(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mp_slay(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mp_damage(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mp_log(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mp_restore(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpfavor(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mp_open_passage(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mp_fill_in(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mp_close_passage(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpnothing(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpdream(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpapply(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpapplyb(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mp_deposit(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mp_withdraw(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpdelay(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mppeace(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mppkset(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static int simple_damage(CharacterInstance ch, CharacterInstance victim, int dam, int dt)
        {
            // TODO
            return 0;
        }

        public static void do_mphate(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mphunt(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpfear(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpoowner(CharacterInstance ch, string argument)
        {
            // TODO
        }
    }
}
