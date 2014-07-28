using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class DeityData : Entity
    {
        public string Filename { get; set; }
        public string Description { get; set; }
        public int Alignment { get; set; }
        public int Worshippers { get; set; }
        public int SCorpse { get; set; }
        public int SDeityObj { get; set; }
        public int SAvatar { get; set; }
        public int SRecall { get; set; }
        public int Flee { get; set; }
        public int Flee_NPCRace { get; set; }
        public int Flee_NPCFoe { get; set; }
        public int Kill { get; set; }
        public int Kill_Magic { get; set; }
        public int Kill_NPCRace { get; set; }
        public int Kill_NPCFoe { get; set; }
        public int Sacrifice { get; set; }
        public int BuryCorpse { get; set; }
        public int AidSpell { get; set; }
        public int Aid { get; set; }
        public int Backstab { get; set; }
        public int Steal { get; set; }
        public int Die { get; set; }
        public int Die_NPCRace { get; set; }
        public int Die_NPCFoe { get; set; }
        public int SpellAid { get; set; }
        public int DigCorpse { get; set; }
        public int Race { get; set; }
        public int Race2 { get; set; }
        public int Class { get; set; }
        public int Gender { get; set; }
        public RaceTypes NPCRace { get; set; }
        public RaceTypes NPCFoe { get; set; }
        public int Suscept { get; set; }
        public int Element { get; set; }
        public ExtendedBitvector AffectedBy { get; set; }
        public int SusceptNum { get; set; }
        public int ElementNum { get; set; }
        public int AffectedNum { get; set; }
        public int ObjStat { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        public DeityData(long id, string name) : base(id, name)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="mod"></param>
        /// <returns></returns>
        public int FuzzifyFavor(int field, int mod)
        {
            int fieldvalue = 0;
            switch (field)
            {
                case 0:
                    fieldvalue = Flee;
                    break;
                case 1:
                    fieldvalue = Flee_NPCRace;
                    break;
                case 2:
                    fieldvalue = Kill;
                    break;
                case 3:
                    fieldvalue = Kill_NPCRace;
                    break;
                case 4:
                    fieldvalue = Kill_Magic;
                    break;
                case 5:
                    fieldvalue = Sacrifice;
                    break;
                case 6:
                    fieldvalue = BuryCorpse;
                    break;
                case 7:
                    fieldvalue = AidSpell;
                    break;
                case 8:
                    fieldvalue = Aid;
                    break;
                case 9:
                    fieldvalue = Steal;
                    break;
                case 10:
                    fieldvalue = Backstab;
                    break;
                case 11:
                    fieldvalue = Die;
                    break;
                case 12:
                    fieldvalue = Die_NPCRace;
                    break;
                case 13:
                    fieldvalue = SpellAid;
                    break;
                case 14:
                    fieldvalue = DigCorpse;
                    break;
                case 15:
                    fieldvalue = Die_NPCFoe;
                    break;
                case 16:
                    fieldvalue = Flee_NPCFoe;
                    break;
                case 17:
                    fieldvalue = Kill_NPCFoe;
                    break;
            }

            return SmaugRandom.Fuzzy(fieldvalue / mod);
        }

        public void UpdateCharacterBits(CharacterInstance ch)
        {
            //if (ch.PlayerData.Favor > AffectedNum)
            //    ch.AffectedBy.SetBits(AffectedBy);
            if (ch.PlayerData.Favor > ElementNum)
                ch.Resistance.SetBit(Element);
            if (ch.PlayerData.Favor < SusceptNum)
                ch.Susceptibility.SetBit(Suscept);
        }
    }
}
