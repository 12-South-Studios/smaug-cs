using Realm.Library.Common.Extensions;
using Realm.Library.Common.Extensions;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;
using SmaugCS.Repository;

namespace SmaugCS.Commands.Admin
{
    public static class AtObj
    {
        public static void do_atobj(CharacterInstance ch, string argument)
        {
            ch.SetColor(ATTypes.AT_IMMORT);

            var firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "At where what?")) return;

            ObjectInstance obj = RepositoryManager.Instance.GetEntity<ObjectInstance>(argument);
            if (CheckFunctions.CheckIf(ch, () => obj?.InRoom == null,
                "No such object in existence.")) return;

            RoomTemplate location = obj.InRoom;
            if (location.IsPrivate())
            {
                if (CheckFunctions.CheckIf(ch, () => ch.Trust < LevelConstants.GetLevel(ImmortalTypes.Greater),
                    "That room is private right now.")) return;
                ch.SendTo("Overriding private flag!");
            }

            CharacterInstance victim = location.IsDoNotDisturb(ch);
            if (victim != null)
            {
                ch.PagerPrintf("That room is \"do not disturb\" right now.");
                victim.PagerPrintf("Your do-not-disturb flag just foiled %s's atobj command.", ch.Name);
                return;
            }

            ch.SetColor(ATTypes.AT_PLAIN);
            RoomTemplate original = ch.CurrentRoom;
            ch.CurrentRoom.RemoveFrom(ch);
            location.AddTo(ch);
            interp.interpret(ch, argument);

            if (ch.CharDied()) return;
            ch.CurrentRoom.RemoveFrom(ch);
            original.AddTo(ch);
        }
    }
}
