using Realm.Library.Common;

namespace SmaugCS.Weather
{
    public enum HumidityTypes
    {
        [Range(Minimum = 81)]
        Extremely,

        [Range(Minimum = 61, Maximum = 80)]
        Moderately,

        [Range(Minimum = 41, Maximum = 60)]
        Minorly,

        [Range(Minimum = 21, Maximum = 40)]
        Humid,

        [Range(Maximum = 20)]
        None
    }
}
