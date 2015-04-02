-- Lookups.LUA
-- Lookup data for the MUD
-- Revised: 2014.03.03
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
LoadPartMessages();

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
LoadObjectAffectStrings();

function LoadCorpseDescs()
	LAddLookup("CorpseDescs", "The corpse of %s is in the last stages of decay.");
	LAddLookup("CorpseDescs", "The corpse of %s is crawling with vermin.");
	LAddLookup("CorpseDescs", "The corpse of %s fills the air with a foul stench.");
	LAddLookup("CorpseDescs", "The corpse of %s is buzzing with flies.");
	LAddLookup("CorpseDescs", "The corpse of %s lies here.");
end
LoadCorpseDescs();

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
LoadSpellFlags();

function LoadSpellSaves()
	LAddLookup("SpellSaves", "none");
	LAddLookup("SpellSaves", "poison_death");
	LAddLookup("SpellSaves", "wands");
	LAddLookup("SpellSaves", "para_petri");
	LAddLookup("SpellSaves", "breath");
	LAddLookup("SpellSaves", "spell_staff");
end
LoadSpellSaves();

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
LoadSpellSaveEffects();

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
LoadSpellDamageTypes();

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
LoadSpellActionTypes();

function LoadSpellPowerTypes()
	LAddLookup("SpellPowerTypes", "none");
	LAddLookup("SpellPowerTypes", "minor");
	LAddLookup("SpellPowerTypes", "greater");
	LAddLookup("SpellPowerTypes", "major");
end
LoadSpellPowerTypes();

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
LoadSpellClassTypes();

function LoadTargetTypes()
	LAddLookup("TargetTypes", "ignore");
	LAddLookup("TargetTypes", "offensive");
	LAddLookup("TargetTypes", "defensive");
	LAddLookup("TargetTypes", "self");
	LAddLookup("TargetTypes", "objinv");
end
LoadTargetTypes();

function LoadLiquidTypes()
	LAddLookup("LiquidTypes", "Beverage");
	LAddLookup("LiquidTypes", "Alcohol");
	LAddLookup("LiquidTypes", "Poison");
	LAddLookup("LiquidTypes", "Blood");
end
LoadLiquidTypes();

function LoadModTypes()
	LAddLookup("ModTypes", "Drunk");
	LAddLookup("ModTypes", "Full");
	LAddLookup("ModTypes", "Thirst");
	LAddLookup("ModTypes", "Bloodthirst");
end
LoadModTypes();

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
LoadValidColors();

function LoadDayNames()
	LAddLookup("DayNames", "the Moon");
	LAddLookup("DayNames", "the Bull");
	LAddLookup("DayNames", "Deception");
	LAddLookup("DayNames", "Thunder");
	LAddLookup("DayNames", "Freedom");
	LAddLookup("DayNames", "the Great Gods");
	LAddLookup("DayNames", "the Sun");
end
LoadDayNames();

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
LoadMonthNames();

function LoadSeasonNames()
	LAddLookup("SeasonNames", "&gspring");
	LAddLookup("SeasonNames", "&ysummer");
	LAddLookup("SeasonNames", "&Oautumn");
	LAddLookup("SeasonNames", "&Cwinter");
end
LoadSeasonNames();

function LoadWhereNames()
	LAddLookup("WhereNames", "<used as light>     ");
	LAddLookup("WhereNames", "<worn on finger>    ");
	LAddLookup("WhereNames", "<worn on finger>    ");
	LAddLookup("WhereNames", "<worn around neck>  ");
	LAddLookup("WhereNames", "<worn around neck>  ");
	LAddLookup("WhereNames", "<worn on body>      ");
	LAddLookup("WhereNames", "<worn on head>      ");
	LAddLookup("WhereNames", "<worn on legs>      ");
	LAddLookup("WhereNames", "<worn on feet>      ");
	LAddLookup("WhereNames", "<worn on hands>     ");
	LAddLookup("WhereNames", "<worn on arms>      ");
	LAddLookup("WhereNames", "<worn as shield>    ");
	LAddLookup("WhereNames", "<worn about body>   ");
	LAddLookup("WhereNames", "<worn about waist>  ");
	LAddLookup("WhereNames", "<worn around wrist> ");
	LAddLookup("WhereNames", "<worn around wrist> ");
	LAddLookup("WhereNames", "<wielded>           ");
	LAddLookup("WhereNames", "<held>              ");
	LAddLookup("WhereNames", "<dual wielded>      ");
	LAddLookup("WhereNames", "<worn on ears>      ");
	LAddLookup("WhereNames", "<worn on eyes>      ");
	LAddLookup("WhereNames", "<missile wielded>   ");
	LAddLookup("WhereNames", "<worn on back>  ");
	LAddLookup("WhereNames", "<worn over face>  ");
	LAddLookup("WhereNames", "<worn around ankle>  ");
	LAddLookup("WhereNames", "<worn around ankle>  ");
