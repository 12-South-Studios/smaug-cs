using System;
using System.IO;
using System.Linq;
using SmaugCS.Common;
using SmaugCS.Common.Enumerations;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Objects;
using SmaugCS.Logging;
using SmaugCS.Repository;

namespace SmaugCS.Extensions.Character
{
    public static class Affect
    {
        private static int Depth { get; set; }

        public static void ModifyAffect(this CharacterInstance ch, AffectData affect, bool add)
        {
            var mod = affect.Modifier;
            if (add)
                mod = ModifyAndAddAffect(ch, affect, mod);
            else
            {
                ch.AffectedBy.RemoveBit(affect.Type);

                if ((int) affect.Location%Program.REVERSE_APPLY == (int) ApplyTypes.RecurringSpell)
                {
                    mod = Math.Abs(mod);
                    var skill = RepositoryManager.Instance.SKILLS.Values.ToList()[mod];

                    if (!Macros.IS_VALID_SN(mod) || skill == null || skill.Type != SkillTypes.Spell)
                        throw new InvalidDataException(string.Format("RecurringSpell with bad SN {0}", mod));
                    ch.AffectedBy.RemoveBit(AffectedByTypes.RecurringSpell);
                    return;
                }

                switch ((int) affect.Location%Program.REVERSE_APPLY)
                {
                    case (int) ApplyTypes.Affect:
                        return;
                    case (int) ApplyTypes.ExtendedAffect:
                        ch.AffectedBy.RemoveBit(mod);
                        return;
                    case (int) ApplyTypes.Resistance:
                        ch.Resistance.RemoveBit(mod);
                        return;
                    case (int) ApplyTypes.Immunity:
                        ch.Immunity.RemoveBit(mod);
                        return;
                    case (int) ApplyTypes.Susceptibility:
                        ch.Susceptibility.RemoveBit(mod);
                        return;
                    case (int) ApplyTypes.Remove:
                        return;
                }
                mod = 0 - mod;
            }

            switch ((int)affect.Location % Program.REVERSE_APPLY)
            {
                case (int)ApplyTypes.Strength:
                    ch.ModStrength += mod;
                    break;
                case (int)ApplyTypes.Dexterity:
                    ch.ModDexterity += mod;
                    break;
                case (int)ApplyTypes.Intelligence:
                    ch.ModIntelligence += mod;
                    break;
                case (int)ApplyTypes.Wisdom:
                    ch.ModWisdom += mod;
                    break;
                case (int)ApplyTypes.Constitution:
                    ch.ModConstitution += mod;
                    break;
                case (int)ApplyTypes.Charisma:
                    ch.ModCharisma += mod;
                    break;
                case (int)ApplyTypes.Luck:
                    ch.ModLuck += mod;
                    break;
                case (int)ApplyTypes.Gender:
                    //ch.Gender = (ch.Gender + mod) % 3;
                    // TODO Fix this
                    //if (ch.Gender < 0)
                    //    ch.Gender += 2;
                    //ch.Gender = Check.Range(0, ch.Gender, 2);
                    break;

                case (int)ApplyTypes.Height:
                    ch.Height += mod;
                    break;
                case (int)ApplyTypes.Weight:
                    ch.Weight += mod;
                    break;
                case (int)ApplyTypes.Mana:
                    ch.MaximumMana += mod;
                    break;
                case (int)ApplyTypes.Hit:
                    ch.MaximumHealth += mod;
                    break;
                case (int)ApplyTypes.Movement:
                    ch.MaximumMovement += mod;
                    break;
                case (int)ApplyTypes.ArmorClass:
                    ch.ArmorClass += mod;
                    break;
                case (int)ApplyTypes.HitRoll:
                    ch.HitRoll.SizeOf += mod;
                    break;
                case (int)ApplyTypes.DamageRoll:
                    ch.DamageRoll.SizeOf += mod;
                    break;

                case (int)ApplyTypes.SaveVsPoison:
                    ch.SavingThrows.SaveVsPoisonDeath += mod;
                    break;
                case (int)ApplyTypes.SaveVsRod:
                    ch.SavingThrows.SaveVsWandRod += mod;
                    break;
                case (int)ApplyTypes.SaveVsParalysis:
                    ch.SavingThrows.SaveVsParalysisPetrify += mod;
                    break;
                case (int)ApplyTypes.SaveVsBreath:
                    ch.SavingThrows.SaveVsBreath += mod;
                    break;
                case (int)ApplyTypes.SaveVsSpell:
                    ch.SavingThrows.SaveVsSpellStaff += mod;
                    break;

                case (int)ApplyTypes.Affect:
                    //ch.AffectedBy.Bits[0].SetBit(mod);
                    break;
                case (int)ApplyTypes.ExtendedAffect:
                    ch.AffectedBy.SetBit(mod);
                    break;
                case (int)ApplyTypes.Resistance:
                    ch.Resistance.SetBit(mod);
                    break;
                case (int)ApplyTypes.Immunity:
                    ch.Immunity.SetBit(mod);
                    break;
                case (int)ApplyTypes.Susceptibility:
                    ch.Susceptibility.SetBit(mod);
                    break;
                case (int)ApplyTypes.Remove:
                    //ch.AffectedBy.Bits[0].RemoveBit(mod);
                    break;

                case (int)ApplyTypes.Full:
                    if (!ch.IsNpc())
                        ((PlayerInstance)ch).PlayerData.ConditionTable[ConditionTypes.Full] =
                            (((PlayerInstance)ch).PlayerData.ConditionTable[ConditionTypes.Full] + mod).GetNumberThatIsBetween(0, 48);
                    break;
                case (int)ApplyTypes.Thirst:
                    if (!ch.IsNpc())
                        ((PlayerInstance)ch).PlayerData.ConditionTable[ConditionTypes.Thirsty] =
                            (((PlayerInstance)ch).PlayerData.ConditionTable[ConditionTypes.Thirsty] + mod).GetNumberThatIsBetween(0, 48);
                    break;
                case (int)ApplyTypes.Drunk:
                    if (!ch.IsNpc())
                        ((PlayerInstance)ch).PlayerData.ConditionTable[ConditionTypes.Drunk] =
                            (((PlayerInstance)ch).PlayerData.ConditionTable[ConditionTypes.Drunk] + mod).GetNumberThatIsBetween(0, 48);
                    break;
                case (int)ApplyTypes.Blood:
                    if (!ch.IsNpc())
                        ((PlayerInstance)ch).PlayerData.ConditionTable[ConditionTypes.Bloodthirsty] =
                            (((PlayerInstance)ch).PlayerData.ConditionTable[ConditionTypes.Bloodthirsty] + mod).GetNumberThatIsBetween(0, ch.Level + 10);
                    break;

                case (int)ApplyTypes.MentalState:
                    ch.MentalState = (ch.MentalState + mod).GetNumberThatIsBetween(-100, 100);
                    break;
                case (int)ApplyTypes.Emotion:
                    ch.EmotionalState = (ch.EmotionalState).GetNumberThatIsBetween(-100, 100);
                    break;

                case (int)ApplyTypes.StripSN:
                    if (Macros.IS_VALID_SN(mod))
                        ch.StripAffects(mod);
                    else
                        LogManager.Instance.Bug("apply_modify: ApplyTypes.StripSN invalid SN %d", mod);
                    break;

                case (int)ApplyTypes.WearSpell:
                case (int)ApplyTypes.RemoveSpell:
                    if ((ch.CurrentRoom.Flags.IsSet(RoomFlags.NoMagic)
                         || ch.Immunity.IsSet(ResistanceTypes.Magic))
                        || (((int)affect.Location % Program.REVERSE_APPLY) == (int)ApplyTypes.WearSpell && !add)
                        || (((int)affect.Location % Program.REVERSE_APPLY) == (int)ApplyTypes.RemoveSpell && add)
                        || handler.SavingCharacter == ch
                        || handler.LoadingCharacter == ch)
                        return;

                    mod = Math.Abs(mod);
                    var skill = RepositoryManager.Instance.SKILLS.Values.ToList()[mod];

                    if (Macros.IS_VALID_SN(mod) && skill != null && skill.Type == SkillTypes.Spell)
                    {
                        if (skill.Target == TargetTypes.Ignore ||
                            skill.Target == TargetTypes.InventoryObject)
                        {
                            LogManager.Instance.Bug("ApplyTypes.WearSpell trying to apply bad target spell. SN is %d.", mod);
                            return;
                        }
                        var retcode = skill.SpellFunction.Value.Invoke(mod, ch.Level, ch, ch);
                        if (retcode == ReturnTypes.CharacterDied || ch.CharDied())
                            return;
                    }
                    break;

                case (int)ApplyTypes.Track:
                    ch.ModifySkill((int)RepositoryManager.Instance.GetEntity<SkillData>("track").Type, mod, add);
                    break;

                // TODO Add the rest

                default:
                    LogManager.Instance.Bug("affect_modify: unknown location %d", affect.Location);
                    return;
            }

            var wield = ch.GetEquippedItem(WearLocations.Wield);
            if (!ch.IsNpc() && handler.SavingCharacter != ch
                && wield != null && wield.GetWeight() > LookupConstants.str_app[ch.GetCurrentStrength()].Wield)
            {
                if (Depth == 0)
                {
                    Depth++;
                    comm.act(ATTypes.AT_ACTION, "You are too weak to wield $p any longer.", ch, wield, null,
                             ToTypes.Character);
                    comm.act(ATTypes.AT_ACTION, "$n stops wielding $p.", ch, wield, null, ToTypes.Room);
                    ch.Unequip(wield);
                    Depth--;
                }
            }
        }

