namespace SmaugCS.Constants.Enums
{
    public enum TrapTypes
    {
        None = 0,

        [Descriptor("surrounded by a green cloud of gas", "poison")]
        PoisonGas = 1, 

        [Descriptor("hit by a dart", "poison")]
        PoisonDart,

        [Descriptor("pricked by a needle", "poison")]
        PoisonNeedle,

        [Descriptor("stabbed by a dagger", "poison")]
        PoisonDagger, 

        [Descriptor("struck with an arrow", "poison")]
        PoisonArrow, 

        [Descriptor("surrounded by a red cloud of gas", "blindness")]
        BlindnessGas,

        [Descriptor("surrounded by a yellow cloud of gass", "sleep")]
        SleepingGas,

        [Descriptor("struck by a burst of flame", "fireball")]
        Flame,

        [Descriptor("hit by an explosion", "fireball")]
        Explosion,

        [Descriptor("covered by a spray of acid", "acid blast")]
        AcidSpray, 

        [Descriptor("suddenly shocked", "")]
        ElectricShock,

        [Descriptor("sliced by a razor sharp blade", "")]
        Blade,

        [Descriptor("surrounded by a mysterious aura", "change sex")]
        SexChange
    }
}
