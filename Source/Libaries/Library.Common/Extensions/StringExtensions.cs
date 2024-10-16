﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Common.Extensions;

/// <summary>
/// Extension class for String objects
/// </summary>
public static class StringExtensions
{
  /// <summary>
  /// 
  /// </summary>
  /// <param name="value"></param>
  /// <param name="compareTo"></param>
  /// <returns></returns>
  public static bool NotEquals(this string value, string compareTo)
  {
    return !value.Equals(compareTo);
  }

  /// <summary>
  /// Simple extension wrapper around the String.IsNullOrEmpty function
  /// </summary>
  /// <param name="value"></param>
  /// <returns></returns>
  public static bool IsNullOrEmpty(this string value)
  {
    return string.IsNullOrEmpty(value);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="value"></param>
  /// <returns></returns>
  public static bool IsNullOrWhitespace(this string value)
  {
    return string.IsNullOrWhiteSpace(value);
  }

  /// <summary>
  /// Returns true if the string is completely numeric
  /// </summary>
  /// <param name="value"></param>
  /// <returns></returns>
  public static bool IsNumber(this string value)
  {
    try
    {
      return int.TryParse(value, out int _);
    }
    catch (Exception)
    {
      return false;
    }
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="value"></param>
  /// <returns></returns>
  public static List<string> ToWords(this string value)
  {
    return value.Split(' ').ToList();
  }

  /// <summary>
  /// Checks the equality of two strings, regardless of case
  /// </summary>
  /// <param name="value"></param>
  /// <param name="compareTo"></param>
  /// <returns></returns>
  public static bool EqualsIgnoreCase(this string value, string compareTo)
  {
    return value.Equals(compareTo, StringComparison.OrdinalIgnoreCase);
  }

  /// <summary>
  /// Checks if the string starts with another starts, regardless of case
  /// </summary>
  /// <param name="value"></param>
  /// <param name="startsWith"></param>
  /// <returns></returns>
  public static bool StartsWithIgnoreCase(this string value, string startsWith)
  {
    return value.StartsWith(startsWith, StringComparison.OrdinalIgnoreCase);
  }

  /// <summary>
  /// Checks if the string contains another string, regardless of case
  /// </summary>
  /// <param name="value"></param>
  /// <param name="contains"></param>
  /// <returns></returns>
  public static bool ContainsIgnoreCase(this string value, string contains)
  {
    return value.Contains(contains, StringComparison.OrdinalIgnoreCase);
  }

  /// <summary>
  /// Converts the string to a 32-bit integer value
  /// </summary>
  /// <param name="argument"></param>
  /// <returns></returns>
  public static int ToInt32(this string argument)
  {
    int val;
    try
    {
      val = Convert.ToInt32(argument);
    }
    catch
    {
      val = 0;
    }

    return val;
  }

  /// <summary>
  /// Converts the string to a boolean value
  /// </summary>
  /// <param name="argument"></param>
  /// <returns></returns>
  public static bool ToBoolean(this string argument)
  {
    return argument.EqualsIgnoreCase("true")
           || argument.ToInt32() == 1
           || argument.EqualsIgnoreCase("t");
  }

  private static readonly char[] WordListDelimiters = { ' ', '-' };

  /// <summary>
  /// Determines if the given string is equal to any values in the passed string list
  /// </summary>
  /// <param name="value"></param>
  /// <param name="wordList"></param>
  /// <returns></returns>
  /// <remarks>Was formerly known as is_name</remarks>
  public static bool IsEqual(this string value, string wordList)
  {
    string[] words = wordList.Split(WordListDelimiters);
    return words.Any(word => word.EqualsIgnoreCase(value));
  }

  /// <summary>
  /// Determines if the given string is equal to any of the prefix values in the passed string list
  /// </summary>
  /// <param name="value"></param>
  /// <param name="wordList"></param>
  /// <returns></returns>
  /// <remarks>Was formerly known as is_name2</remarks>
  public static bool IsEqualPrefix(this string value, string wordList)
  {
    string[] words = wordList.Split(WordListDelimiters);
    return words.Any(word => word.StartsWithIgnoreCase(value));
  }

  /// <summary>
  /// Checks all words in the given string against the passed word list
  /// </summary>
  /// <param name="value"></param>
  /// <param name="wordList"></param>
  /// <returns></returns>
  /// <remarks>Was formerly known as nifty_is_name</remarks>
  public static bool IsAnyEqual(this string value, string wordList)
  {
    if (string.IsNullOrWhiteSpace(value))
      return false;

    string[] wordsToCheck = value.Split(WordListDelimiters);
    return wordsToCheck.Any(word => word.IsEqual(wordList));
  }

  /// <summary>
  /// Checks all words in the given string for a prefix match against the passed word list
  /// </summary>
  /// <param name="value"></param>
  /// <param name="wordList"></param>
  /// <returns></returns>
  /// <remarks>Was formerly known as nifty_is_name_prefix</remarks>
  public static bool IsAnyEqualPrefix(this string value, string wordList)
  {
    if (string.IsNullOrWhiteSpace(value))
      return false;

    string[] wordsToCheck = value.Split(WordListDelimiters);
    return wordsToCheck.Any(word => word.IsEqualPrefix(wordList));
  }

  /// <summary>
  /// Repeats a character a specified number of times
  /// </summary>
  /// <param name="chatToRepeat"></param>
  /// <param name="repeat"></param>
  /// <returns></returns>
  public static string Repeat(this char chatToRepeat, int repeat)
  {
    return new string(chatToRepeat, repeat);
  }

  /// <summary>
  /// Repeats a string a specified number of times
  /// </summary>
  /// <param name="stringToRepeat"></param>
  /// <param name="repeat"></param>
  /// <returns></returns>
  public static string Repeat(this string stringToRepeat, int repeat)
  {
    StringBuilder builder = new(repeat * stringToRepeat.Length);
    for (int i = 0; i < repeat; i++)
      builder.Append(stringToRepeat);
    return builder.ToString();
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="str"></param>
  /// <param name="index"></param>
  /// <param name="c"></param>
  /// <returns></returns>
  public static string SetChar(this string str, int index, char c)
  {
    char[] charArray = str.ToCharArray();
    charArray[index] = c;
    return new string(charArray);
  }

  /// <summary>
  /// Appends an 'a' or an 'an' to the front of a string
  /// </summary>
  /// <param name="value"></param>
  /// <returns></returns>
  public static string AOrAn(this string value)
  {
    return value[0].IsVowel() ? "an " + value : "a " + value;
  }

  /// <summary>
  /// Returns true if a string contains only alphanumeric characters
  /// </summary>
  /// <param name="str"></param>
  /// <returns></returns>
  public static bool IsAlphaNum(this string str)
  {
    return !string.IsNullOrEmpty(str)
           && str.ToCharArray().All(c => char.IsLetter(c)
                                         || char.IsNumber(c));
  }

  /// <summary>
  /// Convert a string to a byte array.
  /// </summary>
  public static IEnumerable<byte> ToByteArray(this string value)
  {
    Validation.IsNotNullOrEmpty(value, "value");

    ASCIIEncoding encoding = new();
    return encoding.GetBytes(value);
  }

  /// <summary>
  /// Gets the first word in the string
  /// </summary>
  /// <param name="value"></param>
  /// <returns></returns>
  public static string FirstWord(this string value)
  {
    Validation.IsNotNullOrEmpty(value, "value");

    return value.ParseWord(1, DelimiterType.Whitespace.ValueOf());
  }

  /// <summary>
  /// Gets the second word in the string
  /// </summary>
  /// <param name="value"></param>
  /// <returns></returns>
  public static string SecondWord(this string value)
  {
    Validation.IsNotNullOrEmpty(value, "value");

    return value.ParseWord(2, DelimiterType.Whitespace.ValueOf());
  }

  /// <summary>
  /// Gets the third word in the string
  /// </summary>
  /// <param name="value"></param>
  /// <returns></returns>
  public static string ThirdWord(this string value)
  {
    Validation.IsNotNullOrEmpty(value, "value");

    return value.ParseWord(3, DelimiterType.Whitespace.ValueOf());
  }

  /// <summary>
  /// Allows a contains check to be passed a string comparison value (allows for case-insensitive contains)
  /// </summary>
  public static bool Contains(this string value, string toCheck, StringComparison comp)
  {
    Validation.IsNotNullOrEmpty(value, "value");
    Validation.IsNotNullOrEmpty(toCheck, "toCheck");

    return value.Contains(toCheck, comp);
  }

  /// <summary>
  /// Replaces the first occurrence of the string
  /// </summary>
  public static string ReplaceFirst(this string value, string search, string replace)
  {
    Validation.IsNotNullOrEmpty(value, "value");
    Validation.IsNotNullOrEmpty(search, "search");
    Validation.IsNotNullOrEmpty(replace, "replace");

    int loc = value.IndexOf(search, StringComparison.Ordinal);
    if (loc == -1) return value;
    string firstHalf = value[..loc];
    string secondHalf = value.Substring(loc + search.Length, value.Length - (loc + search.Length));
    value = firstHalf + replace + secondHalf;
    return value;
  }

  /// <summary>
  /// Capitalizes the first letter of the string
  /// </summary>
  public static string CapitalizeFirst(this string value)
  {
    Validation.IsNotNullOrEmpty(value, "value");

    return char.ToUpper(value[0]) + value[1..];
  }

  /// <summary>
  /// Parses the given word from the string
  /// </summary>
  public static string ParseWord(this string value, int wordNumber, string delimiter)
  {
    Validation.IsNotNullOrEmpty(value, "value");
    Validation.Validate<ArgumentOutOfRangeException>(wordNumber is >= 0 and < int.MaxValue);
    Validation.IsNotNullOrEmpty(delimiter, "delimiter");

    string[] strArray = value.Split(delimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
    return strArray.Length >= wordNumber ? strArray[wordNumber - 1] : string.Empty;
  }

  /// <summary>
  ///
  /// </summary>
  /// <param name="value"></param>
  /// <param name="padChar"></param>
  /// <param name="totalLength"></param>
  /// <returns></returns>
  public static string PadStringToFront(this string value, string padChar, int totalLength)
  {
    return PadString(value, padChar, totalLength, true);
  }

  public static string PadOrTrimToCharacters(this string value, int totalLength, string padChar = " ")
  {
    return value.Length > totalLength ? value[..totalLength] : value.PadString(padChar, totalLength);
  }

  /// <summary>
  ///
  /// </summary>
  /// <param name="value"></param>
  /// <param name="padChar"></param>
  /// <param name="totalLength"></param>
  /// <returns></returns>
  public static string PadString(this string value, string padChar, int totalLength)
  {
    return PadString(value, padChar, totalLength, false);
  }

  /// <summary>
  /// Pads characters to the front or back of the string
  /// </summary>
  private static string PadString(this string value, string padChar, int totalLength, bool toFront)
  {
    Validation.IsNotNullOrEmpty(value, "value");
    Validation.IsNotNullOrEmpty(padChar, "padChar");
    Validation.Validate<ArgumentOutOfRangeException>(totalLength > 0);

    string newString = string.Empty;
    string returnString;

    int length = totalLength - value.Length;
    if (!toFront)
    {
      for (int i = 0; i < length; i++)
        newString += padChar;
      returnString = value + newString;
    }
    else
    {
      for (int i = 0; i < length; i++)
        newString += padChar;

      returnString = newString + value;
    }

    return returnString;
  }

  /// <summary>
  /// Removes the word from the string at the given index
  /// </summary>
  public static string RemoveWord(this string value, int wordNumber)
  {
    Validation.IsNotNullOrEmpty(value, "value");
    Validation.Validate<ArgumentOutOfRangeException>(wordNumber is > 0 and < int.MaxValue);

    string[] strArray = value.Split(DelimiterType.Whitespace.ValueOf().ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

    string newString = string.Empty;
    int count = 0;
    foreach (string str in strArray)
    {
      if (count == wordNumber - 1)
      {
        count++;
        continue;
      }

      newString += str + " ";
      count++;
    }

    // strip the last space off the end
    if (!string.IsNullOrEmpty(newString))
      newString = newString[..^1];
    return newString;
  }

  /// <summary>
  /// Trims the delimiter from the given string
  /// </summary>
  public static string Trim(this string value, string delimiter)
  {
    Validation.IsNotNullOrEmpty(value, "value");
    Validation.IsNotNullOrEmpty(delimiter, "delimiter");

    value = value.TrimStart(delimiter.ToCharArray());
    value = value.TrimEnd(delimiter.ToCharArray());
    return value;
  }

  /// <summary>
  /// Converts a string to a list of words by splitting them using
  /// the passed delimiter array.
  /// </summary>
  public static ICollection<string> Split(this string value, char[] delimiters)
  {
    Validation.IsNotNullOrEmpty(value, "value");
    Validation.IsNotNull(delimiters, "delimiters");
    Validation.Validate(delimiters.Length > 0);

    return value.Split(delimiters).ToList();
  }

  /// <summary>
  /// Parses a numerical value out of the string
  /// </summary>
  public static int ParseQuantity(this string value)
  {
    Validation.IsNotNullOrEmpty(value, "value");

    if (!value.Contains('#')) return 0;

    int qty = 0;
    string quantity = value.ParseWord(1, "#");
    try
    {
      qty = Convert.ToInt32(quantity);
      return qty;
    }
    catch (FormatException)
    {
      return qty;
    }
  }

  /// <summary>
  /// Replaces an array of characters in a string
  /// </summary>
  public static string ReplaceAll(this string value, char[] characters, char replacementCharacter)
  {
    Validation.IsNotNullOrEmpty(value, "value");
    Validation.IsNotNull(characters, "characters");
    Validation.Validate(characters.Length > 0);

    return characters.Aggregate(value, (current, c) => current.Replace(c, replacementCharacter));
  }

  /// <summary>
  /// Removes the array of characters from a string
  /// </summary>
  /// <param name="value"></param>
  /// <param name="characters"></param>
  /// <returns></returns>
  public static string RemoveAll(this string value, ICollection<char> characters)
  {
    Validation.IsNotNullOrEmpty(value, "value");
    Validation.IsNotNull(characters, "characters");
    Validation.Validate(characters.Count > 0);

    StringBuilder sb = new();
    value.Where(t => !characters.Contains(t)).ToList().ForEach(t => sb.Append(t));
    return sb.ToString();
  }

  /// <summary>
  /// Takes a given string and modifies it using the indicated parameters, adding
  /// a new line, or capitalizing the first letter, or appending the/a/an to the front.
  /// </summary>
  public static string AddArticle(this string value, ArticleAppendOptions appendOptions = 0)
  {
    Validation.IsNotNullOrEmpty(value, "value");

    StringBuilder sb = new(value);
    if (appendOptions.HasFlag(ArticleAppendOptions.TheToFront))
      sb.Insert(0, "the ");
    else
    {
      if (value[0].IsVowel())
        sb.Insert(0, "an ");
      else if (value.StartsWith("the"))
      {
        // do nothing
      }
      else
        sb.Insert(0, "a ");
    }

    if (appendOptions.HasFlag(ArticleAppendOptions.CapitalizeFirstLetter))
      sb = sb.CapitalizeFirst();

    if (appendOptions.HasFlag(ArticleAppendOptions.NewLineToEnd))
      sb.Insert(0, Environment.NewLine);

    return sb.ToString();
  }

  /// <summary>
  /// Compares two strings and returns an enumerated value of the result.
  /// This function is similar to strcmp or strcasecmp in c++
  /// </summary>
  /// <param name="value"></param>
  /// <param name="toCompare"></param>
  /// <returns></returns>
  public static CaseCompareResult CaseCompare(this string value, string toCompare)
  {
    Validation.IsNotNullOrEmpty(value, "value");

    int result = string.CompareOrdinal(value.ToLower(), toCompare.ToLower());
    if (result == 0) return CaseCompareResult.Equal;
    return result < 0 ? CaseCompareResult.LessThan : CaseCompareResult.GreaterThan;
  }
}