        private static int ModifyAndAddAffect(CharacterInstance ch, AffectData affect, int mod)
        {
            ch.AffectedBy.SetBit(affect.Type);
            if ((int) affect.Location%Program.REVERSE_APPLY == (int) ApplyTypes.RecurringSpell)
            {
                mod = Math.Abs(mod);
                var skill = RepositoryManager.Instance.SKILLS.Values.ToList()[mod];

                if (Macros.IS_VALID_SN(mod) && skill != null && skill.Type == SkillTypes.Spell)
                    ch.AffectedBy.SetBit(AffectedByTypes.RecurringSpell);
                else
                    throw new InvalidDataException($"RecurringSpell with bad SN {mod}");
            }
            return mod;
        }

        public static void RemoveAffect(this CharacterInstance ch, AffectData paf)
        {
            if (ch.Affects == null || ch.Affects.Count == 0)
                throw new InvalidDataException($"Character {ch.ID} has no affects");

            ch.ModifyAffect(paf, false);

            ch.CurrentRoom?.RemoveAffect(paf);

            ch.Affects.Remove(paf);
        }

        public static void AddAffect(this CharacterInstance ch, AffectData affect)
        {
            if (affect == null)
                throw new ArgumentNullException(nameof(affect));

            var newAffect = new AffectData
            {
                Type = affect.Type,
                Duration = affect.Duration,
                Location = affect.Location,
                Modifier = affect.Modifier,
                Flags = affect.Flags
            };

            ch.Affects.Add(newAffect);
            ch.ModifyAffect(newAffect, true);

            ch.CurrentRoom?.AddAffect(newAffect);
        }

