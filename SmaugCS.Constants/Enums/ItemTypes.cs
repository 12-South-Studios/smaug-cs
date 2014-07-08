
using Realm.Library.Common;

namespace SmaugCS.Constants.Enums
{
    public enum ItemTypes
    {
        None,
        Light,

        [CharacterColor(ATTypes.AT_MAGIC)]
        Scroll,

        [CharacterColor(ATTypes.AT_MAGIC)]
        Wand,

        [CharacterColor(ATTypes.AT_MAGIC)]
        Staff,
        Weapon,

        [Name("_fireweapon")]
        FireWeapon,

        [Name("_missile")]
        Missile,

        [CharacterColor(ATTypes.AT_YELLOW)]
        Treasure,
        Armor,
        Potion,

        [Name("_worn")]
        Worn,
        Furniture,
        Trash,

        [Name("_oldtrap")]
        OldTrap,
        Container,

        [Name("_note")]
        Note,

        [Name("drinkcon")]
        [CharacterColor(ATTypes.AT_THIRSTY)]
        DrinkContainer,
        Key,

        [CharacterColor(ATTypes.AT_HUNGRY)]
        Food,

        [CharacterColor(ATTypes.AT_YELLOW)]
        Money,
        Pen,
        Boat,

        [Name("corpse")]
        NpcCorpse,

        [Name("corpse_pc")]
        PlayerCorpse,

        [CharacterColor(ATTypes.AT_THIRSTY)]
        Fountain,
        Pill,

        [CharacterColor(ATTypes.AT_BLOOD)]
        Blood,
        BloodStain,
        Scraps,
        Pipe,

        [Name("herbcon")]
        HerbContainer,
        Herb,
        Incense,

        [CharacterColor(ATTypes.AT_FIRE)]
        Fire,
        Book,
        Switch,
        Lever,
        PullChain,
        Button,
        Dial,
        Rune,
        RunePouch,
        Match,
        Trap,
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
        MissileWeapon,
        Projectile,
        Quiver,
        Shovel,
        Salve,

        [CharacterColor(ATTypes.AT_HUNGRY)]
        Cook,
        KeyRing,
        Odor,
        Chance,

        [Name("mix")]
        DrinkMixture
    }
}
