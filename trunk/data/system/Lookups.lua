-- Lookups.LUA
-- Lookup data for the MUD
-- Revised: 2014.02.18
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadPartMessages()
	LAddLookup("PartMessages", "$n\'s severed head plops from its neck.");
	LAddLookup("PartMessages", "$n\'s arm is sliced from $s dead body.");
	LAddLookup("PartMessages", "$n\'s leg is sliced from $s dead body.");
	LAddLookup("PartMessages", "$n\'s heart is torn from $s chest.");
	LAddLookup("PartMessages", "$n\'s brains spill grotesquely from $s head.");
	LAddLookup("PartMessages", "$n\'s guts spill grotesquely from $s torso.");
	LAddLookup("PartMessages", "$n\'s hand is sliced from $s dead body.");
	LAddLookup("PartMessages", "$n\'s foot is sliced from $s dead body.");
	LAddLookup("PartMessages", "A finger is sliced from $n\'s dead body.");
	LAddLookup("PartMessages", "$n\'s ear is sliced from $s dead body.");
	LAddLookup("PartMessages", "$n\'s eye is gouged from its socket.");
	LAddLookup("PartMessages", "$n\'s tongue is torn from $s mouth.");
	LAddLookup("PartMessages", "An eyestalk is sliced from $n\'s dead body.");
	LAddLookup("PartMessages", "A tentacle is severed from $n\'s dead body.");
	LAddLookup("PartMessages", "A fin is sliced from $n\'s dead body.");
	LAddLookup("PartMessages", "A wing is severed from $n\'s dead body.");
	LAddLookup("PartMessages", "$n\'s tail is sliced from $s dead body.");
	LAddLookup("PartMessages", "A scale falls from the body of $n.");
	LAddLookup("PartMessages", "A claw is torn from $n\'s dead body.");
	LAddLookup("PartMessages", "$n\'s fangs are torn from $s mouth.");
	LAddLookup("PartMessages", "A horn is wrenched from the body of $n.");
	LAddLookup("PartMessages", "$n\'s tusk is torn from $s dead body.");
	LAddLookup("PartMessages", "$n\'s tail is sliced from $s dead body.");
	LAddLookup("PartMessages", "A ridged scale falls from the body of $n.");
	LAddLookup("PartMessages", "$n\'s beak is sliced from $s dead body.");
	LAddLookup("PartMessages", "$n\'s haunches are sliced from $s dead body.");
	LAddLookup("PartMessages", "A hoof is sliced from $n\'s dead body.");
	LAddLookup("PartMessages", "A paw is sliced from $n\'s dead body.");
	LAddLookup("PartMessages", "$n\'s foreleg is sliced from $s dead body.");
	LAddLookup("PartMessages", "Some feathers fall from $n\'s dead body.");
	LAddLookup("PartMessages", "r1 message.");
	LAddLookup("PartMessages", "r2 message.");
end

function LoadObjectAffectStrings()
	LAddLookup("ObjectAffectStrings", "(Invis)");
	LAddLookup("ObjectAffectStrings", "(Red Aura)");
	LAddLookup("ObjectAffectStrings", "(Flaming Red)");
	LAddLookup("ObjectAffectStrings", "(Flaming Grey)");
	LAddLookup("ObjectAffectStrings", "(Flaming White)");
	LAddLookup("ObjectAffectStrings", "(Smouldering Red-Grey)");
	LAddLookup("ObjectAffectStrings", "(Smouldering Red-White)");
	LAddLookup("ObjectAffectStrings", "(Smouldering Grey-White)");
	LAddLookup("ObjectAffectStrings", "(Magical)");
	LAddLookup("ObjectAffectStrings", "(Glowing)");
	LAddLookup("ObjectAffectStrings", "(Humming)");
	LAddLookup("ObjectAffectStrings", "(Hidden)");
	LAddLookup("ObjectAffectStrings", "(Buried)");
	LAddLookup("ObjectAffectStrings", "(PROTO)");
	LAddLookup("ObjectAffectStrings", "(Trap)");
	LAddLookup("ObjectAffectStrings", "the faint glow of something");
	LAddLookup("ObjectAffectStrings", "You see the faint glow of something nearby.");
end

