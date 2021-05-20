using System;
using System.Collections.Generic;
using Realm.Library.Common.Extensions;
using SmaugCS.Common.Enumerations;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Commands.Admin
{
    public static class Authorize
    {
        public static void do_authorize(CharacterInstance ch, string argument)
        {
            ch.SetColor(ATTypes.AT_LOG);

            var firstArg = argument.FirstWord();
            if (string.IsNullOrEmpty(firstArg))
            {
                ShowAuthorizeSyntax((PlayerInstance)ch);
                return;
            }

            CharacterInstance victim = act_wiz.get_waiting_desc(ch, firstArg);
            if (victim == null) return;
            if (victim.IsNpc()) return;

            victim.SetColor(ATTypes.AT_IMMORT);

            var secondArg = argument.SecondWord();
            if (string.IsNullOrEmpty(secondArg))
            {
                AuthorizeCharacter((PlayerInstance)ch, (PlayerInstance)victim);
                return;
            }

            if (AuthorizeFunctionTable.ContainsKey(secondArg.ToLower()))
            {
                AuthorizeFunctionTable[secondArg.ToLower()].Invoke((PlayerInstance) ch, (PlayerInstance) victim);
                return;
            }

            ch.SendTo("Invalid argument");
        }

        private static readonly Dictionary<string, Action<PlayerInstance, PlayerInstance>> AuthorizeFunctionTable = new Dictionary
            <string, Action<PlayerInstance, PlayerInstance>>
        {
            {"accept", AuthorizeCharacter},
            {"yes", AuthorizeCharacter},
            {"immsim", NameDenied_SimilarToImmortal},
            {"i", NameDenied_SimilarToImmortal},
            {"mobsim", NameDenied_SimilarToMobile},
            {"m", NameDenied_SimilarToMobile},
            {"swear", NameDenied_SwearWord},
            {"s", NameDenied_SwearWord},
            {"plain", NameDenied_Plain},
            {"p", NameDenied_Plain},
            {"unprou", NameDenied_Unpronounceable},
            {"u", NameDenied_Unpronounceable},
            {"no", AuthorizationDenied},
            {"deny", AuthorizationDenied},
            {"name", NameDenied},
            {"n", NameDenied}
        };

        private static void NameDenied(PlayerInstance ch, PlayerInstance victim)
        {
            victim.PlayerData.AuthState = AuthorizationStates.Denied;

            throw new NotImplementedException();
        }

        private static void AuthorizationDenied(PlayerInstance ch, PlayerInstance victim)
        {
            throw new NotImplementedException();
        }

        private static void NameDenied_Unpronounceable(PlayerInstance ch, PlayerInstance victim)
        {
            throw new NotImplementedException();
        }

        private static void NameDenied_Plain(PlayerInstance ch, PlayerInstance victim)
        {
            victim.PlayerData.AuthState = AuthorizationStates.Denied;

            throw new NotImplementedException();
        }

        private static void NameDenied_SwearWord(PlayerInstance ch, PlayerInstance victim)
        {
            victim.PlayerData.AuthState = AuthorizationStates.Denied;

            throw new NotImplementedException();
        }

        private static void NameDenied_SimilarToMobile(PlayerInstance ch, PlayerInstance victim)
        {
            victim.PlayerData.AuthState = AuthorizationStates.Denied;

            throw new NotImplementedException();
        }

        private static void NameDenied_SimilarToImmortal(PlayerInstance ch, PlayerInstance victim)
        {
            victim.PlayerData.AuthState = AuthorizationStates.Denied;
            
            throw new NotImplementedException();
        }

        private static void AuthorizeCharacter(PlayerInstance ch, PlayerInstance victim)
        {
            victim.PlayerData.AuthState = AuthorizationStates.Authorized;
            victim.PlayerData.AuthorizedBy = ch.Name;

            throw new NotImplementedException();
        }

        private static void ShowAuthorizeSyntax(PlayerInstance ch)
        {
            throw new NotImplementedException();
        }
    }
}
