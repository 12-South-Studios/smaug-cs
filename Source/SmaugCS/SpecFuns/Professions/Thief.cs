using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;

namespace SmaugCS.SpecFuns.Professions
{
    public static class Thief
    {
        public static bool Execute(MobileInstance ch, IManager dbManager)
        {
            if (ch.CurrentPosition != PositionTypes.Standing) return false;
            if (SmaugRandom.Bits(2) != 0) return false;

            foreach (var victim in ch.CurrentRoom.Persons.Where(victim => victim != ch)
                      .Where(victim => !victim.IsNpc())
                      .Where(victim => victim.Level < LevelConstants.ImmortalLevel)
                      .Where(ch.CanSee))
            {
                if (victim.IsAwake() && SmaugRandom.Between(0, ch.Level) == 0)
                {
                    comm.act(ATTypes.AT_ACTION, "You discover $n's hands in your sack of gold!", ch, null, victim,
                             ToTypes.Victim);
                    comm.act(ATTypes.AT_ACTION, "$N discovers $n's hands in $S sack of gold!", ch, null, victim,
                             ToTypes.NotVictim);
                    return true;
                }

                var maxgold = ch.Level*ch.Level*1000;
                var gold = victim.CurrentCoin*
                           SmaugRandom.Between(1, 2.GetNumberThatIsBetween(ch.Level/4, 10))/100;

                ch.CurrentCoin += 9*gold/10;
                victim.CurrentCoin -= gold;
                if (ch.CurrentCoin <= maxgold) return true;

                ch.CurrentRoom.Area.BoostEconomy(ch.CurrentCoin - maxgold/2);
                ch.CurrentCoin = maxgold/2;
                return true;
            }
            return false;
        }
    }
}
