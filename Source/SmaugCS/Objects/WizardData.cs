using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Objects
{
    public class WizardData
    {
        public string Name { get; set; }
        public int Level { get; set; }

        public int Load(IEnumerable<string> lines)
        {
            var level = 0;

            foreach (var tuple in lines.Where(x => !x.EqualsIgnoreCase("end")).Select(line => line.FirstArgument()))
            {
                switch (tuple.Item1.ToLower())
                {
                    case "level":
                        Level = tuple.Item2.ToInt32();
                        break;
                    case "pcflags":
                        var flags = tuple.Item2.ToInt32();
                        if (flags.IsSet((int)PCFlags.Retired))
                            level = LevelConstants.MaxLevel - 15;
                        if (flags.IsSet((int)PCFlags.Guest))
                            level = LevelConstants.MaxLevel - 16;
                        break;
                }
            }

            return level;
        }
    }
}
