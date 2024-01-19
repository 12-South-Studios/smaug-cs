using Autofac;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.MudProgs;
using System.Collections.Generic;
using System.Linq;

namespace SmaugCS.Commands
{
    public static class Say
    {
        public static void do_say(CharacterInstance ch, string argument)
        {
#if !SCRAMBLE
            var speaking = -1;
            /*foreach (int key in GameConstants.LanguageTable.Keys
                .Where(key => (key & ch.Speaking) > 0))
            {
                speaking = key;
                break;
            }*/
#endif

            if (CheckFunctions.CheckIfEmptyString(ch, argument, "Say what?")) return;
            if (CheckFunctions.CheckIf(ch,
                args => ((CharacterInstance)args[0]).CurrentRoom.Flags.IsSet((int)RoomFlags.Silence),
                "You can't do that here.", new List<object> { ch })) return;

            var actflags = ch.Act;
            if (ch.IsNpc())
                ch.Act.RemoveBit((int)ActFlags.Secretive);

            foreach (var vch in ch.CurrentRoom.Persons.Where(vch => vch != ch))
            {
                var sbuf = argument;
                if (vch.IsIgnoring(ch))
                {
                    if (!ch.IsImmortal() || vch.Trust > ch.Trust)
                        continue;

                    vch.SetColor(ATTypes.AT_IGNORE);
                    vch.Printf("You attempt to ignore %s, but are unable to do so.\r\n", ch.Name);
                }

#if !SCRAMBLE
                /*if (speaking != -1 && (!ch.IsNpc() || ch.Speaking != 0))
                {
                    int speakswell = vch.KnowsLanguage(ch.Speaking, ch).GetLowestOfTwoNumbers(ch.KnowsLanguage(ch.Speaking, vch));
                    if (speakswell < 75)
                        sbuf = act_comm.TranslateLanguage(speakswell, argument, GameConstants.LanguageTable[speaking]);
                }*/
#else
                if (KnowsLanguage(vch, ch.Speaking, ch) == 0 &&
                    (!ch.IsNpc() || ch.Speaking != 0))
                    sbuf = ScrambleText(argument, ch.Speaking);
#endif
                sbuf = sbuf.Drunkify(ch);

                // TODO Toggle global mobtrigger flag
                comm.act(ATTypes.AT_SAY, "$n says '$t'", ch, sbuf, vch, ToTypes.Victim);
            }

            ch.Act = actflags;
            // TODO Toggle global mobtrigger flag
            comm.act(ATTypes.AT_SAY, "You say '%T'", ch, null, argument.Drunkify(ch), ToTypes.Character);

            if (ch.CurrentRoom.Flags.IsSet((int)RoomFlags.LogSpeech))
            {
                // TODO
                // db.append_to_file(SystemConstants.GetSystemFile(SystemFileTypes.Log),
                 //   $"{(ch.IsNpc() ? ch.ShortDescription : ch.Name)}: {argument}");
            }

            MudProgHandler.ExecuteMobileProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Speech, argument, ch);
            if (ch.CharDied())
                return;
            MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Speech, argument, ch);
            if (ch.CharDied())
                return;
            MudProgHandler.ExecuteRoomProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Speech, argument, ch);
        }
    }
}
