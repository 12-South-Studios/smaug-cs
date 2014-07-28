using SmaugCS.Data;

namespace SmaugCS.SpecFuns
{
    class BreathLightning
    {
        public static bool DoSpecBreathLightning(CharacterInstance ch)
        {
            return Dragon.DoSpecDragon(ch, "lightning breath");
        }
    }
}