        public static void StripAffects(this CharacterInstance ch, int sn)
        {
            foreach (var affect in ch.Affects.Where(affect => (int)affect.Type == sn))
                ch.RemoveAffect(affect);
        }

        public static void JoinAffect(this CharacterInstance ch, AffectData paf)
        {
            if (ch.Affects == null || ch.Affects.Count == 0)
                return;

            var matchingAffects = ch.Affects.Where(x => x.Type == paf.Type);
            foreach (var affect in matchingAffects)
            {
                paf.Duration = 1000000.GetLowestOfTwoNumbers(paf.Duration + affect.Duration);
                paf.Modifier = paf.Modifier > 0 ? 5000.GetLowestOfTwoNumbers(paf.Modifier + affect.Modifier) : affect.Modifier;
                ch.RemoveAffect(affect);
                break;
            }

            ch.AddAffect(paf);
        }

        public static void aris_affect(this CharacterInstance ch, AffectData paf)
        {
            //ch.AffectedBy.SetBits(paf.BitVector);
            switch ((int)paf.Location % Program.REVERSE_APPLY)
            {
                case (int)ApplyTypes.Affect:
                    //ch.AffectedBy.Bits[0].SetBit(paf.Modifier);
                    break;
                case (int)ApplyTypes.Resistance:
                    ch.Resistance.SetBit(paf.Modifier);
                    break;
                case (int)ApplyTypes.Immunity:
                    ch.Immunity.SetBit(paf.Modifier);
                    break;
                case (int)ApplyTypes.Susceptibility:
                    ch.Susceptibility.SetBit(paf.Modifier);
                    break;
            }
        }

