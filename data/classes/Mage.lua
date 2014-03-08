-- MAGE.LUA
-- This is the Mage Class file for the MUD
-- Revised: 2014.03.07
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadClass()
	newClass = LCreateClass("Mage", 0);
	class.this = newClass;
	class.this:SetPrimaryAttribute("intelligence");
	class.this.Weapon = 10312;
	class.this.Guild = 3018;
	class.this.SkillAdept = 95;
	class.this.ToHitArmorClass0 = 18;
	class.this.ToHitArmorClass32 = 10;
	class.this.MinimumHealthGain = 6;
	class.this.MaximumHealthGain = 8;
	class.this.UseMana = true;
	class.this.BaseExperience = 1250;

	LoadClassSkills(class.this);
	LoadLanguageSkills(class.this);
end

function LoadClassSkills(class)
	class:AddSkill("acetum primus", 37, 95);
	class:AddSkill("acid blast", 20, 95);
	class:AddSkill("acid breath", 43, 95);
	class:AddSkill("aggressive style", 24, 50);
	class:AddSkill("aid", 10, 95);
	class:AddSkill("antimagic shell", 17, 95);
	class:AddSkill("aqua breath", 28, 95);
	class:AddSkill("armor", 5, 95);
	class:AddSkill("astral walk", 30, 95);
	class:AddSkill("berserk style", 42, 20);
	class:AddSkill("black fist", 23, 95);
	class:AddSkill("black hand", 2, 95);
	class:AddSkill("black lightning", 46, 95);
	class:AddSkill("blazebane", 30, 95);
	class:AddSkill("blazeward", 27, 95);
	class:AddSkill("blindness", 8, 95);
	class:AddSkill("bludgeons", 1, 50);
	class:AddSkill("brew", 25, 95);
	class:AddSkill("burning hands", 5, 95);
	class:AddSkill("caustic fount", 34, 95);
	class:AddSkill("charm person", 14, 95);
	class:AddSkill("chill touch", 3, 95);
	class:AddSkill("climb", 10, 95);
	class:AddSkill("colour spray", 11, 95);
	class:AddSkill("continual light", 4, 95);
	class:AddSkill("control weather", 10, 95);
	class:AddSkill("cook", 1, 95);
	class:AddSkill("create fire", 6, 95);
	class:AddSkill("create spring", 10, 95);
	class:AddSkill("cuff", 20, 60);
	class:AddSkill("curse", 12, 75);
	class:AddSkill("defensive style", 5, 75);
	class:AddSkill("demonskin", 20, 95);
	class:AddSkill("detect invis", 2, 95);
	class:AddSkill("detect magic", 2, 95);
	class:AddSkill("detect traps", 11, 85);
	class:AddSkill("dig", 2, 80);
	class:AddSkill("disenchant weapon", 12, 95);
	class:AddSkill("dispel magic", 11, 95);
	class:AddSkill("disruption", 8, 95);
	class:AddSkill("dodge", 30, 30);
	class:AddSkill("dragon wit", 9, 95);
	class:AddSkill("dragonskin", 16, 95);
	class:AddSkill("dream", 24, 95);
	class:AddSkill("eldritch sphere", 33, 95);
	class:AddSkill("elven beauty", 3, 95);
	class:AddSkill("enchant weapon", 12, 95);
	class:AddSkill("energy drain", 13, 95);
	class:AddSkill("enhanced damage", 44, 30);
	class:AddSkill("ethereal fist", 32, 95);
	class:AddSkill("ethereal funnel", 45, 95);
	class:AddSkill("ethereal shield", 40, 95);
	class:AddSkill("evasive style", 1, 80);
	class:AddSkill("extradimensional portal", 13, 95);
	class:AddSkill("faerie fire", 4, 95);
	class:AddSkill("faerie fog", 10, 95);
	class:AddSkill("farsight", 16, 95);
	class:AddSkill("fire breath", 44, 95);
	class:AddSkill("fireball", 15, 95);
	class:AddSkill("fireshield", 35, 95);
	class:AddSkill("flexible arms", 1, 60);
	class:AddSkill("float", 5, 95);
	class:AddSkill("fly", 7, 95);
	class:AddSkill("frost breath", 41, 95);
	class:AddSkill("galvanic whip", 4, 95);
	class:AddSkill("gas breath", 45, 95);
	class:AddSkill("hand of chaos", 37, 95);
	class:AddSkill("iceshield", 22, 95);
	class:AddSkill("identify", 10, 95);
	class:AddSkill("infravision", 6, 95);
	class:AddSkill("inner warmth", 24, 95);
	class:AddSkill("invis", 4, 95);
	class:AddSkill("kindred strength", 7, 95);
	class:AddSkill("knock", 32, 95);
	class:AddSkill("know alignment", 8, 95);
	class:AddSkill("lightning bolt", 9, 95);
	class:AddSkill("lightning breath", 42, 95);
	class:AddSkill("locate object", 6, 95);
	class:AddSkill("long blades", 2, 50);
	class:AddSkill("magic missile", 1, 95);
	class:AddSkill("magnetic thrust", 27, 95);
	class:AddSkill("mass invis", 15, 95);
	class:AddSkill("meditate", 16, 95);
	class:AddSkill("midas touch", 13, 95);
	class:AddSkill("mount", 7, 95);
	class:AddSkill("pass door", 18, 95);
	class:AddSkill("peek", 45, 25);
	class:AddSkill("portal", 21, 95);
	class:AddSkill("pugilism", 1, 50);
	class:AddSkill("quantum spike", 48, 95);
	class:AddSkill("razorbait", 18, 95);
	class:AddSkill("recharge", 33, 80);
	class:AddSkill("refresh", 5, 75);
	class:AddSkill("remove invis", 6, 95);
	class:AddSkill("remove trap", 20, 80);
	class:AddSkill("sagacity", 12, 95);
	class:AddSkill("scribe", 30, 100);
	class:AddSkill("scry", 15, 95);
	class:AddSkill("search", 8, 95);
	class:AddSkill("second attack", 10, 45);
	class:AddSkill("shadowform", 47, 95);
	class:AddSkill("shield", 13, 95);
	class:AddSkill("shocking grasp", 7, 95);
	class:AddSkill("shockshield", 45, 75);
	class:AddSkill("short blades", 1, 95);
	class:AddSkill("sleep", 14, 95);
	class:AddSkill("slink", 9, 95);
	class:AddSkill("sonic resonance", 19, 95);
	class:AddSkill("spectral furor", 14, 95);
	class:AddSkill("standard style", 1, 60);
	class:AddSkill("stone skin", 17, 95);
	class:AddSkill("strike", 15, 70);
	class:AddSkill("sulfurous spray", 18, 95);
	class:AddSkill("summon", 20, 95);
	class:AddSkill("swat", 10, 80);
	class:AddSkill("swordbait", 23, 95);
	class:AddSkill("talonous arms", 1, 70);
	class:AddSkill("teleport", 8, 95);
	class:AddSkill("third attack", 30, 30);
	class:AddSkill("trance", 32, 95);
	class:AddSkill("transport", 17, 95);
	class:AddSkill("trollish vigor", 6, 95);
	class:AddSkill("true sight", 43, 35);
	class:AddSkill("valiance", 14, 95);
	class:AddSkill("ventriloquate", 1, 95);
	class:AddSkill("weaken", 2, 95);
	class:AddSkill("winter mist", 26, 95);
	class:AddSkill("word of recall", 12, 95);
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