﻿using System;
using System.Collections.Generic;
using System.Linq;
using SmaugCS.Common;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Managers;

namespace SmaugCS.SpecFuns
{
    public static class CastMage
    {
        public static bool DoSpecCastMage(CharacterInstance ch)
        {
            SpecFunHandler handler = new SpecFunHandler(DatabaseManager.Instance);
            handler.summon_if_hating(ch);

            if (!ch.IsInCombatPosition())
                return false;

            CharacterInstance victim =
                ch.CurrentRoom.Persons.Where(v => !v.Equals(ch))
                  .FirstOrDefault(vch => SmaugRandom.Bits(2) == 0 && fight.who_fighting(vch).Equals(ch));

            if (victim == null || victim.Equals(ch))
                return false;

            SkillData skill = handler.PickSpell(SpellLevelLookupTable, ch.Level);
            if (skill == null || skill.SpellFunction == null)
                return false;

            skill.SpellFunction.Value.DynamicInvoke(new object[] { skill.ID, ch.Level, ch, victim });
            return true;
        }

        private static readonly Dictionary<int, Tuple<int, string>> SpellLevelLookupTable = new Dictionary<int, Tuple<int, string>>()
            {
                {0, new Tuple<int, string>(0, "magic missile")},
                {1, new Tuple<int, string>(3, "chill touch")},
                {2, new Tuple<int, string>(7, "weaken")},
                {3, new Tuple<int, string>(8, "galvanic whip")},
                {4, new Tuple<int, string>(11, "colour spray")},
                {5, new Tuple<int, string>(12, "weaken")},
                {6, new Tuple<int, string>(13, "energy drain")},
                {7, new Tuple<int, string>(14, "spectral furor")},
                {8, new Tuple<int, string>(15, "fireball")},
                {9, new Tuple<int, string>(15, "fireball")},
                {-1, new Tuple<int, string>(20, "acid blast")}
            };
    }
}