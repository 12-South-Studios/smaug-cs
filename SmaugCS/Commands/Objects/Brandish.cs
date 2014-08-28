﻿using System.IO;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Objects
{
    public static class Brandish
    {
        public static void do_brandish(CharacterInstance ch, string argument)
        {
            ObjectInstance obj = ch.GetEquippedItem(WearLocations.Hold);
            if (CheckFunctions.CheckIfNullObject(ch, obj, "You hold nothing in your hand.")) return;
            if (CheckFunctions.CheckIfTrue(ch, obj.ItemType != ItemTypes.Staff, "You can brandish only with a staff."))
                return;

            if (obj.Value[3] <= 0)
                throw new InvalidDataException(string.Format("Object {0} has no skill ID assigned to Value[3]", obj.ID));

            Macros.WAIT_STATE(ch, 2*GameConstants.GetSystemValue<int>("PulseViolence"));

            if (obj.Value[2] > 0)
                BrandishStaff(ch, obj);

            if (--obj.Value[2] <= 0)
            {
                comm.act(ATTypes.AT_MAGIC, "$p blazes bright and vanishes from $n's hands!", ch, obj, null, ToTypes.Room);
                comm.act(ATTypes.AT_MAGIC, "$p blazes bright and is gone!", ch, obj, null, ToTypes.Character);

                //if(obj.Serial == cur_obj)
                //    global_objcode = rOBJ_USED;

                handler.extract_obj(obj);
            }
        }

        private static void BrandishStaff(CharacterInstance ch, ObjectInstance obj)
        {
            if (!mud_prog.oprog_use_trigger(ch, obj, null, null))
            {
                comm.act(ATTypes.AT_MAGIC, "$n brandishes $p.", ch, obj, null, ToTypes.Room);
                comm.act(ATTypes.AT_MAGIC, "You brandish $p.", ch, obj, null, ToTypes.Character);
            }

            foreach (CharacterInstance vch in ch.CurrentRoom.Persons)
            {
                if (!vch.IsNpc() && vch.Act.IsSet(PlayerFlags.WizardInvisibility) &&
                    vch.PlayerData.WizardInvisible >= LevelConstants.ImmortalLevel)
                    continue;

                SkillData skill = DatabaseManager.Instance.SKILLS.Get(obj.Value[3]);
                switch (skill.Target)
                {
                    case TargetTypes.Ignore:
                        if (vch != ch) continue;
                        break;
                    case TargetTypes.OffensiveCharacter:
                        if (ch.IsNpc() ? vch.IsNpc() : !vch.IsNpc())
                            continue;
                        break;
                    case TargetTypes.DefensiveCharacter:
                        if (ch.IsNpc() ? !vch.IsNpc() : vch.IsNpc())
                            continue;
                        break;
                    case TargetTypes.Self:
                        if (vch != ch) continue;
                        break;
                    default:
                        throw new InvalidDataException(string.Format("Bad Target {0} for Skill {1} on Object {2}",
                            skill.Target, skill.ID, obj.ID));
                }

                ReturnTypes retcode = magic.obj_cast_spell((int)skill.ID, obj.Value[0], ch, vch, null);
                if (retcode == ReturnTypes.CharacterDied || retcode == ReturnTypes.BothDied)
                    throw new InvalidDataException(string.Format("Character {0} died using Skill {1} from Object {2}",
                        ch.ID, skill.ID, obj.ID));
            }
        }
    }
}
