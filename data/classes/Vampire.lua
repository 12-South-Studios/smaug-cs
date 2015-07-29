-- VAMPIRE.LUA
-- This is the Vampire Class file for the MUD
-- Revised: 2013.03.05
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadClass()
	newClass = LCreateClass("Vampire", 4);
	class.this = newClass;
	class.this:SetPrimaryAttribute("PermanentConstitution");
	class.this.Weapon = 10312;
	class.this.Guild = 3036;
	class.this.SkillAdept = 95;
	class.this.ToHitArmorClass0 = 18;
	class.this.ToHitArmorClass32 = 7;
	class.this.MinimumHealthGain = 9;
	class.this.MaximumHealthGain = 14;
	class.this.BaseExperience = 1500;
	
	LoadClassSkills(class.this);
	LoadLanguageSkills(class.this);
	LoadCombatStyles(class.this);
	LoadWeaponSkills(class.this);
end

function LoadClassSkills(class)
	class:AddSkill("aid", 21, 60);
	class:AddSkill("bite", 11, 95);
	class:AddSkill("bloodlet", 11, 95);
	class:AddSkill("broach", 41, 60);
	class:AddSkill("chill touch", 3, 95);
	class:AddSkill("climb", 8, 95);
	class:AddSkill("control weather", 18, 95);
	class:AddSkill("cook", 1, 95);
	class:AddSkill("detect magic", 20, 85);
	class:AddSkill("detect traps", 20, 90);
	class:AddSkill("dig", 2, 95);
	class:AddSkill("disarm", 25, 95);
	class:AddSkill("disenchant weapon", 40, 85);
	class:AddSkill("dispel magic", 25, 95);
	class:AddSkill("dodge", 22, 95);
	class:AddSkill("dominate", 14, 95);
	class:AddSkill("elbow", 15, 85);
	class:AddSkill("enchant  weapon", 40, 85);
	class:AddSkill("energy drain", 17, 95);
	class:AddSkill("enhanced damage", 5, 95);
	class:AddSkill("feed", 3, 95);
	class:AddSkill("float", 1, 95);
	class:AddSkill("fly", 10, 95);
	class:AddSkill("grasp suspiria", 50, 95);
	class:AddSkill("harm", 13, 95);
	class:AddSkill("hide", 3, 95);
	class:AddSkill("identify", 15, 95);
	class:AddSkill("infravision", 1, 95);
	class:AddSkill("invis", 10, 95);
	class:AddSkill("kindred strength", 15, 95);
	class:AddSkill("knee", 20, 60);
	class:AddSkill("know alignment", 6, 95);
	class:AddSkill("leap", 18, 80);
	class:AddSkill("lightning bolt", 17, 95);
	class:AddSkill("locate object", 20, 95);
	class:AddSkill("mistform", 18, 95);
	class:AddSkill("mistwalk", 31, 90);
	class:AddSkill("mount", 16, 95);
	class:AddSkill("occulutus visum", 12, 95);
	class:AddSkill("parry", 11, 95);
	class:AddSkill("peek", 16, 95);
	class:AddSkill("poison", 6, 95);
	class:AddSkill("scan", 36, 85);
	class:AddSkill("scry", 29, 95);
	class:AddSkill("search", 9, 95);
	class:AddSkill("shocking grasp", 12, 95);
	class:AddSkill("shriek", 35, 95);
	class:AddSkill("sleep", 12, 95);
	class:AddSkill("sneak", 3, 95);
	class:AddSkill("steal", 6, 95);
	class:AddSkill("stone skin", 30, 95);
	class:AddSkill("teleport", 11, 95);
	class:AddSkill("transport", 45, 40);
	class:AddSkill("ventriloquate", 9, 95);
	class:AddSkill("vomica pravus", 13, 45);
	class:AddSkill("weaken", 6, 95);
	class:AddSkill("word of recall", 30, 95);
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
	class:AddSkill("aggressive style", 4, 95);
	class:AddSkill("berserk style", 12, 80);
	class:AddSkill("defensive style", 16, 90);
	class:AddSkill("standard style", 1, 90);
	class:AddSkill("evasive style", 12, 90);
	class:AddSkill("second attack", 11, 95);
	class:AddSkill("third attack", 23, 95);
	class:AddSkill("fourth attack", 38, 85);
end

function LoadWeaponSkills(class)
	class:AddSkill("bludgeons", 1, 50);
	class:AddSkill("dual wield", 35, 95);
	class:AddSkill("flexible arms", 1, 40);
	class:AddSkill("long blades", 1, 85);
	class:AddSkill("pugilism", 1, 95);
	class:AddSkill("shield", 27, 95);
	class:AddSkill("talonous arms", 1, 95);
	class:AddSkill("short blades", 1, 95);	
end

LoadClass();

-- EOF