using Library.Common.Objects;
using System.Collections.Generic;

namespace SmaugCS.Data;

public class CommandData(long id, string name) : Entity(id, name)
{
  public DoFunction DoFunction { get; set; }
  public string FunctionName { get; set; }
  public int Flags { get; set; }
  public int Position { get; set; }
  public int Level { get; set; }
  public LogAction Log { get; set; }
  public UseHistory userec { get; set; }
  public int lag_count { get; set; }

  private static readonly Dictionary<int, int> ModifiedPositionTable = new()
  {
    { 5, 6 },
    { 6, 8 },
    { 7, 9 },
    { 8, 12 },
    { 9, 13 },
    { 10, 14 },
    { 11, 15 }
  };

  public int ModifiedPosition
  {
    get
    {
      int originalPosition = Position;
      if (originalPosition < 100 && ModifiedPositionTable.TryGetValue(originalPosition, out int value))
        originalPosition = value;
      return originalPosition >= 100 ? originalPosition - 100 : originalPosition;
    }
  }
}