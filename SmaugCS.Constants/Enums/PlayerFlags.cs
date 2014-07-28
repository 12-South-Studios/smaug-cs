using System;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum PlayerFlags
    {
        IsNpc = 1 << 0,
        BoughtPet = 1 << 1,
        ShoveDrag = 1 << 2,
        AutoExit = 1 << 3,
        AutoLoot = 1 << 4,
        AutoSacrifice = 1 << 5,
        Blank = 1 << 6,
        Outcast = 1 << 7,
        Brief = 1 << 8,
        Combine = 1 << 9,
        Prompt = 1 << 10,
        TelnetGA = 1 << 11,
        HolyLight = 1 << 12,
        WizardInvisibility = 1 << 13,
        RoomVNum = 1 << 14,
        Silence = 1 << 15,
        NoEmote = 1 << 16,

        [Descriptor(new[] { "(ATTACKER) " })]
        Attacker = 1 << 17,
        NoTell = 1 << 18,
        Log = 1 << 19,
        Deny = 1 << 20,
        Freeze = 1 << 21,

        [Descriptor(new[] { "(THIEF) " })]
        Thief = 1 << 22,

        [Descriptor(new[] { "(KILLER) " })]
        Killer = 1 << 23,

        [Descriptor(new[] { "(LITTERBUG) " })]
        Litterbug = 1 << 24,
        Ansi = 1 << 25,
        Rip = 1 << 26,
        Nice = 1 << 27,
        Flee = 1 << 28,
        AutoGold = 1 << 29,
        AutoMap = 1 << 30,

        [Descriptor(new []{"[AFK]"})]
        AwayFromKeyboard = 1 << 31,
        InvisibilePrompt = 1 << 32,
        Compass = 1 << 33
    }
}
