using System;
using System.Collections.Generic;
using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Managers;
using SmaugCS.Repository;
using SmaugCS.Spells;
using SmaugCS.Spells.Smaug;

namespace SmaugCS.SpecFuns
{
    public static class CastAdept
    {
        private static readonly Dictionary<int, Tuple<string, string>> ActLookupTable = new Dictionary
            <int, Tuple<string, string>>
            {
                {0, new Tuple<string, string>("ciroht", "armor")},
                {1, new Tuple<string, string>("sunimod", "bless")},
                {2, new Tuple<string, string>("suah", "cure blindness")},
                {3, new Tuple<string, string>("nran", "cure light")},
                {4, new Tuple<string, string>("nyrcs", "cure poison")},
                {5, new Tuple<string, string>("gartla", "refresh")},
                {6, new Tuple<string, string>("naimad", "cure serious")},
                {7, new Tuple<string, string>("gorog", "remove curse")}
            };

        public static bool DoSpecCastAdept(MobileInstance ch)
        {
            if (!ch.IsAwake() || ch.CurrentFighting != null) return false;

            var victim = ch.CurrentRoom.Persons.Where(vch => vch != ch)
                  .Where(ch.CanSee)
                  .FirstOrDefault(vch => SmaugRandom.Bits(1) != 0);
            if (victim == null) return false;

            var bits = SmaugRandom.Bits(3);
            var actLookupValue = ActLookupTable[bits];

            comm.act(ATTypes.AT_MAGIC, 
                string.Format("$n utters the word '{0}'.", actLookupValue.Item1), 
                ch, null, null, ToTypes.Room);
            return CastSpell(ch, victim, bits, actLookupValue.Item2);
        }

        private static bool CastSpell(MobileInstance ch, CharacterInstance victim, int bitFlag, string spellName)
        {
            var skill = RepositoryManager.Instance.GetEntity<SkillData>(spellName);
            
            switch (bitFlag)
            {
                case 0:
                case 1:
                case 3:
                case 5:
                case 6:
                    Smaug.spell_smaug((int)skill.ID, ch.Level, ch, victim);
                    return true;
                case 2:
                    CureBlindness.spell_cure_blindness((int)skill.ID, ch.Level, ch, victim);
                    return true;
                case 4:
                    CurePoison.spell_cure_poison((int)skill.ID, ch.Level, ch, victim);
                    return true;
                case 7:
                    RemoveCurse.spell_remove_curse((int)skill.ID, ch.Level, ch, victim);
                    return true;
            }

            return false;
        }
    }
}
