-- WARRIOR.LUA
-- This is the Warrior Class file for the MUD
-- Revised: 2013.11.25
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadClass()
	newClass = LCreateClass("Warrior", 3);
	class.this = newClass;
	class.this:SetPrimaryAttribute("PermanentStrength");
	class.this.Weapon = 10313;
	class.this.Guild = 3022;
	class.this.SkillAdept = 85;
	class.this.ToHitArmorClass0 = 18;
	class.this.ToHitArmorClass32 = 6;
	class.this.MinimumHealthGain = 11;
	class.this.MaximumHealthGain = 15;
	class.this.BaseExperience = 1150;

	LoadClassSkills(class.this);
	LoadLanguageSkills(class.this);
end

function LoadClassSkills(class)
	class:AddSkill("aid", 6, 80);
	class:AddSkill("bash", 30, 85);
	class:AddSkill("blitz", 28, 80);
	class:AddSkill("climb", 2, 85);
	class:AddSkill("cook", 1, 95);
	class:AddSkill("dig", 2, 95);
	class:AddSkill("disarm", 20, 85);
	class:AddSkill("dodge", 19, 70);
	class:AddSkill("doorbash", 8, 85);
	class:AddSkill("elbow", 18, 90);
	class:AddSkill("enhanced damage", 1, 95);
	class:AddSkill("grip", 25, 90);
	class:AddSkill("headbutt", 25, 90);
	class:AddSkill("hitall", 50, 95);
	class:AddSkill("jab", 20, 95);
	class:AddSkill("kick", 1, 85);
	class:AddSkill("lunge", 22, 90);
	class:AddSkill("mount", 3, 85);
	class:AddSkill("parry", 1, 85);
	class:AddSkill("pummel", 30, 80);
	class:AddSkill("punch", 25, 90);
	class:AddSkill("punt", 15, 95);
	class:AddSkill("rescue", 4, 80);
	class:AddSkill("roundhouse", 25, 70);
	class:AddSkill("scan", 46, 45);
	class:AddSkill("search", 5, 85);
	class:AddSkill("shoulder", 18, 90);
	class:AddSkill("stun", 48, 85);
	class:AddSkill("track", 38, 85);
	class:AddSkill("uppercut", 28, 80);
end

function LoadLanguageSkills(class)
	class:AddSkill("common", 1, 99);
	class:AddSkill("dwarven", 1, 99);
	class:AddSkill("elvish", 1, 99);
	class:AddSkill("gith", 1, 99);
	class:AddSkill("goblin", 1, 99);
	class:AddSkill("halfling", 1, 99);
	class:AddSkill("ogre", 1, 99);
	class:AddSkill("orcish", 1, 99);
	class:AddSkill("pixie", 1, 99);
	class:AddSkill("trollese", 1, 99);
end

function LoadCombatStyles(class)
	class:AddSkill("aggressive style", 1, 95);
	class:AddSkill("berserk style", 10 ,95);
	class:AddSkill("defensive style", 2, 95);
	class:AddSkill("evasive style", 2, 95);
	class:AddSkill("standard style", 1, 95);
	class:AddSkill("second attack", 5, 95);
	class:AddSkill("third attack", 23, 95);
	class:AddSkill("fourth attack", 32, 90);
	class:AddSkill("fifth attack", 42, 85);
end

function LoadWeaponSkills(class)
	class:AddSkill("bludgeons", 1, 95);
	class:AddSkill("talonous arms", 1, 80);
	class:AddSkill("short blades", 1, 60);
	class:AddSkill("pugilism", 1, 95);
	class:AddSkill("long blades", 1, 95);
	class:AddSkill("flexible arms", 1, 40);
	class:AddSkill("dual wield", 15, 85);
end

LoadClass();

-- EOF