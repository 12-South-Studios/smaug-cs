using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Repository;
using System.Collections.Generic;

namespace SmaugCS
{
    public static class player
    {
        private static readonly Dictionary<int, string> affect_locations = new Dictionary<int, string>
        {
                { (int)ApplyTypes.None, "NIL"},
                { (int)ApplyTypes.Strength, " STR  "},
                { (int)ApplyTypes.Dexterity, " DEX  "},
                { (int)ApplyTypes.Intelligence, " INT  "},
                { (int)ApplyTypes.Wisdom, " WIS  "},
                { (int)ApplyTypes.Constitution, " CON  "},
                { (int)ApplyTypes.Charisma, " CHA  "},
                { (int)ApplyTypes.Luck, " LCK  "},
                { (int)ApplyTypes.Gender, " SEX  "},
                { (int)ApplyTypes.Class, " CLASS"},
                { (int)ApplyTypes.Level, " LVL  "},
                { (int)ApplyTypes.Age, " AGE  "},
                { (int)ApplyTypes.Mana, " MANA "},
                { (int)ApplyTypes.Hit, " HV   "},
                { (int)ApplyTypes.Movement, " MOVE "},
                { (int)ApplyTypes.Gold, " GOLD "},
                { (int)ApplyTypes.Experience, " EXP  "},
                { (int)ApplyTypes.ArmorClass, " AC   "},
                { (int)ApplyTypes.HitRoll, " HITRL"},
                { (int)ApplyTypes.DamageRoll, " DAMRL"},
                { (int)ApplyTypes.SaveVsPoison, "SV POI"},
                { (int)ApplyTypes.SaveVsRod, "SV ROD"},
                { (int)ApplyTypes.SaveVsParalysis, "SV PARA"},
                { (int)ApplyTypes.SaveVsBreath, "SV BRTH"},
                { (int)ApplyTypes.SaveVsSpell, "SV SPLL"},
                { (int)ApplyTypes.Height, "HEIGHT"},
                { (int)ApplyTypes.Weight, "WEIGHT"},
                { (int)ApplyTypes.Affect, "AFF BY"},
                { (int)ApplyTypes.Resistance, "RESIST"},
                { (int)ApplyTypes.Immunity, "IMMUNE"},
                { (int)ApplyTypes.Susceptibility, "SUSCEPT"},
                { (int)ApplyTypes.WeaponSpell, "WEAPON"},
                { (int)ApplyTypes.Backstab, "BACKSTB"},
                { (int)ApplyTypes.Pick, " PICK  "},
                { (int)ApplyTypes.Track, " TRACK "},
                { (int)ApplyTypes.Steal, " STEAL "},
                { (int)ApplyTypes.Sneak, " SNEAK "},
                { (int)ApplyTypes.Hide, " HIDE  "},
                { (int)ApplyTypes.Palm, " PALM  "},
                { (int)ApplyTypes.Detrap, " DETRAP"},
                { (int)ApplyTypes.Dodge, " DODGE "},
                { (int)ApplyTypes.Peek, " PEEK  "},
                { (int)ApplyTypes.Scan, " SCAN  "},
                { (int)ApplyTypes.Gouge, " GOUGE "},
                { (int)ApplyTypes.Mount, " MOUNT "},
                { (int)ApplyTypes.Disarm, " DISARM"},
                { (int)ApplyTypes.Kick, " KICK  "},
                { (int)ApplyTypes.Parry, " PARRY "},
                { (int)ApplyTypes.Bash, " BASH  "},
                { (int)ApplyTypes.Stun, " STUN  "},
                { (int)ApplyTypes.Punch, " PUNCH "},
                { (int)ApplyTypes.Climb, " CLIMB "},
                { (int)ApplyTypes.Grip, " GRIP  "},
                { (int)ApplyTypes.Scribe, " SCRIBE"},
                { (int)ApplyTypes.Brew, " BREW  "},
                { (int)ApplyTypes.WearSpell, " WEAR  "},
                { (int)ApplyTypes.RemoveSpell, " REMOVE"},
                { (int)ApplyTypes.Emotion, "EMOTION"},
                { (int)ApplyTypes.MentalState, " MENTAL"},
                { (int)ApplyTypes.StripSN, " DISPEL"},
                { (int)ApplyTypes.Remove, " REMOVE"},
                { (int)ApplyTypes.Dig, " DIG   "},
                { (int)ApplyTypes.Full, " HUNGER"},
                { (int)ApplyTypes.Thirst, " THIRST"},
                { (int)ApplyTypes.Drunk, " DRUNK "},
                { (int)ApplyTypes.Blood, " BLOOD "},
                { (int)ApplyTypes.Cook, " COOK  "},
                { (int)ApplyTypes.RecurringSpell, " RECURR"},
                { (int)ApplyTypes.Contagious, "CONTGUS"},
                { (int)ApplyTypes.Odor, " ODOR  "},
                { (int)ApplyTypes.RoomFlag, " RMFLG "},
                { (int)ApplyTypes.SectorType, " SECTOR"},
                { (int)ApplyTypes.RoomLight, " LIGHT "},
                { (int)ApplyTypes.TeleportVnum, " TELEVN"},
                { (int)ApplyTypes.TeleportDelay, " TELEDY"}
            };


        public static string tiny_affect_loc_name(int location)
        {
            return affect_locations.ContainsKey(location)
                       ? affect_locations[location]
                       : "(?)";
        }

        public static string get_class(CharacterInstance ch)
        {
            if (ch.IsNpc())
                return ch.CurrentClass.ToString();
            if (!ch.IsNpc())
            {
                var cls = Program.RepositoryManager.GetClass(ch.CurrentClass);
                return cls.Name;
            }
            return "Unknown";
        }

        public static string get_race(CharacterInstance ch)
        {
            if (ch.IsNpc())
                return ch.CurrentRace.ToString();
            if (!ch.IsNpc())
            {
                var race = Program.RepositoryManager.GetRace(ch.CurrentRace);
                return race.Name;
            }
            return "Unknown";
        }

        public static void set_title(PlayerInstance ch, string title)
        {
            ch.PlayerData.Title = char.IsLetterOrDigit(title[0]) ? title.Substring(1) : title;
        }
    }
}
