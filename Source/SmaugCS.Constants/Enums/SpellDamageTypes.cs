using System;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum SpellDamageTypes
    {
        None = 1 << 0,

        [DamageResistance(ResistanceType = ResistanceTypes.Fire)]
        Fire = 1 << 1,

        [DamageResistance(ResistanceType = ResistanceTypes.Cold)]
        Cold = 1 << 2,

        [DamageResistance(ResistanceType = ResistanceTypes.Electricity)]
        Electricty = 1 << 3,

        [DamageResistance(ResistanceType = ResistanceTypes.Energy)]
        Energy = 1 << 4,

        [DamageResistance(ResistanceType = ResistanceTypes.Acid)]
        Acid = 1 << 5,

        [DamageResistance(ResistanceType = ResistanceTypes.Poison)]
        Poison = 1 << 6,

        [DamageResistance(ResistanceType = ResistanceTypes.Drain)]
        Drain = 1 << 7
    }
}
