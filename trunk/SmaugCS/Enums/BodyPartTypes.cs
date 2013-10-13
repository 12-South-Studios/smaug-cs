using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmaugCS.Enums
{
    [Flags]
    public enum BodyPartTypes
    {
        Head = 1 << 0,
        Arms = 1 << 1,
        Legs = 1 << 2,
        Heart = 1 << 3,
        Brains = 1 << 4,
        Guts = 1 << 5,
        Hands = 1 << 6,
        Feet = 1 << 7,
        Fingers = 1 << 8,
        Ear = 1 << 9,
        Eye = 1 << 10,
        LongTongue = 1 << 11,
        EyeStalks = 1 << 12,
        Tentacles = 1 << 13,
        Fins = 1 << 14,
        Wings = 1 << 15,
        Tail = 1 << 16,
        Scales = 1 << 17,
        Claws = 1 << 18,
        Fangs = 1 << 19,
        Horns = 1 << 20,
        Tusks = 1 << 21,
        TailAttack = 1 << 22,
        SharpScales = 1 << 23,
        Beak = 1 << 24,
        Haunch = 1 << 25,
        Hooves = 1 << 26,
        Paws = 1 << 27,
        ForeLegs = 1 << 28,
        Feathers = 1 << 29
    }
}
