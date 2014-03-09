using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Spells
{
    class NotFound
    {
        public static int spell_null(int sn, int level, CharacterInstance ch, object vo)
        {
            return spell_notfound(sn, level, ch, vo);
        }

        public static int spell_notfound(int sn, int level, CharacterInstance ch, object vo)
        {
            color.send_to_char("That's not a spell!\r\n", ch);
            return (int)ReturnTypes.None;
        }
    }
}
