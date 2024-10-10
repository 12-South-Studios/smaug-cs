using Library.Common.Extensions;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Combat;

public static class Consider
{
  public static void do_consider(CharacterInstance ch, string argument)
  {
    string arg = argument.FirstWord();
    if (CheckFunctions.CheckIfEmptyString(ch, arg, "Consider killing whom?")) return;

    CharacterInstance victim = ch.GetCharacterInRoom(arg);
    if (CheckFunctions.CheckIfNullObject(ch, victim, "They're not here.")) return;
    if (CheckFunctions.CheckIfEquivalent(ch, ch, victim,
          "You decide you're pretty sure you could take yourself in a fight.")) return;

    int levelDiff = victim.Level - ch.Level;

    string msg = GetLevelConsiderMessage(levelDiff);
    comm.act(ATTypes.AT_CONSIDER, msg, ch, null, victim, ToTypes.Character);

    levelDiff = (victim.MaximumHealth - ch.MaximumHealth) / 6;
    msg = GetHealthConsiderMessage(levelDiff);
    comm.act(ATTypes.AT_CONSIDER, msg, ch, null, victim, ToTypes.Character);
  }

  private static string GetHealthConsiderMessage(int diff)
  {
    const string lookupName = "HealthConsider";

    if (diff <= -200)
      return Program.LookupManager.GetLookup(lookupName, 0);
    if (diff <= -150)
      return Program.LookupManager.GetLookup(lookupName, 1);
    if (diff <= -100)
      return Program.LookupManager.GetLookup(lookupName, 2);
    if (diff <= -50)
      return Program.LookupManager.GetLookup(lookupName, 3);
    if (diff <= 0)
      return Program.LookupManager.GetLookup(lookupName, 4);
    if (diff <= 50)
      return Program.LookupManager.GetLookup(lookupName, 5);
    if (diff <= 100)
      return Program.LookupManager.GetLookup(lookupName, 6);
    return diff <= 150
      ? Program.LookupManager.GetLookup(lookupName, 7)
      : Program.LookupManager.GetLookup(lookupName, diff <= 200 ? 8 : 9);
  }

  private static string GetLevelConsiderMessage(int diff)
  {
    const string lookupName = "LevelConsider";

    return diff switch
    {
      <= -10 => Program.LookupManager.GetLookup(lookupName, 0),
      <= -5 => Program.LookupManager.GetLookup(lookupName, 1),
      <= -2 => Program.LookupManager.GetLookup(lookupName, 2),
      <= 1 => Program.LookupManager.GetLookup(lookupName, 3),
      _ => diff <= 4
        ? Program.LookupManager.GetLookup(lookupName, 4)
        : Program.LookupManager.GetLookup(lookupName, diff <= 9 ? 5 : 6)
    };
  }
}