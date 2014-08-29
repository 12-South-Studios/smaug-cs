using System.Collections.Generic;
using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Logging;
using SmaugCS.Managers;

namespace SmaugCS.Extensions
{
    public static class Affect
    {
        public static void RemoveAffect(this CharacterInstance ch, AffectData paf)
        {
            if (ch.Affects == null || ch.Affects.Count == 0)
            {
                LogManager.Instance.Bug("affect_remove (%s, %d): no affect", ch.Name, paf != null ? paf.Type : 0);
                return;
            }

            handler.affect_modify(ch, paf, false);

            if (ch.CurrentRoom != null)
                ch.CurrentRoom.RemoveAffect(paf);

            ch.Affects.Remove(paf);
        }

        public static void AddAffect(this CharacterInstance ch, AffectData affect)
        {
            if (affect == null)
            {
                LogManager.Instance.Bug("%s (%s, NULL)", "affect_to_char", ch.Name);
                return;
            }

            AffectData newAffect = AffectData.Create();
            newAffect.Type = affect.Type;
            newAffect.Duration = affect.Duration;
            newAffect.Location = affect.Location;
            newAffect.Modifier = affect.Modifier;
            newAffect.BitVector = affect.BitVector;

            ch.Affects.Add(newAffect);
            handler.affect_modify(ch, newAffect, true);

            if (ch.CurrentRoom != null)
                ch.CurrentRoom.AddAffect(newAffect);
        }

        public static void StripAffects(this CharacterInstance ch, int sn)
        {
            foreach (AffectData affect in ch.Affects.Where(affect => (int)affect.Type == sn))
                ch.RemoveAffect(affect);
        }

        public static void JoinAffect(this CharacterInstance ch, AffectData paf)
        {
            if (ch.Affects == null || ch.Affects.Count == 0)
                return;

            IEnumerable<AffectData> matchingAffects = ch.Affects.Where(x => x.Type == paf.Type);
            foreach (var affect in matchingAffects)
            {
                paf.Duration = 1000000.GetLowestOfTwoNumbers(paf.Duration + affect.Duration);
                paf.Modifier = paf.Modifier > 0 ? 5000.GetLowestOfTwoNumbers(paf.Modifier + affect.Modifier) : affect.Modifier;
                ch.RemoveAffect(affect);
                break;
            }

            ch.AddAffect(paf);
        }

        public static void aris_affect(this CharacterInstance ch, AffectData paf)
        {
            //ch.AffectedBy.SetBits(paf.BitVector);
            switch ((int)paf.Location % Program.REVERSE_APPLY)
            {
                case (int)ApplyTypes.Affect:
                    //ch.AffectedBy.Bits[0].SetBit(paf.Modifier);
                    break;
                case (int)ApplyTypes.Resistance:
                    ch.Resistance.SetBit(paf.Modifier);
                    break;
                case (int)ApplyTypes.Immunity:
                    ch.Immunity.SetBit(paf.Modifier);
                    break;
                case (int)ApplyTypes.Susceptibility:
                    ch.Susceptibility.SetBit(paf.Modifier);
                    break;
            }
        }

        public static void update_aris(this CharacterInstance ch)
        {
            if (ch.IsNpc() || ch.IsImmortal())
                return;

            bool hiding = ch.IsAffected(AffectedByTypes.Hide);

            //ch.AffectedBy.ClearBits();
            ch.Resistance = 0;
            ch.Immunity = 0;
            ch.Susceptibility = 0;
            //ch.NoAffectedBy.ClearBits();
            ch.NoResistance = 0;
            ch.NoImmunity = 0;
            ch.NoSusceptibility = 0;

            RaceData myRace = DatabaseManager.Instance.GetRace(ch.CurrentRace);
            //ch.AffectedBy.SetBits(myRace.AffectedBy);
            ch.Resistance.SetBit(myRace.Resistance);
            ch.Susceptibility.SetBit(myRace.Susceptibility);

            ClassData myClass = DatabaseManager.Instance.GetClass(ch.CurrentClass);
            //ch.AffectedBy.SetBits(myClass.AffectedBy);
            ch.Resistance.SetBit(myClass.Resistance);
            ch.Susceptibility.SetBit(myClass.Susceptibility);

            if (ch.PlayerData.CurrentDeity != null)
            {
                // if (ch.PlayerData.Favor > ch.PlayerData.CurrentDeity.AffectedNum)
                //    ch.AffectedBy.SetBits(ch.PlayerData.CurrentDeity.AffectedBy);
                if (ch.PlayerData.Favor > ch.PlayerData.CurrentDeity.ElementNum)
                    ch.Resistance.SetBit(ch.PlayerData.CurrentDeity.Element);
                if (ch.PlayerData.Favor < ch.PlayerData.CurrentDeity.SusceptNum)
                    ch.Susceptibility.SetBit(ch.PlayerData.CurrentDeity.Suscept);
            }

            foreach (AffectData affect in ch.Affects)
                ch.aris_affect(affect);

            foreach (ObjectInstance obj in ch.Carrying
                .Where(x => x.WearLocation != WearLocations.None))
            {
                foreach (AffectData affect in obj.Affects)
                    ch.aris_affect(affect);
                // TODO figure this out
            }

            if (ch.CurrentRoom != null)
            {
                foreach (AffectData affect in ch.CurrentRoom.Affects)
                    ch.aris_affect(affect);
            }

            // TODO: Polymorph

            if (hiding)
                ch.AffectedBy.SetBit((int)AffectedByTypes.Hide);
        }
    }
}
