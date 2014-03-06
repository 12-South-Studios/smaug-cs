-- AUGURER.LUA
-- This is the Augurer Class file for the MUD
-- Revised: 2014.03.03
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadClass()
	newClass = LCreateClass("Augurer", 7);
	class.this = newClass;
	class.this:SetPrimaryAttribute("wisdom");
	class.this.Weapon = 10312;
	class.this.Guild = 21430;
	class.this.SkillAdept = 95;
	class.this.ToHitArmorClass0 = 18;
	class.this.ToHitArmorClass32 = 7;
	class.this.MinimumHealthGain = 9;
	class.this.MaximumHealthGain = 14;
	class.this.UseMana = true;
	class.this.BaseExperience = 1350;
	
	LoadClassSkills(class.this);
	LoadLanguageSkills(class.this);
	LoadCombatStyles(class.this);
	LoadWeaponSkills(class.this);
end

function LoadClassSkills(class)
	class:AddSkill("aid", 16, 90);
	class:AddSkill("aqua breath", 14, 80);
	class:AddSkill("armor", 2, 90);
	class:AddSkill("bless", 16, 95);
	class:AddSkill("blindness", 30, 90);
	class:AddSkill("cause critical", 17, 95);
	class:AddSkill("cause serious", 9, 95);
	class:AddSkill("charm person", 27, 90);
	class:AddSkill("chill touch", 5, 95);
	class:AddSkill("continual light", 3, 90);
	class:AddSkill("cook", 1, 95);
	class:AddSkill("create food", 4, 95);
	class:AddSkill("create spring", 6, 95);
	class:AddSkill("create water", 5, 95);
	class:AddSkill("cuff", 10, 90);
	class:AddSkill("cure light", 9, 75);
	class:AddSkill("detect hidden", 18, 95);
	class:AddSkill("detect invis", 12, 95);
	class:AddSkill("detect magic", 8, 95);
	class:AddSkill("dig", 2, 85);
	class:AddSkill("disenchant weapon", 27, 90);
	class:AddSkill("dispel magic", 13, 75);
	class:AddSkill("earthquake", 10, 90);
	class:AddSkill("enchant weapon", 27, 90);
	class:AddSkill("energy drain", 14, 90);
	class:AddSkill("enhanced damage", 22, 75);
	class:AddSkill("faerie fire", 34, 95);
	class:AddSkill("fireshield", 40, 95);
	class:AddSkill("float", 11, 95);
	class:AddSkill("fly", 24, 95);
	class:AddSkill("harm", 26, 95);
	class:AddSkill("helical flow", 42, 85);
	class:AddSkill("hide", 17, 90);
	class:AddSkill("identify", 16, 90);
	class:AddSkill("inner warmth", 20, 90);
	class:AddSkill("invis", 12, 95);
	class:AddSkill("kick", 4, 90);
	class:AddSkill("lightning bolt", 10, 90);
	class:AddSkill("locate object", 25, 90);
	class:AddSkill("meditate", 12, 95);
	class:AddSkill("midas touch", 14, 95);
	class:AddSkill("mount", 6, 95);
	class:AddSkill("nostrum", 29, 95);
	class:AddSkill("parry", 22, 95);
	class:AddSkill("pass door", 47, 95);
	class:AddSkill("poison", 19, 90);
	class:AddSkill("refresh", 8, 85);
	class:AddSkill("remove invis", 13, 85);
	class:AddSkill("rescue", 14, 50);
	class:AddSkill("sanctuary", 35, 95);
	class:AddSkill("scan", 3, 85);
	class:AddSkill("scorching surge", 28, 95);
	class:AddSkill("search", 10, 80);
	class:AddSkill("shocking grasp", 7, 90);
	class:AddSkill("sleep", 18, 85);
	class:AddSkill("sneak", 15, 90);
	class:AddSkill("spiral blast", 46, 95);
	class:AddSkill("stone skin", 37, 95);
	class:AddSkill("strike", 15, 90);
	class:AddSkill("weaken", 7, 80);
	class:AddSkill("word of recall", 24, 95);
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
	class:AddSkill("aggressive style", 1, 60);
	class:AddSkill("berserk style", 38, 40);
	class:AddSkill("defensive style", 4, 85);
	class:AddSkill("evasive style", 1, 90);
	class:AddSkill("fourth attack", 41, 95);
	class:AddSkill("second attack", 15, 33);
	class:AddSkill("standard style", 1, 70);
	class:AddSkill("third attack", 31, 66);
end
		
function LoadWeaponSkills(class)
	class:AddSkill("talonous arms", 1, 40);
	class:AddSkill("bludgeons", 1, 75);
	class:AddSkill("dual wield", 17, 80);
	class:AddSkill("flexible arms", 1, 95);
	class:AddSkill("long blades", 1, 45);
	class:AddSkill("pugilism", 1, 90);
	class:AddSkill("short blades", 1, 85);
	class:AddSkill("shield", 23, 90);
end

LoadClass();

-- EOF