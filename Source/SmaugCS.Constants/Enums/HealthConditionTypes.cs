using Realm.Library.Common;

namespace SmaugCS.Constants.Enums
{
    public enum HealthConditionTypes
    {
        [Value(100)]
        [Descriptor("{0} is in perfect health.", "{0} are in perfect health.")]
        PerfectHealth,

        [Value(90)]
        [Descriptor("{0} is slightly scratched.", "{0} are slightly scratched.")]
        SlightlyScratched,

        [Value(80)]
        [Descriptor("{0} has a few bruises.", "{0} have a few bruises.")]
        FewBruises,

        [Value(70)]
        [Descriptor("{0} has some cuts.", "{0} have some cuts.")]
        SomeCuts,

        [Value(60)]
        [Descriptor("{0} has several wounds.", "{0} have several wounds.")]
        SeveralWounds,

        [Value(50)]
        [Descriptor("{0} has many nasty wounds.", "{0} have many nasty wounds.")]
        ManyNastyWounds,

        [Value(40)]
        [Descriptor("{0} is bleeding freely.", "{0} are bleeding freely.")]
        BleedingFreedly,

        [Value(30)]
        [Descriptor("{0} is covered in blood.", "{0} are covered in blood.")]
        CoveredInBlood,

        [Value(20)]
        [Descriptor("{0} is leaking guts.", "{0} are leaking guts.")]
        LeakingGuts,

        [Value(10)]
        [Descriptor("{0} is almost dead.", "{0} are almost dead.")]
        AlmostDead,

        [Value(0)]
        [Descriptor("{0} is DYING.", "{0} are DYING.")]
        Dying
    }
}
