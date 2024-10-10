using Library.Common.Attributes;

namespace SmaugCS.Constants.Enums;

public enum ItemTypes
{
    None,

    [Auctionable]
    Light,

    [Auctionable]
    [CharacterColor(ATType = ATTypes.AT_MAGIC)]
    Scroll,

    [Auctionable]
    [CharacterColor(ATType = ATTypes.AT_MAGIC)]
    [Valued]
    Wand,

    [Auctionable]
    [CharacterColor(ATType = ATTypes.AT_MAGIC)]
    [Valued]
    Staff,

    [Auctionable]
    [Valued]
    Weapon,

    [Name("_fireweapon")]
    FireWeapon,

    [Name("_missile")]
    Missile,

    [Auctionable]
    [CharacterColor(ATType = ATTypes.AT_YELLOW)]
    Treasure,

    [Auctionable]
    [Valued]
    Armor,

    [Auctionable]
    Potion,

    [Name("_worn")]
    Worn,
    Furniture,
    Trash,

    [Name("_oldtrap")]
    OldTrap,

    [Auctionable]
    Container,

    [Name("_note")]
    Note,

    [Auctionable]
    [Name("drinkcon")]
    [CharacterColor(ATType = ATTypes.AT_THIRSTY)]
    DrinkContainer,
    Key,

    [Auctionable]
    [CharacterColor(ATType = ATTypes.AT_HUNGRY)]
    Food,

    [CharacterColor(ATType = ATTypes.AT_YELLOW)]
    Money,

    [Auctionable]
    Pen,

    [Auctionable]
    Boat,

    [Name("corpse")]
    NpcCorpse,

    [Name("corpse_pc")]
    PlayerCorpse,

    [CharacterColor(ATType = ATTypes.AT_THIRSTY)]
    Fountain,

    [Auctionable]
    Pill,

    [CharacterColor(ATType = ATTypes.AT_BLOOD)]
    Blood,
    BloodStain,
    Scraps,

    [Auctionable]
    Pipe,

    [Auctionable]
    [Name("herbcon")]
    HerbContainer,
    Herb,

    [Auctionable]
    Incense,

    [Auctionable]
    [CharacterColor(ATType = ATTypes.AT_FIRE)]
    Fire,

    [Auctionable]
    Book,
    Switch,
    Lever,
    PullChain,
    Button,
    Dial,

    [Auctionable]
    Rune,

    [Auctionable]
    RunePouch,

    [Auctionable]
    Match,
    Trap,

    [Auctionable]
    Map,
    Portal,
    Paper,
    Tinder,
    LockPick,
    Spike,
    Disease,
    Oil,
    Fuel,

    [Name("_empty1")]
    Empty1, // Open for Use

    [Name("_empty2")]
    Empty2, // Open for Use

    [Auctionable]
    MissileWeapon,
    Projectile,

    [Auctionable]
    Quiver,
    Shovel,
    Salve,

    [Auctionable]
    [CharacterColor(ATType = ATTypes.AT_HUNGRY)]
    Cook,

    [Auctionable]
    KeyRing,
    Odor,
    Chance,

    [Name("mix")]
    DrinkMixture
}