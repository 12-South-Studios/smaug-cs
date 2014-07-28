using SmaugCS.Data;

namespace SmaugCS.SpecFuns
{
    class BreathFrost
    {
        public static bool DoSpecBreathFrost(CharacterInstance ch)
        {
            return Dragon.DoSpecDragon(ch, "frost breath");
        }
    }
}
