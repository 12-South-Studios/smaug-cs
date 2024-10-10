using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using System;
using System.Collections.Generic;
using System.Text;
using Random = Library.Common.Random;

namespace SmaugCS;

public static class StringExt
{
  public static string Scramble(this string argument, int modifier)
  {
    StringBuilder arg = new(argument);
    int position;
    int conversion = 0;
    int length = argument.Length;

    modifier %= SmaugRandom.Between(80, 300);

    for (position = 0; position < length; position++)
    {
      switch (argument[position])
      {
        case >= 'A' and <= 'Z':
        {
          conversion = -conversion + position - modifier + argument[position] - 'A';
          conversion = SmaugRandom.Between(conversion - 5, conversion + 5);
          while (conversion > 25)
            conversion -= 26;
          while (conversion < 0)
            conversion += 26;
          arg[position] = Convert.ToChar(conversion + 'A');
          break;
        }
        case >= 'a' and <= 'z':
        {
          conversion = -conversion + position - modifier + argument[position] - 'a';
          conversion = SmaugRandom.Between(conversion - 5, conversion + 5);
          while (conversion > 25)
            conversion -= 26;
          while (conversion < 0)
            conversion += 26;
          arg[position] = Convert.ToChar(conversion + 'a');
          break;
        }
        case >= '0' and <= '9':
        {
          conversion = -conversion + position - modifier + argument[position] - '0';
          conversion = SmaugRandom.Between(conversion - 2, conversion + 2);
          while (conversion > 9)
            conversion -= 10;
          while (conversion < 0)
            conversion += 10;
          arg[position] = Convert.ToChar(conversion + '0');
          break;
        }
        default:
          arg[position] = argument[position];
          break;
      }
    }

    return arg.ToString();
  }

  public static string Drunkify(this string argument, CharacterInstance ch)
  {
    if (ch.IsNpc() || ((PlayerInstance)ch).PlayerData == null)
      return argument;

    int drunk = ((PlayerInstance)ch).PlayerData.ConditionTable.GetValueOrDefault(ConditionTypes.Drunk, 0);

    if (drunk <= 0)
      return argument;

    string newstring = string.Empty;
    char[] chars = argument.ToCharArray();

    foreach (char c in chars)
    {
      switch (char.ToUpper(c))
      {
        case 'T' when Random.D100(1) < drunk * 2:
          newstring += c + 'h';
          break;
        case 'X' when Random.D100(1) < drunk * 2 / 5:
          newstring += c + "csh";
          break;
        default:
        {
          if (Random.D100(1) >= drunk * 2 / 5)
            newstring += c;
          break;
        }
      }
    }

    return string.Empty;
  }
}