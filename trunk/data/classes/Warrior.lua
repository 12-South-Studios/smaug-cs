-- WARRIOR.LUA
-- This is the Warrior Class file for the MUD
-- Revised: 2013.11.25
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadClass()
	newClass = LCreateClass("Warrior", 3);
	class.this = newClass;
	class.this:SetPrimaryAttribute("strength");
	class.this.Weapon = 10313;
	class.this.Guild = 3022;
	class.this.SkillAdept = 85;
	class.this.ToHitArmorClass0 = 18;
	class.this.ToHitArmorClass32 = 6;
	class.this.MinimumHealthGain = 11;
	class.this.MaximumHealthGain = 15;
	class.this.BaseExperience = 1150;

	LoadClassSkills(class.this);
	LoadClassTitles(class.this);
end

function LoadClassSkills(class)
	class:AddSkill("aggressive style", 1, 95);
	class:AddSkill("aid", 6, 80);
	class:AddSkill("bash", 30, 85);

--[[
Skill 'berserk style' 10 95
Skill 'blitz' 28 80
Skill 'bludgeons' 1 95
Skill 'climb' 2 85
Skill 'common' 1 99
Skill 'cook' 1 95
Skill 'defensive style' 2 95
Skill 'dig' 2 95
Skill 'disarm' 20 85
Skill 'dodge' 19 70
Skill 'doorbash' 8 85
Skill 'dual wield' 15 85
Skill 'dwarven' 1 99
Skill 'elbow' 18 90
Skill 'elvish' 1 99
Skill 'enhanced damage' 1 95
Skill 'evasive style' 2 95
Skill 'fifth attack' 42 85
Skill 'flexible arms' 1 40
Skill 'fourth attack' 32 90
Skill 'gith' 1 99
Skill 'goblin' 1 99
Skill 'grip' 25 90
Skill 'halfling' 1 99
Skill 'headbutt' 25 90
Skill 'hitall' 50 95
Skill 'jab' 20 95
Skill 'kick' 1 85
Skill 'long blades' 1 95
Skill 'lunge' 22 90
Skill 'mount' 3 85
Skill 'ogre' 1 99
Skill 'orcish' 1 99
Skill 'parry' 1 85
Skill 'pixie' 1 99
Skill 'pugilism' 1 95
Skill 'pummel' 30 80
Skill 'punch' 25 90
Skill 'punt' 15 95
Skill 'rescue' 4 80
Skill 'roundhouse' 25 70
Skill 'scan' 46 45
Skill 'search' 5 85
Skill 'second attack' 5 95
Skill 'short blades' 1 60
Skill 'shoulder' 18 90
Skill 'standard style' 1 95
Skill 'stun' 48 85
Skill 'talonous arms' 1 80
Skill 'third attack' 23 95
Skill 'track' 38 85
Skill 'trollese' 1 99
Skill 'uppercut' 28 80
--]]
end

function LoadClassTitles(class)
	class:AddTitle(0, "Man", "Woman");
	class:Addtitle(1, "Swordpupil", "Swordpupil");
	
--[[
Title
Recruit~
Recruit~
Title
Sentry~
Sentress~
Title
Fighter~
Fighter~
Title
Soldier~
Soldier~
Title
Warrior~
Warrior~
Title
Veteran~
Veteran~
Title
Swordsman~
Swordswoman~
Title
Fencer~
Fenceress~
Title
Combatant~
Combatess~
Title
Hero~
Heroine~
Title
Myrmidon~
Myrmidon~
Title
Swashbuckler~
Swashbuckleress~
Title
Mercenary~
Mercenaress~
Title
Swordmaster~
Swordmistress~
Title
Lieutenant~
Lieutenant~
Title
Champion~
Lady Champion~
Title
Dragoon~
Lady Dragoon~
Title
Cavalier~
Lady Cavalier~
Title
Knight~
Lady Knight~
Title
Grand Knight~
Grand Knight~
Title
Master Knight~
Master Knight~
Title
Paladin~
Paladin~
Title
Grand Paladin~
Grand Paladin~
Title
Demon Slayer~
Demon Slayer~
Title
Greater Demon Slayer~
Greater Demon Slayer~
Title
Dragon Slayer~
Dragon Slayer~
Title
Greater Dragon Slayer~
Greater Dragon Slayer~
Title
Underlord~
Underlord~
Title
Overlord~
Overlord~
Title
Master of the Sword~
Mistress of the Sword~
Title
Master of the Shield~
Mistress of the Shield~
Title
Master of the Cross~
Mistress of the Cross~
Title
Master of the Rings~
Mistress of the Rings~
Title
Master of the Battle~
Mistress of the Battle~
Title
Baron of Thunder~
Baroness of Thunder~
Title
Baron of Storms~
Baroness of Storms~
Title
Baron of Tornadoes~
Baroness of Tornadoes~
Title
Baron of Hurricanes~
Baroness of Hurricanes~
Title
Baron of Meteors~
Baroness of Meteors~
Title
Duke of Thunder~
Dutchess of Thunder~
Title
Duke of Storms~
Dutchess of Storms~
Title
Duke of Tornadoes~
Dutchess of Tornadoes~
Title
Duke of Hurricanes~
Dutchess of Hurricanes~
Title
Duke of Meteors~
Dutchess of Meteors~
Title
King of Courage~
Queen of Courage~
Title
King of Honor~
Queen of Honor~
Title
King of Strength~
Queen of Strength~
Title
King of Humility~
Queen of Humility~
Title
Avatar~
Avatar~
Title
Neophyte~
Neophyte~
Title
Acolyte~
Acolyte~
Title
Creator~
Creator~
Title
Savior~
Savior~
Title
Demi God~
Demi Goddess~
Title
Immortal~
Immortal~
Title
Lesser God~
Lesser Goddess~
Title
God~
Goddess~
Title
Greater God~
Greater Goddess~
Title
Ascendant God~
Acendant Goddess~
Title
Exalted God~
Exalted Goddess~
Title
Ancient One~
Ancient One~
Title
Eternal One~
Eternal One~
Title
Infinite One~
Infinite One~
Title
Supreme Entity~
Supreme Entity~
End
--]]
end

LoadClass();

-- EOF