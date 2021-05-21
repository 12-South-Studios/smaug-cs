using Realm.Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Objects;
using SmaugCS.Extensions.Player;
using SmaugCS.Helpers;
using System.Linq;

namespace SmaugCS.Commands.Objects
{
    public static class Bury
    {
        public static void do_bury(CharacterInstance ch, string argument)
        {
            var firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "What do you wish to bury?")) return;
            if (handler.FindObject_CheckMentalState(ch)) return;

            var shovel = ch.Carrying.FirstOrDefault(x => x.ItemType == ItemTypes.Shovel);
            var obj = ch.GetObjectOnMeOrInRoom(firstArg);
            if (CheckFunctions.CheckIfNullObject(ch, obj, "You can't find it.")) return;

            obj.Split();
            if (!obj.WearFlags.IsSet(ItemWearFlags.Take))
            {
                if (!obj.ExtraFlags.IsSet(ItemExtraFlags.ClanCorpse) || ch.IsNpc() ||
                    !((PlayerInstance)ch).PlayerData.Flags.IsSet(PCFlags.Deadly))
                {
                    comm.act(ATTypes.AT_PLAIN, "You cannot bury $p.", ch, obj, null, ToTypes.Character);
                    return;
                }
            }

            var sectorType = ch.CurrentRoom.SectorType;
            if (CheckFunctions.CheckIfTrue(ch, sectorType == SectorTypes.City || sectorType == SectorTypes.Inside,
                "The floor is too hard to dig through.")) return;
            if (CheckFunctions.CheckIfTrue(ch,
                sectorType == SectorTypes.DeepWater || sectorType == SectorTypes.ShallowWater ||
                sectorType == SectorTypes.Underwater, "you cannot bury something here.")) return;
            if (CheckFunctions.CheckIfTrue(ch, sectorType == SectorTypes.Air, "What?  In the air?!")) return;

            var carryWeight = 5.GetHighestOfTwoNumbers(ch.CanCarryMaxWeight() / 10);
            if (CheckFunctions.CheckIfTrue(ch, shovel == null && obj.GetWeight() > carryWeight,
                "You'd need a shovel to bury something that big.")) return;

            var move = obj.GetWeight() * 50 * (shovel != null ? 1 : 5) /
                       1.GetHighestOfTwoNumbers(ch.CanCarryMaxWeight());
            move = 2.GetNumberThatIsBetween(move, 1000);
            if (CheckFunctions.CheckIfTrue(ch, move > ch.CurrentMovement,
                "You don't have the energy to bury something of that size.")) return;

            ch.CurrentMovement -= move;
            if (obj.ItemType == ItemTypes.NpcCorpse || obj.ItemType == ItemTypes.PlayerCorpse)
                ((PlayerInstance)ch).AdjustFavor(DeityFieldTypes.BuryCorpse, 1);

            comm.act(ATTypes.AT_ACTION, "You solemnly bury $p...", ch, obj, null, ToTypes.Character);
            comm.act(ATTypes.AT_ACTION, "$n solemnly buries $p...", ch, obj, null, ToTypes.Room);
            obj.ExtraFlags.SetBit(ItemExtraFlags.Buried);
            Macros.WAIT_STATE(ch, 10.GetNumberThatIsBetween(move / 2, 100));
        }
    }
}
