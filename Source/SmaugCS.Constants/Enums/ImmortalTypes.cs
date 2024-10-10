using Library.Common.Attributes;

namespace SmaugCS.Constants.Enums;

public enum ImmortalTypes
{
    [Value(-15)]
    Hero,

    [Value(-14)]
    [ImmortalHelpCategory(Value = "M_GODLVL1_")]
    Immortal,

    [Value(0)]
    [ImmortalHelpCategory(Value = "M_GODLVL15_")]
    Supreme,

    [Value(-1)]
    [ImmortalHelpCategory(Value = "M_GODLVL14_")]
    Infinite,

    [Value(-2)]
    [ImmortalHelpCategory(Value = "M_GODLVL13_")]
    Eternal,

    [Value(-3)]
    [ImmortalHelpCategory(Value = "M_GODLVL12_")]
    Implementor,

    [Value(-4)]
    [ImmortalHelpCategory(Value = "M_GODLVL11_")]
    SubImplementor,

    [Value(-5)]
    [ImmortalHelpCategory(Value = "M_GODLVL10_")]
    Ascendant,

    [Value(-6)]
    [ImmortalHelpCategory(Value = "M_GODLVL9_")]
    Greater,

    [Value(-7)]
    [ImmortalHelpCategory(Value = "M_GODLVL8_")]
    God,

    [Value(-8)]
    [ImmortalHelpCategory(Value = "M_GODLVL7_")]
    Lesser,

    [Value(-9)]
    [ImmortalHelpCategory(Value = "M_GODLVL6_")]
    TrueImmortal,

    [Value(-10)]
    [ImmortalHelpCategory(Value = "M_GODLVL5_")]
    Demi,

    [Value(-11)]
    [ImmortalHelpCategory(Value = "M_GODLVL4_")]
    Savior,

    [Value(-12)]
    [ImmortalHelpCategory(Value = "M_GODLVL3_")]
    Creator,

    [Value(-13)]
    [ImmortalHelpCategory(Value = "M_GODLVL2_")]
    Acolyte,

    [Value(-14)]
    Neophyte,

    [Value(-15)]
    Avatar
}