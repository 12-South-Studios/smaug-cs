using System;
using Realm.Library.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;
using NumberExtensions = SmaugCS.Common.NumberExtensions;

namespace SmaugCS.Commands.Admin
{
    class Advance
    {
        public static void do_advance(CharacterInstance ch, string argument)
        {
            ch.SetColor(ATTypes.AT_IMMORT);

            string firstArg = argument.FirstWord();
            string secondArg = argument.SecondWord();

            if (CheckFunctions.CheckIf(ch,
                () => (string.IsNullOrEmpty(firstArg) || string.IsNullOrEmpty(secondArg) || !secondArg.IsNumeric()),
                "Syntax:  advance <character> <Level>")) return;

            var victim = ch.GetCharacterInRoom(firstArg);
            if (CheckFunctions.CheckIfNullObject(ch, victim, "That character is not in the room.")) return;
            if (CheckFunctions.CheckIfNpc(ch, victim, "You cannot advance a non-player-character.")) return;
            if (CheckFunctions.CheckIf(ch, () => (ch.Trust <= victim.Trust || ch == victim), "You can't do that."))
                return;

            var level = Convert.ToInt32(secondArg);
            if (CheckFunctions.CheckIf(ch, () => (level < 1 || level > GameConstants.GetConstant<int>("MaximumLevel")),
                string.Format("Level range is 1 to {0}.", GameConstants.GetConstant<int>("MaximumLevel")))) return;
            if (CheckFunctions.CheckIf(ch, () => (level > ch.Trust), "Level limited to your trust level.")) return;

            if (level <= victim.Level)
                LowerVictimLevel(ch, victim, level);
            else
                RaiseVictimLevel(ch, victim, level);

            for (int i = victim.Level; i < level; i++)
            {
                if (level < LevelConstants.ImmortalLevel)
                    victim.SendTo("You raise a level!");
                victim.Level += 1;
                //todo advance_level(victim);
            }

            victim.Experience = victim.GetExperienceLevel(victim.Level);
            victim.Trust = 0;
        }

        private static void RaiseVictimLevel(CharacterInstance ch, CharacterInstance victim, int level)
        {
            ch.Printf("Raising %s from level %d to level %d!", victim.Name, victim.Level, level);
            if (victim.Level >= LevelConstants.AvatarLevel)
            {
                victim.SetColor(ATTypes.AT_WHITE);
                comm.act(ATTypes.AT_IMMORT, "$n makes some arcane gestures with $s hands, then points $s finger at you!",
                    ch, null, victim, ToTypes.Victim);
                comm.act(ATTypes.AT_IMMORT, "$n makes some arcane gestures with $s hands, then points $s finger at $N!", 
                    ch, null, victim, ToTypes.NotVictim);
                victim.SetColor(ATTypes.AT_WHITE);
                victim.SendTo("You suddenly feel very strange...");
                victim.SetColor(ATTypes.AT_LBLUE);
            }

            try
            {
                var immortalType = EnumerationExtensions.GetEnum<ImmortalTypes>(level);
                
                var attrib = Common.EnumerationExtensions.GetAttribute<ImmortalHelpCategoryAttribute>(immortalType);
                if (attrib != null)
                    Help.do_help(victim, attrib.Value);
                else 
                    victim.SendTo("The gods feel fit to raise your level!");

                if (immortalType != ImmortalTypes.Immortal) return;

                victim.SetColor(ATTypes.AT_WHITE);
                victim.SendTo("You awake... all your possessions are gone.");
                   
                //todo remove objects in inventory
            }
            catch (ArgumentException)
            {
                victim.SendTo("The gods feel fit to raise your level!");
            }
        }
 
        private static void LowerVictimLevel(CharacterInstance ch, CharacterInstance victim, int level)
        {
            victim.SetColor(ATTypes.AT_IMMORT);

            if (victim.Level >= LevelConstants.AvatarLevel && victim.IsImmortal())
            {
                if (((PlayerInstance) victim).PlayerData.bestowments != string.Empty)
                    ((PlayerInstance) victim).PlayerData.bestowments = string.Empty;

                NumberExtensions.RemoveBit(victim.Act, PlayerFlags.HolyLight);
                if (!((PlayerInstance) victim).IsRetired())
                {
                    // todo remove immortal data
                }
            }

            if (level < victim.Level)
            {
                int tempLevel = victim.Level;
                victim.Level = level;
                handler.check_switch(victim, false);
                victim.Level = tempLevel;

                ch.Printf("Demoting %s from level %d to level %d!", victim.Name, victim.Level);
                victim.SendTo("Cursed and forsaken!  The gods have lowered your level...");
            }
            else
            {
                ch.Printf("%s is already level %d.  Re-advancing...", victim.Name, level);
                victim.SendTo("Deja vu!  Your mind reels as you re-live your past levels!");
            }

            victim.Level = 1;
            victim.Experience = victim.GetExperienceLevel(1);
            victim.MaximumHealth = GameConstants.GetConstant<int>("DefaultMaximumHealth");
            victim.MaximumMana = GameConstants.GetConstant<int>("DefaultMaximumMana");
            victim.MaximumMovement = GameConstants.GetConstant<int>("DefaultMaximumMovement");

            // todo zero skills

            victim.Practice = 0;
            victim.CurrentHealth = victim.MaximumHealth;
            victim.CurrentMana = victim.MaximumMana;
            victim.CurrentMovement = victim.MaximumMovement;
            //todo advance_level(victim);

            ((PlayerInstance) victim).PlayerData.rank = string.Empty;
            ((PlayerInstance) victim).PlayerData.WizardInvisible = victim.Trust;

            if (victim.Level <= LevelConstants.AvatarLevel)
            {
                NumberExtensions.RemoveBit(victim.Act, PlayerFlags.WizardInvisibility);
                ((PlayerInstance) victim).PlayerData.WizardInvisible = 0;
            }
        }
    }
}
