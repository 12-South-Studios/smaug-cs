using SmaugCS.Common;
using SmaugCS.Enums;

// ReSharper disable CheckNamespace
namespace SmaugCS.Objects
// ReSharper restore CheckNamespace
{
    public class MobTemplate : Template
    {
        public SpecialFunction SpecialFunction { get; set; }
        public ShopData Shop { get; set; }
        public RepairShopData RepairShop { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string spec_funname { get; set; }
        public int Count { get; set; }
        public int killed { get; set; }
        public int Gender { get; set; }
        public int Level { get; set; }
        public ExtendedBitvector Act { get; set; }
        public ExtendedBitvector AffectedBy { get; set; }
        public int Alignment { get; set; }
        public int ToHitArmorClass0 { get; set; }
        public int ArmorClass { get; set; }
        public DiceData HitDice { get; set; }
        public DiceData DamageDice { get; set; }
        public int NumberOfAttacks { get; set; }
        public int Gold { get; set; }
        public int Experience { get; set; }
        public int ExtraFlags { get; set; }
        public int Immunity { get; set; }
        public int Susceptibility { get; set; }
        public int Resistance { get; set; }
        public ExtendedBitvector Attacks { get; set; }
        public ExtendedBitvector Defenses { get; set; }
        public int Speaks { get; set; }
        public int Speaking { get; set; }
        public PositionTypes Position { get; set; }
        public PositionTypes DefPosition { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public int Race { get; set; }
        public int Class { get; set; }
        public int PermanentStrength { get; set; }
        public int PermanentIntelligence { get; set; }
        public int PermanentWisdom { get; set; }
        public int PermanentDexterity { get; set; }
        public int PermanentConstitution { get; set; }
        public int PermanentCharisma { get; set; }
        public int PermanentLuck { get; set; }
        public SavingThrowData SavingThrows { get; set; }

        public MobTemplate()
        {
            SavingThrows = new SavingThrowData();
            Act = new ExtendedBitvector();
            AffectedBy = new ExtendedBitvector();
            Attacks = new ExtendedBitvector();
            Defenses = new ExtendedBitvector();
            HitDice = new DiceData();
            DamageDice = new DiceData();
        }
    }
}