end
LoadWhereNames();
		
function LoadAttackTypes()
	LAddLookup("AttackTypes", "hit");
	LAddLookup("AttackTypes", "slice");
	LAddLookup("AttackTypes", "stab");
	LAddLookup("AttackTypes", "slash");
	LAddLookup("AttackTypes", "whip");
	LAddLookup("AttackTypes", "claw");
	LAddLookup("AttackTypes", "blast");
	LAddLookup("AttackTypes", "pound");
	LAddLookup("AttackTypes", "crush");
	LAddLookup("AttackTypes", "grep");
	LAddLookup("AttackTypes", "bite");
	LAddLookup("AttackTypes", "pierce");
	LAddLookup("AttackTypes", "suction");
	LAddLookup("AttackTypes", "bolt");
	LAddLookup("AttackTypes", "arrow");
	LAddLookup("AttackTypes", "dart");
	LAddLookup("AttackTypes", "stone");
	LAddLookup("AttackTypes", "pea");
end
LoadAttackTypes();

function LoadDirectionNames()
	LAddLookup("DirectionNames", "north");
	LAddLookup("DirectionNames", "east");
	LAddLookup("DirectionNames", "south");
	LAddLookup("DirectionNames", "west");
	LAddLookup("DirectionNames", "up");
	LAddLookup("DirectionNames", "down");
	LAddLookup("DirectionNames", "northeast");
	LAddLookup("DirectionNames", "northwest");
	LAddLookup("DirectionNames", "southeast");
	LAddLookup("DirectionNames", "southwest");
	LAddLookup("DirectionNames", "somewhere");
end
LoadDirectionNames();

function LoadReverseDirectionNames()
	LAddLookup("ReverseDirectionNames", "the south");
	LAddLookup("ReverseDirectionNames", "the west");
	LAddLookup("ReverseDirectionNames", "the north");
	LAddLookup("ReverseDirectionNames", "the east");
	LAddLookup("ReverseDirectionNames", "below");
	LAddLookup("ReverseDirectionNames", "above");
	LAddLookup("ReverseDirectionNames", "the southwest");
	LAddLookup("ReverseDirectionNames", "the southeast");
	LAddLookup("ReverseDirectionNames", "the northwest");
	LAddLookup("ReverseDirectionNames", "the northeast");
	LAddLookup("ReverseDirectionNames", "somewhere");
end
LoadReverseDirectionNames();

function LoadHallucinatedObjectShortNames()
	LAddLookup("HallucinatedShortNames", "a sword");
	LAddLookup("HallucinatedShortNames", "a stick");
	LAddLookup("HallucinatedShortNames", "something shiny");
	LAddLookup("HallucinatedShortNames", "something");
	LAddLookup("HallucinatedShortNames", "something interesting");
	LAddLookup("HallucinatedShortNames", "something colorful");
	LAddLookup("HallucinatedShortNames", "something that looks cool");
	LAddLookup("HallucinatedShortNames", "a nifty thing");
	LAddLookup("HallucinatedShortNames", "a cloak of flowing colors");
	LAddLookup("HallucinatedShortNames", "a mystical flaming sword");
	LAddLookup("HallucinatedShortNames", "a swarm of insects");
	LAddLookup("HallucinatedShortNames", "a deathbane");
	LAddLookup("HallucinatedShortNames", "a figment of your imagination");
	LAddLookup("HallucinatedShortNames", "your gravestone");
	LAddLookup("HallucinatedShortNames", "the long lost boots of Ranger Thoric");
	LAddLookup("HallucinatedShortNames", "a glowing tome of arcane knowledge");
	LAddLookup("HallucinatedShortNames", "a long sough secret");
	LAddLookup("HallucinatedShortNames", "the meaning of it all");
	LAddLookup("HallucinatedShortNames", "the answer");
	LAddLookup("HallucinatedShortNames", "the key to life, the universe and everything");
