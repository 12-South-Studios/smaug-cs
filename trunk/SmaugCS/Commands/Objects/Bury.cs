﻿using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Extensions;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Objects
{
    public static class Bury
    {
        public static void do_bury(CharacterInstance ch, string argument)
        {
            string firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "What do you wish to bury?")) return;
            if (handler.FindObject_CheckMentalState(ch)) return;

            ObjectInstance shovel = ch.Carrying.FirstOrDefault(x => x.ItemType == ItemTypes.Shovel);
            ObjectInstance obj = ch.GetObjectOnMeOrInRoom(firstArg);
            if (CheckFunctions.CheckIfNullObject(ch, obj, "You can't find it.")) return;

            handler.separate_obj(obj);
            if (!obj.WearFlags.IsSet(ItemWearFlags.Take))
            {
                if (!obj.ExtraFlags.IsSet(ItemExtraFlags.ClanCorpse) || ch.IsNpc() ||
                    !ch.PlayerData.Flags.IsSet(PCFlags.Deadly))
                {
                    comm.act(ATTypes.AT_PLAIN, "You cannot bury $p.", ch, obj, null, ToTypes.Character);
                    return;
                }
            }

            SectorTypes sectorType = ch.CurrentRoom.SectorType;
            if (CheckFunctions.CheckIfTrue(ch, sectorType == SectorTypes.City || sectorType == SectorTypes.Inside,
                "The floor is too hard to dig through.")) return;
            if (CheckFunctions.CheckIfTrue(ch,
                sectorType == SectorTypes.DeepWater || sectorType == SectorTypes.ShallowWater ||
                sectorType == SectorTypes.Underwater, "you cannot bury something here.")) return;
            if (CheckFunctions.CheckIfTrue(ch, sectorType == SectorTypes.Air, "What?  In the air?!")) return;

            int carryWeight = 5.GetHighestOfTwoNumbers(ch.CanCarryMaxWeight()/10);
            if (CheckFunctions.CheckIfTrue(ch, shovel == null && obj.GetObjectWeight() > carryWeight,
                "You'd need a shovel to bury something that big.")) return;

            int move = (obj.GetObjectWeight()*50*(shovel != null ? 1 : 5))/
                       1.GetHighestOfTwoNumbers(ch.CanCarryMaxWeight());
            move = 2.GetNumberThatIsBetween(move, 1000);
            if (CheckFunctions.CheckIfTrue(ch, move > ch.CurrentMovement,
                "You don't have the energy to bury something of that size.")) return;

            ch.CurrentMovement -= move;
            if (obj.ItemType == ItemTypes.NpcCorpse || obj.ItemType == ItemTypes.PlayerCorpse)
                ch.AdjustFavor(DeityFieldTypes.BuryCorpse, 1);

            comm.act(ATTypes.AT_ACTION, "You solemnly bury $p...", ch, obj, null, ToTypes.Character);
            comm.act(ATTypes.AT_ACTION, "$n solemnly buries $p...", ch, obj, null, ToTypes.Room);
            obj.ExtraFlags.SetBit(ItemExtraFlags.Buried);
            Macros.WAIT_STATE(ch, 10.GetNumberThatIsBetween(move/2, 100));
        }
    }
}