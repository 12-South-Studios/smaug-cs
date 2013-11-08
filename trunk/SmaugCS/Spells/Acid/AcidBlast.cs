using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Realm.Library.Common;
using SmaugCS.Objects;

namespace SmaugCS.Spells
{
    public class AcidBlast
    {
        public static int spell_acid_blast(int sn, int level, CharacterInstance ch, object vo)
        {
            CharacterInstance victim = vo.CastAs<CharacterInstance>();

                int damage = Common.SmaugRandom.RollDice(level, 6);
            
            if (SavingThrowData.CheckSaveVsSpellStaff(level, victim))
                damage /= 2;

            return fight.damage(ch, victim, damage, sn);
        }
    }
}