end
LoadHallucinatedObjectShortNames();

function LoadHallucinatedObjectLongNames()
	LAddLookup("HallucinatedLongNames", "A nice looking sword catches your eye.");
	LAddLookup("HallucinatedLongNames", "The ground is covered in small sticks.");
	LAddLookup("HallucinatedLongNames", "Something shiny catches your eye.");
	LAddLookup("HallucinatedLongNames", "Something catches your attention.");
	LAddLookup("HallucinatedLongNames", "Something interesting catches your eye.");
	LAddLookup("HallucinatedLongNames", "Something colorful flows by.");
	LAddLookup("HallucinatedLongNames", "Something that looks cool calls out to you.");
	LAddLookup("HallucinatedLongNames", "A nifty thing of great importance stands here.");
	LAddLookup("HallucinatedLongNames", "A cloak of flowing colors asks you to wear it.");
	LAddLookup("HallucinatedLongNames", "A mystical flaming sowrd awaits your grasp.");
	LAddLookup("HallucinatedLongNames", "A swarm of insects buzzes in your face!");
	LAddLookup("HallucinatedLongNames", "The extremely rare Deathbane lies at your feet.");
	LAddLookup("HallucinatedLongNames", "A figment of your imagination is at your command.");
	LAddLookup("HallucinatedLongNames", "You notice a gravestone here... upon closer examination, it reads your name.");
	LAddLookup("HallucinatedLongNames", "The long lost boots of Ranger Thoric lie off to the side.");
	LAddLookup("HallucinatedLongNames", "A glowing tome of arcane knowledge hovers in the air before you.");
	LAddLookup("HallucinatedLongNames", "A long sough secret of all mankind is now clear to you.");
	LAddLookup("HallucinatedLongNames", "The meaning of it all, so simple, so clear... of course!");
	LAddLookup("HallucinatedLongNames", "The answer.  One.  It's always been One.");
	LAddLookup("HallucinatedLongNames", "The key to life, the universe and everything awaits your hand.");
end
LoadHallucinatedObjectLongNames();

