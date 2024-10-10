using System.Collections.Generic;
using System.Linq;
using Library.Common.Extensions;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Deity;

public static class Deities
{
  public static void do_deities(CharacterInstance ch, string argument)
  {
    if (argument.IsNullOrEmpty())
    {
      DisplayDeityList(ch);
      return;
    }

    DeityData deity = Program.RepositoryManager.GetEntity<DeityData>(argument);
    if (CheckFunctions.CheckIfNullObject(ch, deity, "&gThat deity does not exist.")) return;

    ch.PagerPrintfColor("&gDeity:        &G%s", deity.Name);
    ch.PagerPrintfColor("&gDescription:\n\r&G%s", deity.Description);
  }

  private static void DisplayDeityList(CharacterInstance ch)
  {
    ch.SendToPagerColor("&gFor detailed information on a deity, try 'deities <deity>' or 'help deities'");
    ch.SendToPagerColor("Deity			Worshippers");

    List<DeityData> deities = Program.RepositoryManager.DEITIES.Values.ToList();
    if (CheckFunctions.CheckIfTrue(ch, deities.Count == 0, "&gThere are no deities on this world.")) return;

    foreach (DeityData deity in deities)
      ch.PagerPrintfColor($"&G{deity.Name,-14}	&g{deity.Worshippers}");
  }
}