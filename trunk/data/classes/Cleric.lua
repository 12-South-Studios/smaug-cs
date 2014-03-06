-- CLERIC.LUA
-- This is the Cleric Class file for the MUD
-- Revised: 2014.03.05
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadClass()
	newClass = LCreateClass("Cleric", 1);
	class.this = newClass;
	class.this:SetPrimaryAttribute("wisdom");
	class.this.Weapon = 10315;
	class.this.Guild = 3003;
	class.this.SkillAdept = 95;
	class.this.ToHitArmorClass0 = 18;
	class.this.ToHitArmorClass32 = 12;
	class.this.MinimumHealthGain = 7;
	class.this.MaximumHealthGain = 10;
	class.this.UseMana = true;
	class.this.BaseExperience = 900;

	LoadClassSkills(class.this);
	LoadLanguageSkills(class.this);
end

function LoadClassSkills(class)
	class:AddSkill("aggressive style", 20, 50);
	class:AddSkill("aid", 4, 95);
	class:AddSkill("alertness", 23, 92);
	class:AddSkill("animate dead", 42, 85);
	class:AddSkill("antimagic shell", 42, 70);
	class:AddSkill("aqua breath", 22, 95);
	class:AddSkill("armor", 1, 95);
	class:AddSkill("benediction", 19, 95);
	class:AddSkill("berserk style", 40, 20);
	class:AddSkill("bless", 5, 95);
	class:AddSkill("blindness", 5, 95);
	class:AddSkill("bludgeons", 1, 95);
	class:AddSkill("brew", 40, 70);
	class:AddSkill("call lightning", 12, 80);
	class:AddSkill("cause critical", 9, 95);
	class:AddSkill("cause light", 1, 95);
	class:AddSkill("cause serious", 5, 95);
	class:AddSkill("charged beacon", 28, 95);
	class:AddSkill("climb", 9, 95);
	class:AddSkill("continual light", 2, 95);
	class:AddSkill("control weather", 13, 95);
	class:AddSkill("cook", 1, 95);
	class:AddSkill("create food", 3, 95);
	class:AddSkill("create spring", 10, 95);
	class:AddSkill("create water", 2, 95);
	class:AddSkill("cuff", 15, 80);
	class:AddSkill("cure blindness", 4, 95);
	class:AddSkill("cure critical", 9, 95);
	class:AddSkill("cure light", 1, 95);
	class:AddSkill("cure poison", 9, 90);
	class:AddSkill("cure serious", 5, 95);
	class:AddSkill("curse", 12, 95);
	class:AddSkill("defensive style", 5, 75);
	class:AddSkill("detect evil", 4, 95);
	class:AddSkill("detect hidden", 7, 95);
	class:AddSkill("detect invis", 5, 95);
	class:AddSkill("detect magic", 3, 95);
	class:AddSkill("detect poison", 5, 95);
	class:AddSkill("detect traps", 7, 75);
	class:AddSkill("dig", 2, 85);
	class:AddSkill("dispel evil", 10, 95);
	class:AddSkill("dispel magic", 16, 95);
	class:AddSkill("divinity", 42, 95);
	class:AddSkill("dodge", 25, 50);
	class:AddSkill("dream", 22, 95);
	class:AddSkill("earthquake", 7, 95);
	class:AddSkill("ethereal funnel", 50, 95);
	class:AddSkill("evasive style", 1, 80);
	class:AddSkill("faerie fire", 2, 95);
	class:AddSkill("faerie fog", 14, 95);
	class:AddSkill("fatigue", 23, 92);
	class:AddSkill("fireshield", 28, 95);
	class:AddSkill("flamestrike", 13, 95);
	class:AddSkill("flexible arms", 1, 60);
	class:AddSkill("float", 8, 95);
	class:AddSkill("fly", 12, 95);
	class:AddSkill("fortify", 15, 95);
	class:AddSkill("grounding", 28, 95);
	class:AddSkill("harm", 15, 95);
	class:AddSkill("heal", 14, 95);
	class:AddSkill("holy sanctity", 45, 95);
	class:AddSkill("identify", 10, 95);
	class:AddSkill("indignation", 27, 95);
	class:AddSkill("infravision", 9, 80);
	class:AddSkill("knock", 44, 70);
	class:AddSkill("know alignment", 5, 95);
	class:AddSkill("lethargy", 12, 95);
	class:AddSkill("locate object", 10, 95);
	class:AddSkill("major invocation", 18, 95);
	class:AddSkill("mass invis", 17, 95);
	class:AddSkill("meditate", 14, 95);
	class:AddSkill("midas touch", 21, 95);
	class:AddSkill("minor invocation", 10, 95);
	class:AddSkill("mount", 5, 95);
	class:AddSkill("necromantic touch", 40, 95);
	class:AddSkill("poison", 8, 95);
	class:AddSkill("protection", 6, 95);
	class:AddSkill("pugilism", 1, 50);
	class:AddSkill("recharge", 44, 65);
	class:AddSkill("refresh", 3, 95);
	class:AddSkill("remove curse", 12, 95);
	class:AddSkill("remove trap", 15, 70);
	class:AddSkill("rescue", 33, 20);
	class:AddSkill("resilience", 40, 95);
	class:AddSkill("restoration", 30, 95);
	class:AddSkill("sanctuary", 13, 95);
	class:AddSkill("scribe", 48, 60);
	class:AddSkill("scry", 21, 95);
	class:AddSkill("search", 6, 95);
	class:AddSkill("second attack", 15, 50);
	class:AddSkill("shockshield", 35, 95);
	class:AddSkill("short blades", 1, 60);
	class:AddSkill("solar flight", 39, 80);
	class:AddSkill("spiritual wrath", 48, 95);
	class:AddSkill("standard style", 1, 60);
	class:AddSkill("summon", 8, 95);
	class:AddSkill("swat", 20, 80);
	class:AddSkill("talonous arms", 1, 50);
	class:AddSkill("trance", 41, 95);
	class:AddSkill("transport", 25, 95);
	class:AddSkill("true sight", 38, 95);
	class:AddSkill("unravel defense", 33, 95);
	class:AddSkill("uplift", 37, 95);
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

LoadClass();

-- EOF