function LoadCorpseDescs()
	LAddLookup("CorpseDescs", "The corpse of %s is in the last stages of decay.");
	LAddLookup("CorpseDescs", "The corpse of %s is crawling with vermin.");
	LAddLookup("CorpseDescs", "The corpse of %s fills the air with a foul stench.");
	LAddLookup("CorpseDescs", "The corpse of %s is buzzing with flies.");
	LAddLookup("CorpseDescs", "The corpse of %s lies here.");
end

function LoadSpellFlags()
	LAddLookup("SpellFlags", "water");
	LAddLookup("SpellFlags", "earth");
	LAddLookup("SpellFlags", "air");
	LAddLookup("SpellFlags", "astral");
	LAddLookup("SpellFlags", "area");
	LAddLookup("SpellFlags", "distant");
	LAddLookup("SpellFlags", "reverse");
	LAddLookup("SpellFlags", "noself");
	LAddLookup("SpellFlags", "_unused2_");
	LAddLookup("SpellFlags", "accumulative");
	LAddLookup("SpellFlags", "recastable");
	LAddLookup("SpellFlags", "noscribe");
	LAddLookup("SpellFlags", "nobrew");
	LAddLookup("SpellFlags", "group");
	LAddLookup("SpellFlags", "object");
	LAddLookup("SpellFlags", "character");
	LAddLookup("SpellFlags", "secretskill");
	LAddLookup("SpellFlags", "pksensitive");
	LAddLookup("SpellFlags", "stoponfail");
	LAddLookup("SpellFlags", "nofight");
	LAddLookup("SpellFlags", "nodispel");
	LAddLookup("SpellFlags", "randomtarget");
	LAddLookup("SpellFlags", "r2");
	LAddLookup("SpellFlags", "r3");
	LAddLookup("SpellFlags", "r4");
	LAddLookup("SpellFlags", "r5");
	LAddLookup("SpellFlags", "r6");
	LAddLookup("SpellFlags", "r7");
	LAddLookup("SpellFlags", "r8");
	LAddLookup("SpellFlags", "r9");
	LAddLookup("SpellFlags", "r10");
	LAddLookup("SpellFlags", "r11");
end

function LoadSpellSaves()
	LAddLookup("SpellSaves", "none");
	LAddLookup("SpellSaves", "poison_death");
	LAddLookup("SpellSaves", "wands");
	LAddLookup("SpellSaves", "para_petri");
	LAddLookup("SpellSaves", "breath");
	LAddLookup("SpellSaves", "spell_staff");
end

function LoadSpellSaveEffects()
	LAddLookup("SpellSaveEffects", "none");
	LAddLookup("SpellSaveEffects", "negate");
	LAddLookup("SpellSaveEffects", "eightdam");
	LAddLookup("SpellSaveEffects", "quarterdam");
	LAddLookup("SpellSaveEffects", "halfdam");
	LAddLookup("SpellSaveEffects", "3qtrdam");
	LAddLookup("SpellSaveEffects", "reflect");
	LAddLookup("SpellSaveEffects", "absorb");
end

function LoadSpellDamageTypes()
	LAddLookup("SpellDamageTypes", "none");
	LAddLookup("SpellDamageTypes", "fire");
	LAddLookup("SpellDamageTypes", "cold");
	LAddLookup("SpellDamageTypes", "electricity");
	LAddLookup("SpellDamageTypes", "energy");
	LAddLookup("SpellDamageTypes", "acid");
	LAddLookup("SpellDamageTypes", "poison");
	LAddLookup("SpellDamageTypes", "drain");
end

function LoadSpellActionTypes()
	LAddLookup("SpellActionTypes", "none");
	LAddLookup("SpellActionTypes", "create");
	LAddLookup("SpellActionTypes", "destroy");
	LAddLookup("SpellActionTypes", "resist");
	LAddLookup("SpellActionTypes", "suscept");
	LAddLookup("SpellActionTypes", "divinate");
	LAddLookup("SpellActionTypes", "obscure");
	LAddLookup("SpellActionTypes", "change");
end

function LoadSpellPowerTypes()
	LAddLookup("SpellPowerTypes", "none");
	LAddLookup("SpellPowerTypes", "minor");
	LAddLookup("SpellPowerTypes", "greater");
	LAddLookup("SpellPowerTypes", "major");
end

