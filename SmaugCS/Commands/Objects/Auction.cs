using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Realm.Library.Common;
using SmaugCS.Auction;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Objects
{
    public static class Auction
    {
        public static void do_auction(CharacterInstance ch, string argument)
        {
            if (ch.IsNpc()) return;
            
            color.set_char_color(ATTypes.AT_LBLUE, ch);
            if (CheckFunctions.CheckIfTrue(ch, ch.Level < GameConstants.GetSystemValue<int>("MinimumAuctionLevel"),
                "You need to gain more experience to use the auction...")) return;

            // TODO: Do we really want time restrictions on auctions?

            string firstArg = argument.FirstWord();
            if (firstArg.IsNullOrEmpty())
                ReviewAuction(ch, argument);
            if (ch.IsImmortal() && firstArg.EqualsIgnoreCase("stop"))
                StopAuction(ch, argument);
            if (firstArg.EqualsIgnoreCase("bid"))
                PlaceBid(ch, argument);
            PlaceItemForAuction(ch, argument);
        }

        private static void PlaceItemForAuction(CharacterInstance ch, string argument)
        {
            string firstArg = argument.FirstWord();
            ObjectInstance obj = ch.GetCarriedObject(firstArg);

            if (CheckFunctions.CheckIfNullObject(ch, obj, "You aren't carrying that.")) return;
            if (CheckFunctions.CheckIfTrue(ch, obj.Timer > 0, "You can't auction objects that are decaying.")) return;
            if (CheckFunctions.CheckIfSet(ch, obj.ExtraFlags, ItemExtraFlags.Personal,
                "Personal items may not be auctioned.")) return;

        }

        private static void PlaceBid(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfTrue(ch, AuctionManager.Instance.Auction == null,
                "There isn't anything being auctioned right now.")) return;

            AuctionData auction = AuctionManager.Instance.Auction;

            if (CheckFunctions.CheckIfTrue(ch, ch.Level < auction.ItemForSale.Level,
                "This object's level is too high for your use.")) return;
            if (CheckFunctions.CheckIfEquivalent(ch, ch, auction.Seller, "You can't bid on your own item!")) return;

            string secondArg = argument.SecondWord();
            if (CheckFunctions.CheckIfEmptyString(ch, secondArg, "Bid how much?")) return;

            int newbet = Program.parsebet(auction.BidAmount, secondArg);
            if (CheckFunctions.CheckIfTrue(ch, newbet < auction.StartingBid,
                "You must place a bid that is higher than the starting bet.")) return;
            if (CheckFunctions.CheckIfTrue(ch, newbet < (auction.BidAmount + 10000),
                "You must bid at least 10,000 coins over the current bid.")) return;
            if (CheckFunctions.CheckIfTrue(ch, newbet < ch.CurrentCoin, "You don't have that much money!")) return;
            if (CheckFunctions.CheckIfTrue(ch, newbet > 2000000000, "You can't bid over 2 billion coins.")) return;

            string thirdArg = argument.ThirdWord();
            if (CheckFunctions.CheckIfTrue(ch, thirdArg.IsNullOrEmpty() || auction.ItemForSale.Name.IsAnyEqual(thirdArg),
                "That item is not being auctioned right now.")) return;

            if (auction.Buyer != null && auction.Buyer != auction.Seller)
                auction.Buyer.CurrentCoin += auction.BidAmount;

            ch.CurrentCoin -= newbet;
            if (GameManager.Instance.SystemData.SaveFlags.IsSet(AutoSaveFlags.Auction))
                save.save_char_obj(ch);

            auction.Buyer = ch;
            auction.BidAmount = newbet;
            auction.GoingCounter = 0;
            auction.PulseFrequency = GameConstants.GetSystemValue<int>("PulseAuction");
            ChatManager.talk_auction(string.Format("A bid of {0} coin has been received on {1}.", newbet,
                auction.ItemForSale.ShortDescription));
        }

        private static void StopAuction(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfTrue(ch, AuctionManager.Instance.Auction == null,
                "There is no auction to stop.")) return;

            color.set_char_color(ATTypes.AT_LBLUE, ch);

            AuctionData auction = AuctionManager.Instance.Auction;

            ChatManager.talk_auction(string.Format("Sale of {0} has been stopped by an Immortal.",
                auction.ItemForSale.ShortDescription));
            auction.ItemForSale.ToCharacter(auction.Seller);

            if (GameManager.Instance.SystemData.SaveFlags.IsSet(AutoSaveFlags.Auction))
                save.save_char_obj(auction.Seller);

            AuctionManager.Instance.Auction = null;
            if (auction.Buyer != null && auction.Buyer != auction.Seller)
            {
                auction.Buyer.CurrentCoin += auction.BidAmount;
                color.send_to_char("Your money has been returned.", auction.Buyer);
            }
        }

        private static void ReviewAuction(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfTrue(ch, AuctionManager.Instance.Auction == null,
                "There is nothing being auctioned right now.  What would you like to auction?")) return;

            color.set_char_color(ATTypes.AT_BLUE, ch);
            color.send_to_char("Auctions:", ch);

            DisplayAuctionDetails(ch, AuctionManager.Instance.Auction);
        }

        private static void DisplayAuctionDetails(CharacterInstance ch, AuctionData auction)
        {
            if (auction.BidAmount > 0)
                color.ch_printf(ch, "Current bid on this item is %s coin.", auction.BidAmount);
            else 
                color.send_to_char("No bids on this item have been received.", ch);

            color.set_char_color(ATTypes.AT_LBLUE, ch);
            color.ch_printf(ch,
                "Object '%s' is %s, special properties: %s\r\nIts weight is %d, value is %d, and level is %d.",
                auction.ItemForSale.Name, auction.ItemForSale.Name.AOrAn(), auction.ItemForSale.ExtraFlags.ToString(),
                auction.ItemForSale.Weight, auction.ItemForSale.Cost, auction.ItemForSale.Level);

            if (auction.ItemForSale.WearLocation != WearLocations.Light)
                color.ch_printf(ch, "Item's wear location: %s", auction.ItemForSale.WearLocation);

            color.set_char_color(ATTypes.AT_BLUE, ch);

            DisplaySpecificItemTypeDetails(ch, auction.ItemForSale);

            foreach (AffectData af in auction.ItemForSale.ObjectIndex.Affects)
                handler.showaffect(ch, af);

            foreach (AffectData af in auction.ItemForSale.Affects)
                handler.showaffect(ch, af);

            if ((auction.ItemForSale.ItemType == ItemTypes.Container
                || auction.ItemForSale.ItemType == ItemTypes.KeyRing
                || auction.ItemForSale.ItemType == ItemTypes.Quiver) 
                && auction.ItemForSale.Contents.Any())
            {
                color.set_char_color(ATTypes.AT_OBJECT, ch);
                color.send_to_char("Contents:", ch);
                act_info.show_list_to_char(auction.ItemForSale.Contents, ch, true, false);
            }

            if (ch.IsImmortal())
            {
                color.ch_printf(ch, "Seller: %s.  Bidder: %s.  Round: %d,",
                    auction.Seller.Name, auction.Buyer.Name, auction.GoingCounter + 1);
                color.ch_printf(ch, "Time left in round: %d.", auction.PulseFrequency);
            }
        }

        private static void DisplaySpecificItemTypeDetails(CharacterInstance ch, ObjectInstance obj)
        {
            switch (obj.ItemType)
            {
                case ItemTypes.Container:
                case ItemTypes.KeyRing:
                case ItemTypes.Quiver:
                    DisplayContainerDetails(ch, obj);
                    break;
                case ItemTypes.Pill:
                case ItemTypes.Scroll: 
                case ItemTypes.Potion:
                    DisplayConsumableDetails(ch, obj);
                    break;
                case ItemTypes.Staff:
                case ItemTypes.Wand:
                    DisplayMagicImplementDetails(ch, obj);
                    break;
                case ItemTypes.MissileWeapon:
                case ItemTypes.Weapon:
                    DisplayWeaponDetails(ch, obj);
                    break;
                case ItemTypes.Armor:
                    DisplayArmorDetails(ch, obj);
                    break;
            }
        }

        private static void DisplayContainerDetails(CharacterInstance ch, ObjectInstance obj)
        {
            color.ch_printf(ch, "%s appears to %s.", obj.ShortDescription.CapitalizeFirst(), 
                GetObjectValueText(obj.Value[0]));
        }

        private static string GetObjectValueText(int value)
        {
            const string txt = "have a {0} capacity";
            string val;

            if (value < 76)
                val = "small";
            else if (value < 150)
                val = "small to medium";
            else if (value < 300)
                val = "medium";
            else if (value < 500)
                val = "medium to large";
            else if (value < 751)
                val = "large";
            else
                val = "giant";

            return string.Format(txt, val);
        }

        private static void DisplayConsumableDetails(CharacterInstance ch, ObjectInstance obj)
        {
            color.ch_printf(ch, "Level %d spells of: ", obj.Value[0]);

            SkillData skill;
            if (obj.Value[1] >= 0)
            {
                skill = DatabaseManager.Instance.SKILLS.Get(obj.Value[1]);
                if (skill != null)
                    color.send_to_char(string.Format(" '{0}'", skill.Name), ch);
            }

            if (obj.Value[2] >= 0)
            {
                skill = DatabaseManager.Instance.SKILLS.Get(obj.Value[2]);
                if (skill != null)
                    color.send_to_char(string.Format(" '{0}'", skill.Name), ch);
            }

            if (obj.Value[3] >= 0)
            {
                skill = DatabaseManager.Instance.SKILLS.Get(obj.Value[3]);
                if (skill != null)
                    color.send_to_char(string.Format(" '{0}'", skill.Name), ch);
            }
            color.send_to_char(".", ch);
        }

        private static void DisplayMagicImplementDetails(CharacterInstance ch, ObjectInstance obj)
        {
            color.ch_printf(ch, "Has %d(%d) charges of level %d", obj.Value[1], obj.Value[2], obj.Value[0]);

            if (obj.Value[3] >= 0)
            {
                SkillData skill = DatabaseManager.Instance.SKILLS.Get(obj.Value[3]);
                if (skill != null)
                    color.send_to_char(string.Format(" '{0}'", skill.Name), ch);
            }
            color.send_to_char(".", ch);
        }

        private static void DisplayWeaponDetails(CharacterInstance ch, ObjectInstance obj)
        {
            color.ch_printf(ch, "Damage is %d to %d (Average %d).%s",
                obj.Value[1], obj.Value[2], (obj.Value[1] + obj.Value[2])/2,
                obj.ExtraFlags.IsSet(ItemExtraFlags.Poisoned) ? "This weapon is poisoned." : string.Empty);
        }

        private static void DisplayArmorDetails(CharacterInstance ch, ObjectInstance obj)
        {
            color.ch_printf(ch, "Armor class is %d.", obj.Value[0]);
        }
    }
}
