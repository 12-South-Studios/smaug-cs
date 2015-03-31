using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Extensions.Player
{
    public static class Deity
    {
        public static void CheckForExtremeFavor(this PlayerInstance ch, int oldfavor)
        {
            DeityData deity = ch.PlayerData.CurrentDeity;
            if ((oldfavor > deity.AffectedNum && ch.PlayerData.Favor <= deity.AffectedNum)
                || (oldfavor > deity.ElementNum && ch.PlayerData.Favor <= deity.Element)
                || (oldfavor < deity.SusceptNum && ch.PlayerData.Favor >= deity.SusceptNum))
                ch.update_aris();
        }

        public static void AdjustFavor(this PlayerInstance ch, DeityFieldTypes field, int mod)
        {
            if (ch.IsNpc() || ch.PlayerData.CurrentDeity == null)
                return;

            int oldfavor = ch.PlayerData.Favor;
            DeityData deity = ch.PlayerData.CurrentDeity;

            if (((ch.CurrentAlignment - deity.Alignment) > 650)
                 || ((ch.CurrentAlignment - deity.Alignment) < -650)
                && (deity.Alignment != 0))
            {
                ch.PlayerData.Favor -= 2;
                ch.PlayerData.Favor = ch.PlayerData.Favor.GetNumberThatIsBetween(-2500, 2500);

                deity.UpdateCharacterBits(ch);
                ch.CheckForExtremeFavor(oldfavor);
            }
            else
            {
                ch.PlayerData.Favor += deity.FuzzifyFavor(field, mod < 1 ? 1 : mod);
                ch.PlayerData.Favor = ch.PlayerData.Favor.GetNumberThatIsBetween(-2500, 2500);
            }
        }
    }
}
