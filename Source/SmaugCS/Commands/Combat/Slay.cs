using System;
using System.Collections.Generic;
using Library.Common.Extensions;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Combat;

public static class Slay
{
  private static readonly Dictionary<string, Action<CharacterInstance, CharacterInstance>> SlayTable = new()
  {
    { "immolate", Immolate },
    { "shatter", Shatter },
    { "demon", Demon },
    { "pounce", Pounce },
    { "slit", Slit },
    { "dog", Dog }
  };

  public static void do_slay(CharacterInstance ch, string argument)
  {
    string firstArg = argument.FirstWord();
    if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Slay whom?")) return;

    CharacterInstance victim = ch.GetCharacterInRoom(firstArg);
    if (CheckFunctions.CheckIfNullObject(ch, victim, "They aren't here.")) return;
    if (CheckFunctions.CheckIfEquivalent(ch, ch, victim, "Suicide is a mortal sin.")) return;
    if (CheckFunctions.CheckIfTrue(ch, !victim.IsNpc() && victim.Trust >= ch.Trust, "You failed.")) return;

    string secondArg = argument.SecondWord();
    if (SlayTable.ContainsKey(secondArg.ToLower()))
      SlayTable[secondArg.ToLower()].Invoke(ch, victim);
    else
      Default(ch, victim);

    handler.set_cur_char(victim);
    ch.RawKill(victim);
  }

  private static void Immolate(CharacterInstance ch, CharacterInstance victim)
  {
    comm.act(ATTypes.AT_FIRE, "Your fireball turns $N into a blazing inferno.", ch, null, victim, ToTypes.Character);
    comm.act(ATTypes.AT_FIRE, "$n releases a searing fireball in your direction.", ch, null, victim, ToTypes.Victim);
    comm.act(ATTypes.AT_FIRE, "$n points at $N, who bursts into a flaming inferno.", ch, null, victim,
      ToTypes.NotVictim);
  }

  private static void Shatter(CharacterInstance ch, CharacterInstance victim)
  {
    comm.act(ATTypes.AT_LBLUE, "You freeze $N with a glance and shatter the frozen corpse into tiny shards.", ch, null,
      victim, ToTypes.Character);
    comm.act(ATTypes.AT_LBLUE, "$n freezes you with a glance and shatters your frozen body into tiny shards.", ch, null,
      victim, ToTypes.Victim);
    comm.act(ATTypes.AT_LBLUE, "$n freezes $N with a glance and shatters $S frozen body into tiny shards.", ch, null,
      victim, ToTypes.NotVictim);
  }

  private static void Demon(CharacterInstance ch, CharacterInstance victim)
  {
    comm.act(ATTypes.AT_IMMORT, "You gesture, and a slavering demon appears.  With a horrible grin, the", ch, null,
      victim, ToTypes.Character);
    comm.act(ATTypes.AT_IMMORT, "foul creature turns on $N, who screams in panic before being eaten alive.", ch, null,
      victim,
      ToTypes.Character);
    comm.act(ATTypes.AT_IMMORT, "$n gestures, and a slavering demon appears.  The foul creature turns on", ch, null,
      victim, ToTypes.Victim);
    comm.act(ATTypes.AT_IMMORT, "you with a horrible grin.   You scream in panic before being eaten alive.", ch, null,
      victim,
      ToTypes.Victim);
    comm.act(ATTypes.AT_IMMORT, "$n gestures, and a slavering demon appears.  With a horrible grin, the", ch, null,
      victim,
      ToTypes.NotVictim);
    comm.act(ATTypes.AT_IMMORT, "foul creature turns on $N, who screams in panic before being eaten alive.", ch, null,
      victim,
      ToTypes.NotVictim);
  }

  private static void Pounce(CharacterInstance ch, CharacterInstance victim)
  {
    comm.act(ATTypes.AT_BLOOD,
      "Leaping upon $N with bared fangs, you tear open $S throat and toss the corpse to the ground...", ch, null,
      victim, ToTypes.Character);
    comm.act(ATTypes.AT_BLOOD,
      "In a heartbeat, $n rips $s fangs through your throat!  Your blood sprays and pours to the ground as your life ends...",
      ch, null, victim, ToTypes.Victim);
    comm.act(ATTypes.AT_BLOOD,
      "Leaping suddenly, $n sinks $s fangs into $N's throat.  As blood sprays and gushes to the ground, $n tosses $N's dying body away.",
      ch, null, victim, ToTypes.NotVictim);
  }

  private static void Slit(CharacterInstance ch, CharacterInstance victim)
  {
    comm.act(ATTypes.AT_BLOOD, "You calmly slit $N's throat.", ch, null, victim, ToTypes.Character);
    comm.act(ATTypes.AT_BLOOD, "$n reaches out with a clawed finger and calmly slits your throat.", ch, null, victim,
      ToTypes.Victim);
    comm.act(ATTypes.AT_BLOOD, "$n calmly slits $N's throat.", ch, null, victim, ToTypes.NotVictim);
  }

  private static void Dog(CharacterInstance ch, CharacterInstance victim)
  {
    comm.act(ATTypes.AT_BLOOD, "You order your dogs to rip $N to shreds.", ch, null, victim, ToTypes.Character);
    comm.act(ATTypes.AT_BLOOD, "$n orders $s dogs to rip you apart.", ch, null, victim, ToTypes.Victim);
    comm.act(ATTypes.AT_BLOOD, "$n orders $s dogs to rip $N to shreds.", ch, null, victim, ToTypes.NotVictim);
  }

  private static void Default(CharacterInstance ch, CharacterInstance victim)
  {
    comm.act(ATTypes.AT_IMMORT, "You slay $N in cold blood!", ch, null, victim, ToTypes.Character);
    comm.act(ATTypes.AT_IMMORT, "$n slays you in cold blood!", ch, null, victim, ToTypes.Victim);
    comm.act(ATTypes.AT_IMMORT, "$n slays $N in cold blood!", ch, null, victim, ToTypes.NotVictim);
  }
}