using Realm.Library.Common.Extensions;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Repository;
using System.Collections.Generic;
using System.Linq;

namespace SmaugCS.Commands
{
    public static class Areas
    {
        public static void do_areas(CharacterInstance ch, string argument)
        {
            var firstArg = argument.FirstWord();
            var secondArg = argument.SecondWord();

            List<AreaData> areas;
            if (firstArg.EqualsIgnoreCase("help") || firstArg == "?")
            {
                ch.SendTo("Syntax:  area            ->      lists areas in alphanumeric order");
                ch.SendTo("         area <a>        ->      lists areas with soft max less than parameter a");
                ch.SendTo("         area <a> <b>    ->      lists areas with soft max bewteen numbers a and b");
                ch.SendTo("         area old        ->      list areas in order loaded");
                return;
            }

            if (firstArg.IsNumber() && secondArg.IsNullOrEmpty())
            {
                var lowerBound = long.Parse(firstArg);
                areas = RepositoryManager.Instance.AREAS.Values.Where(x => x.HighSoftRange >= lowerBound).ToList();
            }
            else if (firstArg.IsNumber() && !secondArg.IsNullOrEmpty() && secondArg.IsNumber())
            {
                var lowerBound = int.Parse(firstArg);
                var upperBound = int.Parse(secondArg);
                if (lowerBound > upperBound)
                {
                    var swap = lowerBound;
                    lowerBound = upperBound;
                    upperBound = swap;
                }
                areas = RepositoryManager.Instance.AREAS.Values.Where(x => x.HighSoftRange >= lowerBound && x.LowSoftRange <= upperBound).ToList();
            }
            else if (firstArg.EqualsIgnoreCase("old"))
            {
                areas = RepositoryManager.Instance.AREAS.Values.ToList();
            }
            else
            {
                areas = RepositoryManager.Instance.AREAS.Values.OrderBy(x => x.Name).ToList();
            }

            var header1 = "   Author    |             Area                     | Recommended |  Enforced\n\r";
            var header2 = "-------------+--------------------------------------+-------------+----------\n\r";
            var footer1 = "-----------------------------------------------------------------------------";
            ch.SetColor(ATTypes.AT_PLAIN);
            ch.SendToPager(header1);
            ch.SendToPager(header2);
            foreach (var area in areas)
            {
                ch.SendToPager($"{area.Author.PadOrTrimToCharacters(12)} | {area.Author.PadOrTrimToCharacters(36)} | " +
                    $"{area.LowSoftRange.ToString().PadOrTrimToCharacters(4)} - {area.HighSoftRange.ToString().PadOrTrimToCharacters(4)} | " +
                    $"{area.LowHardRange.ToString().PadOrTrimToCharacters(3)} - {area.HighHardRange.ToString().PadOrTrimToCharacters(3)} \n\r");
            }
            ch.SendTo(footer1);
        }
    }
}
