using Realm.Library.Common.Extensions;
using SmaugCS.Auction;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Objects;
using SmaugCS.Helpers;
using SmaugCS.Managers;
using SmaugCS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmaugCS.Commands.Objects
{
    public static class Auction
    {
        public static void do_auction(CharacterInstance ch, string argument, IAuctionManager auctionManager = null)
        {
            if (ch.IsNpc()) return;

            ch.SetColor(ATTypes.AT_LBLUE);
            if (CheckFunctions.CheckIfTrue(ch, ch.Level < GameConstants.GetSystemValue<int>("MinimumAuctionLevel"),
                "You need to gain more experience to use the auction...")) return;

            // TODO: Do we really want time restrictions on auctions?

            var firstArg = argument.FirstWord();
            if (firstArg.IsNullOrEmpty())
                ReviewAuction(ch, auctionManager);
            if (ch.IsImmortal() && firstArg.EqualsIgnoreCase("stop"))
                StopAuction(ch, "Sale of {0} has been stopped by an Immortal.", auctionManager);
            if (firstArg.EqualsIgnoreCase("bid"))
                PlaceBid(ch, argument, auctionManager);
            PlaceItemForAuction(ch, argument, auctionManager);
        }

        private static void PlaceItemForAuction(CharacterInstance ch, string argument, IAuctionManager auctionManager)
        {
            var firstArg = argument.FirstWord();
            var obj = ch.GetCarriedObject(firstArg);

            if (CheckFunctions.CheckIfNullObject(ch, obj, "You aren't carrying that.")) return;
            if (CheckFunctions.CheckIfTrue(ch, obj.Timer > 0, "You can't auction objects that are decaying.")) return;
            if (CheckFunctions.CheckIfSet(ch, obj.ExtraFlags, ItemExtraFlags.Personal,
                "Personal items may not be auctioned.")) return;

            if (CheckFunctions.CheckIfTrue(ch,
                (auctionManager ?? AuctionManager.Instance).Repository.History.Any(x => x.ItemForSale == obj.ObjectIndex.ID),
                "Such an item has been auctioned recently, try again later.")) return;

            var secondArg = argument.SecondWord();
            if (CheckFunctions.CheckIfEmptyString(ch, secondArg, "Auction it for what?")) return;
            if (CheckFunctions.CheckIfTrue(ch, !secondArg.IsNumber(),
                "You must input a number at which to start the auction.")) return;

            var startingBid = secondArg.ToInt32();
            if (CheckFunctions.CheckIfTrue(ch, startingBid <= 0, "You can't auction something for nothing!")) return;

            if ((auctionManager ?? AuctionManager.Instance).Auction != null)
            {
                comm.act(ATTypes.AT_TELL, "Try again later - $p is being auctioned right now!", ch,
                    (auctionManager ?? AuctionManager.Instance).Auction.ItemForSale, null, ToTypes.Character);
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
            obj.RemoveFrom();
            if (GameManager.Instance.SystemData.SaveFlags.IsSet(AutoSaveFlags.Auction))
                save.save_char_obj(ch);

            (auctionManager ?? AuctionManager.Instance).StartAuction(ch, obj, startingBid);
            ChatManager.talk_auction($"A new item is being auctioned: {obj.ShortDescription} at {startingBid} coin.");
        }

        private static void PlaceBid(CharacterInstance ch, string argument, IAuctionManager auctionManager)
        {
            if (CheckFunctions.CheckIfNullObject(ch, (auctionManager ?? AuctionManager.Instance).Auction,
                "There isn't anything being auctioned right now.")) return;

            var auction = (auctionManager ?? AuctionManager.Instance).Auction;

            if (CheckFunctions.CheckIfTrue(ch, ch.Level < auction.ItemForSale.Level,
                "This object's level is too high for your use.")) return;
            if (CheckFunctions.CheckIfEquivalent(ch, ch, auction.Seller, "You can't bid on your own item!")) return;

            var secondArg = argument.SecondWord();
            if (CheckFunctions.CheckIfEmptyString(ch, secondArg, "Bid how much?")) return;

            var bid = Program.parsebet(auction.BidAmount, secondArg);
            if (CheckFunctions.CheckIfTrue(ch, bid < auction.StartingBid,
                "You must place a bid that is higher than the starting bet.")) return;
            if (CheckFunctions.CheckIfTrue(ch, bid < auction.BidAmount + 10000,
                "You must bid at least 10,000 coins over the current bid.")) return;
            if (CheckFunctions.CheckIfTrue(ch, bid < ch.CurrentCoin, "You don't have that much money!")) return;
            if (CheckFunctions.CheckIfTrue(ch, bid > GameConstants.GetSystemValue<int>("MaximumAuctionBid"),
                $"You can't bid over {GameConstants.GetSystemValue<int>("MaximumAuctionBid")} coins."))
                return;

            var thirdArg = argument.ThirdWord();
            if (CheckFunctions.CheckIfTrue(ch, thirdArg.IsNullOrEmpty() || auction.ItemForSale.Name.IsAnyEqual(thirdArg),
                "That item is not being auctioned right now.")) return;

            if (auction.Buyer != null && auction.Buyer != auction.Seller)
                auction.Buyer.CurrentCoin += auction.BidAmount;

            ch.CurrentCoin -= bid;
            if (GameManager.Instance.SystemData.SaveFlags.IsSet(AutoSaveFlags.Auction))
                save.save_char_obj(ch);

            (auctionManager ?? AuctionManager.Instance).PlaceBid(ch, bid);

            ChatManager.talk_auction($"A bid of {bid} coin has been received on {auction.ItemForSale.ShortDescription}.");
        }

        public static void StopAuction(CharacterInstance ch, string argument, IAuctionManager auctionManager)
        {
            if (CheckFunctions.CheckIfNullObject(ch, (auctionManager ?? AuctionManager.Instance).Auction, "There is no auction to stop."))
                return;

            ch.SetColor(ATTypes.AT_LBLUE);

            var auction = (auctionManager ?? AuctionManager.Instance).Auction;

            ChatManager.talk_auction(string.Format(argument, auction.ItemForSale.ShortDescription));
            auction.ItemForSale.AddTo(auction.Seller);

            if (GameManager.Instance.SystemData.SaveFlags.IsSet(AutoSaveFlags.Auction))
                save.save_char_obj(auction.Seller);

            if (auction.Buyer != null && auction.Buyer != auction.Seller)
            {
                auction.Buyer.CurrentCoin += auction.BidAmount;
                auction.Buyer.SendTo("Your money has been returned.");
            }

            (auctionManager ?? AuctionManager.Instance).StopAuction();
        }

        private static void ReviewAuction(CharacterInstance ch, IAuctionManager auctionManager)
        {
            if (CheckFunctions.CheckIfNullObject(ch, (auctionManager ?? AuctionManager.Instance).Auction,
                "There is nothing being auctioned right now.  What would you like to auction?")) return;

            ch.SetColor(ATTypes.AT_BLUE);
            ch.SendTo("Auctions:");

            var auction = (auctionManager ?? AuctionManager.Instance).Auction;
            if (auction.BidAmount > 0)
                ch.Printf("Current bid on this item is %s coin.", auction.BidAmount);
            else
                ch.SendTo("No bids on this item have been received.");

            ch.SetColor(ATTypes.AT_LBLUE);
            ch.Printf("Object '%s' is %s, special properties: %s\r\nIts weight is %d, value is %d, and level is %d.",
                auction.ItemForSale.Name, auction.ItemForSale.Name.AOrAn(), auction.ItemForSale.ExtraFlags.ToString(),
                auction.ItemForSale.Weight, auction.ItemForSale.Cost, auction.ItemForSale.Level);

            if (auction.ItemForSale.WearLocation != WearLocations.Light)
                ch.Printf("Item's wear location: %s", auction.ItemForSale.WearLocation);

            ch.SetColor(ATTypes.AT_BLUE);

            if (DisplayTable.ContainsKey(auction.ItemForSale.ItemType))
                DisplayTable[auction.ItemForSale.ItemType].Invoke(ch, auction.ItemForSale);

            foreach (var af in auction.ItemForSale.ObjectIndex.Affects)
                handler.showaffect(ch, af);

            foreach (var af in auction.ItemForSale.Affects)
                handler.showaffect(ch, af);

            if ((auction.ItemForSale.ItemType == ItemTypes.Container
                || auction.ItemForSale.ItemType == ItemTypes.KeyRing
                || auction.ItemForSale.ItemType == ItemTypes.Quiver)
                && auction.ItemForSale.Contents.Any())
            {
                ch.SetColor(ATTypes.AT_OBJECT);
                ch.SendTo("Contents:");
                act_info.show_list_to_char(auction.ItemForSale.Contents.ToList(), (PlayerInstance)ch, true, false);
            }

            if (ch.IsImmortal())
            {
                ch.Printf("Seller: %s.  Bidder: %s.  Round: %d,",
                    auction.Seller.Name, auction.Buyer.Name, auction.GoingCounter + 1);
                ch.Printf("Time left in round: %d.", auction.PulseFrequency);
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
            ch.Printf("%s appears to %s.", obj.ShortDescription.CapitalizeFirst(),
                GetObjectValueText(obj.Value.ToList()[0]));
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
            ch.Printf("Level %d spells of: ", obj.Value.ToList()[0]);

            SkillData skill;
            if (obj.Value.ToList()[1] >= 0)
            {
                skill = RepositoryManager.Instance.SKILLS.Get(obj.Value.ToList()[1]);
                if (skill != null)
                    ch.SendTo($" '{skill.Name}'");
            }

            if (obj.Value.ToList()[2] >= 0)
            {
                skill = RepositoryManager.Instance.SKILLS.Get(obj.Value.ToList()[2]);
                if (skill != null)
                    ch.SendTo($" '{skill.Name}'");
            }

            if (obj.Value.ToList()[3] >= 0)
            {
                skill = RepositoryManager.Instance.SKILLS.Get(obj.Value.ToList()[3]);
                if (skill != null)
                    ch.SendTo($" '{skill.Name}'");
            }
            ch.SendTo(".");
        }

        private static void DisplayMagicImplementDetails(CharacterInstance ch, ObjectInstance obj)
        {
            ch.Printf("Has %d(%d) charges of level %d", obj.Value.ToList()[1], obj.Value.ToList()[2], obj.Value.ToList()[0]);

            if (obj.Value.ToList()[3] >= 0)
            {
                var skill = RepositoryManager.Instance.SKILLS.Get(obj.Value.ToList()[3]);
                if (skill != null)
                    ch.SendTo($" '{skill.Name}'");
            }
            ch.SendTo(".");
        }

        private static void DisplayWeaponDetails(CharacterInstance ch, ObjectInstance obj)
        {
            ch.Printf("Damage is %d to %d (Average %d).%s",
                obj.Value.ToList()[1], obj.Value.ToList()[2], (obj.Value.ToList()[1] + obj.Value.ToList()[2]) / 2,
                obj.ExtraFlags.IsSet(ItemExtraFlags.Poisoned) ? "This weapon is poisoned." : string.Empty);
        }

        private static void DisplayArmorDetails(CharacterInstance ch, ObjectInstance obj)
        {
            ch.Printf("Armor class is %d.", obj.Value.ToList()[0]);
        }
    }
}
