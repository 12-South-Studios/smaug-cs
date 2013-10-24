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
                ch.PlayerData.Favor += ch.PlayerData.CurrentDeity.FuzzifyFavor(field, mod < 1 ? 1 : mod);
                ch.PlayerData.Favor = Check.Range(-2500, ch.PlayerData.Favor, 2500);
            }
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
