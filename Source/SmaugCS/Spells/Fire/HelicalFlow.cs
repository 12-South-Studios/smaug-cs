using System.Collections.Generic;
using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Spells.Fire;

class HelicalFlow
{
  private static readonly List<RoomFlags> FailRoomFlags =
    [RoomFlags.Private, RoomFlags.Solitary, RoomFlags.NoAstral, RoomFlags.Death, RoomFlags.Prototype];
  
  public static ReturnTypes spell_helical_flow(int sn, int level, CharacterInstance ch, object vo)
  {
    SkillData skill = Program.RepositoryManager.SKILLS.Get(sn);

    CharacterInstance victim = ch.GetCharacterInWorld((string)vo);
    if (victim == null
      || victim == ch 
      || victim.CurrentRoom == null 
      || victim.CurrentRoom.Flags.IsSet(RoomFlags.Private)
      || FailRoomFlags.Any(x => victim.CurrentRoom.Flags.IsSet(x))
      || ch.CurrentRoom.Flags.IsSet(RoomFlags.NoRecall)
      || victim.Level >= level + 15 
      || (victim.CanPKill() && !ch.IsNpc() && !ch.IsPKill())
      || (victim.IsNpc() && victim.Act.IsSet((int)ActFlags.Prototype))
      || (victim.IsNpc() && victim.SavingThrows.CheckSaveVsSpellStaff(level, victim))
      || !victim.CurrentRoom.Area.IsInHardRange(ch) 
      || (victim.CurrentRoom.Area.Flags.IsSet(AreaFlags.NoPlayerVsPlayer) && ch.IsPKill()))
    {
      ch.FailedCast(skill, victim, null);
      return ReturnTypes.SpellFailed;
    }

    comm.act(ATTypes.AT_MAGIC, "$n coils into an ascending column of colour, vanishing into thin air.", 
      ch, null, null, ToTypes.Room);
    ch.CurrentRoom.RemoveFrom(ch);
    victim.CurrentRoom.AddTo(ch, Program.RepositoryManager);
    comm.act(ATTypes.AT_MAGIC, "A coil of colours descends from above, revealing $n as it dissipates.", 
      ch, null, null, ToTypes.Room);
    // TODO do_look(ch, "auto");
    return ReturnTypes.None;
  }
}