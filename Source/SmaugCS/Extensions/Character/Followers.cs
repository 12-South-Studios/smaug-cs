using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Managers;
using SmaugCS.Repository;

namespace SmaugCS.Extensions.Character
{
    public static class Followers
    {
        public static void AddFollower(this CharacterInstance ch, CharacterInstance master)
        {
            if (ch.Master != null) return;

            ch.Master = master;
            ch.Leader = null;

            if (ch.IsNpc() && ch.Act.IsSet(ActFlags.Pet) && !master.IsNpc())
                ((PlayerInstance)master).PlayerData.Pet = ch;

            if (master.CanSee(ch))
                comm.act(ATTypes.AT_ACTION, "$n now follows you.", ch, null, master, ToTypes.Victim);

            comm.act(ATTypes.AT_ACTION, "You now follow $N.", ch, null, master, ToTypes.Character);
        }

        public static void StopFollower(this CharacterInstance ch)
        {
            if (ch.Master == null) return;

            if (ch.IsNpc() && !ch.Master.IsNpc()
                && ((PlayerInstance)ch.Master).PlayerData.Pet == ch)
                ((PlayerInstance)ch.Master).PlayerData.Pet = null;

            if (ch.IsAffected(AffectedByTypes.Charm))
            {
                ch.AffectedBy.RemoveBit(AffectedByTypes.Charm);
                //ch.RemoveAffect(gsn_charm_person);    TODO Fix this!
                if (!ch.Master.IsNpc())
                    ((PlayerInstance)ch.Master).PlayerData.NumberOfCharmies--;
            }

            if (ch.Master.CanSee(ch))
            {
                if (!(!ch.Master.IsNpc() && ch.IsImmortal()
                      && !ch.Master.IsImmortal()))
                    comm.act(ATTypes.AT_ACTION, "$n stops following you.", ch, null, ch.Master, ToTypes.Victim);
            }

            comm.act(ATTypes.AT_ACTION, "You stop following $N.", ch, null, ch.Master, ToTypes.Character);

            ch.Master = null;
            ch.Leader = null;
        }

        public static void DieFollower(this CharacterInstance ch)
        {
            if (ch.Master != null)
                ch.StopFollower();

            ch.Leader = null;

            foreach (
                var fch in
                    RepositoryManager.Instance.CHARACTERS.CastAs<Repository<long, CharacterInstance>>().Values)
            {
                if (fch.Master == ch)
                    ch.StopFollower();
                if (fch.Leader == ch)
                    fch.Leader = fch;
            }
        }

        public static bool IsSameGroup(this CharacterInstance ch, CharacterInstance victim)
        {
            return ch.Leader == victim.Leader;
        }
    }
}