function LoadNPCRaces()
	LAddLookup("NPCRaces", "human");
	LAddLookup("NPCRaces", "elf");
	LAddLookup("NPCRaces", "dwarf");
	LAddLookup("NPCRaces", "halfling");
	LAddLookup("NPCRaces", "pixie");
	LAddLookup("NPCRaces", "vampire");
	LAddLookup("NPCRaces", "half-ogre");
	LAddLookup("NPCRaces", "half-orc");
	LAddLookup("NPCRaces", "half-troll");
	LAddLookup("NPCRaces", "half-elf");
	LAddLookup("NPCRaces", "gith");
	LAddLookup("NPCRaces", "drow");
	LAddLookup("NPCRaces", "sea-elf");
	LAddLookup("NPCRaces", "lizardman");
	LAddLookup("NPCRaces", "gnome");
	LAddLookup("NPCRaces", "r5");		-- unused
	LAddLookup("NPCRaces", "r6");		-- unused
	LAddLookup("NPCRaces", "r7");		-- unused
	LAddLookup("NPCRaces", "r8");		-- unused
	LAddLookup("NPCRaces", "troll");
	LAddLookup("NPCRaces", "ant");
	LAddLookup("NPCRaces", "ape");
	LAddLookup("NPCRaces", "baboon");
	LAddLookup("NPCRaces", "bat");
	LAddLookup("NPCRaces", "bear");
	LAddLookup("NPCRaces", "bee");
	LAddLookup("NPCRaces", "beetle");
	LAddLookup("NPCRaces", "boar");
	LAddLookup("NPCRaces", "bugbear");
	LAddLookup("NPCRaces", "cat");
	LAddLookup("NPCRaces", "dog");
	LAddLookup("NPCRaces", "dragon");
	LAddLookup("NPCRaces", "ferret");
	LAddLookup("NPCRaces", "fly");
	LAddLookup("NPCRaces", "gargoyle");
	LAddLookup("NPCRaces", "gelatin");
	LAddLookup("NPCRaces", "ghoul");
	LAddLookup("NPCRaces", "gnoll");
	LAddLookup("NPCRaces", "gnome");	-- duplicate
	LAddLookup("NPCRaces", "goblin");
	LAddLookup("NPCRaces", "golem");
	LAddLookup("NPCRaces", "gorgon");
	LAddLookup("NPCRaces", "harpy");
	LAddLookup("NPCRaces", "hobgoblin");
	LAddLookup("NPCRaces", "kobold");
	LAddLookup("NPCRaces", "lizardman");	-- duplicate
	LAddLookup("NPCRaces", "locust");
	LAddLookup("NPCRaces", "lycanthrope");
	LAddLookup("NPCRaces", "minotaur");
	LAddLookup("NPCRaces", "mold");
	LAddLookup("NPCRaces", "mule");
	LAddLookup("NPCRaces", "neanderthal");
	LAddLookup("NPCRaces", "ooze");
	LAddLookup("NPCRaces", "orc");
	LAddLookup("NPCRaces", "rat");
	LAddLookup("NPCRaces", "rustmonster");
	LAddLookup("NPCRaces", "shadow");
	LAddLookup("NPCRaces", "shapeshifter");
	LAddLookup("NPCRaces", "shrew");
	LAddLookup("NPCRaces", "shrieker");
	LAddLookup("NPCRaces", "skeleton");
	LAddLookup("NPCRaces", "slime");
	LAddLookup("NPCRaces", "snake");
	LAddLookup("NPCRaces", "spider");
	LAddLookup("NPCRaces", "stirge");
	LAddLookup("NPCRaces", "thoul");
	LAddLookup("NPCRaces", "troglodyte");
	LAddLookup("NPCRaces", "undead");
	LAddLookup("NPCRaces", "wight");
	LAddLookup("NPCRaces", "wolf");
	LAddLookup("NPCRaces", "worm");
	LAddLookup("NPCRaces", "zombie");
	LAddLookup("NPCRaces", "bovine");	-- uh cow?
	LAddLookup("NPCRaces", "canine");	-- uh dog?
	LAddLookup("NPCRaces", "feline");		-- uh cat?
	LAddLookup("NPCRaces", "porcine");
	LAddLookup("NPCRaces", "mammal");
	LAddLookup("NPCRaces", "rodent");
	LAddLookup("NPCRaces", "avis");
	LAddLookup("NPCRaces", "reptile");
	LAddLookup("NPCRaces", "amphibian");
	LAddLookup("NPCRaces", "fish");
	LAddLookup("NPCRaces", "crustacean");
	LAddLookup("NPCRaces", "insect");
	LAddLookup("NPCRaces", "spirit");
	LAddLookup("NPCRaces", "magical");
	LAddLookup("NPCRaces", "horse");
	LAddLookup("NPCRaces", "animal");
	LAddLookup("NPCRaces", "humanoid");
	LAddLookup("NPCRaces", "monster");
	LAddLookup("NPCRaces", "god");
end
LoadNPCRaces();

function LoadNPCClasses()
	LAddLookup("NPCClasses", "mage");
	LAddLookup("NPCClasses", "cleric");
	LAddLookup("NPCClasses", "thief");
	LAddLookup("NPCClasses", "warrior");
	LAddLookup("NPCClasses", "vampire");
	LAddLookup("NPCClasses", "druid");
	LAddLookup("NPCClasses", "ranger");
	LAddLookup("NPCClasses", "augurer");
	LAddLookup("NPCClasses", "paladin");
	LAddLookup("NPCClasses", "nephandi");
	LAddLookup("NPCClasses", "savage");
	LAddLookup("NPCClasses", "pirate");
	LAddLookup("NPCClasses", "pc12");		-- unused
	LAddLookup("NPCClasses", "pc13");		-- unused
	LAddLookup("NPCClasses", "pc14");		-- unused
	LAddLookup("NPCClasses", "pc15");		-- unused
	LAddLookup("NPCClasses", "pc16");		-- unused
	LAddLookup("NPCClasses", "pc17");		-- unused
	LAddLookup("NPCClasses", "pc18");		-- unused
	LAddLookup("NPCClasses", "pc19");		-- unused
	LAddLookup("NPCClasses", "baker");
	LAddLookup("NPCClasses", "butcher");
	LAddLookup("NPCClasses", "blacksmith");
	LAddLookup("NPCClasses", "mayor");
	LAddLookup("NPCClasses", "king");
	LAddLookup("NPCClasses", "queen");
