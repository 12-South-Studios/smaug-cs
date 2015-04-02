using Realm.Library.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Exceptions;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Objects;
using SmaugCS.Helpers;

namespace SmaugCS.Commands
{
    public static class Apply
    {
        public static void do_apply(CharacterInstance ch, string argument)
        {
            var firstArg = StringExtensions.FirstWord(argument);
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Apply what?")) return;

            var secondArg = StringExtensions.SecondWord(argument);

            if (CheckFunctions.CheckIfNotNullObject(ch, ch.CurrentFighting, "You're too busy fighting...")) return;
            if (handler.FindObject_CheckMentalState(ch)) return;

            var salve = ch.GetCarriedObject(firstArg);
            if (CheckFunctions.CheckIfNullObject(ch, salve, "You do not have that.")) return;

            CharacterInstance victim;
            ObjectInstance obj = null;

            if (string.IsNullOrEmpty(secondArg))
                victim = ch;
            else 
            {
                victim = ch.GetCharacterInRoom(secondArg);
                obj = ch.GetObjectOnMeOrInRoom(secondArg);

                if (CheckFunctions.CheckIfTrue(ch, victim == null && obj == null, "Apply it to what or whom?")) return;
            }

            if (CheckFunctions.CheckIfNotNullObject(ch, obj, "You can't do that... yet.")) return;
            if (CheckFunctions.CheckIfNotNullObject(ch, victim.CurrentFighting,
                "Wouldn't work very well while they're fighting...")) return;

            if (salve.ItemType != ItemTypes.Salve)
            {
                ApplyNonSalve(salve, ch, victim);
                return;
            }

            salve.Split();
            salve.Values.Charges -= 1;

            if (!mud_prog.oprog_use_trigger(ch, salve, null, null))
                UseSalve(salve, ch, victim);

            Macros.WAIT_STATE(ch, salve.Values.Delay);
            var retcode = ch.ObjectCastSpell((int)salve.Values.Skill1ID, (int)salve.Values.SpellLevel, victim);
            if (retcode == ReturnTypes.None)
                retcode = ch.ObjectCastSpell((int)salve.Values.Skill2ID, (int)salve.Values.SpellLevel, victim);
            if (retcode == ReturnTypes.CharacterDied || retcode == ReturnTypes.BothDied)
                throw new CharacterDiedException("Salve {0}, Actor {1}, Victim {2}", salve.ID, ch.ID, victim.ID);

            if (!handler.obj_extracted(salve) && salve.Values.Charges <= 0)
                salve.Extract();
        }

        private static void ApplyNonSalve(ObjectInstance salve, CharacterInstance actor, CharacterInstance victim)
        {
            if (victim == actor)
            {
                comm.act(ATTypes.AT_ACTION, "$n starts to rub $p on $mself...", actor, salve, victim, ToTypes.Room);
                comm.act(ATTypes.AT_ACTION, "You try to rub $p on yourself....", actor, salve, victim, ToTypes.Character);
                return;
            }

            comm.act(ATTypes.AT_ACTION, "$n starts to rub $p on $N...", actor, salve, victim, ToTypes.NotVictim);
            comm.act(ATTypes.AT_ACTION, "$n starts to rub $p on you...", actor, salve, victim, ToTypes.Victim);
            comm.act(ATTypes.AT_ACTION, "You try to rub $p on $N...", actor, salve, victim, ToTypes.Character);
        }

        private static void UseSalve(ObjectInstance salve, CharacterInstance actor, CharacterInstance victim)
        {
            if (string.IsNullOrEmpty(salve.Description))
            {
                misc.actiondesc(actor, salve);
                return;
            }

            string notVictimOrRoomMessage, victimMessage, selfMessage;
            
            if (salve.Values.Charges < 1)
            {
                notVictimOrRoomMessage = victim != actor
                    ? "$n rubs the last of $p onto $N."
                    : "$n rubs the last of $p onto $mself.";
                victimMessage = victim != actor
                    ? "$n rubs the last of $p onto you." : string.Empty;
                selfMessage = victim != actor
                    ? "You rub the last of $p onto $N"
                    : "You rub the last of $p onto yourself.";
            }
            else
            {
                notVictimOrRoomMessage = victim != actor
                    ? "$n rubs $p onto $N."
                    : "$n rubs $p onto $mself.";
                victimMessage = victim != actor
                    ? "$n rubs $p onto you." : string.Empty;
                selfMessage = victim != actor
                    ? "You rub $p onto $N."
                    : "You rub $p onto yourself.";
            }

            comm.act(ATTypes.AT_ACTION, notVictimOrRoomMessage, actor, salve, victim,
                victim != actor ? ToTypes.NotVictim : ToTypes.Room);
            if (victim != actor)
                comm.act(ATTypes.AT_ACTION, victimMessage, actor, salve, victim, ToTypes.Victim);
            comm.act(ATTypes.AT_ACTION, selfMessage, actor, salve, null, ToTypes.Character);
        }
    }
}
