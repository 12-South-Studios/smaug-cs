using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Realm.Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Commands.Player
{
    public static class Worth
    {
        private class LookupLevel : IComparable
        {
            public int Level { get; set; }
            public string Text { get; set; }

            public int CompareTo(object obj)
            {
                if (obj.GetType() != typeof(LookupLevel))
                    throw new InvalidCastException();

                var compareTo = (LookupLevel) obj;

                if (Level < compareTo.Level)
                    return -1;
                return Level > compareTo.Level ? 1 : 0;
            }
        }

        private static readonly List<LookupLevel> DeityFavorList = new List<LookupLevel>
            {
                new LookupLevel {Level = 2250, Text = "loved"},
                new LookupLevel {Level = 2000, Text = "cherished"},
                new LookupLevel {Level = 1750, Text = "honored"},
                new LookupLevel {Level = 1500, Text = "praised"},
                new LookupLevel {Level = 1250, Text = "favored"},
                new LookupLevel {Level = 1000, Text = "respected"},
                new LookupLevel {Level = 750, Text = "liked"},
                new LookupLevel {Level = 250, Text = "tolerated"},
                new LookupLevel {Level = -250, Text = "ignored"},
                new LookupLevel {Level = -750, Text = "shunned"},
                new LookupLevel {Level = -1000, Text = "disliked"},
                new LookupLevel {Level = -1250, Text = "dishonored"},
                new LookupLevel {Level = -1500, Text = "disowned"},
                new LookupLevel {Level = -1750, Text = "abandoned"},
                new LookupLevel {Level = -2000, Text = "despised"},
                new LookupLevel {Level = -2250, Text = "hated"},
                new LookupLevel {Level = -3000, Text = "damned"}
            };

        private static readonly List<LookupLevel> AlignmentList = new List<LookupLevel>
            {
                new LookupLevel {Level = 900, Text = "devout"},
                new LookupLevel {Level = 700, Text = "noble"},
                new LookupLevel {Level = 350, Text = "honorable"},
                new LookupLevel {Level = 100, Text = "worthy"},
                new LookupLevel {Level = -100, Text = "neutral"},
                new LookupLevel {Level = -350, Text = "base"},
                new LookupLevel {Level = -700, Text = "evil"},
                new LookupLevel {Level = -900, Text = "ignoble"},
                new LookupLevel {Level = -1001, Text = "fiendish"}
            };

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "argument")]
        public static void do_worth(CharacterInstance ch, string argument)
        {
            if (ch.IsNpc())
                return;

            ch.SetPagerColor(ATTypes.AT_SCORE);
            ch.PagerPrintf("\r\nWorth for {0}{1}.\r\n", ch.Name, ((PlayerInstance)ch).PlayerData.Title);
            ch.SendToPager(" ----------------------------------------------------------------------------\r\n");

            var buffer = "|Level: {0} |Favor: {1} |Alignment: {2} |Experience: {3}|\r\n";
            buffer = string.Format(buffer, ch.Level.ToString(CultureInfo.InvariantCulture).PadLeft(4),
                                   GetDeityFavor(ch).PadLeft(9),
                                   (ch.Level < 10
                                        ? GetAlignment(ch)
                                        : ch.CurrentAlignment.ToString(CultureInfo.InvariantCulture)).PadLeft(9),
                                   ch.Experience.ToString(CultureInfo.InvariantCulture).PadLeft(9));

            ch.PagerPrintf(buffer);
            ch.SendToPager(" ----------------------------------------------------------------------------\r\n");

            buffer = "|Glory: {0} |Weight: {1} |Style: {2} |Gold: {3} |\r\n";
            buffer = string.Format(buffer, ((PlayerInstance)ch).PlayerData.quest_curr.ToString(CultureInfo.InvariantCulture).PadLeft(4),
                                   ch.CarryWeight.ToString(CultureInfo.InvariantCulture).PadLeft(9),
                                   ch.CurrentStyle.GetName().PadLeft(13), ch.CurrentCoin.ToPunctuation().PadLeft(14));
            ch.PagerPrintf(buffer);
            ch.SendToPager(" ----------------------------------------------------------------------------\r\n");

            if (ch.Level < 15 && !ch.IsPKill())
               ch.PagerPrintf("|            |Hitroll: -------- |Damroll: ----------- |                     |\r\n");
            else
                ch.PagerPrintf("|            |Hitroll: {0} |Damroll: {1} |                     |\r\n",
                                   ch.GetHitroll().ToString(CultureInfo.InvariantCulture).PadLeft(8),
                                   ch.GetDamroll().ToString(CultureInfo.InvariantCulture).PadLeft(11));
            ch.SendToPager(" ----------------------------------------------------------------------------\r\n");
        }

        private static string GetDeityFavor(CharacterInstance ch)
        {
            if (((PlayerInstance)ch).PlayerData.CurrentDeity == null)
                return "N/A";

            DeityFavorList.Sort();
            var deityLookup = DeityFavorList.FirstOrDefault(level => ((PlayerInstance)ch).PlayerData.Favor > level.Level);
            return deityLookup != null ? deityLookup.Text : "N/A";
        }

        private static string GetAlignment(CharacterInstance ch)
        {
            AlignmentList.Sort();
            var alignLookup = AlignmentList.FirstOrDefault(level => ch.CurrentAlignment > level.Level);
            return alignLookup != null ? alignLookup.Text : "neutral";
        }
    }
}
