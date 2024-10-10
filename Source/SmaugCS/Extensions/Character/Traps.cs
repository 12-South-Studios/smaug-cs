using System;
using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Objects;
using SmaugCS.Spells;
using EnumerationExtensions = Library.Common.Extensions.EnumerationExtensions;

namespace SmaugCS.Extensions.Character;

public static class Traps
{
  private const string TrapTypeLookupDefault = "hit by a trap";

  public static ReturnTypes SpringTheTrap(this CharacterInstance ch, ObjectInstance obj)
  {
    int level = obj.Value.ToList()[2];
    string txt = string.Empty;
    TrapTypes trapType = TrapTypes.None;
    DescriptorAttribute attrib = null;
    try
    {
      trapType = EnumerationExtensions.GetEnum<TrapTypes>(obj.Value.ToList()[1]);
      attrib = trapType.GetAttribute<DescriptorAttribute>();
      txt = attrib.Messages.FirstOrDefault();
    }
    catch (ArgumentException)
    {
      txt = TrapTypeLookupDefault;
    }

    int dam = SmaugRandom.Between(obj.Value.ToList()[2], obj.Value.ToList()[2] * 2);

    comm.act(ATTypes.AT_HITME, $"You are {txt}!", ch, null, null, ToTypes.Character);
    comm.act(ATTypes.AT_ACTION, $"$n is {txt}.", ch, null, null, ToTypes.Room);

    --obj.Value.ToList()[0];
    if (obj.Value.ToList()[0] <= 0)
      obj.Extract();

    ReturnTypes returnCode = ReturnTypes.None;
    if (!string.IsNullOrEmpty(attrib?.Messages.ToList()[1]))
    {
      SkillData skill = Program.RepositoryManager.GetEntity<SkillData>(attrib.Messages.ToList()[1]);
      returnCode = ch.ObjectCastSpell((int)skill.Id, level, ch);
    }

    switch (trapType)
    {
      case TrapTypes.Blade or TrapTypes.ElectricShock:
      case TrapTypes.PoisonArrow or TrapTypes.PoisonDagger or TrapTypes.PoisonDart or TrapTypes.PoisonNeedle 
        when returnCode == ReturnTypes.None:
        returnCode = ch.CauseDamageTo(ch, dam, Program.TYPE_UNDEFINED);
        break;
    }

    return returnCode;
  }

  public static ReturnTypes CheckObjectForTrap(this CharacterInstance ch, ObjectInstance obj, TrapTriggerTypes flag)
  {
    if (obj.Contents.Count == 0)
      return ReturnTypes.None;

    ReturnTypes returnCode = ReturnTypes.None;

    foreach (ObjectInstance check in obj.Contents.Where(check => check.ItemType == ItemTypes.Trap
                                                                 && check.Value.ToList()[3].IsSet(flag)))
    {
      returnCode = ch.SpringTheTrap(check);
      if (returnCode != ReturnTypes.None)
        return returnCode;
    }

    return returnCode;
  }

  public static ReturnTypes CheckRoomForTrap(this CharacterInstance ch, int flag)
  {
    if (ch.CurrentRoom.Contents.Count == 0)
      return ReturnTypes.None;

    ReturnTypes returnCode = ReturnTypes.None;

    foreach (ObjectInstance check in ch.CurrentRoom.Contents.Where(check => check.ItemType == ItemTypes.Trap
                                                                            && check.Value.ToList()[3].IsSet(flag)))
    {
      returnCode = ch.SpringTheTrap(check);
      if (returnCode != ReturnTypes.None)
        return returnCode;
    }

    return returnCode;
  }
}