using SmaugCS.Common;
using SmaugCS.Objects;

namespace SmaugCS
{
    public static class deity
    {
        public static void adjust_favor(CharacterInstance ch, int field, int mod)
        {
            if (ch.IsNpc() || ch.PlayerData.CurrentDeity == null)
                return;

            int oldfavor = ch.PlayerData.Favor;

            if ((ch.CurrentAlignment - ch.PlayerData.CurrentDeity.Alignment > 650
                 || ch.CurrentAlignment - ch.PlayerData.CurrentDeity.Alignment < -650)
                && ch.PlayerData.CurrentDeity.Alignment != 0)
            {
                ch.PlayerData.Favor -= 2;
                ch.PlayerData.Favor = Check.Range(-2500, ch.PlayerData.Favor, 2500);

                UpdateCharacterBits(ch);
                CheckForExtremeFavor(ch, oldfavor);
            }
            else
            {
                ch.PlayerData.Favor += FuzzifyFavorBasedOnField(ch.PlayerData.CurrentDeity, field, mod < 1 ? 1 : mod);
                ch.PlayerData.Favor = Check.Range(-2500, ch.PlayerData.Favor, 2500);
            }
        }

        private static int FuzzifyFavorBasedOnField(DeityData deity, int field, int mod)
        {
            int fieldvalue = 0;
            switch (field)
            {
                case 0:
                    fieldvalue = deity.Flee;
                    break;
                case 1:
                    fieldvalue = deity.Flee_NPCRace;
                    break;
                case 2:
                    fieldvalue = deity.Kill;
                    break;
                case 3:
                    fieldvalue = deity.Kill_NPCRace;
                    break;
                case 4:
                    fieldvalue = deity.Kill_Magic;
                    break;
                case 5:
                    fieldvalue = deity.Sacrifice;
                    break;
                case 6:
                    fieldvalue = deity.BuryCorpse;
                    break;
                case 7:
                    fieldvalue = deity.AidSpell;
                    break;
                case 8:
                    fieldvalue = deity.Aid;
                    break;
                case 9:
                    fieldvalue = deity.Steal;
                    break;
                case 10:
                    fieldvalue = deity.Backstab;
                    break;
                case 11:
                    fieldvalue = deity.Die;
                    break;
                case 12:
                    fieldvalue = deity.Die_NPCRace;
                    break;
                case 13:
                    fieldvalue = deity.SpellAid;
                    break;
                case 14:
                    fieldvalue = deity.DigCorpse;
                    break;
                case 15:
                    fieldvalue = deity.Die_NPCFoe;
                    break;
                case 16:
                    fieldvalue = deity.Flee_NPCFoe;
                    break;
                case 17:
                    fieldvalue = deity.Kill_NPCFoe;
                    break;
            }

            return SmaugRandom.Fuzzy(fieldvalue / mod);
        }

        /// <summary>
        /// If favor crosses over the line then strip the affect
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="oldfavor"></param>
        private static void CheckForExtremeFavor(CharacterInstance ch, int oldfavor)
        {
            if ((oldfavor > ch.PlayerData.CurrentDeity.AffectedNum &&
                 ch.PlayerData.Favor <= ch.PlayerData.CurrentDeity.AffectedNum)
                ||
                (oldfavor > ch.PlayerData.CurrentDeity.ElementNum && ch.PlayerData.Favor <= ch.PlayerData.CurrentDeity.Element)
                ||
                (oldfavor < ch.PlayerData.CurrentDeity.SusceptNum &&
                 ch.PlayerData.Favor >= ch.PlayerData.CurrentDeity.SusceptNum))
                ch.update_aris();
        }

        private static void UpdateCharacterBits(CharacterInstance ch)
        {
            if (ch.PlayerData.Favor > ch.PlayerData.CurrentDeity.AffectedNum)
                ch.AffectedBy.SetBits(ch.PlayerData.CurrentDeity.AffectedBy);
            if (ch.PlayerData.Favor > ch.PlayerData.CurrentDeity.ElementNum)
                ch.Resistance.SetBit(ch.PlayerData.CurrentDeity.Element);
            if (ch.PlayerData.Favor < ch.PlayerData.CurrentDeity.SusceptNum)
                ch.Susceptibility.SetBit(ch.PlayerData.CurrentDeity.Suscept);
        }
    }
}
