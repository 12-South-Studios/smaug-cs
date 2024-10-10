using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Spells;

public static class NotFound
{
    public static ReturnTypes NullSpell(int sn, int level, CharacterInstance ch, object vo)
    {
            return SpellNotFound(sn, level, ch, vo);
        }

    public static ReturnTypes SpellNotFound(int sn, int level, CharacterInstance ch, object vo)
    {
            ch.SendTo("That's not a spell!");
            return ReturnTypes.None;
        }
}