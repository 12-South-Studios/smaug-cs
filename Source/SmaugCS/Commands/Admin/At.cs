using Realm.Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Repository;

namespace SmaugCS.Commands
{
    public static class At
    {
        public static void do_at(CharacterInstance ch, string argument)
        {
            ch.SetColor(ATTypes.AT_IMMORT);

            var firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "At where what?")) return;

            RoomTemplate location = null;
            CharacterInstance worldCharacter = null;

            if (argument.IsNumber())
                location = RepositoryManager.Instance.GetEntity<RoomTemplate>(argument);
            else if (argument.EqualsIgnoreCase("pk"))
            {
                // todo get last pkroom
            }
            else
            {
                worldCharacter = RepositoryManager.Instance.GetEntity<CharacterInstance>(argument);
                if (CheckFunctions.CheckIf(ch, () => worldCharacter?.CurrentRoom == null,
                    "No such mobile or player in the world.")) return;
            }

            if (location == null && worldCharacter != null)
                location = worldCharacter.CurrentRoom;

            if (CheckFunctions.CheckIfNullObject(ch, location, "No such location exists.")) return;

            if (worldCharacter != null && !worldCharacter.IsNpc())
            {
                if (((PlayerInstance)worldCharacter).PlayerData.Flags.IsSet(PCFlags.DoNotDisturb))
                {
                    ch.PagerPrintf("Sorry. %s does not wish to be disturbed.", worldCharacter.Name);
                    worldCharacter.PagerPrintf("Your Do-Not-Disturb flag just foiled %s's AT command.", ch.Name);
                    return;
                }
            }

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
                victim.PagerPrintf("Your do-not-disturb flag just foiled %s's atmob command.", ch.Name);
                return;
            }

            ch.SetColor(ATTypes.AT_PLAIN);
            RoomTemplate original = ch.CurrentRoom;
            ch.CurrentRoom.RemoveFrom(ch);
            location.AddTo(ch);
            interp.interpret(ch, argument);

            if (!ch.CharDied())
            {
                ch.CurrentRoom.RemoveFrom(ch);
                original.AddTo(ch);
            }
        }
    }
}
