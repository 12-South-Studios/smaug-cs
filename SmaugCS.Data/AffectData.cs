using System.Xml.Serialization;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Data
{
    [XmlRoot("Affect")]
    public class AffectData
    {
        [XmlElement("AffectedBy")]
        public AffectedByTypes Type { get; set; }

        [XmlElement]
        public int Duration { get; set; }

        [XmlElement]
        public ApplyTypes Location { get; set; }

        [XmlElement]
        public int Modifier { get; set; }

        [XmlElement]
        public ExtendedBitvector BitVector { get; set; }

        [XmlElement]
        public int SkillNumber { get; set; }
        /*public void SaveFUSS(TextWriterProxy proxy)
        {
            if ((int)Type < 0 && (int)Type >= db.SKILLS.Count)
            {
                proxy.Write("Affect       {0} {1} {2} {3} {4}\n", (int)Type, Duration,
                                          (Location == ApplyTypes.WeaponSpell || Location == ApplyTypes.WearSpell ||
                                           Location == ApplyTypes.RemoveSpell || Location == ApplyTypes.StripSN ||
                                           Location == ApplyTypes.RecurringSpell) && Macros.IS_VALID_SN(Modifier)
                                              ? db.GetSkill(Modifier).slot
                                              : Modifier, Location, BitVector);
            }
            else
            {
                proxy.Write("AffectData   '{0}' {1} {2} {3} {4}\n", db.GetSkill((int)Type).Name, Duration,
                                          (Location == ApplyTypes.WeaponSpell || Location == ApplyTypes.WearSpell ||
                                           Location == ApplyTypes.RemoveSpell || Location == ApplyTypes.StripSN ||
                                           Location == ApplyTypes.RecurringSpell) && Macros.IS_VALID_SN(Modifier)
                                              ? db.GetSkill(Modifier).slot
                                              : Modifier, Location, BitVector);
            }
        }*/
    }
}
