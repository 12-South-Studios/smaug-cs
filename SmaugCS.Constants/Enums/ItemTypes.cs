
using Realm.Library.Common;

namespace SmaugCS.Constants.Enums
{
    public enum ItemTypes
    {
        None,
        Light,
        Scroll,
        Wand,
        Staff,
        Weapon,

        [Name("_fireweapon")]
        FireWeapon,

        [Name("_missile")]
        Missile,
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
        DrinkContainer,
        Key,
        Food,
        Money,
        Pen,
        Boat,

        [Name("corpse")]
        NpcCorpse,

        [Name("corpse_pc")]
        PlayerCorpse,
        Fountain,
        Pill,
        Blood,
        BloodStain,
        Scraps,
        Pipe,

        [Name("herbcon")]
        HerbContainer,
        Herb,
        Incense,
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
        Cook,
        KeyRing,
        Odor,
        Chance,

        [Name("mix")]
        DrinkMixture
    }
}
