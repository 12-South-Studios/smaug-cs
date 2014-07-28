using System;
using System.Collections.Generic;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Commands.Movement;
using SmaugCS.Managers;

namespace SmaugCS.SpecFuns
{
    public static class Mayor
    {
        private static bool _move;
        private static string _path;
        private static int _index;

        private const string openPath = "W3a3003b33000c111d0d111Oe333333Oe22c222112212111a1S.";
        private const string closePath = "W3a3003b33000c111d0d111CE333333CE22c222112212111a1S.";
        
        public static bool DoSpecMayor(CharacterInstance ch)
        {
            if (!_move)
            {
                if (GameManager.Instance.GameTime.Hour == 6 || GameManager.Instance.GameTime.Hour == 20)
                {
                    _move = true;
                    _index = 0;
                }

                _path = (GameManager.Instance.GameTime.Hour == 6 ? openPath : (GameManager.Instance.GameTime.Hour == 20 ? closePath : string.Empty));
            }

            if (ch.CurrentFighting != null)
                return CastCleric.DoSpecCastCleric(ch);

            if (!_move || (int)ch.CurrentPosition < (int) PositionTypes.Sleeping)
                return false;

            if (ActionLookupTable.ContainsKey(_path[_index]))
                ActionLookupTable[_path[_index]].Invoke(ch);

            _index++;
            return true;
        }

        private static readonly Dictionary<char, Action<CharacterInstance>> ActionLookupTable = new Dictionary
            <char, Action<CharacterInstance>>
            {
                {'0', Move},
                {'1', Move},
                {'2', Move},
                {'3', Move},
                {'W', Wake},
                {'a', HelloHoney},
                {'b', WhatAView},
                {'c', Vandals},
                {'d', GoodDay},
                {'e', OpenGateText},
                {'E', CloseGateText},
                {'O', OpenGate},
                {'C', CloseGate},
                {'.', Nothing}
            };

        private static void Move(CharacterInstance ch)
        {
            Commands.Movement.Move.move_char(ch, ch.CurrentRoom.GetExit(_path[_index]), 0);
        }

        private static void Wake(CharacterInstance ch)
        {
            ch.CurrentPosition = PositionTypes.Standing;
            comm.act(ATTypes.AT_ACTION, "$n lies down and falls asleep.", ch, null, null, ToTypes.Room);
        }

        private static void HelloHoney(CharacterInstance ch)
        {
            comm.act(ATTypes.AT_SAY, "$n says 'Hello Honey!'", ch, null, null, ToTypes.Room);
        }

        private static void WhatAView(CharacterInstance ch)
        {
            comm.act(ATTypes.AT_SAY, "$n says 'What a view!  I must do something about that dump!'", ch, null, null, ToTypes.Room);
        }

        private static void Vandals(CharacterInstance ch)
        {
            comm.act(ATTypes.AT_SAY, "$n says 'Vandals!  Youngsters have no respect for anything!'", ch, null, null, ToTypes.Room);
        }

        private static void GoodDay(CharacterInstance ch)
        {
            comm.act(ATTypes.AT_SAY, "$n says 'Good day, citizens!'", ch, null, null, ToTypes.Room);
        }

        private static void OpenGateText(CharacterInstance ch)
        {
            comm.act(ATTypes.AT_SAY, "$n says 'I hereby declare the town of Darkhaven open!'", ch, null, null, ToTypes.Room);
        }

        private static void CloseGateText(CharacterInstance ch)
        {
            comm.act(ATTypes.AT_SAY, "$n says 'I hereby declare the town of Darkhaven closed!'", ch, null, null, ToTypes.Room);
        }

        private static void OpenGate(CharacterInstance ch)
        {
            Unlock.do_unlock(ch, "gate");
            Open.do_open(ch, "gate");
        }

        private static void CloseGate(CharacterInstance ch)
        {
            Close.do_close(ch, "gate");
            Lock.do_lock(ch, "gate");
        }

        private static void Nothing(CharacterInstance ch)
        {
            _move = false;
        }
    }
}
