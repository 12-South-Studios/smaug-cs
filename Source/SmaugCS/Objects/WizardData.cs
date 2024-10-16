﻿using System;
using Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using System.Collections.Generic;
using System.Linq;

namespace SmaugCS;

public class WizardData
{
    public string Name { get; set; }
    public int Level { get; set; }

    public int Load(IEnumerable<string> lines)
    {
            int level = 0;

            foreach (Tuple<string, string> tuple in lines.Where(x => !x.EqualsIgnoreCase("end")).Select(line => line.FirstArgument()))
            {
                switch (tuple.Item1.ToLower())
                {
                    case "level":
                        Level = tuple.Item2.ToInt32();
                        break;
                    case "pcflags":
                        int flags = tuple.Item2.ToInt32();
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