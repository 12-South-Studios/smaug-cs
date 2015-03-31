using Realm.Library.Common;

namespace SmaugCS.Weather
{
    public enum WindSpeedTypes
    {
        [Range(Maximum = 10)]
        Calm,

        [Range(Minimum = 11, Maximum = 20)]
        Breezy,

        [Range(Minimum = 21, Maximum = 40)]
        Blustery,

        [Range(Minimum = 41, Maximum = 60)]
        Windy,

        [Range(Minimum = 61, Maximum = 80)]
        Gusty,

        [Range(Minimum = 81)]
        GaleForce
    }
}
