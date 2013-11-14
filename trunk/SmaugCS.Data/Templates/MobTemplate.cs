using System.Collections.Generic;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Shops;

namespace SmaugCS.Data.Templates
{
    public class MobTemplate : Template
    {
        public SpecialFunction SpecialFunction { get; set; }
        public ShopData Shop { get; set; }
        public RepairShopData RepairShop { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string SpecFun { get; set; }
        public int Count { get; set; }
        public int TimesKilled { get; set; }
        public string Gender { get; set; }
        public int Level { get; set; }
        public string Act { get; set; }
        public string AffectedBy { get; set; }
        public string BodyParts { get; set; }
        public DiceData HitDice { get; set; }
        public DiceData DamageDice { get; set; }
        public int NumberOfAttacks { get; set; }
        public int Gold { get; set; }
        public int Experience { get; set; }
        public int ExtraFlags { get; set; }
        public string Immunity { get; set; }
        public string Susceptibility { get; set; }
        public string Resistance { get; set; }
        public string Attacks { get; set; }
        public string Defenses { get; set; }
        public string Speaks { get; set; }
        public string Speaking { get; set; }
        public string Position { get; set; }
        public string DefPosition { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public string Race { get; set; }
        public string Class { get; set; }
        public SavingThrowData SavingThrows { get; set; }
        public Dictionary<StatisticTypes, int> Statistics { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public MobTemplate(long id, string name)
            : base(id, name)
        {
            SavingThrows = new SavingThrowData();
            HitDice = new DiceData();
            DamageDice = new DiceData();
            Statistics = new Dictionary<StatisticTypes, int>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetStatistic(StatisticTypes type)
        {
            return Statistics.ContainsKey(type) ? Statistics[type] : 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="align"></param>
        /// <param name="level"></param>
        /// <param name="thac0"></param>
        /// <param name="ac"></param>
        /// <param name="gold"></param>
        /// <param name="xp"></param>
        public void SetStats1(int align, int level, int thac0, int ac, int gold, int xp)
        {
            Statistics[StatisticTypes.Alignment] = align;
            Level = level;
            Statistics[StatisticTypes.ToHitArmorClass0] = thac0;
            Statistics[StatisticTypes.ArmorClass] = ac;
            Gold = gold;
            Experience = xp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberHitDice"></param>
        /// <param name="sizeHitDice"></param>
        /// <param name="bonusHitDice"></param>
        public void SetStats2(int numberHitDice, int sizeHitDice, int bonusHitDice)
        {
            HitDice = new DiceData { NumberOf = numberHitDice, SizeOf = sizeHitDice, Bonus = bonusHitDice };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberDmgDice"></param>
        /// <param name="sizeDmgDice"></param>
        /// <param name="bonusDmgDice"></param>
        public void SetStats3(int numberDmgDice, int sizeDmgDice, int bonusDmgDice)
        {
            DamageDice = new DiceData { NumberOf = numberDmgDice, SizeOf = sizeDmgDice, Bonus = bonusDmgDice };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="height"></param>
        /// <param name="weight"></param>
        /// <param name="numberAttacks"></param>
        /// <param name="hitRoll"></param>
        /// <param name="dmgRoll"></param>
        public void SetStats4(int height, int weight, int numberAttacks, int hitRoll, int dmgRoll)
        {
            Height = height;
            Weight = weight;
            NumberOfAttacks = numberAttacks;
            Statistics[StatisticTypes.HitRoll] = hitRoll;
            Statistics[StatisticTypes.DamageRoll] = dmgRoll;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strength"></param>
        /// <param name="intelligence"></param>
        /// <param name="wisdom"></param>
        /// <param name="dexterity"></param>
        /// <param name="constitution"></param>
        /// <param name="charisma"></param>
        /// <param name="luck"></param>
        public void SetAttribs(int strength, int intelligence, int wisdom, int dexterity, int constitution, int charisma,
                               int luck)
        {
            Statistics[StatisticTypes.Strength] = strength;
            Statistics[StatisticTypes.Intelligence] = intelligence;
            Statistics[StatisticTypes.Wisdom] = wisdom;
            Statistics[StatisticTypes.Dexterity] = dexterity;
            Statistics[StatisticTypes.Constitution] = constitution;
            Statistics[StatisticTypes.Charisma] = charisma;
            Statistics[StatisticTypes.Luck] = luck;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saveVsDeath"></param>
        /// <param name="saveVsWand"></param>
        /// <param name="saveVsParalysis"></param>
        /// <param name="saveVsBreath"></param>
        /// <param name="saveVsSpell"></param>
        public void SetSaves(int saveVsDeath, int saveVsWand, int saveVsParalysis, int saveVsBreath, int saveVsSpell)
        {
            SavingThrows = new SavingThrowData
                               {
                                   SaveVsPoisonDeath = saveVsDeath,
                                   SaveVsWandRod = saveVsWand,
                                   SaveVsParalysisPetrify = saveVsParalysis,
                                   SaveVsBreath = saveVsBreath,
                                   SaveVsSpellStaff = saveVsSpell
                               };
        }

        public void AddShop(ShopData shop)
        {
            Shop = shop;
        }

        /*public void SaveFUSS(TextWriterProxy proxy, bool install)
        {
            if (install)
                Act.RemoveBit((int)ActFlags.Prototype);

            proxy.Write("#MOBILE\n");
            proxy.Write("Vnum       {0}\n", Vnum);
            proxy.Write("Keywords   {0}~\n", Name);
            proxy.Write("Short      {0}~\n", ShortDescription);
            if (!LongDescription.IsNullOrEmpty())
                proxy.Write("Long       {0}~\n", LongDescription);
            if (!Description.IsNullOrEmpty())
                proxy.Write("Desc       {0}~\n", Description);
            proxy.Write("Race       {0}~\n", db.GetRace(Race).Name);
            proxy.Write("Class      {0}~\n", db.GetClass(Class).Name);
            proxy.Write("Position   {0}~\n", BuilderConstants.npc_position[(int)Position]);
            proxy.Write("DefPos     {0}~\n", BuilderConstants.npc_position[(int)DefPosition]);
            if (SpecialFunction != null && !SpecialFunctionName.IsNullOrEmpty())
                proxy.Write("Specfun    {0}~\n", SpecialFunctionName);
            proxy.Write("Gender     {0}~\n", BuilderConstants.npc_sex[Gender]);
            proxy.Write("Actflags   {0}~\n", Act.GetFlagString(BuilderConstants.act_flags));
            if (!AffectedBy.IsEmpty())
                proxy.Write("Affected   {0}~\n", AffectedBy.GetFlagString(BuilderConstants.a_flags));
            proxy.Write("Stats1     {0} {1} {2} {3} {4} {5}\n", GetStatistic(StatisticTypes.Alignment), Level,
                        GetStatistic(StatisticTypes.ToHitArmorClass0), GetStatistic(StatisticTypes.ArmorClass), Gold,
                        Experience);
            proxy.Write("Stats2     {0} {1} {2}\n", HitDice.NumberOf, HitDice.SizeOf, HitDice.Bonus);
            proxy.Write("Stats3     {0} {1} {2}\n", DamageDice.NumberOf, DamageDice.SizeOf, DamageDice.Bonus);
            proxy.Write("Stats4     {0} {1} {2} {3} {4}\n", Height, Weight, NumberOfAttacks,
                        GetStatistic(StatisticTypes.HitRoll), GetStatistic(StatisticTypes.DamageRoll));
            proxy.Write("Attribs    {0} {1} {2} {3} {4} {5} {6}\n", GetStatistic(StatisticTypes.Strength),
                        GetStatistic(StatisticTypes.Intelligence), GetStatistic(StatisticTypes.Wisdom),
                        GetStatistic(StatisticTypes.Dexterity), GetStatistic(StatisticTypes.Constitution),
                        GetStatistic(StatisticTypes.Charisma), GetStatistic(StatisticTypes.Luck));
            proxy.Write("Saves      {0} {1} {2} {3} {4}\n", SavingThrows.SaveVsPoisonDeath, SavingThrows.SaveVsWandRod,
                        SavingThrows.SaveVsParalysisPetrify, SavingThrows.SaveVsBreath, SavingThrows.SaveVsSpellStaff);

            // TODO Finish Speaks and Speaking

            if (ExtraFlags > 0)
                proxy.Write("Bodyparts  {0}~\n", ExtraFlags.GetFlagString(BuilderConstants.part_flags));
            if (Resistance > 0)
                proxy.Write("Resist     {0}~\n", Resistance.GetFlagString(BuilderConstants.ris_flags));
            if (Immunity > 0)
                proxy.Write("Immune     {0}~\n", Immunity.GetFlagString(BuilderConstants.ris_flags));
            if (Susceptibility > 0)
                proxy.Write("Suscept    {0}~\n", Susceptibility.GetFlagString(BuilderConstants.ris_flags));
            if (!Attacks.IsEmpty())
                proxy.Write("Attacks    {0}~\n", Attacks.GetFlagString(BuilderConstants.attack_flags));
            if (!Defenses.IsEmpty())
                proxy.Write("Defenses   {0}~\n", Defenses.GetFlagString(BuilderConstants.defense_flags));

            if (Shop != null && Shop.GetType() == typeof(ItemShopData))
            {
                ItemShopData itemShop = Shop.CastAs<ItemShopData>();
                proxy.Write("ShopData  {0} {1} {2} {3} {4} {5} {6} {7} {8}\n", (int)itemShop.ItemTypes[0],
                            (int)itemShop.ItemTypes[1], (int)itemShop.ItemTypes[2], (int)itemShop.ItemTypes[3],
                            (int)itemShop.ItemTypes[4], itemShop.ProfitBuy, itemShop.ProfitSell, itemShop.OpenHour,
                            itemShop.CloseHour);
            }

            if (RepairShop != null)
            {
                proxy.Write("RepairData {0} {1} {2} {3} {4} {5} {6}\n", RepairShop.ItemTypes[0], RepairShop.ItemTypes[1],
                            RepairShop.ItemTypes[2], RepairShop.ProfitFix, RepairShop.ShopType, RepairShop.OpenHour,
                            RepairShop.CloseHour);
            }

            foreach (MudProgData mp in MudProgs)
                mp.Save(proxy);

            proxy.Write("#ENDMOBILE\n\n");
        }*/
    }
}
