using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Managers;

namespace SmaugCS.SpecFuns
{
    public static class Poison
    {
        public static bool DoSpecPoison(CharacterInstance ch)
        {
            if (!ch.IsInCombatPosition())
                return false;

            CharacterInstance victim = fight.who_fighting(ch);
            if (victim == null || (SmaugRandom.Percent() > (2*ch.Level)))
                return false;

            comm.act(ATTypes.AT_HIT, "You bite $N!", ch, null, victim, ToTypes.Character);
            comm.act(ATTypes.AT_ACTION, "$n bites $N!", ch, null, victim, ToTypes.NotVictim);
            comm.act(ATTypes.AT_POISON, "$n bites you!", ch, null, victim, ToTypes.Victim);
            Spells.Poison.spell_poison(DatabaseManager.Instance.LookupSkill("poison"), ch.Level, ch, victim);

            return true;
        }
    }
}
