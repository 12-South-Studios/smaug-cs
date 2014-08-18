﻿using System;
using System.Collections.Generic;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Extensions;
using SmaugCS.Logging;
using SmaugCS.Managers;

namespace SmaugCS
{
    public static class skills
    {
        public static void skill_notfound(CharacterInstance ch, string argument)
        {
            color.send_to_char("Huh?\r\n", ch);
        }

        public static int get_ssave(string name)
        {
            return LookupConstants.GetFlagIndex(name, LookupManager.Instance.GetLookups("SpellSaves"));
        }

        public static int get_starget(string name)
        {
            return LookupConstants.GetFlagIndex(name, LookupManager.Instance.GetLookups("TargetTypes"));
        }

        public static int get_sdamage(string name)
        {
            return LookupConstants.GetFlagIndex(name, LookupManager.Instance.GetLookups("SpellDamageTypes"));
        }

        public static int get_saction(string name)
        {
            return LookupConstants.GetFlagIndex(name, LookupManager.Instance.GetLookups("SpellActionTypes"));
        }

        public static int get_ssave_effect(string name)
        {
            return LookupConstants.GetFlagIndex(name, LookupManager.Instance.GetLookups("SpellSaveEffects"));
        }

        public static int get_sflag(string name)
        {
            return LookupConstants.GetFlagIndex(name, LookupManager.Instance.GetLookups("SpellFlags"));
        }

        public static int get_spower(string name)
        {
            return LookupConstants.GetFlagIndex(name, LookupManager.Instance.GetLookups("SpellPowerTypes"));
        }

        public static int get_sclass(string name)
        {
            return LookupConstants.GetFlagIndex(name, LookupManager.Instance.GetLookups("SpellClassTypes"));
        }

        public static bool is_legal_kill(CharacterInstance ch, CharacterInstance vch)
        {
            if (ch.IsNpc() || vch.IsNpc())
                return true;
            if (!ch.IsPKill() || !vch.IsPKill())
                return false;
            return ch.PlayerData.Clan == null || ch.PlayerData.Clan != vch.PlayerData.Clan;
        }

 

        public static bool check_illegal_psteal(CharacterInstance ch, CharacterInstance victim)
        {
            // TODO
            return false;
        }



        public static CharacterInstance scan_for_victim(CharacterInstance ch, ExitData pexit, string name)
        {
            // TODO
            return null;
        }

        public static ObjectInstance find_projectile(CharacterInstance ch, int type)
        {
            // TODO
            return null;
        }

        public static int ranged_got_target(CharacterInstance ch, CharacterInstance victim,
                                            ObjectInstance weapon, ObjectInstance projectile, short dist, short dt, string stxt,
                                            short color)
        {
            // TODO
            return 0;
        }

        public static ReturnTypes ranged_attack(CharacterInstance ch, string argument, ObjectInstance weapon,
                                        ObjectInstance projectile, int sn, int range)
        {
            // TODO
            return 0;
        }
    }
}