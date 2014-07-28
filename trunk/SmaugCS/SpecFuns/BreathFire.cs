using SmaugCS.Data;

namespace SmaugCS.SpecFuns
{
    class BreathFire
    {
        public static bool DoSpecBreathFire(CharacterInstance ch)
        {
            return Dragon.DoSpecDragon(ch, "fire breath");
        }
    }
}