end
LoadNPCClasses();

function LoadSlashBladeMessages()
	LAddLookup("SlashBladeMessages", "miss");
	LAddLookup("SlashBladeMessages", "barely scratch");
	LAddLookup("SlashBladeMessages", "scratch");
	LAddLookup("SlashBladeMessages", "nick");
	LAddLookup("SlashBladeMessages", "cut");
	LAddLookup("SlashBladeMessages", "hit");
	LAddLookup("SlashBladeMessages", "tear");
	LAddLookup("SlashBladeMessages", "rip");
	LAddLookup("SlashBladeMessages", "gash");
	LAddLookup("SlashBladeMessages", "lacerate");
	LAddLookup("SlashBladeMessages", "hack");
	LAddLookup("SlashBladeMessages", "maul");
	LAddLookup("SlashBladeMessages", "rend");
	LAddLookup("SlashBladeMessages", "decimate");
	LAddLookup("SlashBladeMessages", "_mangle_");
	LAddLookup("SlashBladeMessages", "_devastate_");
	LAddLookup("SlashBladeMessages", "_cleave_");
	LAddLookup("SlashBladeMessages", "_butcher_");
	LAddLookup("SlashBladeMessages", "DISEMBOWEL");
	LAddLookup("SlashBladeMessages", "DISFIGURE");
	LAddLookup("SlashBladeMessages", "GUT");
	LAddLookup("SlashBladeMessages", "EVISCERATE");
	LAddLookup("SlashBladeMessages", "* SLAUGHTER *");
	LAddLookup("SlashBladeMessages", "*** ANNIHILATE ***");
end
LoadSlashBladeMessages();		

function LoadPierceBladeMessages()
	LAddLookup("PierceBladeMessages", "misses");
	LAddLookup("PierceBladeMessages", "barely scratches");
	LAddLookup("PierceBladeMessages", "scratches");
	LAddLookup("PierceBladeMessages", "nicks");
	LAddLookup("PierceBladeMessages", "cuts");
	LAddLookup("PierceBladeMessages", "hits");
	LAddLookup("PierceBladeMessages", "tears");
	LAddLookup("PierceBladeMessages", "rips");
	LAddLookup("PierceBladeMessages", "gashes");
	LAddLookup("PierceBladeMessages", "lacerates");
	LAddLookup("PierceBladeMessages", "hacks");
	LAddLookup("PierceBladeMessages", "mauls");
	LAddLookup("PierceBladeMessages", "rends");
	LAddLookup("PierceBladeMessages", "decimates");
	LAddLookup("PierceBladeMessages", "_mangles_");
	LAddLookup("PierceBladeMessages", "_devastates_");
	LAddLookup("PierceBladeMessages", "_cleaves_");
	LAddLookup("PierceBladeMessages", "_butchers_");
	LAddLookup("PierceBladeMessages", "DISEMBOWELS");
	LAddLookup("PierceBladeMessages", "DISFIGURES");
	LAddLookup("PierceBladeMessages", "GUTS");
	LAddLookup("PierceBladeMessages", "EVISCERATES");
	LAddLookup("PierceBladeMessages", "* SLAUGHTERS *");
	LAddLookup("PierceBladeMessages", "*** ANNIHILATES ***");
end
LoadPierceBladeMessages();

