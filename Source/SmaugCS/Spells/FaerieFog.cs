using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using System.Linq;

namespace SmaugCS.Spells
{
    public static class FaerieFog
    {
        public static ReturnTypes spell_faerie_fog(int sn, int level, CharacterInstance ch, object vo)
        {
            comm.act(ATTypes.AT_MAGIC, "$n conjures a cloud of purple smoke.", ch, null, null, ToTypes.Room);
            comm.act(ATTypes.AT_MAGIC, "You conjure a cloud of purple smoke.", ch, null, null, ToTypes.Character);

            foreach (var person in ch.CurrentRoom.Persons
                .Where(person => person.IsNpc() || !person.Act.IsSet((int)PlayerFlags.WizardInvisibility))
                .Where(person => person != ch && !person.SavingThrows.CheckSaveVsSpellStaff(level, person)))
            {
                // todo finish this magic.c:3127
                // affect_strip(person, gsn_invis)
                // affect_strip(person, gsn_mass_invis)
                // affect_strip(person, gsn_sneak)

                person.AffectedBy.RemoveBit((int)AffectedByTypes.Hide);
                person.AffectedBy.RemoveBit((int)AffectedByTypes.Invisible);
                person.AffectedBy.RemoveBit((int)AffectedByTypes.Sneak);

                comm.act(ATTypes.AT_MAGIC, "$n is revealed!", person, null, null, ToTypes.Room);
                comm.act(ATTypes.AT_MAGIC, "You are revealed!", person, null, null, ToTypes.Character);
            }

            return ReturnTypes.None;
        }
    }
}
