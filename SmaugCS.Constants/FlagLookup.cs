using System;
using System.Collections.Generic;
using Realm.Library.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Constants
{
    /// <summary>
    /// 
    /// </summary>
    public static class FlagLookup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="sourceList"></param>
        /// <returns></returns>
        public static int GetIndexOf(string value, List<string> sourceList)
        {
            return sourceList.FindIndex(x => x.EqualsIgnoreCase(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int get_otype(string value)
        {
            return GetIndexOf(value, BuilderConstants.o_types);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int get_aflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.a_flags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int get_trapflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.trap_flags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int get_atype(string value)
        {
            return GetIndexOf(value, BuilderConstants.a_types);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int get_wearloc(string value)
        {
            return GetIndexOf(value, BuilderConstants.wear_locs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int get_secflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.sec_flags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int get_exflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.ex_flags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int get_pulltype(string value)
        {
            if (value.EqualsIgnoreCase("none") || value.EqualsIgnoreCase("clear"))
                return 0;

            int index = GetIndexOf(value, BuilderConstants.ex_pmisc);
            if (index > -1)
                return index + (int)PlaneTypes.Water;

            index = GetIndexOf(value, BuilderConstants.ex_pair);
            if (index > -1)
                return index + (int)PlaneTypes.Air;

            index = GetIndexOf(value, BuilderConstants.ex_pearth);
            if (index > -1)
                return index + (int)PlaneTypes.Earth;

            index = GetIndexOf(value, BuilderConstants.ex_pfire);
            if (index > -1)
                return index + (int)PlaneTypes.Fire;

            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int get_attackflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.attack_flags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int get_rflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.r_flags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int get_mpflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.mprog_flags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int get_oflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.o_flags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int get_areaflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.area_flags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int get_wflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.w_flags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int get_actflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.act_flags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int get_pcflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.pc_flags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int get_plrflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.plr_flags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int get_risflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.ris_flags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int get_cmdflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.cmd_flags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int get_trigflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.trig_flags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int get_partflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.part_flags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int get_defenseflag(string value)
        {
            return GetIndexOf(value, BuilderConstants.defense_flags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int get_npc_position(string value)
        {
            return GetIndexOf(value, BuilderConstants.npc_position);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int get_npc_sex(string value)
        {
            return GetIndexOf(value, BuilderConstants.npc_sex);
        }
    }
}
