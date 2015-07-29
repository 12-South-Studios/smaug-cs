-- DRUID.LUA
-- This is the Druid Class file for the MUD
-- Revised: 2014.03.07
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadClass()
	newClass = LCreateClass("Druid", 5);
	class.this = newClass;
	class.this:SetPrimaryAttribute("PermanentWisdom");
	class.this.Weapon = 10315;
	class.this.Guild = 3037;
	class.this.SkillAdept = 95;
	class.this.ToHitArmorClass0 = 18;
	class.this.ToHitArmorClass32 = 7;
	class.this.MinimumHealthGain = 9;
	class.this.MaximumHealthGain = 14;
	class.this.UseMana = true;
	class.this.BaseExperience = 1350;
	
	LoadClassSkills(class.this);
	LoadLanguageSkills(class.this);
end

function LoadClassSkills(class)
	class:AddSkill("acidmist", 41, 80);
	class:AddSkill("aggressive style", 7, 85);
	class:AddSkill("aid", 6, 95);
	class:AddSkill("aqua breath", 18, 95);
	class:AddSkill("armor", 1, 95);
	class:AddSkill("berserk style", 28, 45);
	class:AddSkill("bless", 5, 95);
	class:AddSkill("blindness", 5, 95);
	class:AddSkill("bludgeons", 1, 95);
	class:AddSkill("call lightning", 12, 95);
	class:AddSkill("cause critical", 9, 95);
	class:AddSkill("cause light", 1, 95);
	class:AddSkill("cause serious", 5, 95);
	class:AddSkill("climb", 6, 95);
	class:AddSkill("continual light", 2, 95);
	class:AddSkill("control weather", 13, 95);
	class:AddSkill("cook", 1, 95);
	class:AddSkill("create fire", 6, 95);
	class:AddSkill("create food", 3, 95);
	class:AddSkill("create spring", 10, 95);
	class:AddSkill("create water", 2, 95);
	class:AddSkill("cure blindness", 5, 95);
	class:AddSkill("cure critical", 12, 95);
	class:AddSkill("cure light", 2, 95);
	class:AddSkill("cure poison", 8, 95);
	class:AddSkill("cure serious", 10, 90);
	class:AddSkill("curse", 12, 95);
	class:AddSkill("defensive style", 17, 80);
	class:AddSkill("detect evil", 5, 95);
	class:AddSkill("detect hidden", 8, 95);
	class:AddSkill("detect invis", 4, 95);
	class:AddSkill("detect magic", 3, 95);
	class:AddSkill("detect poison", 13, 95);
	class:AddSkill("detect traps", 8, 85);
	class:AddSkill("dig", 2, 90);
	class:AddSkill("disarm", 21, 95);
	class:AddSkill("disenchant weapon", 30, 80);
	class:AddSkill("dispel evil", 12, 90);
	class:AddSkill("dispel magic", 14, 95);
	class:AddSkill("dream", 17, 95);
	class:AddSkill("earthquake", 8, 95);
	class:AddSkill("enchant weapon", 30, 80);
	class:AddSkill("energy drain", 35, 95);
	class:AddSkill("enhanced damage", 7, 80);
	class:AddSkill("evasive style", 1, 75);
	class:AddSkill("faerie fire", 4, 95);
	class:AddSkill("faerie fog", 11, 95);
	class:AddSkill("fireball", 13, 95);
	class:AddSkill("fireshield", 35, 95);
	class:AddSkill("flamestrike", 16, 95);
	class:AddSkill("flexible arms", 1, 30);
	class:AddSkill("float", 6, 95);
	class:AddSkill("fly", 9, 95);
	class:AddSkill("fourth attack", 45, 65);
	class:AddSkill("harm", 17, 95);
	class:AddSkill("heal", 19, 95);
	class:AddSkill("hide", 45, 95);
	class:AddSkill("iceshield", 25, 95);
	class:AddSkill("identify", 13, 95);
	class:AddSkill("infravision", 6, 95);
	class:AddSkill("invis", 10, 95);
	class:AddSkill("kick", 6, 95);
	class:AddSkill("kindred strength", 20, 95);
	class:AddSkill("know alignment", 9, 95);
	class:AddSkill("lightning bolt", 15, 95);
	class:AddSkill("locate object", 13, 95);
	class:AddSkill("long blades", 1, 50);
	class:AddSkill("magic missile", 9, 95);
	class:AddSkill("mass invis", 20, 95);
	class:AddSkill("mount", 4, 95);
	class:AddSkill("parry", 40, 95);
	class:AddSkill("pass door", 24, 95);
	class:AddSkill("plant pass", 35, 90);
	class:AddSkill("poison", 10, 95);
	class:AddSkill("protection", 11, 80);
	class:AddSkill("pugilism", 1, 90);
	class:AddSkill("refresh", 7, 95);
	class:AddSkill("remove curse", 17, 85);
	class:AddSkill("remove invis", 14, 95);
	class:AddSkill("remove trap", 18, 70);
	class:AddSkill("rescue", 20, 95);
	class:AddSkill("sanctuary", 30, 95);
	class:AddSkill("scan", 15, 60);
	class:AddSkill("scry", 17, 95);
	class:AddSkill("search", 6, 95);
	class:AddSkill("second attack", 15, 95);
	class:AddSkill("shield", 40, 95);
	class:AddSkill("shocking grasp", 18, 95);
	class:AddSkill("shockshield", 45, 95);
	class:AddSkill("short blades", 1, 30);
	class:AddSkill("sleep", 34, 95);
	class:AddSkill("sneak", 17, 95);
	class:AddSkill("spurn", 15, 80);
	class:AddSkill("standard style", 1, 80);
	class:AddSkill("stone skin", 35, 95);
	class:AddSkill("swipe", 15, 80);
	class:AddSkill("talonous arms", 1, 30);
	class:AddSkill("teleport", 50, 95);
	class:AddSkill("third attack", 30, 90);
	class:AddSkill("track", 35, 95);
	class:AddSkill("true sight", 48, 45);
	class:AddSkill("venomshield", 29, 95);
	class:AddSkill("weaken", 15, 95);
	class:AddSkill("word of recall", 20, 95);
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