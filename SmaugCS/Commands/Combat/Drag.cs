using System;
using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Combat
{
    public static class Drag
    {
        public static void do_drag(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfNpc(ch, ch, "Only characters can drag.")) return;

            TimerData timer = ch.Timers.FirstOrDefault(x => x.Type == TimerTypes.PKilled);
            if (CheckFunctions.CheckIfNotNullObject(ch, timer, "You can't drag a player right now.")) return;

            string firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Drag whom?")) return;

            CharacterInstance victim = handler.get_char_room(ch, firstArg);
            if (CheckFunctions.CheckIfNullObject(ch, victim, "They aren't here.")) return;
            if (CheckFunctions.CheckIfEquivalent(ch, ch, victim,
                "You take yourself by the scruff of your neck, but go nowhere.")) return;
            if (CheckFunctions.CheckIfNpc(ch, victim, "You can only drag characters.")) return;

            if (CheckFunctions.CheckIfTrue(ch, !victim.Act.IsSet(PlayerFlags.ShoveDrag)
                                               || !victim.PlayerData.Flags.IsSet(PCFlags.Deadly),
                "That character doesn't seem to appreciate your attentions.")) return;

            timer = victim.Timers.FirstOrDefault(x => x.Type == TimerTypes.PKilled);
            if (CheckFunctions.CheckIfNotNullObject(ch, timer, "You can't drag that player right now.")) return;

            if (CheckFunctions.CheckIf(ch, HelperFunctions.IsFighting,
                "You try, but can't get close enough.", new List<object> {ch})) return;

            if (CheckFunctions.CheckIfTrue(ch, !ch.PlayerData.Flags.IsSet(PCFlags.Deadly)
                                               && ch.PlayerData.Flags.IsSet(PCFlags.Deadly),
                "You can't drag a deadly character.")) return;

            if (CheckFunctions.CheckIfTrue(ch, !ch.PlayerData.Flags.IsSet(PCFlags.Deadly) 
                && (int)ch.CurrentPosition > 3, "They don't seem to need your assistance.")) return;

            string secondArg = argument.SecondWord();
            if (CheckFunctions.CheckIfEmptyString(ch, secondArg, "Drag them in which direction?")) return;

            if (CheckFunctions.CheckIfTrue(ch, Math.Abs(ch.Level - victim.Level) > 5,
                "There is too great an experience difference for you to even bother.")) return;

            // TODO
        }
    }
}
