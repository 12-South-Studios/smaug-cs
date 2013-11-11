using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Realm.Library.Common;
using Realm.Library.Lua;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Templates;

namespace SmaugCS.Data.Instances
{
    [XmlRoot("Character")]
    public class CharacterInstance : Instance
    {
        public CharacterInstance Master { get; set; }
        public CharacterInstance Leader { get; set; }
        public FightingData CurrentFighting { get; set; }
        public int NumberFighting { get; set; }
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
        public PositionTypes CurrentPosition { get; set; }
        public PositionTypes CurrentDefensivePosition { get; set; }
        public StyleTypes CurrentStyle { get; set; }
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
        public long HomeVNum { get; set; }
        public long ResetVnum { get; set; }
        public int ResetNum { get; set; }
        public LuaInterfaceProxy LuaVM { get; set; }

        public CharacterInstance(int id, string name)
            : base(id, name)
        {
            Colors = new Dictionary<ATTypes, char>();
            SavingThrows = new SavingThrowData();
            LuaVM = new LuaInterfaceProxy();
        }

        ~CharacterInstance()
        {
            Dispose(true);
        }

        #region IDisposable

        /// <summary>
        /// Dispose of any internal resources
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                LuaVM.Close();
            }
        }

        #endregion


        public MobTemplate MobIndex
        {
            get { return Parent.CastAs<MobTemplate>(); }
        }

    }
}
