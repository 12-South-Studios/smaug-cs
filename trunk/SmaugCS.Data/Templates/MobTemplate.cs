using System.Collections.Generic;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Shops;

// ReSharper disable once CheckNamespace
namespace SmaugCS.Data
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
        public string ActFlags { get; set; }
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
        public string DefensivePosition { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public string Race { get; set; }
        public string Class { get; set; }
        public SavingThrowData SavingThrows { get; set; }
        public Dictionary<StatisticTypes, int> Statistics { get; set; }
        public string PlayerName { get; set; }

        public MobTemplate(long id, string name)
            : base(id, name)
        {
            SavingThrows = new SavingThrowData();
            HitDice = new DiceData();
            DamageDice = new DiceData();
            Statistics = new Dictionary<StatisticTypes, int>();

            ShortDescription = string.Format("A newly created {0}", name);
            LongDescription = string.Format("Somebody abandoned a newly created {0} here.", name);
            Level = 1;
            Position = "standing";
            DefensivePosition = "standing";
            Class = "warrior";
            Race = "human";
            Gender = "male";
        }

        public int GetStatistic(StatisticTypes type)
        {
            return Statistics.ContainsKey(type) ? Statistics[type] : 0;
        }

        public void SetStats1(int align, int level, int thac0, int ac, int gold, int xp)
        {
            Statistics[StatisticTypes.Alignment] = align;
            Level = level;
            Statistics[StatisticTypes.ToHitArmorClass0] = thac0;
            Statistics[StatisticTypes.ArmorClass] = ac;
            Gold = gold;
            Experience = xp;
        }

        public void SetStats2(int numberHitDice, int sizeHitDice, int bonusHitDice)
        {
            HitDice = new DiceData
            {
                NumberOf = numberHitDice, 
                SizeOf = sizeHitDice,
                Bonus = bonusHitDice
            };
        }

        public void SetStats3(int numberDmgDice, int sizeDmgDice, int bonusDmgDice)
        {
            DamageDice = new DiceData
            {
                NumberOf = numberDmgDice, 
                SizeOf = sizeDmgDice,
                Bonus = bonusDmgDice
            };
        }

        public void SetStats4(int height, int weight, int numberAttacks, int hitRoll, int dmgRoll)
        {
            Height = height;
            Weight = weight;
            NumberOfAttacks = numberAttacks;
            Statistics[StatisticTypes.HitRoll] = hitRoll;
            Statistics[StatisticTypes.DamageRoll] = dmgRoll;
        }

        public void SetAttributes(int strength, int intelligence, int wisdom, int dexterity, int constitution, int charisma,
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

        public void AddConversation(string keyword, string text)
        {
            if (string.IsNullOrEmpty(keyword) || string.IsNullOrEmpty(text))
                return;

            MudProgData mp = new MudProgData
            {
                Type = MudProgTypes.Speech,
                ArgList = keyword,
                Script = string.Format("LMobEmote(\"{0}\");", text)
            };

            AddMudProg(mp);
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
            proxy.Write("DefPos     {0}~\n", BuilderConstants.npc_position[(int)DefensivePosition]);
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
