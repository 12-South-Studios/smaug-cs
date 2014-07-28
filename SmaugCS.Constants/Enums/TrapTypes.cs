namespace SmaugCS.Constants.Enums
{
    public enum TrapTypes
    {
        None = -1,

        [Descriptor(new []{ "surrounded by a green cloud of gas", "poison"})]
        PoisonGas = 1, 

        [Descriptor(new []{"hit by a dart", "poison"})]
        PoisonDart,

        [Descriptor(new []{"pricked by a needle", "poison"})]
        PoisonNeedle,

        [Descriptor(new []{"stabbed by a dagger", "poison"})]
        PoisonDagger, 

        [Descriptor(new []{"struck with an arrow", "poison"})]
        PoisonArrow, 

        [Descriptor(new []{"surrounded by a red cloud of gas", "blindness"})]
        BlindnessGas,

        [Descriptor(new []{"surrounded by a yellow cloud of gass", "sleep"})]
        SleepingGas,

        [Descriptor(new []{"struck by a burst of flame", "fireball"})]
        Flame,

        [Descriptor(new []{"hit by an explosion", "fireball"})]
        Explosion,

        [Descriptor(new []{"covered by a spray of acid", "acid blast"})]
        AcidSpray, 

        [Descriptor(new []{"suddenly shocked", ""})]
        ElectricShock,

        [Descriptor(new []{"sliced by a razor sharp blade", ""})]
        Blade,

        [Descriptor(new []{"surrounded by a mysterious aura", "change sex"})]
        SexChange
    }
}