function LoadSlashBluntMessages()
	LAddLookup("SlashBluntMessages", "miss");
	LAddLookup("SlashBluntMessages", "barely scuff");
	LAddLookup("SlashBluntMessages", "scuff");
	LAddLookup("SlashBluntMessages", "pelt");
	LAddLookup("SlashBluntMessages", "bruise");
	LAddLookup("SlashBluntMessages", "strike");
	LAddLookup("SlashBluntMessages", "thrash");
	LAddLookup("SlashBluntMessages", "batter");
	LAddLookup("SlashBluntMessages", "flog");
	LAddLookup("SlashBluntMessages", "pummel");
	LAddLookup("SlashBluntMessages", "smash");
	LAddLookup("SlashBluntMessages", "maul");
	LAddLookup("SlashBluntMessages", "bludgeon");
	LAddLookup("SlashBluntMessages", "decimate");
	LAddLookup("SlashBluntMessages", "_shatter_");
	LAddLookup("SlashBluntMessages", "_devastate_");
	LAddLookup("SlashBluntMessages", "_maim_");
	LAddLookup("SlashBluntMessages", "_cripple_");
	LAddLookup("SlashBluntMessages", "MUTILATE");
	LAddLookup("SlashBluntMessages", "DISFIGURE");
	LAddLookup("SlashBluntMessages", "MASSACRE");
	LAddLookup("SlashBluntMessages", "PULVERIZE");
	LAddLookup("SlashBluntMessages", "* OBLITERATE *");
	LAddLookup("SlashBluntMessages", "*** ANNIHILATE ***");
end
LoadSlashBluntMessages();

function LoadPierceBluntMessages()
    LAddLookup("PierceBluntMessages", "misses");
    LAddLookup("PierceBluntMessages", "barely scuffs");
    LAddLookup("PierceBluntMessages", "scuffs");
    LAddLookup("PierceBluntMessages", "pelts");
    LAddLookup("PierceBluntMessages", "bruises");
    LAddLookup("PierceBluntMessages", "strikes");
    LAddLookup("PierceBluntMessages", "thrashes");
    LAddLookup("PierceBluntMessages", "batters");
    LAddLookup("PierceBluntMessages", "flogs");
    LAddLookup("PierceBluntMessages", "pummels");
    LAddLookup("PierceBluntMessages", "smashes");
    LAddLookup("PierceBluntMessages", "mauls");
    LAddLookup("PierceBluntMessages", "bludgeons");
    LAddLookup("PierceBluntMessages", "decimates");
    LAddLookup("PierceBluntMessages", "_shatters");
    LAddLookup("PierceBluntMessages", "_devastates");
    LAddLookup("PierceBluntMessages", "_maims_");
    LAddLookup("PierceBluntMessages", "_cripples_");
    LAddLookup("PierceBluntMessages", "MUTILATES");
    LAddLookup("PierceBluntMessages", "DISFIGURES");
    LAddLookup("PierceBluntMessages", "MASSACRES");
    LAddLookup("PierceBluntMessages", "PULVERIZES");
    LAddLookup("PierceBluntMessages", "* OBLIERATES *");
    LAddLookup("PierceBluntMessages", "*** ANNIHILATES ***");
end
LoadPierceBluntMessages();

function LoadSlashGenericMessages()
    LAddLookup("SlashGenericMessages", "miss");
    LAddLookup("SlashGenericMessages", "brush");
    LAddLookup("SlashGenericMessages", "scratch");
    LAddLookup("SlashGenericMessages", "graze");
    LAddLookup("SlashGenericMessages", "nick");
    LAddLookup("SlashGenericMessages", "jolt");
    LAddLookup("SlashGenericMessages", "wound");
    LAddLookup("SlashGenericMessages", "injure");
    LAddLookup("SlashGenericMessages", "hit");
    LAddLookup("SlashGenericMessages", "jar");
    LAddLookup("SlashGenericMessages", "thrash");
    LAddLookup("SlashGenericMessages", "maul");
    LAddLookup("SlashGenericMessages", "decimate");
    LAddLookup("SlashGenericMessages", "_traumatize_");
    LAddLookup("SlashGenericMessages", "_devastate_");
    LAddLookup("SlashGenericMessages", "_maim__");
    LAddLookup("SlashGenericMessages", "_demolish_");
    LAddLookup("SlashGenericMessages", "MUTILATE");
    LAddLookup("SlashGenericMessages", "MASSACRE");
    LAddLookup("SlashGenericMessages", "PULVERIZE");
    LAddLookup("SlashGenericMessages", "DESTROY");
    LAddLookup("SlashGenericMessages", "* OBLITERATE *");
    LAddLookup("SlashGenericMessages", "*** ANNIHILATE ***");
    LAddLookup("SlashGenericMessages", "**** SMITE ****");