        public static void update_aris(this CharacterInstance ch)
        {
            if (ch.IsNpc() || ch.IsImmortal())
                return;

            var hiding = ch.IsAffected(AffectedByTypes.Hide);

            //ch.AffectedBy.ClearBits();
            ch.Resistance = 0;
            ch.Immunity = 0;
            ch.Susceptibility = 0;
            //ch.NoAffectedBy.ClearBits();
            ch.NoResistance = 0;
            ch.NoImmunity = 0;
            ch.NoSusceptibility = 0;

            var myRace = RepositoryManager.Instance.GetRace(ch.CurrentRace);
            //ch.AffectedBy.SetBits(myRace.AffectedBy);
            ch.Resistance.SetBit(myRace.Resistance);
            ch.Susceptibility.SetBit(myRace.Susceptibility);

            var myClass = RepositoryManager.Instance.GetClass(ch.CurrentClass);
            //ch.AffectedBy.SetBits(myClass.AffectedBy);
            ch.Resistance.SetBit(myClass.Resistance);
            ch.Susceptibility.SetBit(myClass.Susceptibility);

            if (!ch.IsNpc() && ((PlayerInstance)ch).PlayerData.CurrentDeity != null)
            {
                // if (ch.PlayerData.Favor > ch.PlayerData.CurrentDeity.AffectedNum)
                //    ch.AffectedBy.SetBits(ch.PlayerData.CurrentDeity.AffectedBy);
                if (((PlayerInstance)ch).PlayerData.Favor > ((PlayerInstance)ch).PlayerData.CurrentDeity.ElementNum)
                    ch.Resistance.SetBit(((PlayerInstance)ch).PlayerData.CurrentDeity.Element);
                if (((PlayerInstance)ch).PlayerData.Favor < ((PlayerInstance)ch).PlayerData.CurrentDeity.SusceptNum)
                    ch.Susceptibility.SetBit(((PlayerInstance)ch).PlayerData.CurrentDeity.Suscept);
            }

            foreach (var affect in ch.Affects)
                ch.aris_affect(affect);

            foreach (var obj in ch.Carrying
                .Where(x => x.WearLocation != WearLocations.None))
            {
                foreach (var affect in obj.Affects)
                    ch.aris_affect(affect);
                // TODO figure this out
            }

            if (ch.CurrentRoom != null)
            {
                foreach (var affect in ch.CurrentRoom.Affects)
                    ch.aris_affect(affect);
            }

            // TODO: Polymorph

            if (hiding)
                ch.AffectedBy = ch.AffectedBy.SetBit(AffectedByTypes.Hide);
        }
    }
}
