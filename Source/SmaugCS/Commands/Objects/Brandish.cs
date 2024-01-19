using Autofac;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.MudProgs;
using SmaugCS.Repository;
using SmaugCS.Spells;
using System.IO;
using System.Linq;

namespace SmaugCS.Commands
{
    public static class Brandish
    {
        public static void do_brandish(CharacterInstance ch, string argument)
        {
            var obj = ch.GetEquippedItem(WearLocations.Hold);
            if (CheckFunctions.CheckIfNullObject(ch, obj, "You hold nothing in your hand.")) return;
            if (CheckFunctions.CheckIfTrue(ch, obj.ItemType != ItemTypes.Staff, "You can brandish only with a staff."))
                return;

            if (obj.Value.ToList()[3] <= 0)
                throw new InvalidDataException($"Object {obj.ID} has no skill ID assigned to Value[3]");

            Macros.WAIT_STATE(ch, 2 * GameConstants.GetSystemValue<int>("PulseViolence"));

            if (obj.Value.ToList()[2] > 0)
                BrandishStaff(ch, obj);

            if (--obj.Value.ToList()[2] <= 0)
            {
                comm.act(ATTypes.AT_MAGIC, "$p blazes bright and vanishes from $n's hands!", ch, obj, null, ToTypes.Room);
                comm.act(ATTypes.AT_MAGIC, "$p blazes bright and is gone!", ch, obj, null, ToTypes.Character);
                obj.Extract();
            }
        }

        private static void BrandishStaff(CharacterInstance ch, ObjectInstance obj)
        {
            if (!MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Use, ch, obj, null, null))
            {
                comm.act(ATTypes.AT_MAGIC, "$n brandishes $p.", ch, obj, null, ToTypes.Room);
                comm.act(ATTypes.AT_MAGIC, "You brandish $p.", ch, obj, null, ToTypes.Character);
            }

            foreach (var vch in ch.CurrentRoom.Persons)
            {
                if (!vch.IsNpc())
                {
                    var pch = (PlayerInstance)ch;
                    if (pch.Act.IsSet((int)PlayerFlags.WizardInvisibility) &&
                        pch.PlayerData.WizardInvisible >= LevelConstants.ImmortalLevel)
                        continue;
                }

                var skill = Program.RepositoryManager.SKILLS.Get(obj.Value.ToList()[3]);
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
                        throw new InvalidDataException(
                            $"Bad Target {skill.Target} for Skill {skill.ID} on Object {obj.ID}");
                }

                var retcode = ch.ObjectCastSpell((int)skill.ID, obj.Value.ToList()[0], vch);
                if (retcode == ReturnTypes.CharacterDied || retcode == ReturnTypes.BothDied)
                    throw new InvalidDataException($"Character {ch.ID} died using Skill {skill.ID} from Object {obj.ID}");
            }
        }
    }
}