end
LoadSlashGenericMessages();

function LoadPierceGenericMessages()
    LAddLookup("PierceGenericMessages", "misses");
    LAddLookup("PierceGenericMessages", "brushes");
    LAddLookup("PierceGenericMessages", "scratches");
    LAddLookup("PierceGenericMessages", "grazes");
    LAddLookup("PierceGenericMessages", "nicks");
    LAddLookup("PierceGenericMessages", "jolts");
    LAddLookup("PierceGenericMessages", "wounds");
    LAddLookup("PierceGenericMessages", "injures");
    LAddLookup("PierceGenericMessages", "hits");
    LAddLookup("PierceGenericMessages", "jarts");
    LAddLookup("PierceGenericMessages", "thrashes");
    LAddLookup("PierceGenericMessages", "mauls");
    LAddLookup("PierceGenericMessages", "decimates");
    LAddLookup("PierceGenericMessages", "_traumatizes_");
    LAddLookup("PierceGenericMessages", "_devastates_");
    LAddLookup("PierceGenericMessages", "_maims_");
    LAddLookup("PierceGenericMessages", "_demolishes_");
    LAddLookup("PierceGenericMessages", "MUTILATES");
    LAddLookup("PierceGenericMessages", "MASSACRES");
    LAddLookup("PierceGenericMessages", "PULVERIZES");
    LAddLookup("PierceGenericMessages", "DESTROYS");
    LAddLookup("PierceGenericMessages", "* OBLITERATES *");
    LAddLookup("PierceGenericMessages", "*** ANNIHILATES ***");
    LAddLookup("PierceGenericMessages", "**** SMITES ****");
end
LoadPierceGenericMessages();

function LoadAttackTable() 
    LAddLookup("AttackTable", "hit");
    LAddLookup("AttackTable", "slice");
    LAddLookup("AttackTable", "stab");
    LAddLookup("AttackTable", "slash");
    LAddLookup("AttackTable", "whip");
    LAddLookup("AttackTable", "claw");
    LAddLookup("AttackTable", "blast");
    LAddLookup("AttackTable", "pound");
    LAddLookup("AttackTable", "crush");
    LAddLookup("AttackTable", "grep");
    LAddLookup("AttackTable", "bite");
    LAddLookup("AttackTable", "pierce");
    LAddLookup("AttackTable", "suction");
    LAddLookup("AttackTable", "bolt");
    LAddLookup("AttackTable", "arrow");
    LAddLookup("AttackTable", "dart");
    LAddLookup("AttackTable", "stone");
    LAddLookup("AttackTable", "pea");
end
LoadAttackTable();

function LoadTemperatureSettings()
	LAddLookup("TemperatureSettings", "cold");
	LAddLookup("TemperatureSettings", "cool");
	LAddLookup("TemperatureSettings", "normal");
	LAddLookup("TemperatureSettings", "warm");
	LAddLookup("TemperatureSettings", "hot");
end
LoadTemperatureSettings();

function LoadPrecipitationSettings()
	LAddLookup("PrecipitationSettings", "arid");
	LAddLookup("PrecipitationSettings", "dry");
	LAddLookup("PrecipitationSettings", "normal");
	LAddLookup("PrecipitationSettings", "damp");
	LAddLookup("PrecipitationSettings", "wet");
end
LoadPrecipitationSettings();

function LoadWindSettings()
	LAddLookup("WindSettings", "still");
	LAddLookup("WindSettings", "calm");
	LAddLookup("WindSettings", "normal");
	LAddLookup("WindSettings", "breezy");
	LAddLookup("WindSettings", "windy");
end
LoadWindSettings();

