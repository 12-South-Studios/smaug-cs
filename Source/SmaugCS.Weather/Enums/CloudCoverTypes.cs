using Realm.Library.Common;

namespace SmaugCS.Weather.Enums
{
    public enum CloudCoverTypes
    {
        [Range(Minimum = 81)]
        Extremely,

        [Range(Minimum = 61, Maximum = 80)]
        Moderately,

        [Range(Minimum = 41, Maximum = 60)]
        Partly,

        [Range(Minimum = 21, Maximum = 40)]
        Slightly,

        [Range(Maximum = 20)]
        None
    }
}
