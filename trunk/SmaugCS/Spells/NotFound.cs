using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;

namespace SmaugCS.Spells
{
    public static class NotFound
    {
        public static ReturnTypes NullSpell(int sn, int level, CharacterInstance ch, object vo)
        {
            return SpellNotFound(sn, level, ch, vo);
        }

        public static ReturnTypes SpellNotFound(int sn, int level, CharacterInstance ch, object vo)
        {
            color.send_to_char("That's not a spell!", ch);
            return ReturnTypes.None;
        }
    }
}