function LoadSpellClassTypes()
	LAddLookup("SpellClassTypes", "none");
	LAddLookup("SpellClassTypes", "lunar");
	LAddLookup("SpellClassTypes", "solar");
	LAddLookup("SpellClassTypes", "travel");
	LAddLookup("SpellClassTypes", "summon");
	LAddLookup("SpellClassTypes", "life");
	LAddLookup("SpellClassTypes", "death");
	LAddLookup("SpellClassTypes", "illusion");
end

function LoadTargetTypes()
	LAddLookup("TargetTypes", "ignore");
	LAddLookup("TargetTypes", "offensive");
	LAddLookup("TargetTypes", "defensive");
	LAddLookup("TargetTypes", "self");
	LAddLookup("TargetTypes", "objinv");
end

function LoadLiquidTypes()
	LAddLookup("LiquidTypes", "Beverage");
	LAddLookup("LiquidTypes", "Alcohol");
	LAddLookup("LiquidTypes", "Poison");
	LAddLookup("LiquidTypes", "Blood");
end

function LoadModTypes()
	LAddLookup("ModTypes", "Drunk");
	LAddLookup("ModTypes", "Full");
	LAddLookup("ModTypes", "Thirst");
	LAddLookup("ModTypes", "Bloodthirst");
end

function LoadValidColors()
	LAddLookup("ValidColors", "black");
	LAddLookup("ValidColors", "dred");
	LAddLookup("ValidColors", "dgreen");
	LAddLookup("ValidColors", "orange");
	LAddLookup("ValidColors", "dblue");
	LAddLookup("ValidColors", "purple");
	LAddLookup("ValidColors", "cyan");
	LAddLookup("ValidColors", "grey");
	LAddLookup("ValidColors", "dgrey");
	LAddLookup("ValidColors", "red");
	LAddLookup("ValidColors", "green");
	LAddLookup("ValidColors", "yellow");
	LAddLookup("ValidColors", "blue");
	LAddLookup("ValidColors", "pink");
	LAddLookup("ValidColors", "lblue");
	LAddLookup("ValidColors", "white");
end

function LoadDayNames()
	LAddLookup("DayNames", "the Moon");
	LAddLookup("DayNames", "the Bull");
	LAddLookup("DayNames", "Deception");
	LAddLookup("DayNames", "Thunder");
	LAddLookup("DayNames", "Freedom");
	LAddLookup("DayNames", "the Great Gods");
	LAddLookup("DayNames", "the Sun");
end

function LoadMonthNames()
	LAddLookup("MonthNames", "Winter");
	LAddLookup("MonthNames", "the Winter Wolf");
	LAddLookup("MonthNames", "the Frost Giant");
	LAddLookup("MonthNames", "the Old Forces");
	LAddLookup("MonthNames", "the Grand Struggle");
	LAddLookup("MonthNames", "the Spring");
	LAddLookup("MonthNames", "Nature");
	LAddLookup("MonthNames", "Futility");
	LAddLookup("MonthNames", "the Dragon");
	LAddLookup("MonthNames", "the Sun");
	LAddLookup("MonthNames", "the Heat");
	LAddLookup("MonthNames", "the Battle");
	LAddLookup("MonthNames", "the Dark Shades");
	LAddLookup("MonthNames", "the Shadows");
	LAddLookup("MonthNames", "the Long Shadows");
	LAddLookup("MonthNames", "the Ancient Darkness");
	LAddLookup("MonthNames", "the Great Evil");
end

function LoadSeasonNames()
	LAddLookup("SeasonNames", "&gspring");
	LAddLookup("SeasonNames", "&ysummer");
	LAddLookup("SeasonNames", "&Oautumn");
	LAddLookup("SeasonNames", "&Cwinter");
end

LoadPartMessages();
LoadObjectAffectStrings();
LoadCorpseDescs();
LoadSpellFlags();
LoadSpellSaves();
LoadSpellSaveEffects();
LoadSpellDamageTypes();
LoadSpellActionTypes();
LoadSpellPowerTypes();
LoadSpellClassTypes();
LoadTargetTypes();
LoadLiquidTypes();
LoadModTypes();
LoadValidColors();
LoadDayNames();
LoadMonthNames();
LoadSeasonNames();

-- EOF
