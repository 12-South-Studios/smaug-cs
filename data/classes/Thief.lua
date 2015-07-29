-- THIEF.LUA
-- This is the Thief Class file for the MUD
-- Revised: 2013.12.03
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadClass()
	newClass = LCreateClass("Thief", 2);
	class.this = newClass;
	class.this:SetPrimaryAttribute("PermanentDexterity");
	class.this.Weapon = 10312;
	class.this.Guild = 3028;
	class.this.SkillAdept = 85;
	class.this.ToHitArmorClass0 = 18;
	class.this.ToHitArmorClass32 = 8;
	class.this.MinimumHealthGain = 8;
	class.this.MaximumHealthGain = 13;
	class.this.BaseExperience = 750;

	LoadClassSkills(class.this);
	LoadLanguageSkills(class.this);
end

function LoadClassSkills(class)
	class:AddSkill("aggressive style", 1, 80);
	class:AddSkill("aid", 18, 70);
	class:AddSkill("backstab", 1, 95);
	class:AddSkill("bash", 49, 60);
	class:AddSkill("berserk style", 33 ,45);
	class:AddSkill("bludgeons", 1, 40);
	class:AddSkill("circle", 25, 95);
	class:AddSkill("climb", 1, 85);
	class:AddSkill("cook", 1, 95);
	class:AddSkill("defensive style", 1, 95);
	class:AddSkill("detrap", 16, 85);
	class:AddSkill("dig", 2, 90);
	class:AddSkill("disarm", 10, 90);
	class:AddSkill("dodge", 1, 95);
	class:AddSkill("dual wield", 40, 85);
	class:AddSkill("enhanced damage", 44, 55);
	class:AddSkill("evasive style", 2, 95);
	class:AddSkill("flexible arms", 1, 95);
	class:AddSkill("fourth attack", 47, 30);
	class:AddSkill("gouge", 20, 90);
	class:AddSkill("headbutt", 20, 60);
	class:AddSkill("hide", 1, 85);
	class:AddSkill("knee", 15, 70);
	class:AddSkill("leap", 20, 95);
	class:AddSkill("long blades", 1, 80);
	class:AddSkill("mount", 5, 85);
	class:AddSkill("peek", 1, 95);
	class:AddSkill("pick lock", 1, 95);
	class:AddSkill("poison weapon", 27, 85);
	class:AddSkill("pugilism", 1, 85);
	class:AddSkill("punch", 30, 50);
	class:AddSkill("scan", 7, 90);
	class:AddSkill("search", 3, 85);
	class:AddSkill("second attack", 10, 90);
	class:AddSkill("short blades", 1, 95);
	class:AddSkill("sneak", 1, 95);
	class:AddSkill("spinkick", 19, 90);
	class:AddSkill("standard style", 1, 85);
	class:AddSkill("steal", 1, 95);
	class:AddSkill("swipe", 10, 85);
	class:AddSkill("talonous arms", 1, 80);
	class:AddSkill("third attack", 33, 70);
	class:AddSkill("track", 15, 85);
	class:AddSkill("tumble", 46, 95);
	class:AddSkill("vault", 25, 90);
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

LoadClass();

-- EOF