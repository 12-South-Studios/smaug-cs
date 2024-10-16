﻿using Library.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SmaugCS.Common;

public static class StringExtensions
{
  public static bool IsAllUpper(this string value) => Regex.IsMatch(value, @"^[A-Z ]+$");

  public static string TrimHash(this string value) => value.TrimEnd('~');

  /// <summary>
  /// Given a string like 14.foo, returns 14 and foo as a tuple
  /// </summary>
  /// <param name="value"></param>
  /// <returns>Tuple where Item1 is the number and Item2 is the remainder</returns>
  public static Tuple<int, string> NumberArgument(this string value)
  {
    string[] words = value.Split('.');

    if (words.Length < 2 || !words[0].IsNumber()) return new Tuple<int, string>(1, value);

    if (!int.TryParse(words[0], out int val))
      return null;

    string outVal = string.Empty;
    for (int i = 1; i < words.Length; i++)
      outVal += words[i];

    return new Tuple<int, string>(val, outVal);
  }

  public static bool IsNumberArgument(this string value)
  {
    string[] words = value.Split('.');
    return words.Length >= 2 && words[0].IsNumber();
  }

  public static int GetNumberArgument(this string value)
  {
    string[] words = value.Split('.');

    if (words.Length < 2 || !words[0].IsNumber()) return 0;

    return !int.TryParse(words[0], out int val) ? 0 : val;
  }

  public static string StripNumberArgument(this string value)
  {
    if (!value.IsNumberArgument())
      return value;

    string[] words = value.Split('.');
    string outVal = string.Empty;
    for (int i = 1; i < words.Length; i++)
      outVal += words[i];
    return outVal;
  }

  /// <summary>
  /// Pick off one argument from a string and return a tuple
  /// </summary>
  /// <param name="value"></param>
  /// <returns>Tuple where Item1 is the first word and Item2 is the remainder</returns>
  /// <remarks>Was formerly known as one_argument</remarks>
  public static Tuple<string, string> FirstArgument(this string value)
    => new(value.FirstWord(), value.RemoveWord(1));

  /// <summary>
  /// Picks off one argument from a string and returns the rest.  Uses quotes
  /// </summary>
  /// <param name="value"></param>
  /// <returns>Tuple where Item1 is the first word and Item2 is the remainder</returns>
  /// <remarks>Was formerly known as one_argument2</remarks>
  public static Tuple<string, string> FirstArgumentWithQuotes(this string value)
  {
    if (!value.Contains('"') && !value.Contains('\''))
      return value.FirstArgument();

    Tuple<string, string> returnVal = value.FirstArgument();

    // First argument starts with a quote so find end and make that the entire "first argument"
    if (!returnVal.Item1.StartsWith('\"') && !returnVal.Item1.StartsWith('\'')) return returnVal;

    int index = returnVal.Item2.IndexOfAny(['\"', '\'']);
    string newStr = string.Concat(returnVal.Item1, " ", returnVal.Item2.AsSpan(0, index + 1));
    string remainder = returnVal.Item2.Remove(0, index + 1);
    returnVal = new Tuple<string, string>(newStr, remainder);

    return returnVal;
  }

  /// <summary>
  /// Converts the given string to a bitvector
  /// </summary>
  /// <param name="argument"></param>
  /// <returns></returns>
  public static ExtendedBitvector ToBitvector(this string argument)
  {
    ExtendedBitvector bit = new();

    List<string> untrimmed = argument.TrimEnd('~').Split('&').ToList();

    List<string> numbers = [];
    untrimmed.ForEach(s => numbers.Add(s.Trim()));

    foreach (string number in numbers)
    {
      if (!int.TryParse(number, out int num))
        continue;

      bit.SetBit(num);
    }

    return bit;
  }

  /// <summary>
  /// Replaces tildes in the given string with the replacement character
  /// </summary>
  /// <param name="str"></param>
  /// <param name="defaultChar"></param>
  /// <returns></returns>
  public static string SmashTilde(this string str, string defaultChar = "-") => str.HideTilde(defaultChar);

  /// <summary>
  /// Hides the tilde in teh given string, replacing it with a given character
  /// </summary>
  /// <param name="str"></param>
  /// <param name="hiddenChar"></param>
  /// <returns></returns>
  public static string HideTilde(this string str, string hiddenChar = "*") => str.Replace("~", hiddenChar);

  /// <summary>
  /// Unhides the tilde in a string, replacing the given character with a tilde
  /// </summary>
  /// <param name="str"></param>
  /// <param name="hiddenChar"></param>
  /// <returns></returns>
  public static string UnhideTilde(this string str, string hiddenChar = "*") => str.Replace(hiddenChar, "~");

  /// <summary>
  /// Picks off one argument from a string and returns the rest
  /// </summary>
  /// <param name="str"></param>
  /// <param name="firstWord"></param>
  /// <returns></returns>
  public static string OneArgument(this string str, out string firstWord)
  {
    firstWord = string.Empty;
    if (!str.IsNullOrEmpty()) return string.Empty;

    firstWord = str.FirstWord();
    return str.RemoveWord(0);
  }
}