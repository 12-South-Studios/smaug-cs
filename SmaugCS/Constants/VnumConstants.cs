using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS
{
    public static class VnumConstants
    {
        #region Well-known object vnums
        public static int OBJ_VNUM_MONEY_ONE { get { return GameConstants.GetVnum("ObjectMoneySingle"); } }
        public static int OBJ_VNUM_MONEY_SOME { get { return GameConstants.GetVnum("ObjectMoneyMulti"); } }
        public static int OBJ_VNUM_CORPSE_NPC { get { return GameConstants.GetVnum("ObjectCorpseNPC"); } }
        public static int OBJ_VNUM_CORPSE_PC { get { return GameConstants.GetVnum("ObjectCorpsePC"); } }
        public static int OBJ_VNUM_SEVERED_HEAD = 12;
        public static int OBJ_VNUM_TORN_HEART = 13;
        public static int OBJ_VNUM_SLICED_ARM = 14;
        public static int OBJ_VNUM_SLICED_LEG = 15;
        public static int OBJ_VNUM_SPILLED_GUTS = 16;
        public static int OBJ_VNUM_BLOOD = 17;
        public static int OBJ_VNUM_BLOODSTAIN = 18;
        public static int OBJ_VNUM_SCRAPS = 19;
        public static int OBJ_VNUM_MUSHROOM = 20;
        public static int OBJ_VNUM_LIGHT_BALL = 21;
        public static int OBJ_VNUM_SPRING = 22;
        public static int OBJ_VNUM_SKIN = 23;
        public static int OBJ_VNUM_SLICE = 24;
        public static int OBJ_VNUM_SHOPPING_BAG = 25;
        public static int OBJ_VNUM_BLOODLET = 26;
        public static int OBJ_VNUM_FIRE { get { return GameConstants.GetVnum("ObjectFire"); } }
        public static int OBJ_VNUM_TRAP { get { return GameConstants.GetVnum("ObjectTrap"); } }
        public static int OBJ_VNUM_PORTAL = 32;
        public static int OBJ_VNUM_BLACK_POWDER = 33;
        public static int OBJ_VNUM_SCROLL_SCRIBING = 34;
        public static int OBJ_VNUM_FLASK_BREWING = 35;
        public static int OBJ_VNUM_NOTE = 36;
        public static int OBJ_VNUM_DEITY = 64;
        public static int OBJ_VNUM_SCHOOL_VEST = 10308;
        public static int OBJ_VNUM_SCHOOL_SHIELD = 10310;
        public static int OBJ_VNUM_SCHOOL_BANNER = 10311;
        public static int OBJ_VNUM_SCHOOL_DAGGER = 10312;
        public static int OBJ_VNUM_SCHOOL_SWORD = 10313;
        public static int OBJ_VNUM_SCHOOL_MACE = 10315;
        #endregion

        #region Room virtual numbers
        public static int ROOM_VNUM_LIMBO { get { return GameConstants.GetVnum("RoomLimbo"); } }
        public static int ROOM_VNUM_POLY = 3;
        public static int ROOM_VNUM_CHAT = 1200;
        public static int ROOM_VNUM_TEMPLE { get { return GameConstants.GetVnum("RoomTemple"); } }
        public static int ROOM_VNUM_ALTAR { get { return GameConstants.GetVnum("RoomAltar"); } }
        public static int ROOM_VNUM_SCHOOL { get { return GameConstants.GetVnum("RoomSchool"); } }
        public static int ROOM_AUTH_START = 100;
        public static int ROOM_VNUM_HALLOFFALLEN = 21195;
        public static int ROOM_VNUM_DEADLY = 3009;
        public static int ROOM_VNUM_HELL = 6;
        #endregion


    }
}
