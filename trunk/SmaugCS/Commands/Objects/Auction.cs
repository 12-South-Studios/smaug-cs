using System;
using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Auction;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
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
                StopAuction(ch, "Sale of {0} has been stopped by an Immortal.");
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

            if (CheckFunctions.CheckIfTrue(ch,
                AuctionManager.Instance.Repository.History.Any(x => x.ItemForSale == obj.ObjectIndex.ID),
                "Such an item has been auctioned recently, try again later.")) return;

            string secondArg = argument.SecondWord();
            if (CheckFunctions.CheckIfEmptyString(ch, secondArg, "Auction it for what?")) return;
            if (CheckFunctions.CheckIfTrue(ch, !secondArg.IsNumber(),
                "You must input a number at which to start the auction.")) return;

            int startingBid = secondArg.ToInt32();
            if (CheckFunctions.CheckIfTrue(ch, startingBid <= 0, "You can't auction something for nothing!")) return;

            if (AuctionManager.Instance.Auction != null)
            {
                comm.act(ATTypes.AT_TELL, "Try again later - $p is being auctioned right now!", ch,
                    AuctionManager.Instance.Auction.ItemForSale, null, ToTypes.Character);
                if (!ch.IsImmortal())
                    Macros.WAIT_STATE(ch, GameConstants.GetSystemValue<int>("PulseViolence"));
                return;
            }

            if (!obj.HasAttribute<AuctionableAttribute>())
            {
                comm.act(ATTypes.AT_TELL, "You cannot auction $Ts.", ch, null, obj.GetItemTypeName(), ToTypes.Character);
                return;
            }

            obj.Split();
            obj.FromCharacter();
            if (GameManager.Instance.SystemData.SaveFlags.IsSet(AutoSaveFlags.Auction))
                save.save_char_obj(ch);

            AuctionManager.Instance.StartAuction(ch, obj, startingBid);
            ChatManager.talk_auction(string.Format("A new item is being auctioned: {0} at {1} coin.",
                obj.ShortDescription, startingBid));
        }

        private static void PlaceBid(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfNullObject(ch, AuctionManager.Instance.Auction,
                "There isn't anything being auctioned right now.")) return;

            AuctionData auction = AuctionManager.Instance.Auction;

            if (CheckFunctions.CheckIfTrue(ch, ch.Level < auction.ItemForSale.Level,
                "This object's level is too high for your use.")) return;
            if (CheckFunctions.CheckIfEquivalent(ch, ch, auction.Seller, "You can't bid on your own item!")) return;

            string secondArg = argument.SecondWord();
            if (CheckFunctions.CheckIfEmptyString(ch, secondArg, "Bid how much?")) return;

            int bid = Program.parsebet(auction.BidAmount, secondArg);
            if (CheckFunctions.CheckIfTrue(ch, bid < auction.StartingBid,
                "You must place a bid that is higher than the starting bet.")) return;
            if (CheckFunctions.CheckIfTrue(ch, bid < (auction.BidAmount + 10000),
                "You must bid at least 10,000 coins over the current bid.")) return;
            if (CheckFunctions.CheckIfTrue(ch, bid < ch.CurrentCoin, "You don't have that much money!")) return;
            if (CheckFunctions.CheckIfTrue(ch, bid > GameConstants.GetSystemValue<int>("MaximumAuctionBid"),
                string.Format("You can't bid over {0} coins.", GameConstants.GetSystemValue<int>("MaximumAuctionBid"))))
                return;

            string thirdArg = argument.ThirdWord();
            if (CheckFunctions.CheckIfTrue(ch, thirdArg.IsNullOrEmpty() || auction.ItemForSale.Name.IsAnyEqual(thirdArg),
                "That item is not being auctioned right now.")) return;

            if (auction.Buyer != null && auction.Buyer != auction.Seller)
                auction.Buyer.CurrentCoin += auction.BidAmount;

            ch.CurrentCoin -= bid;
            if (GameManager.Instance.SystemData.SaveFlags.IsSet(AutoSaveFlags.Auction))
                save.save_char_obj(ch);

            AuctionManager.Instance.PlaceBid(ch, bid);

            ChatManager.talk_auction(string.Format("A bid of {0} coin has been received on {1}.", bid,
                auction.ItemForSale.ShortDescription));
        }

        public static void StopAuction(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfNullObject(ch, AuctionManager.Instance.Auction, "There is no auction to stop."))
                return;

            color.set_char_color(ATTypes.AT_LBLUE, ch);

            AuctionData auction = AuctionManager.Instance.Auction;

            ChatManager.talk_auction(string.Format(argument, auction.ItemForSale.ShortDescription));
            auction.ItemForSale.ToCharacter(auction.Seller);

            if (GameManager.Instance.SystemData.SaveFlags.IsSet(AutoSaveFlags.Auction))
                save.save_char_obj(auction.Seller);

            if (auction.Buyer != null && auction.Buyer != auction.Seller)
            {
                auction.Buyer.CurrentCoin += auction.BidAmount;
                color.send_to_char("Your money has been returned.", auction.Buyer);
            }

            AuctionManager.Instance.StopAuction();
        }

        private static void ReviewAuction(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfNullObject(ch, AuctionManager.Instance.Auction,
                "There is nothing being auctioned right now.  What would you like to auction?")) return;

            color.set_char_color(ATTypes.AT_BLUE, ch);
            color.send_to_char("Auctions:", ch);

            AuctionData auction = AuctionManager.Instance.Auction;
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

            if (DisplayTable.ContainsKey(auction.ItemForSale.ItemType))
                DisplayTable[auction.ItemForSale.ItemType].Invoke(ch, auction.ItemForSale);

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
                act_info.show_list_to_char(auction.ItemForSale.Contents, (PlayerInstance)ch, true, false);
            }

            if (ch.IsImmortal())
            {
                color.ch_printf(ch, "Seller: %s.  Bidder: %s.  Round: %d,",
                    auction.Seller.Name, auction.Buyer.Name, auction.GoingCounter + 1);
                color.ch_printf(ch, "Time left in round: %d.", auction.PulseFrequency);
            }
        }

        private static readonly Dictionary<ItemTypes, Action<CharacterInstance, ObjectInstance>> DisplayTable = new Dictionary
            <ItemTypes, Action<CharacterInstance, ObjectInstance>>
        {
            {ItemTypes.Container, DisplayContainerDetails},
            {ItemTypes.KeyRing, DisplayContainerDetails},
            {ItemTypes.Quiver, DisplayContainerDetails},
            {ItemTypes.Pill, DisplayConsumableDetails},
            {ItemTypes.Scroll, DisplayConsumableDetails},
            {ItemTypes.Potion, DisplayConsumableDetails},
            {ItemTypes.Staff, DisplayMagicImplementDetails},
            {ItemTypes.Wand, DisplayMagicImplementDetails},
            {ItemTypes.MissileWeapon, DisplayWeaponDetails},
            {ItemTypes.Weapon, DisplayWeaponDetails},
            {ItemTypes.Armor, DisplayArmorDetails}
        };

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
