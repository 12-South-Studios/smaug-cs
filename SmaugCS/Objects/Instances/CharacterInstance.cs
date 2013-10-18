using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using SmaugCS.Commands;
using SmaugCS.Commands.Movement;
using SmaugCS.Common;
using SmaugCS.Enums;
using SmaugCS.Language;
using SmaugCS.Managers;
using SmaugCS.Organizations;

// ReSharper disable CheckNamespace
namespace SmaugCS.Objects
// ReSharper restore CheckNamespace
{
    [XmlRoot("Character")]
    public class CharacterInstance : Instance
    {
        public CharacterInstance Master { get; set; }
        public CharacterInstance Leader { get; set; }
        public FightingData CurrentFighting { get; set; }
        public CharacterInstance ReplyTo { get; set; }
        public CharacterInstance RetellTo { get; set; }
        public CharacterInstance Switched { get; set; }
        public CharacterInstance CurrentMount { get; set; }
        public HuntHateFearData CurrentHunting { get; set; }
        public HuntHateFearData CurrentFearing { get; set; }
        public HuntHateFearData CurrentHating { get; set; }
        public List<VariableData> Variables { get; set; }
        public SpecialFunction SpecialFunction { get; set; }
        public string SpecialFunctionName { get; set; }
        public MudProgActData mpact { get; set; }
        public int mpactnum { get; set; }
        public uint mpscriptpos { get; set; }
        public DescriptorData Descriptor { get; set; }
        public List<NoteData> NoteList { get; set; }
        public List<NoteData> Comments { get; set; }
        public List<ObjectInstance> Carrying { get; set; }
        public RoomTemplate CurrentRoom { get; set; }
        public RoomTemplate PreviousRoom { get; set; }
        public PlayerData PlayerData { get; set; }
        public DoFunction LastCommand { get; set; }
        public DoFunction PreviousCommand { get; set; }
        public object DestinationBuffer { get; set; }
        public string alloc_ptr { get; set; }
        public object spare_ptr { get; set; }
        public int tempnum { get; set; }
        public EditorData CurrentEditor { get; set; }
        public List<TimerData> Timers { get; set; }
        public CharacterMorph CurrentMorph { get; set; }
        public string LongDescription { get; set; }
        public int num_fighting { get; set; }
        public CharacterSubStates SubState { get; set; }
        public GenderTypes Gender { get; set; }
        public ClassTypes CurrentClass { get; set; }
        public RaceTypes CurrentRace { get; set; }
        public int Level { get; set; }
        public int Trust { get; set; }
        public int played { get; set; }
        public DateTime logon { get; set; }
        public DateTime save_time { get; set; }
        public int wait { get; set; }
        public int CurrentHealth { get; set; }
        public int MaximumHealth { get; set; }
        public int CurrentMana { get; set; }
        public int MaximumMana { get; set; }
        public int CurrentMovement { get; set; }
        public int MaximumMovement { get; set; }
        public int Practice { get; set; }
        public int NumberOfAttacks { get; set; }
        public int CurrentCoin { get; set; }
        public int Experience { get; set; }
        public ExtendedBitvector Act { get; set; }
        public ExtendedBitvector AffectedBy { get; set; }
        public ExtendedBitvector NoAffectedBy { get; set; }
        public int CarryWeight { get; set; }
        public int CarryNumber { get; set; }
        public int ExtraFlags { get; set; }
        public int NoImmunity { get; set; }
        public int NoResistance { get; set; }
        public int NoSusceptibility { get; set; }
        public int Immunity { get; set; }
        public int Resistance { get; set; }
        public int Susceptibility { get; set; }
        public ExtendedBitvector Attacks { get; set; }
        public ExtendedBitvector Defenses { get; set; }
        public int Speaks { get; set; }
        public int Speaking { get; set; }
        public SavingThrowData SavingThrows { get; set; }
        public int CurrentAlignment { get; set; }
        public DiceData BareDice { get; set; }
        public int ToHitArmorClass0 { get; set; }
        public DiceData DamageRoll { get; set; }
        public DiceData HitRoll { get; set; }
        public PositionTypes Position { get; set; }
        public PositionTypes DefPosition { get; set; }
        public int style { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public int ArmorClass { get; set; }
        public int wimpy { get; set; }
        public int Deaf { get; set; }
        public int PermanentStrength { get; set; }
        public int PermanentIntelligence { get; set; }
        public int PermanentWisdom { get; set; }
        public int PermanentDexterity { get; set; }
        public int PermanentConstitution { get; set; }
        public int PermanentCharisma { get; set; }
        public int PermanentLuck { get; set; }
        public int ModStrength { get; set; }
        public int ModIntelligence { get; set; }
        public int ModWisdom { get; set; }
        public int ModDexterity { get; set; }
        public int ModConstitution { get; set; }
        public int ModCharisma { get; set; }
        public int ModLuck { get; set; }
        public int MentalState { get; set; }
        public int EmotionalState { get; set; }
        public int retran { get; set; }
        public int regoto { get; set; }
        public int MobInvisible { get; set; }
        public Dictionary<ATTypes, char> Colors { get; set; }
        public int HomeVNum { get; set; }
        public int ResetVnum { get; set; }
        public int ResetNum { get; set; }

        public CharacterInstance()
        {
            Colors = new Dictionary<ATTypes, char>();
            SavingThrows = new SavingThrowData();
        }

        public bool IsImmune(SpellDamageTypes type)
        {
            ResistanceTypes resType = LookupManager.Instance.GetResistanceType(type);
            return resType != ResistanceTypes.Unknown && Immunity.IsSet((int)resType);
        }

        public MobTemplate MobIndex
        {
            get { return Parent.CastAs<MobTemplate>(); }
        }

        #region Macros

        public bool IsIgnoring(CharacterInstance ch)
        {
            // TODO
            return false;
        }

        public bool IsNpc()
        {
            return Act.IsSet((int)ActFlags.IsNpc);
        }

        public bool IsImmortal()
        {
            return Trust >= Program.LEVEL_IMMORTAL;
        }

        public bool IsHero()
        {
            return Trust >= Program.LEVEL_HERO;
        }

        public bool IsAffected(AffectedByTypes affectedBy)
        {
            return AffectedBy.IsSet((int)affectedBy);
        }

        public bool IsFloating()
        {
            return IsAffected(AffectedByTypes.Flying) || IsAffected(AffectedByTypes.Floating);
        }

        public bool IsRetired()
        {
            return PlayerData != null && PlayerData.Flags.IsSet((int)PCFlags.Retired);
        }

        public bool IsGuest()
        {
            return PlayerData != null && PlayerData.Flags.IsSet((int)PCFlags.Guest);
        }

        public bool CanCast()
        {
            return (CurrentClass != ClassTypes.Mage && CurrentClass != ClassTypes.Cleric);
        }
        public bool IsVampire()
        {
            return (!IsNpc() && (CurrentRace == RaceTypes.Vampire) || CurrentClass == ClassTypes.Vampire);
        }
        public bool IsGood()
        {
            return CurrentAlignment >= 350;
        }
        public bool IsEvil()
        {
            return CurrentAlignment <= -350;
        }
        public bool IsNeutral()
        {
            return !IsGood() && !IsEvil();
        }
        public bool IsAwake()
        {
            return Position > PositionTypes.Sleeping;
        }
        public int GetArmorClass()
        {
            return ArmorClass + (IsAwake()
                                   ? GameConstants.dex_app[CurrentDexterity].defensive
                                   : 0) + fight.VAMP_AC(this);
        }
        public int GetHitroll()
        {
            return HitRoll.SizeOf + GameConstants.str_app[CurrentStrength].tohit
                   + (2 - (Math.Abs(MentalState) / 10));
        }
        public int GetDamroll()
        {
            return DamageRoll.SizeOf + DamageRoll.Bonus + GameConstants.str_app[CurrentStrength].todam +
                   ((MentalState > 5 && MentalState < 15) ? 1 : 0);
        }
        public bool IsOutside()
        {
            return !CurrentRoom.Flags.IsSet((int)RoomFlags.Indoors) &&
                   !CurrentRoom.Flags.IsSet((int)RoomFlags.Tunnel);
        }

        public bool IsDrunk(int drunk)
        {
            return SmaugCS.Common.SmaugRandom.Percent() < (PlayerData.ConditionTable[ConditionTypes.Drunk] & 2 / drunk);
        }
        public bool IsClanned()
        {
            return !IsNpc() &&
                PlayerData.Clan != null &&
                PlayerData.Clan.ClanType != ClanTypes.Order &&
                PlayerData.Clan.ClanType != ClanTypes.Guild;
        }
        public bool IsOrdered()
        {
            return !IsNpc() &&
                   PlayerData.Clan != null &&
                   PlayerData.Clan.ClanType == ClanTypes.Order;
        }
        public bool IsGuilded()
        {
            return !IsNpc() &&
                   PlayerData.Clan != null &&
                   PlayerData.Clan.ClanType == ClanTypes.Guild;
        }
        public bool IsDeadlyClan()
        {
            return !IsNpc() &&
                   PlayerData.Clan != null &&
                   PlayerData.Clan.ClanType != ClanTypes.NoKill &&
                   PlayerData.Clan.ClanType != ClanTypes.Order &&
                   PlayerData.Clan.ClanType != ClanTypes.Guild;
        }
        public bool IsDevoted()
        {
            return !IsNpc() && PlayerData.CurrentDeity != null;
        }
        public bool IsIdle()
        {
            return PlayerData != null &&
                   PlayerData.Flags.IsSet((int)PCFlags.Idle);
        }
        public bool IsPKill()
        {
            return PlayerData != null &&
                   PlayerData.Flags.IsSet((int)PCFlags.Deadly);
        }
        public bool CanPKill()
        {
            return IsPKill() && Level >= 5 && CalculateAge() >= 18;
        }

        public bool HasBodyPart(int part)
        {
            return (ExtraFlags == 0 || ExtraFlags.IsSet(part));
        }
        #endregion

        #region Affects
        public bool IsAffectedBy(int sn)
        {
            return Affects.Exists(x => (int)x.Type == sn);
        }

        public void RemoveAffect(AffectData paf)
        {
            if (Affects == null || Affects.Count == 0)
            {
                LogManager.Bug("affect_remove (%s, %d): no affect", Name, paf != null ? paf.Type : 0);
                return;
            }

            handler.affect_modify(this, paf, false);

            if (CurrentRoom != null)
                CurrentRoom.RemoveAffect(paf);

            Affects.Remove(paf);
        }

        public void AddAffect(AffectData affect)
        {
            if (affect == null)
            {
                LogManager.Bug("%s (%s, NULL)", "affect_to_char", Name);
                return;
            }

            AffectData newAffect = new AffectData
            {
                Type = affect.Type,
                Duration = affect.Duration,
                Location = affect.Location,
                Modifier = affect.Modifier,
                BitVector = affect.BitVector
            };

            Affects.Add(newAffect);
            handler.affect_modify(this, newAffect, true);

            if (CurrentRoom != null)
                CurrentRoom.AddAffect(newAffect);
        }

        public void StripAffects(int sn)
        {
            foreach (AffectData affect in Affects.Where(affect => (int)affect.Type == sn))
                RemoveAffect(affect);
        }

        public void JoinAffect(AffectData paf)
        {
            if (Affects == null || Affects.Count == 0)
                return;

            IEnumerable<AffectData> matchingAffects = Affects.Where(x => x.Type == paf.Type);
            foreach (var affect in matchingAffects)
            {
                paf.Duration = Check.Minimum(1000000, paf.Duration + affect.Duration);
                paf.Modifier = paf.Modifier > 0 ? Check.Minimum(5000, paf.Modifier + affect.Modifier) : affect.Modifier;
                RemoveAffect(affect);
                break;
            }

            AddAffect(paf);
        }

        public void aris_affect(AffectData paf)
        {
            AffectedBy.SetBits(paf.BitVector);
            switch ((int)paf.Location % Program.REVERSE_APPLY)
            {
                case (int)ApplyTypes.Affect:
                    AffectedBy.Bits[0].SetBit(paf.Modifier);
                    break;
                case (int)ApplyTypes.Resistance:
                    Resistance.SetBit(paf.Modifier);
                    break;
                case (int)ApplyTypes.Immunity:
                    Immunity.SetBit(paf.Modifier);
                    break;
                case (int)ApplyTypes.Susceptibility:
                    Susceptibility.SetBit(paf.Modifier);
                    break;
            }
        }

        public void update_aris()
        {
            if (IsNpc() || IsImmortal())
                return;

            bool hiding = IsAffected(AffectedByTypes.Hide);

            AffectedBy.ClearBits();
            Resistance = 0;
            Immunity = 0;
            Susceptibility = 0;
            NoAffectedBy.ClearBits();
            NoResistance = 0;
            NoImmunity = 0;
            NoSusceptibility = 0;

            RaceData myRace = db.GetRace(CurrentRace);
            AffectedBy.SetBits(myRace.AffectedBy);
            Resistance.SetBit(myRace.Resistance);
            Susceptibility.SetBit(myRace.Susceptibility);

            ClassData myClass = db.GetClass(CurrentClass);
            AffectedBy.SetBits(myClass.AffectedBy);
            Resistance.SetBit(myClass.Resistance);
            Susceptibility.SetBit(myClass.Susceptibility);

            if (PlayerData.CurrentDeity != null)
            {
                if (PlayerData.Favor > PlayerData.CurrentDeity.AffectedNum)
                    AffectedBy.SetBits(PlayerData.CurrentDeity.AffectedBy);
                if (PlayerData.Favor > PlayerData.CurrentDeity.ElementNum)
                    Resistance.SetBit(PlayerData.CurrentDeity.Element);
                if (PlayerData.Favor < PlayerData.CurrentDeity.SusceptNum)
                    Susceptibility.SetBit(PlayerData.CurrentDeity.Suscept);
            }

            foreach (AffectData affect in Affects)
                aris_affect(affect);

            foreach (ObjectInstance obj in Carrying
                .Where(x => x.WearLocation != WearLocations.None))
            {
                foreach (AffectData affect in obj.Affects)
                    aris_affect(affect);
                // TODO figure this out
            }

            if (CurrentRoom != null)
            {
                foreach (AffectData affect in CurrentRoom.Affects)
                    aris_affect(affect);
            }

            // TODO: Polymorph

            if (hiding)
                AffectedBy.SetBit((int)AffectedByTypes.Hide);
        }
        #endregion

        public void RemoveComment(NoteData note)
        {
            if (Comments == null || Comments.Count == 0 || note == null)
                return;

            Comments.Remove(note);
            save.save_char_obj(this);
        }

        public bool IsCircleFollowing(CharacterInstance victim)
        {
            CharacterInstance tmp;
            do
            {
                tmp = Master;
                if (tmp == this)
                    return true;

            } while (tmp != null);

            return false;
        }

        public bool CanTakePrototype
        {
            get
            {
                if (IsImmortal())
                    return true;
                return IsNpc()
                       && Act.IsSet((int)ActFlags.Prototype);
            }
        }

        public void ModifySkill(int sn, int mod, bool add)
        {
            if (IsNpc())
                return;

            if (add)
                PlayerData.Learned[sn] += mod;
            else
                PlayerData.Learned[sn] = Check.Range(0, PlayerData.Learned[sn] + mod, Macros.GET_ADEPT(this, sn));
        }

        #region Experience
        public int ExperienceWorth
        {
            get
            {
                int wexp = ((int)Math.Pow(Level, 3) * 5) + MaximumHealth;
                wexp -= (ArmorClass - 50) * 2;
                wexp += (BareDice.NumberOf * BareDice.SizeOf + GetDamroll()) * 50;
                wexp += GetHitroll() * Level * 10;
                if (IsAffected(AffectedByTypes.Sanctuary))
                    wexp += (int)(wexp * 1.5);
                if (IsAffected(AffectedByTypes.FireShield))
                    wexp += (int)(wexp * 1.2);
                if (IsAffected(AffectedByTypes.ShockShield))
                    wexp += (int)(wexp * 1.2);
                return Check.Range(Program.MIN_EXP_WORTH, wexp, Program.MAX_EXP_WORTH);
            }
        }

        public int ExperienceBase
        {
            get
            {
                return IsNpc()
                ? 1000
                : db.CLASSES.First(x => x.Type == CurrentClass).BaseExperience;
            }
        }

        public int GetExperienceLevel(int level)
        {
            return ((int)Math.Pow(Check.Maximum(0, level - 1), 3) * ExperienceBase);
        }

        public int GetLevelExperience(int cexp)
        {
            int x = Program.LEVEL_SUPREME;
            int lastx = x;
            int y = 0;

            while (y == 0)
            {
                int tmp = GetExperienceLevel(x);
                lastx = x;
                if (tmp > cexp)
                    x /= 2;
                else if (lastx != x)
                    x += (x / 2);
                else
                    y = x;
            }

            return y < 1 ? 1 : y > Program.LEVEL_SUPREME ? Program.LEVEL_SUPREME : y;
        }
        #endregion

        #region Statistics

        public int CalculateAge()
        {
            if (IsNpc()) return -1;

            int num_days = ((db.GameTime.Month + 1) * db.SystemData.DaysPerMonth) + db.GameTime.Day;
            int ch_days = ((PlayerData.Month + 1) * db.SystemData.DaysPerMonth) + PlayerData.Day;
            int age = db.GameTime.Year - PlayerData.Year;

            if (ch_days - num_days > 0)
                age -= 1;
            return age;
        }

        private int GetCurrentStat(ApplyTypes statistic)
        {
            ClassData currentClass = db.CLASSES.First(x => x.Type == CurrentClass);
            int max = 20;

            if (IsNpc() || currentClass.PrimaryAttribute == (int)statistic)
                max = 25;
            if (currentClass.SecondaryAttribute == (int)statistic)
                max = 22;
            if (currentClass.DeficientAttribute == (int)statistic)
                max = 16;

            return max;
        }

        public int CurrentStrength
        {
            get { return Check.Range(3, PermanentStrength + ModStrength, GetCurrentStat(ApplyTypes.Strength)); }
        }

        public int CurrentIntelligence
        {
            get { return Check.Range(3, PermanentIntelligence + ModIntelligence, GetCurrentStat(ApplyTypes.Intelligence)); }
        }

        public int CurrentWisdom
        {
            get { return Check.Range(3, PermanentWisdom + ModWisdom, GetCurrentStat(ApplyTypes.Wisdom)); }
        }

        public int CurrentDexterity
        {
            get { return Check.Range(3, PermanentDexterity + ModDexterity, GetCurrentStat(ApplyTypes.Dexterity)); }
        }

        public int CurrentConstitution
        {
            get { return Check.Range(3, PermanentConstitution + ModConstitution, GetCurrentStat(ApplyTypes.Constitution)); }
        }

        public int CurrentCharisma
        {
            get { return Check.Range(3, PermanentCharisma + ModCharisma, GetCurrentStat(ApplyTypes.Charisma)); }
        }

        public int CurrentLuck
        {
            get { return Check.Range(3, PermanentLuck + ModLuck, GetCurrentStat(ApplyTypes.Luck)); }
        }

        public int CanCarryN()
        {
            int penalty = 0;

            if (!IsNpc() && Level >= Program.LEVEL_IMMORTAL)
                return Trust * 200;
            if (IsNpc() && Act.IsSet((int)ActFlags.Immortal))
                return Level * 200;
            if (GetEquippedItem(WearLocations.Wield) != null)
                ++penalty;
            if (GetEquippedItem(WearLocations.DualWield) != null)
                ++penalty;
            if (GetEquippedItem(WearLocations.WieldMissile) != null)
                ++penalty;
            if (GetEquippedItem(WearLocations.Hold) != null)
                ++penalty;
            if (GetEquippedItem(WearLocations.Shield) != null)
                ++penalty;
            return Check.Range(5, (Level + 15) / 5 + CurrentDexterity - 13 - penalty, 20);
        }

        public int CanCarryMaxWeight()
        {
            if (!IsNpc() && Level >= Program.LEVEL_IMMORTAL)
                return 1000000;
            if (IsNpc() && Act.IsSet((int)ActFlags.Immortal))
                return 1000000;
            return GameConstants.str_app[CurrentStrength].Carry;
        }

        #endregion

        public ObjectInstance GetEquippedItem(WearLocations location)
        {
            return GetEquippedItem((int)location);
        }
        public ObjectInstance GetEquippedItem(int location)
        {
            ObjectInstance maxObj = null;
            WearLocations wearLoc = EnumerationExtensions.GetEnum<WearLocations>(location);
            foreach (ObjectInstance obj in Carrying.Where(x => x.WearLocation == wearLoc))
            {
                if (obj.ObjectIndex.Layers == 0)
                    return obj;
                if (maxObj == null || obj.ObjectIndex.Layers > maxObj.ObjectIndex.Layers)
                    maxObj = obj;
            }

            return maxObj;
        }

        public int GetEncumberedMove(int movement)
        {
            int max = CanCarryMaxWeight();
            if (CarryWeight >= max)
                return Convert.ToInt16(movement * 4);
            if (CarryWeight >= max * 0.95)
                return Convert.ToInt16(movement * 3.5);
            if (CarryWeight >= max * 0.90)
                return Convert.ToInt16(movement * 3);
            if (CarryWeight >= max * 0.85)
                return Convert.ToInt16(movement * 2.5);
            if (CarryWeight >= max * 0.80)
                return Convert.ToInt16(movement * 2);
            if (CarryWeight >= max * 0.75)
                return Convert.ToInt16(movement * 1.5);
            return movement;
        }

        public void AdvanceLevel()
        {
            string buffer = string.Format("the {0}", tables.GetTitle(CurrentClass, Level, Gender));
            player.set_title(this, buffer);

            ClassData myClass = db.GetClass(CurrentClass);

            int add_hp = GameConstants.con_app[CurrentConstitution].hitp +
                         SmaugCS.Common.SmaugRandom.Between(myClass.MinimumHealth, myClass.MaximumHealth);
            int add_mana = myClass.UseMana
                               ? SmaugCS.Common.SmaugRandom.Between(2, (2 * CurrentIntelligence + CurrentWisdom) / 8)
                               : 0;
            int add_move = SmaugCS.Common.SmaugRandom.Between(5, (CurrentConstitution + CurrentDexterity) / 4);
            int add_prac = GameConstants.wis_app[CurrentWisdom].practice;

            add_hp = Check.Maximum(1, add_hp);
            add_mana = Check.Maximum(0, add_mana);
            add_move = Check.Maximum(10, add_move);

            if (IsPKill())
            {
                add_mana = (int)(add_mana + add_mana * 0.3f);
                add_move = (int)(add_move + add_move * 0.3f);
                add_hp += 1;
                color.send_to_char("Gravoc's Pandect steels your sinews.\r\n", this);
            }

            MaximumHealth += add_hp;
            MaximumMana += add_mana;
            MaximumMovement += add_move;
            Practice += add_prac;

            if (!IsNpc())
                Act.RemoveBit((int)PlayerFlags.BoughtPet);

            if (Level == Program.LEVEL_AVATAR)
                AdvanceLevelAvatar();
            if (Level < Program.LEVEL_IMMORTAL)
            {
                if (IsVampire())
                    buffer = string.Format("Your gain is: {0}/{1} hp, {2}/{3} bp, {4}/{5} mv, {6}/{7} prac.\r\n",
                                           add_hp, MaximumHealth, 1, Level + 10, add_move, MaximumMovement, add_prac,
                                           Practice);
                else
                    buffer = string.Format("Your gain is: {0}/{1} hp, {2}/{3} mana, {4}/{5} mv, {6}/{7} prac.\r\n",
                                           add_hp, MaximumHealth, add_mana, MaximumMana, add_move, MaximumMovement,
                                           add_prac, Practice);

                color.set_char_color(ATTypes.AT_WHITE, this);
                color.send_to_char(buffer, this);
            }
        }

        private void AdvanceLevelAvatar()
        {
            IEnumerable<DescriptorData> descriptors = db.DESCRIPTORS.
                                                  Where(d => d.ConnectionStatus == ConnectionTypes.Playing
                                                             && d.Character != this);
            foreach (DescriptorData d in descriptors)
            {
                color.set_char_color(ATTypes.AT_WHITE, d.Character);
                color.ch_printf(d.Character, "%s has just achieved Avatarhood!\r\n", Name);
            }

            color.set_char_color(ATTypes.AT_WHITE, this);
            Help.do_help(this, "M_ADVHERO_");
        }

        public void GainXP(int gain)
        {
            if (IsNpc() || (Level >= Program.LEVEL_AVATAR))
                return;

            double modgain = gain;
            if (modgain > 0 && IsPKill() && Level < 17)
            {
                if (Level <= 6)
                {
                    color.send_to_char("The Favor of Gravoc fosters your learning.\r\n", this);
                    modgain *= 2;
                }
                if (Level <= 10 && Level >= 7)
                {
                    color.send_to_char("The Hand of Gravoc hastens your learning.\r\n", this);
                    modgain *= 1.75f;
                }
                if (Level <= 13 && Level >= 11)
                {
                    color.send_to_char("The Cunning of Gravoc succors your learning.\r\n", this);
                    modgain *= 1.5f;
                }
                if (Level <= 16 && Level >= 14)
                {
                    color.send_to_char("THe Patronage of Gravoc reinforces your learning.\r\n", this);
                    modgain *= 1.25f;
                }
            }

            RaceData myRace = db.GetRace(CurrentRace);
            modgain *= (myRace.ExperienceMultiplier / 100.0f);

            if (IsPKill() && modgain < 0)
            {
                if (Experience + modgain < GetExperienceLevel(Level))
                {
                    modgain = GetExperienceLevel(Level) - Experience;
                    color.send_to_char("Gravoc's Pandect protects your insight.\r\n", this);
                }
            }

            modgain = Check.Minimum((int)modgain, GetExperienceLevel(Level + 2) - GetExperienceLevel(Level + 1));
            Experience = Check.Maximum(0, Experience + (int)modgain);

            if (Macros.NOT_AUTHORIZED(this) && Experience >= GetExperienceLevel(Level + 1))
            {
                color.send_to_char("You cannot ascend to a higher level until you are authorized.\r\n", this);
                Experience = GetExperienceLevel(Level + 1) - 1;
                return;
            }

            while (Level < Program.LEVEL_AVATAR && Experience >= GetExperienceLevel(Level + 1))
            {
                color.set_char_color(ATTypes.AT_WHITE | ATTypes.AT_BLINK, this);
                Level += 1;
                color.ch_printf(this, "You have not obtained experience level %d!\r\n", Level);
                AdvanceLevel();
            }
        }

        public int HealthGain()
        {
            int gain;
            if (IsNpc())
            {
                gain = Level * 3 / 2;
                if (IsAffected(AffectedByTypes.Poison))
                    gain /= 4;
                return Check.Minimum(gain, MaximumHealth - CurrentHealth);
            }

            gain = Check.Minimum(5, Level);
            switch (Position)
            {
                case PositionTypes.Dead:
                    return 0;
                case PositionTypes.Mortal:
                case PositionTypes.Incapacitated:
                    return -1;
                case PositionTypes.Stunned:
                    return 1;
                case PositionTypes.Sleeping:
                    gain += (int)(CurrentConstitution * 2.0f);
                    break;
                case PositionTypes.Resting:
                    gain += (int)(CurrentConstitution * 1.25f);
                    break;
            }

            if (IsVampire())
                gain = GetModifiedStatGainForVampire(gain);

            if (PlayerData.ConditionTable[ConditionTypes.Full] == 0)
                gain /= 2;
            if (PlayerData.ConditionTable[ConditionTypes.Thirsty] == 0)
                gain /= 2;

            if (IsAffected(AffectedByTypes.Poison))
                gain /= 4;
            return Check.Minimum(gain, MaximumHealth - CurrentHealth);
        }

        public int ManaGain()
        {
            int gain;

            if (IsNpc())
            {
                gain = Level;

                if (IsAffected(AffectedByTypes.Poison))
                    gain /= 4;
                return Check.Minimum(gain, MaximumMana - CurrentMana);
            }

            gain = Check.Minimum(5, Level / 2);
            if (Position < PositionTypes.Sleeping)
                return 0;
            switch (Position)
            {
                case PositionTypes.Sleeping:
                    gain += (int)(CurrentIntelligence * 3.25f);
                    break;
                case PositionTypes.Resting:
                    gain += (int)(CurrentIntelligence * 1.75f);
                    break;
            }

            if (PlayerData.ConditionTable[ConditionTypes.Full] == 0)
                gain /= 2;
            if (PlayerData.ConditionTable[ConditionTypes.Thirsty] == 0)
                gain /= 2;

            if (IsAffected(AffectedByTypes.Poison))
                gain /= 4;
            return Check.Minimum(gain, MaximumMana - CurrentMana);
        }

        public int MovementGain()
        {
            int gain;

            if (IsNpc())
            {
                gain = Level;
                if (IsAffected(AffectedByTypes.Poison))
                    gain /= 4;
                return Check.Minimum(gain, MaximumMovement - CurrentMovement);
            }

            gain = Check.Maximum(15, 2 * Level);

            switch (Position)
            {
                case PositionTypes.Dead:
                    return 0;
                case PositionTypes.Mortal:
                case PositionTypes.Incapacitated:
                    return -1;
                case PositionTypes.Stunned:
                    return 1;
                case PositionTypes.Sleeping:
                    gain += (int)(CurrentDexterity * 4.5f);
                    break;
                case PositionTypes.Resting:
                    gain += (int)(CurrentDexterity * 2.5f);
                    break;
            }

            if (IsVampire())
                gain = GetModifiedStatGainForVampire(gain);

            if (PlayerData.ConditionTable[ConditionTypes.Full] == 0)
                gain /= 2;
            if (PlayerData.ConditionTable[ConditionTypes.Thirsty] == 0)
                gain /= 2;

            if (IsAffected(AffectedByTypes.Poison))
                gain /= 4;

            return Check.Minimum(gain, MaximumMovement - CurrentMovement);
        }

        private int GetModifiedStatGainForVampire(int gain)
        {
            int modGain = gain;
            if (PlayerData.ConditionTable[ConditionTypes.Bloodthirsty] <= 1)
                modGain /= 2;
            else if (PlayerData.ConditionTable[ConditionTypes.Bloodthirsty] >= (8 + Level))
                modGain *= 2;

            if (IsOutside())
            {
                switch (db.GameTime.Sunlight)
                {
                    case SunPositionTypes.Sunset:
                    case SunPositionTypes.Sunrise:
                        modGain /= 2;
                        break;
                    case SunPositionTypes.Light:
                        modGain /= 4;
                        break;
                }
            }
            return modGain;
        }

        public void CheckAlignment()
        {
            if (CurrentAlignment < db.GetRace(CurrentRace).MinimumAlignment
                || CurrentAlignment > db.GetRace(CurrentRace).MaximumAlignment)
            {
                color.set_char_color(ATTypes.AT_BLOOD, this);
                color.send_to_char("Your actions have been incompatible with the ideals of your race. This troubles you.", this);
            }

            if (CurrentClass == ClassTypes.Paladin)
            {
                if (CurrentAlignment < 250)
                {
                    color.set_char_color(ATTypes.AT_BLOOD, this);
                    color.send_to_char("You are wracked with guilt and remorse for your craven actions!\r\n", this);
                    comm.act(ATTypes.AT_BLOOD, "$n prostrates $mself, seeking forgiveness from $s Lord.", this, null, null, ToTypes.Room);
                    handler.worsen_mental_state(this, 15);
                    return;
                }
                if (CurrentAlignment < 500)
                {
                    color.set_char_color(ATTypes.AT_BLOOD, this);
                    color.send_to_char("As you betray your faith, your mind begins to betray you.\r\n", this);
                    comm.act(ATTypes.AT_BLOOD, "$n shudders, judging $s actions unworthy of a Paladin.", this, null, null, ToTypes.Room);
                    handler.worsen_mental_state(this, 6);
                }
            }
        }

        public bool WillFall(int fall)
        {
            if (CurrentRoom.Flags.IsSet((int)RoomFlags.NoFloor)
                && Macros.CAN_GO(this, (short)DirectionTypes.Down)
                && (!IsAffected(AffectedByTypes.Flying)
                    || (CurrentMount != null && !CurrentMount.IsAffected(AffectedByTypes.Flying))))
            {
                if (fall > 80)
                {
                    LogManager.Bug("Falling (in a loop?) more than 80 rooms: vnum {0}", CurrentRoom.Vnum);
                    CurrentRoom.FromRoom(this);
                    db.get_room_index(Program.ROOM_VNUM_TEMPLE).ToRoom(this);
                    return true;
                }

                color.set_char_color(ATTypes.AT_FALLING, this);
                color.send_to_char("You're falling down...\r\n", this);
                Move.move_char(this, CurrentRoom.GetExit((int)DirectionTypes.Down), ++fall);
                return true;
            }
            return false;
        }

        #region Followers
        public void AddFollower(CharacterInstance master)
        {
            if (Master != null)
            {
                LogManager.Bug("non-null master");
                return;
            }

            Master = master;
            Leader = null;

            if (IsNpc() && Act.IsSet((int)ActFlags.Pet)
                && !master.IsNpc())
                master.PlayerData.Pet = this;

            if (handler.can_see(master, this))
                comm.act(ATTypes.AT_ACTION, "$n now follows you.", this, null, master, ToTypes.Victim);

            comm.act(ATTypes.AT_ACTION, "You now follow $N.", this, null, master, ToTypes.Character);
        }

        public void StopFollower()
        {
            if (Master == null)
            {
                LogManager.Bug("null master");
                return;
            }

            if (IsNpc() && !Master.IsNpc()
                && Master.PlayerData.Pet == this)
                Master.PlayerData.Pet = null;

            if (IsAffected(AffectedByTypes.Charm))
            {
                AffectedBy.RemoveBit((int)AffectedByTypes.Charm);
                //ch.RemoveAffect(gsn_charm_person);    TODO Fix this!
                if (!Master.IsNpc())
                    Master.PlayerData.charmies--;
            }

            if (handler.can_see(Master, this))
            {
                if (!(!Master.IsNpc() && IsImmortal()
                      && !Master.IsImmortal()))
                    comm.act(ATTypes.AT_ACTION, "$n stops following you.", this, null, Master, ToTypes.Victim);
            }

            comm.act(ATTypes.AT_ACTION, "You stop following $N.", this, null, Master, ToTypes.Character);

            Master = null;
            Leader = null;
        }

        public void DieFollower()
        {
            if (Master != null)
                StopFollower();

            Leader = null;

            foreach (CharacterInstance fch in db.CHARACTERS)
            {
                if (fch.Master == this)
                    StopFollower();
                if (fch.Leader == this)
                    fch.Leader = fch;
            }
        }

        public bool IsSameGroup(CharacterInstance ch)
        {
            return Leader == ch.Leader;
        }
        #endregion

        #region Languages
        public int KnowsLanguage(int language, CharacterInstance cch)
        {
            if (!IsNpc() && IsImmortal())
                return 100;
            if (IsNpc() && (Speaks == 0
                || Speaks.IsSet((language & ~(int)LanguageTypes.Clan))))
                return 100;
            if (language.IsSet((int)LanguageTypes.Common))
                return 100;

            if ((language & (int)LanguageTypes.Clan) > 0)
            {
                if (IsNpc() || cch.IsNpc())
                    return 100;
                if (PlayerData.Clan == cch.PlayerData.Clan
                    && PlayerData.Clan != null)
                    return 100;
            }

            if (!IsNpc())
            {
                if (db.GetRace(CurrentRace).Language.IsSet(language))
                    return 100;

                for (int i = 0; i < GameConstants.LanguageTable.Keys.Count; i++)
                {
                    if (i == (int)LanguageTypes.Unknown)
                        break;

                    if (language.IsSet(i) && Speaks.IsSet(i))
                    {
                        SkillData skill = db.GetSkill(GameConstants.LanguageTable[i]);
                        if (skill.slot != -1)
                            return PlayerData.Learned[skill.slot];
                    }
                }
            }

            return 0;
        }

        public bool CanLearnLanguage(int language)
        {
            if ((language & (int)LanguageTypes.Clan) > 0)
                return false;
            if (IsNpc() || IsImmortal())
                return false;
            if ((db.GetRace(CurrentRace).Language & language) > 0)
                return false;

            if ((Speaks & language) > 0)
            {
                for (int i = 0; i < GameConstants.LanguageTable.Keys.Count; i++)
                {
                    if (i == (int)LanguageTypes.Unknown)
                        break;

                    if ((language & i) > 0)
                    {
                        if (((int)LanguageTypes.ValidLanguages & i) == 0)
                            return false;

                        SkillData skill = db.GetSkill(GameConstants.LanguageTable[i]);
                        if (skill == null)
                        {
                            LogManager.Bug("can_learn_lang: valid language without sn: %d", i);
                            continue;
                        }
                    }

                    if (PlayerData.Learned[i] >= 99)
                        return false;
                }
            }

            return ((int)LanguageTypes.ValidLanguages & language) > 0;
        }
        #endregion

        #region Hunt-Hate-Fear
        public bool IsHunting(CharacterInstance victim)
        {
            return CurrentHunting != null
                && CurrentHunting.Who == victim;
        }

        public bool IsHating(CharacterInstance victim)
        {
            return CurrentHating != null
                && CurrentHating.Who == victim;
        }

        public bool IsFearing(CharacterInstance victim)
        {
            return CurrentFearing != null
                   && CurrentFearing.Who == victim;
        }

        public bool IsBlind()
        {
            if (!IsNpc() && Act.IsSet((int)PlayerFlags.HolyLight))
                return true;
            if (IsAffected(AffectedByTypes.TrueSight))
                return true;
            if (!IsAffected(AffectedByTypes.Blind))
                return true;

            return false;
        }

        public void StopHunting()
        {
            CurrentHunting = null;
        }

        public void StopHating()
        {
            CurrentHating = null;
        }

        public void StopFearing()
        {
            CurrentFearing = null;
        }

        public void StartHunting(CharacterInstance victim)
        {
            if (CurrentHunting != null)
                StopHunting();

            CurrentHunting = new HuntHateFearData
                                 {
                                     Name = victim.Name,
                                     Who = victim
                                 };
        }

        public void StartFearing(CharacterInstance victim)
        {
            if (CurrentFearing != null)
                StopFearing();

            CurrentFearing = new HuntHateFearData
            {
                Name = victim.Name,
                Who = victim
            };
        }

        public void StartHating(CharacterInstance victim)
        {
            if (CurrentHating != null)
                StopHating();

            CurrentHating = new HuntHateFearData
            {
                Name = victim.Name,
                Who = victim
            };
        }
        #endregion

        public void Equip(ObjectInstance obj, int iWear)
        {
            if (obj.CarriedBy != this)
            {
                LogManager.Bug("%s: obj not being carried by ch", "equip_char");
                return;
            }

            ObjectInstance temp = GetEquippedItem(iWear);
            if (temp != null && (temp.ObjectIndex.Layers == 0 || obj.ObjectIndex.Layers == 0))
            {
                LogManager.Bug("%s: already equipped (%d)", iWear);
                return;
            }

            handler.separate_obj(obj);
            if ((Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiEvil)
                 && IsEvil())
                || (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiGood)
                    && IsGood())
                || (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiNeutral)
                    && IsNeutral()))
            {
                if (handler.LoadingCharacter != this)
                {
                    comm.act(ATTypes.AT_MAGIC, "You are zapped by $p and drop it.", this, obj, null,
                             ToTypes.Character);
                    comm.act(ATTypes.AT_MAGIC, "$n is zapped by $p and drops it.", this, obj, null, ToTypes.Room);
                }
                if (obj.CarriedBy != null)
                    obj.FromCharacter();

                CurrentRoom.ToRoom(obj);
                mud_prog.oprog_zap_trigger(this, obj);

                if (db.SystemData.SaveFlags.IsSet((int)AutoSaveFlags.ZapDrop)
                    && !CharDied())
                    save.save_char_obj(this);
                return;
            }

            ArmorClass -= obj.ApplyArmorClass;
            obj.WearLocation = EnumerationExtensions.GetEnum<WearLocations>(iWear);
            CarryNumber -= obj.GetObjectNumber();
            if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Magical))
                CarryWeight -= obj.GetObjectWeight();

            foreach (AffectData affect in obj.ObjectIndex.Affects)
                AddAffect(affect);
            foreach (AffectData affect in obj.Affects)
                AddAffect(affect);

            if (obj.ItemType == ItemTypes.Light
                && obj.Value[2] != 0
                && CurrentRoom != null)
                ++CurrentRoom.Light;
        }

        public void Unequip(ObjectInstance obj)
        {
            if (obj.WearLocation == WearLocations.None)
            {
                LogManager.Bug("Unequip_char: already unequipped");
                return;
            }

            CarryNumber += obj.GetObjectNumber();
            if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Magical))
                CarryWeight += obj.GetObjectWeight();

            ArmorClass += obj.ApplyArmorClass;
            obj.WearLocation = WearLocations.None;

            foreach (AffectData paf in obj.ObjectIndex.Affects)
                RemoveAffect(paf);
            if (obj.CarriedBy != null)
            {
                foreach (AffectData paf in obj.Affects)
                    RemoveAffect(paf);
            }

            update_aris();

            if (obj.CarriedBy == null)
                return;

            if (obj.ItemType == ItemTypes.Light
                && obj.Value[2] != 0
                && CurrentRoom != null
                && CurrentRoom.Light > 0)
                --CurrentRoom.Light;
        }

        /// <summary>
        /// Expand the name of a character into a string that identifies THAT 
        /// character within a room. E.g. the second 'guard' -> 2. guard
        /// </summary>
        /// <returns></returns>
        public string GetExpandedName()
        {
            if (!IsNpc())
                return Name;

            string name = Name.FirstWord();
            return string.IsNullOrEmpty(name)
                       ? string.Empty
                       : string.Format("{0}.{1}", 1 + CurrentRoom.Persons.Count(rch => name.IsEqual(rch.Name)), name);
        }

        public bool CharDied()
        {
            if (this == handler.CurrentCharacter && handler.CurrentDeadCharacter != null)
                return true;

            return db.ExtractedCharQueue.Any(ccd => ccd.Character == this);
        }
    }
}