function LoadLoginMessages()
    LAddLookup("LoginMessage", "\r\n&GYou did not have enough money for the residence you bid on.\r\nIt has been readded to the auction and you've been penalized.\r\n");
    LAddLookup("LoginMessage", "\r\n&GThere was an error in looking up the seller for the residence\r\nyou had bid on. Residence removed and no interaction has taken place.\r\n");
    LAddLookup("LoginMessage", "\r\n&GThere was no bidder on your residence. Your residence has been\r\nremoved from auction and you have been penalized.\r\n");

end
LoadLoginMessages();

function LoadCorpseDescriptions()
	LAddLookup("CorpseDescs", "The corpse of {0} is in the last stages of decay.");
	LAddLookup("CorpseDescs", "The corpse of {0} is crawling with vermin.");
	LAddLookup("CorpseDescs", "The corpse of {0} fills the air with a foul stench.");
	LAddLookup("CorpseDescs", "The corpse of {0} is buzzing with flies.");
	LAddLookup("CorpseDescs", "The corpse of {0} lies here.");
end
LoadCorpseDescriptions();

function LoadHealthConsiderMessages()
    LAddLookup("HealthConsider", "$N looks like a feather!");
    LAddLookup("HealthConsider", "You could kill $N with your hands tied!");
    LAddLookup("HealthConsider", "Hey! Where'd $N go?");
    LAddLookup("HealthConsider", "$N is a wimp.");
    LAddLookup("HealthConsider", "$N looks weaker than you.");
    LAddLookup("HealthConsider", "$N looks about as strong as you.");
    LAddLookup("HealthConsider", "It would take a bit of luck...");
    LAddLookup("HealthConsider", "It would take a lot of luck, and equipment!");
    LAddLookup("HealthConsider", "Why don't you dig a grave for yourself first!");
    LAddLookup("HealthConsider", "$N is built like a TANK!");
end
LoadHealthConsiderMessages();

function LoadLevelConsiderMessages()
    LAddLookup("LevelConsider", "You are far more experienced than $N.");
    LAddLookup("LevelConsider", "$n is not nearly as experienced as you.");
    LAddLookup("LevelConsider", "You are more experienced than $n.");
    LAddLookup("LevelConsider", "You are just about as experienced as $N.");
    LAddLookup("LevelConsider", "You are not nearly as experienced as $N.");
    LAddLookup("LevelConsider", "$N is far more experienced than you!");
    LAddLookup("LevelConsider", "$N would make a great teacher for you!");
end
LoadLevelConsiderMessages();

--"\r\n&GYou have successfully received your new residence.\r\n",
--"\r\n&GYou have successfully sold your residence.\r\n",
--"\r\n&RYou have been outcast from your clan/order/guild.  Contact a leader\r\nof that organization if you have any questions.\r\n",
--"\r\n&RYou have been silenced.  Contact an immortal if you wish to discuss\r\nyour sentence.\r\n",
--"\r\n&RYou have lost your ability to set your title.  Contact an immortal if you\r\nwish to discuss your sentence.\r\n",
--"\r\n&RYou have lost your ability to set your biography.  Contact an immortal if\r\nyou wish to discuss your sentence.\r\n",
--"\r\n&RYou have been sent to hell.  You will be automatically released when your\r\nsentence is up.  Contact an immortal if you wish to discuss your sentence.\r\n",
--"\r\n&RYou have lost your ability to set your own description.  Contact an\r\nimmortal if you wish to discuss your sentence.\r\n",
--"\r\n&RYou have lost your ability to set your homepage address.  Contact an\r\nimmortal if you wish to discuss your sentence.\r\n",
--"\r\n&RYou have lost your ability to \"beckon\" other players.  Contact an\r\nimmortal if you wish to discuss your sentence.\r\n",
--"\r\n&RYou have lost your ability to send tells.  Contact an immortal if\r\nyou wish to discuss your sentence.\r\n",
--"\r\n&CYour character has been frozen.  Contact an immortal if you wish\r\nto discuss your sentence.\r\n",
--"\r\n&RYou have lost your ability to emote.  Contact an immortal if\r\nyou wish to discuss your sentence.\r\n",
--"RESERVED FOR LINKDEAD DEATH MESSAGES",
--"RESERVED FOR CODE-SENT MESSAGES"
-- EOF