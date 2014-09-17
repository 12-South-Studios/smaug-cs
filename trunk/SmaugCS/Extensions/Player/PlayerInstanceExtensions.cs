using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Managers;

namespace SmaugCS.Extensions
{
    public static class PlayerInstanceExtensions
    {
        public static void StopIdling(this PlayerInstance ch)
        {
            if (ch == null || ch.Descriptor == null
                || ch.Descriptor.ConnectionStatus != ConnectionTypes.Playing)
                return;

            ch.Timer = 0;
            RoomTemplate wasInRoom = ch.PreviousRoom;
            ch.CurrentRoom.FromRoom(ch);
            wasInRoom.ToRoom(ch);
            ch.PreviousRoom = ch.CurrentRoom;

            ch.PlayerData.Flags.RemoveBit((int)PCFlags.Idle);
            comm.act(ATTypes.AT_ACTION, "$n has returned from the world.", ch, null, null, ToTypes.Room);
        }

        public static int GetLearned(this PlayerInstance ch, int skillId)
        {
            if (ch.PlayerData == null) return 0;
            if (ch.PlayerData.Learned == null) return 0;
            if (!ch.PlayerData.Learned.Contains(skillId)) return 0;
            return ch.PlayerData.Learned[skillId];
        }

        public static int CalculateAge(this PlayerInstance ch)
        {
            int numDays = ((GameManager.Instance.GameTime.Month + 1) * GameConstants.GetSystemValue<int>("DaysPerMonth")) +
                           GameManager.Instance.GameTime.Day;
            int chDays = ((ch.PlayerData.Month + 1) *
                           GameConstants.GetSystemValue<int>("DaysPerMonth")) + ch.PlayerData.Day;
            int age = GameManager.Instance.GameTime.Year - ch.PlayerData.Year;

            if (chDays - numDays > 0)
                age -= 1;
            return age;
        }

        private static readonly List<AffectedByTypes> AffectedByList = new List<AffectedByTypes> 
        { 
            AffectedByTypes.FireShield, AffectedByTypes.ShockShield, AffectedByTypes.AcidMist,
            AffectedByTypes.IceShield, AffectedByTypes.VenomShield, AffectedByTypes.Charm
        };

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static void ShowVisibleAffectsOn(this PlayerInstance ch, CharacterInstance victim)
        {
            ATTypes atType = ATTypes.AT_WHITE;
            string description = string.Empty;
            VisibleAffectAttribute attrib = null;

            if (victim.IsAffected(AffectedByTypes.Sanctuary))
            {
                string name = (victim.IsNpc() ? victim.ShortDescription : victim.Name).CapitalizeFirst();

                atType = ATTypes.AT_WHITE;
                if (victim.IsGood())
                    description = string.Format("{0} glows with an aura of divine radiance.\r\n", name);
                else if (victim.IsEvil())
                    description = string.Format("{0} shimmers beneath an aura of dark energy.\r\n", name);
                else
                    description = string.Format("{0} is shrouded in flowing shadow and light.\r\n", name);
            }
            if (!victim.IsNpc() && victim.Switched.IsAffected(AffectedByTypes.Possess))
                attrib = AffectedByTypes.Possess.GetAttribute<VisibleAffectAttribute>();
            else
            {
                AffectedByTypes affectedBy = AffectedByList.FirstOrDefault(victim.IsAffected);
                if (affectedBy != AffectedByTypes.None)
                    attrib = AffectedByTypes.Possess.GetAttribute<VisibleAffectAttribute>();
            }

            if (attrib != null)
            {
                atType = attrib.ATType;
                description = attrib.Description;
            }

            color.set_char_color(atType, ch);
            color.ch_printf(ch, description);
        }
    }
}
