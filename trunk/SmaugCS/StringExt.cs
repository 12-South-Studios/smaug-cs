using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmaugCS.Common;
using SmaugCS.Enums;
using SmaugCS.Objects;

namespace SmaugCS
{
    public static class StringExt
    {
        public static string Scramble(this string argument, int modifier)
        {
            StringBuilder arg = new StringBuilder(argument);
            int position;
            int conversion = 0;
            int length = argument.Length;

            modifier %= SmaugRandom.Between(80, 300);

            for (position = 0; position < length; position++)
            {
                if (argument[position] >= 'A' && argument[position] <= 'Z')
                {
                    conversion = -conversion + position - modifier + argument[position] - 'A';
                    conversion = SmaugRandom.Between(conversion - 5, conversion + 5);
                    while (conversion > 25)
                        conversion -= 26;
                    while (conversion < 0)
                        conversion += 26;
                    arg[position] = Convert.ToChar(conversion + 'A');
                }
                else if (argument[position] >= 'a' && argument[position] <= 'z')
                {
                    conversion = -conversion + position - modifier + argument[position] - 'a';
                    conversion = SmaugRandom.Between(conversion - 5, conversion + 5);
                    while (conversion > 25)
                        conversion -= 26;
                    while (conversion < 0)
                        conversion += 26;
                    arg[position] = Convert.ToChar(conversion + 'a');
                }
                else if (argument[position] >= '0' && argument[position] <= '9')
                {
                    conversion = -conversion + position - modifier + argument[position] - '0';
                    conversion = SmaugRandom.Between(conversion - 2, conversion + 2);
                    while (conversion > 9)
                        conversion -= 10;
                    while (conversion < 0)
                        conversion += 10;
                    arg[position] = Convert.ToChar(conversion + '0');
                }
                else
                {
                    arg[position] = argument[position];
                }
            }

            return arg.ToString();
        }

        public static string Drunkify(this string argument, CharacterInstance ch)
        {
            if (ch.IsNpc() || ch.PlayerData == null)
                return argument;

            int drunk = ch.PlayerData.ConditionTable.ContainsKey(ConditionTypes.Drunk)
                                   ? ch.PlayerData.ConditionTable[ConditionTypes.Drunk]
                                   : 0;

            if (drunk <= 0)
                return argument;

            string newstring = string.Empty;
            char[] chars = argument.ToCharArray();

            foreach (char c in chars)
            {
                if (Char.ToUpper(c) == 'T' && Realm.Library.Common.Random.D100(1) < (drunk * 2))
                    newstring += c + 'h';
                else if (Char.ToUpper(c) == 'X' && Realm.Library.Common.Random.D100(1) < (drunk * 2 / 5))
                    newstring += c + "csh";
                else if (Realm.Library.Common.Random.D100(1) < (drunk * 2 / 5))
                {

                }
                else
                    newstring += c;
            }

            return string.Empty;
        }
    }
}
