-- ASTRAL.LUA
-- This is the zone-file for the Astral Plane
-- Revised: 2013.11.09
-- Author: Jason Murdick
-- Version: 1.0
-- Remarks: Converted from original FUSS astral.are
f = loadfile(GetAppSetting("dataPath") .. "\\modules\\module_base.lua")();

systemLog("=================== AREA 'ASTRAL' INITIALIZING ===================");
newArea = LCreateArea(1, "The Astral Plane");
area.this = newArea;
area.this.Author = "Andi";
area.this.WeatherX = 0;
area.this.WeatherY = 0;
area.this.LowSoftRange = 15;
area.this.HighSoftRange = 35;
area.this.LowHardRange = 0;
area.this.HighHardRange = 60;
area.this.HighEconomy = 2064393;
area.this.ResetMessage = "The astral field glitters with a thousand points of light for a moment...";

systemLog("=================== AREA 'ASTRAL' - MOBILES ===================");
newMobile = LCreateMobile(800, "astral guardian figure");
mobile.this = newMobile;
mobile.this.ShortDescription = "the astral guardian";
mobile.this.LongDescription = "A shimmering grey figure stands vigil before the pearly gate.";
mobile.this.Description = [[You see a shimmering grey figure, vaguely humanoid in shape. Its sole 
striking feature is the item it wields: a pearl wand. The astral guardian seems to notice your 
attention and shifts uneasily.]];
mobile.this.Race = "magical";
mobile.this.Class = "warrior";
mobile.this.Position = "standing";
mobile.this.DefPosition = "standing";
mobile.this.Gender = "neuter";
mobile.this.Act = "npc sentinel";
mobile.this.AffectedBy = "detect_invis detect_hidden sanctuary protect truesight";
mobile.this:SetStats1(0, 38, -20, -15, 100000, 600000);
mobile.this:SetStats2(1, 1, 825);
mobile.this:SetStats3(8, 12, 100);
mobile.this:SetStats4(0, 0, 3, 0, 0);
mobile.this.Speaks = "magical";
mobile.this.Speaking = "common magical";
mobile.this.BodyParts = "guts";
mobile.this.Resistance = "pierce magic";
mobile.this.Immunity = "charm";
mobile.this.Attacks = "punch trip curse fireball";
mobile.this.Defenses = "parry dodge disarm";

newProg = LCreateMudProg("greet_prog");
mprog.this = newProg;
mprog.this.ArgList = "100";
mprog.this.Script = [[
local ch = LGetCurrentCharacter();

if (ch.Level < 10) then
	LMobSay("You may not pass $n, for you will only find death beyond this gate.");
else 
	if (ch.Level > 35) then
		LMobSay("You are too mighty, $n. I will not allow you to pass and tamper with the balance of the astral plane.");
		LMobEmote("firmly grips a pearl wand and points it at you...");
	else 
		LMobSay("Though it be foolish, if you wish to enter the astral plane, you have but to say so...");
	end
end
]]);
mobile.this:AddMudProg(mprog.this);
newProg = LCreateMudProg("speech_prog");
mprog.this = newProg;
mprog.this.ArgList = "p I wish to enter the astral plane";
mprog.this.Script = [[
local ch = LGetCurrentCharacter();

if (ch.Level < 10) then
	LMobEmote("gives you an irritated glare but makes no action.");
else 
	if (ch.Level > 35) then
		LMobSay("You shall not enter while I exist...");
	else 
		LMobSay("So be it!");
		MPEcho("A rolling thunder booms as the gate to the astral plane opens!");
		LMobCommand("unlock north");
		LMobCommand("open north");
		MPForce(ch, "north");
		LMobCommand("close north");
		LMobCommand("lock north");
	end
end
]]);
mobile.this:AddMudProg(mprog.this);
newProg = LCreateMudProg("fight_prog");
mprog.this = newProg;
mprog.this.ArgList = "40";
mprog.this.Script = [[
local ch = LGetCurrentCharacter();

if (ch.Level < 30) then
	LMobSay("You should not have toyed with my powers!");
	LMobCommand("cast ear");
	LMobCommand("cast fire");
	LMobCommand("cast blind");
else 
	MPEchoAt(ch, "The guardian chants a strange phrase and points at you...");
	MPEchoAround(ch, "The guardian chants a strange phrase and points at $n...");
	MPDamage(ch, 100);
end
]]);
mobile.this:AddMudProg(mprog.this);

newMobile = LCreateMobile(801, "nightmare");
mobile.this = newMobile;
mobile.this.ShortDescription = "A pitch-black nightmare";
mobile.this.LongDescription = "A nightmare is here, kicking at you with its flaming hooves.";
mobile.this.Description = [[The nightmare is a wholly evil being, sent out by the rulers of the lower 
planes to torment mortals.  It vaguely resembles a horse, with a hide blacker
than the darkest night, and hooves that burn with unholy fires.]]
mobile.this.Race = "horse";
mobile.this.Class = "warrior";
mobile.this.Position = "standing";
mobile.this.DefPosition = "standing";
mobile.this.Gender = "neuter";
mobile.this.Act = "npc stayarea mountable";
mobile.this.AffectedBy = "detect_evil detect_magic hold infrared curse _flaming _paralysis sneak fireshield";
mobile.this:SetStats1(-950, 18, 2, -2, 6000, 32000);
mobile.this:SetStats2(18, 18, 180);
mobile.this:SetStats3(5, 3, 10);
mobile.this:SetStats4(0, 0, 2, 0, 0);
mobile.this.Speaks = "magical";
mobile.this.Speaking = "magical";
mobile.this.BodyParts = "head legs heart guts feet";
mobile.this.Attacks = "kick firebreath";

newMobile = LCreateMobile(802, "night hag");
mobile.this = newMobile;
mobile.this.ShortDescription = "An evil night hag";
mobile.this.LongDescription = "A night hag reaches out to steal your soul.";
mobile.this.Description = [[You see a shadowy creature with long talons and an evil grin, whose sole 
purpose is to hunt down and slay mortals in order to obtain souls for her foul
master to torment.]]
mobile.this.Race = "undead";
mobile.this.Class = "warrior";
mobile.this.Position = "standing";
mobile.this.DefPosition = "standing";
mobile.this.SpecFun = "spec_cast_mage";
mobile.this.Act = "npc aggressive stayarea";
mobile.this.Gender = "female";
mobile.this.AffectedBy = "detect_invis infrared";
mobile.this:SetStats1(-1000, 24, 0, -1, 12500, 67500);
mobile.this:SetStats2(24, 24, 240);
mobile.this:SetStats3(5, 5, 5);
mobile.this.Speaks = "common";
mobile.this.Speaking = "common";
mobile.this.BodyParts = "head arms legs heart guts hands feet";
mobile.this.Resistance = "sleep charm hold";
mobile.this.Susceptible = "fire blunt";
mobile.this.Attacks = "claws blindness curse";

newProg = LCreateMudProg("greet_prog");
mprog.this = newProg;
mprog.this.ArgList = "100";
mprog.this.Script = [[
LMobCommand("cac");
LMobSay("Now your soul shall be mine!");
]];
mobile.this:AddMudProg(mprog.this);

newMobile = LCreateMobile(803, "stalker invisible");
mobile.this = newMobile;
mobile.this.ShortDescription = "An invisible stalker";
mobile.this.LongDescription = "You see a wavy distortion in the air";
mobile.this.Description = [[ou see only a wavy distortion in the air.  You reach out to touch it, and
feel nothing, not even the slightest of motions.]]
mobile.this.Race = "magical";
mobile.this.Class = "warrior";
mobile.this.Position = "standing";
mobile.this.DefPosition = "standing";
mobile.this.Gender = "neuter";
mobile.this.Act = "npc stayarea";
mobile.this.AffectedBy = "invisible detect_evil detect_invis hold sanctuary curse _flaming _paralysis sneak hide";
mobile.this:SetStats1(200, 20, 3, 0, 0, 19000);
mobile.this:SetStats2(17, 17, 170);
mobile.this:SetStats3(2, 7, 6);
mobile.this.Speaks = "magical";
mobile.this.Speaking = "magical";
mobile.this.BodyParts = "head arms legs heart guts hands feet eye";
mobile.this.Attacks = "bash gouge harm";
mobile.this.Defenses = "dodge";

newMobile = LCreateMobile(804, "soulless one being");
mobile.this = newMobile;
mobile.this.ShortDescription = "A poor soulless being";
mobile.this.LongDescription = "The soulless one wanders mindlessly.";
mobile.this.Description = [[This unfortunate being has had its very soul torn from its body.  It now roams
mindlessly, unaware of its surroundings.]]
mobile.this.Race = "spirit";
mobile.this.Class = "warrior";
mobile.this.Position = "standing";
mobile.this.DefensivePosition = "standing";
mobile.this.Gender = "neuter";
mobile.this.ActFlgas = "npc stayarea wimpy";
mobile.this:SetStats1(0, 12, 8, 5, 250, 7500);
mobile.this:SetStats2(10, 10, 100);
mobile.this:SetStats3(1, 6, 2);
mobile.this.Speaks = "spiritual";
mobile.this.Speaking = "spiritual";
mobile.this.Immunity = "nonmagic";
mobile.this.Attacks = "causecritical harm";

newProg = LCreateMudProg("greet_prog");
mprog.this = newProg;
mprog.this.ArgList = "100";
mprog.this.Script = [[
LMobCommand("moan");
]]);
mobile.this:AddMudProg(mprog.this);

newMobile = LCreateMobile(805, "githyanki hunter");
mobile.this = newMobile;
mobile.this.ShortDescription = "A githyanki hunter";
mobile.this.LongDescription = "A githyanki hunter searches for signs of the githzerai."
mobile.this.Description = [[You see a leathery-skinned humanoid creature that stands almost as tall as a
normal man.  Like all githyanki, his teeth are fanged and his eyes hollow and sunken.]]
mobile.this.Race = "magical";
mobile.this.Class = "warrior";
mobile.this.Position = "standing";
mobile.this.DefensivePosition = "standing";
mobile.this.SpecFun = "spec_thief";
mobile.this.Gender = "male";
mobile.this.Act = "npc scavenger stayarea";
mobile.this.AffectedBy = "infrared sneak";
mobile.this:SetStats1(-500, 11, 6, 6, 1000, 8000);
mobile.this:SetStats2(11, 11, 110);
mobile.this:SetStats3(1, 5, 4);
mobile.this.Speaks = "magical";
mobile.this.Speaking = "common";
mobile.this.BodyParts = "head arms legs heart guts hands feet eye";
mobile.this.Attacks = "punch bash";
mobile.this.Defenses = "disarm";

newProg = LCreateMudProg("rand_prog");
mprog.this = newProg;
mprog.this.ArgList = "5";
mprog.this.Script = [[
LMobEmote(", examines this portion of the astral field for signs of the githzerai.");
]]);
mobile.this:AddMudProg(mprog.this);

newMobile = LCreateMobile(806, "githyanki warrior figure");
mobile.this = newMobile;
mobile.this.ShortDescription = "A githyanki warrior";
mobile.this.LongDescription = "You see a humanoid figure clad in splinted armor.";
mobile.this.Description = [[You see a leathery-skinned humanoid creature that stands as tall as a normal
man.  Like all githyanki, his teeth are fanged and his eyes hollow and sunken.
He is clad in ornately designed splinted armor, and wields a sword that
whistles through the air as it slices towards your neck.]]
mobile.this.Race = "magical";
mobile.this.Class = "warrior";
mobile.this.Position = "standing";
mobile.this.DefPosition = "standing";
mobile.this.Gender = "male";
mobile.this.Act = "npc stayarea prototype";
mobile.this.AffectedBy = "infrared";
mobile.this:SetStats1(-500, 15, 4, 6, 2500, 15000);
mobile.this:SetStats2(15, 15, 150);
mobile.this:SetStats3(1, 5, 6);
mobile.this:SetStats4(0, 0, 2, 0, 0);
mobile.this.Speaks = "common elvish pixie orcish rodent mammal spiritual magical god ancient clan";
mobile.this.Speaking = "common elvish pixie orcish rodent mammal spiritual magical god ancient clan";
mobile.this.BodyParts = "head arms legs heart guts hands feet eye";
mobile.this.Attacks = "kick";
mobile.this.Defenses = "dodge";

newMobile = LCreateMobile(807, "guardian githyanki figure");
mobile.this = newMobile;
mobile.this.ShortDescription = "A githyanki warrior";
mobile.this.LongDescription = "You see a humanoid figure clad in splinted armor.";
mobile.this.Description = [[You see a leathery-skinned humanoid creature that stands as tall as a normal
man.  Like all githyanki, his teeth are fanged and his eyes hollow and sunken.
He is clad in ornately designed splinted armor, and wields a sword that
whistles through the air as it slices towards your neck.]]
mobile.this.Race = "magical";
mobile.this.Class = "warrior";
mobile.this.Position = "standing";
mobile.this.DefPosition = "standing";
mobile.this.Gender = "male";
mobile.this.Act = "npc sentinel aggressive";
mobile.this.AffectedBy = "infrared";
mobile.this:SetStats1(-500, 15, 4, 6, 6000, 15000);
mobile.this:SetStats2(15, 15, 150);
mobile.this:SetStats3(1, 5, 6);
mobile.this:SetStats4(0, 0, 2, 0, 0);
mobile.this.Speaks = "magical";
mobile.this.Speaking = "common";
mobile.this.BodyParts = "head arms legs heart guts hands feet";
mobile.this.Attacks = "punch gouge";
mobile.this.Defenses = "shockshield disarm";

newMobile = LCreateMobile(808, "knight figure githyanki");
mobile.this = newMobile;
mobile.this.ShortDescription = "A githyanki knight";
mobile.this.LongDescription = "A tall figure armored in black observes you quietly.";
mobile.this.Description = [[You see a leathery-skinned humanoid creature that stands a full foot taller
than a normal man.  His features are obscured by a suit of ornately designed
splinted armor -- you only hear the hissing of his breath and the scraping
of his sword against its scabbard as it is unsheathed.]]
mobile.this.Race = "human";
mobile.this.Class = "warrior";
mobile.this.Position = "standing";
mobile.this.DefPosition = "standing";
mobile.this.Gender = "male";
mobile.this.Act = "npc stayarea";
mobile.this.AffectedBy = "infrared";
mobile.this:SetStats1(-500, 25, -2, 6, 10000, 90000);
mobile.this:SetStats2(25, 25, 250);
mobile.this:SetStats3(1, 5, 15);
mobile.this.Speaks = "magical";
mobile.this.Speaking = "common";
mobile.this.BodyParts = "head arms legs heart guts hands feet eye";
mobile.this.Attacks = "punch kick trip drain";
mobile.this.Defenses = "parry";

newMobile = LCreateMobile(809, "githyanki knight protector");
mobile.this = newMobile;
mobile.this.ShortDescription = "A githyanki protector";
mobile.this.LongDescription = "A black knight of the githyanki protects his queen.";
mobile.this.Description = [[You see a leathery-skinned humanoid creature that stands a full foot taller
than a normal man.  His features are obscured by a suit of ornately designed
splinted armor -- you only hear the hissing of his breath and the scraping
of his sword against its scabbard as it is unsheathed.]]
mobile.this.Race = "human";
mobile.this.Class = "warrior";
mobile.this.Position = "standing";
mobile.this.DefPosition = "standing";
mobile.this.Gender = "male";
mobile.this.Act = "npc sentinel aggressive";
mobile.this.AffectedBy = "detect_invis detect_hidden infrared truesight";
mobile.this:SetStats1(-500, 25, -2, 6, 10000, 90000);
mobile.this:SetStats2(25, 25, 250);
mobile.this:SetStats3(1, 5, 15);
mobile.this:SetStats4(0, 0, 3, 0, 0);
mobile.this.Speaks = "magical";
mobile.this.Speaking = "common";
mobile.this.BodyParts = "head arms legs heart guts hands feet eye";
mobile.this.Attacks = "punch trip gouge";
mobile.this.Defenses = "dodge";

newProg = LCreateMudProg("greet_prog");
mprog.this = newProg;
mprog.this.ArgList = "50";
mprog.this.Script = [[
LMobSay("None may approach the Queen of the Gith!");
LMobEmote(", sheathes his blade and charges into battle!");
]]);
mobile.this:AddMudProg(mprog.this);

newMobile = LCreateMobile(810, "githyanki gish");
mobile.this = newMobile;
mobile.this.ShortDescription = "An evil gish";
mobile.this.LongDescription = "A small githyanki laughs at you through fanged teeth.";
mobile.this.Description = [[You see a leathery-skinned humanoid creature that stands nearly as tall as a
normal man.  Like all githyanki, his teeth are fanged and his eyes hollow and
sunken.  The gish are well-versed in the arcane arts, unlike the normal gith
people.  You notice this from the blast of lightning headed towards you.]]
mobile.this.Race = "human";
mobile.this.Class = "warrior";
mobile.this.Position = "standing";
mobile.this.DefPosition = "standing";
mobile.this.SpecFun = "spec_cast_mage";
mobile.this.Gender = "male";
mobile.this.Act = "npc stayarea wimpy";
mobile.this.AffectedBy = "infrared";
mobile.this:SetStats1(-500, 11, 6, 6, 3000, 12500);
mobile.this:SetStats2(11, 11, 110);
mobile.this:SetStats3(1, 5, 2);
mobile.this.Speaks = "magical";
mobile.this.Speaking = "magical";
mobile.this.BodyParts = "head arms legs heart guts hands feet eye";
mobile.this.Attacks = "claws gouge";

newMobile = LCreateMobile(811, "githyanki gish");
mobile.this = newMobile;
mobile.this.ShortDescription = "An evil gish";
mobile.this.LongDescription = "A gish laughs as flame leaps from his hands towards your face.";
mobile.this.Description = [[You see a leathery-skinned humanoid creature that stands nearly as tall as a
normal man.  Like all githyanki, his teeth are fanged and his eyes hollow and
sunken.  The gish are well-versed in the arcane arts, unlike the normal gith
people.  You notice this from the blast of lightning headed towards you.]]
mobile.this.Race = "magical";
mobile.this.Class = "warrior";
mobile.this.Position = "standing";
mobile.this.DefPosition = "standing";
mobile.this.SpecFun = "spec_cast_mage";
mobile.this.Gender = "male";
mobile.this.Act = "npc scavenger aggressive stayarea";
mobile.this.AffectedBy = "infrared";
mobile.this:SetStats1(-500, 11, 6, 6, 3000, 12500);
mobile.this:SetStats2(11, 11, 110);
mobile.this:SetStats3(1, 5, 2);
mobile.this.Speaks = "magical";
mobile.this.Speaking = "common";
mobile.this.BodyParts = "head arms heart guts hands feet";
mobile.this.Immunity = "fire";
mobile.this.Susceptibility = "cold";
mobile.this.Attacks = "gouge fireball";

newMobile = LCreateMobile(812, "gith githyanki warlock figure humanoid");
mobile.this = newMobile;
mobile.this.ShortDescription = "A warlock of the Gith";
mobile.this.LongDescription = "A humanoid figure stands here, holding a silvery sword.";
mobile.this.Description = [[You see a leathery-skinned creature, clad all in black and bearing a silvery-
colored sword.  Like all githyanki, his teeth are fanged and his eyes hollow
and sunken.  As you turn to flee, you feel the impact of a fireball against
your back.]]
mobile.this.Race = "magical";
mobile.this.Class = "warrior";
mobile.this.Position = "standing";
mobile.this.DefPosition = "standing";
mobile.this.SpecFun = "spec_cast_mage";
mobile.this.Gender = "male";
mobile.this.Act = "npc aggressive stayarea";
mobile.this.AffectedBy = "detect_invis detect_magic infrared";
mobile.this:SetStats1(-500, 30, 0, 2, 10000, 55000);
mobile.this:SetStats2(20, 20, 200);
mobile.this:SetStats3(1, 5, 8);
mobile.this:SetSaves(0, 0, 0, 0, -10);
mobile.this.Speaks = "magical";
mobile.this.Speaking = "common";
mobile.this.BodyParts = "head arms heart guts hands feet eye";
mobile.this.Immunity = "charm nonmagic";
mobile.this.Attacks = "gouge fireball";
mobile.this.Defenses = "dodge disarm";

newProg = LCreateMudProg("fight_prog");
mprog.this = newProg;
mprog.this.ArgList = "50";
mprog.this.Script = [[
local ch = LGetCurrentCharacter();
if (ch is nil) then
	LLogError("Current character is nil");
	return;	
end

LMobEmote(", prays a mantra to his Queen for aid.");
MPEchoAt(ch, "Suddenly, floating haunting eyes appear and gaze at you.  Two bolts of blazing blue lightning erupt from them, striking you!");
MPEchoAround(ch, "Suddenly, floating haunting eyes appear and gaze at $n!  Two bolts of blazing blue lightning erupt from them, striking $n!");
LMobCommand("c lightning $n");
LMobCommand("c lightning $n");
]]);
mobile.this:AddMudProg(mprog.this);

newProg = LCreateMudProg("death_prog");
mprog.this = newProg;
mprog.this.ArgList = "100";
mprog.this.Script = [[
if (LRandomPercent() <= 60) then
	MPEcho([[In a final effort to destroy you, the warlock slits his wrists in
mpecho a sickening ritual.  Coils of dark energy begin to flow along the
mpecho ground, forming into a nightmare.]]);
	MPMLoad(801);
	MPForce(LGetLastMob(), "c fireball $n");
else
	MPEcho("With his dying words, the warlock recites a powerful scroll...");
	MPJunk("all.scroll");
	MPDamage(LGetLastRoom(), 100);
end
]]);
mobile.this:AddMudProg(mprog.this);

newMobile = LCreateMobile(813, "githyanki lich queen");
mobile.this = newMobile;
mobile.this.ShortDescription = "The lich-queen";
mobile.this.LongDescription = "The evil lich-queen of the Gith reaches out to destroy you.";
mobile.this.Description = [[The lich-queen of the githyanki was once human, but no longer.  Her skin has
completely shrunken around her skull, held together solely by the force of
unholy magic and the powers of this plane of existence.  The cold glare of her
hollow eyes sends you into paroxysms of fright.]]
mobile.this.Race = "undead";
mobile.this.Class = "warrior";
mobile.this.Position = "standing";
mobile.this.DefPosition = "standing";
mobile.this.SpecFun = "spec_cast_undead";
mobile.this.Gender = "female";
mobile.this.Act = "npc aggressive sentinel";
mobile.this.AffectedBy = "detect_invis detect_magic detect_hidden sanctuary infrared shockshield";
mobile.this:SetStats1(-750, 30, -8, -8, 100000, 475000);
mobile.this:SetStats2(30, 30, 300);
mobile.this:SetStats3(4, 5, 20);
mobile.this:SetSaves(0, 0, 4, 0, 0);
mobile.this.Speaks = "magical";
mobile.this.Speaking = "common";
mobile.this.BodyParts = "head arms heart guts hands feet eye";
mobile.this.Immunity = "charm";
mobile.this.Susceptibility = "fire blunt";
mobile.this.Attacks = "claws trip gouge drain poison";
mobile.this.Defenses = "parry dodge disarm";

newProg = LCreateMudProg("death_prog");
mprog.this = newProg;
mprog.this.ArgList = "100";
mprog.this.Script = [[
local ch = LGetCurrentCharacter();

MPTransfer(ch, 878);
MPGoto(878);
MPEcho([[As the killing blow destroys the lich-queen's corporeal form, wisps of her spirit arise from the floor.  
A haunting voice whispers words of a spidery language and the floor suddenly cracks and opens.  You fall through 
the chasm about the altar into a swirling field of blackness, the cackles of a witch following your descent...]]);
]]);
mobile.this:AddMudProg(mprog.this);

newProg = LCreateMudProg("fight_prog");
mprog.this = newProg;
mprog.this.ArgList = "100";
mprog.this.Script = [[
local ch = LGetCurrentCharacter();

if (LRandomPercent() <= 10) then
	LMobEmote(", summons her minions from the lower planes...");
	MPMLoad(802);
	MPMLoad(809);
else 
	if (LRandomPercent() <= 40) then
		LMobEmote(", curls her flail past $r's defenses, mauling him!", ch);
		MPDamage(ch, 80);
	else 
		LMobEmote(", sneers at you.");
		LMobCommand("c 'acid blast' $n", ch);
	end
end
]]);
mobile.this:AddMudProg(mprog.this);

newProg = LCreateMudProg("greet_prog");
mprog.this = newProg;
mprog.this.ArgList = "60";
mprog.this.Script = [[
local ch = LGetLastCharacter();

LMobCommand("say Yes...more souls to dine on at my leisure!");
MPEcho("Deadly flames erupt from the lich-queen's fingers!");
LMobCOmmand("c 'burning hands' $n", ch);
]]);
mobile.this:AddMudProg(mprog.this);

newMobile = LCreateMobile(814, "lady gith");
mobile.this = newMobile;
mobile.this.ShortDescription = "Lady Gith";
mobile.this.LongDescription = "Lady Gith forms from a cloud of blackness, her face drenched in tears.";
mobile.this.Description = [[A beautiful human maiden stands before you, tears streaming down her face.
Once a mighty warrior, she led the githyanki to freedom from their formers
enslavers, the mind flayers.  She is now imprisoned in this prison due to
an unholy alliance broken by the traitorous lich-queen.  She is consumed
by eternal grief for her people, who have been twisted into monsters by
their ruler.]]
mobile.this.Race = "human";
mobile.this.Class = "warrior";
mobile.this.Position = "standing";
mobile.this.DefPosition = "standing";
mobile.this.Gender = "female";
mobile.this.Act = "npc sentinel";
mobile.this.AffectedBy = "detect_invis detect_hidden sanctuary infrared protect fireshield iceshield";
mobile.this:SetStats1(1000, 35, -15, -10, 5000, 750000);
mobile.this:SetStats2(35, 35, 350);
mobile.this:SetStats3(10, 3, 50);
mobile.this.Speaks = "magical";
mobile.this.Speaking = "common magical";
mobile.this.BodyParts = "head arms heart guts hands feet eye";
mobile.this.Resistance = "fire cold sleep charm hold";
mobile.this.Immunity = "charm sleep nonmagic";
mobile.this.Susceptibility = "electricity energy blunt";
mobile.this.Attacks = "punch kick bash gouge weaken";
mobile.this.Defenses = "parry dodge disarm";

newProg = LCreateMudProg("greet_prog");
mprog.this = newProg;
mprog.this.ArgList = "75";
mprog.this.Script = [[
LMobCommand("say There is no hope for my people.  Though you have defeated their horrible leader, she will return...");
]]);
mobile.this:AddMudProg(mprog.this);

newProg = LCreateMudProg("rand_prog");
mprog.this = newProg;
mprog.this.ArgList = "3";
mprog.this.Script = [[
LMobEmote(", weeps softly.");
]]);
mobile.this:AddMudProg(mprog.this);

newProg = LCreateMudProg("rand_prog");
mprog.this = newProg;
mprog.this.ArgList = "2";
mprog.this.Script = [[
LMobCommand("say If you wish to leave this lair of despair, you have but to say so.  Only my brethren and I are truly chained here...");
]]);
mobile.this:AddMudProg(mprog.this);

newProg = LCreateMudProg("speech_prog");
mprog.this = newProg;
mprog.this.ArgList = "p I wish to leave this lair of despair";
mprog.this.Script = [[
local ch = GetLastCharacter();

LMobCommand("nod");
LMobCommand("say Goodbye, $n", ch);
MPEchoAround("hite light flows from Gith's outstretched fingers and swirls about $n.", ch);
MPEchoAt(ch, "White light flows from Gith's outstretched fingers and swirls about you.");
MPTransfer(ch, 21000);

if (IsPKill(ch)) then
	MPTrans(ch, 3009);
else 
	MPTrans(ch, 21000);
end
]]);
mobile.this:AddMudProg(mprog.this);

newProg = LCreateMudProg("fight_prog");
mprog.this = newProg;
mprog.this.ArgList = "50";
mprog.this.Script = [[
local ch = GetLastCharacter();

LMobCommand("say If only I could die.  This place allows none of my kind peace!");
MPEcho("With unparalleled rage, Lady Gith summons forth a torrent of acid!");
LMobCommand("c 'acid blast' $n", ch);
LMobCommand("c 'acid blast' $n", ch);
]]);
mobile.this:AddMudProg(mprog.this);

newMobile = LCreateMobile(815, "ghost");
mobile.this = newMobile;
mobile.this.ShortDescription = "An insubstantial ghost";
mobile.this.LongDescription = "A ghost wanders here, intent on destroying all life.";
mobile.this.Description = [[The ghost makes no sound.  It is a shadowy shell of its former self, cursed
now to roam forever without rest.  The mind of this creature has been twisted
and destroyed in this torment, and it now intends to destroy all true living
beings it encounters, including you.]]
mobile.this.Race = "undead";
mobile.this.Class = "warrior";
mobile.this.Position = "standing";
mobile.this.DefPosition = "standing";
mobile.this.SpecFun = "spec_cast_undead";
mobile.this.Gender = "neuter";
mobile.this.Act = "npc stayarea";
mobile.this.AffectedBy = "detect_evil detect_invis hold sanctuary infrared curse _flaming _paralysis sneak pass_door";
mobile.this:SetStats1(-1000, 16, 2, 0, 0, 19000);
mobile.this:SetStats2(16, 16, 160);
mobile.this:SetStats3(2, 9, 2);
mobile.this:SetStats4(0, 0, 2, 0, 0);
mobile.this.Speaks = "common";
mobile.this.Speaking = "common";
mobile.this.Resistance = "cold sleep charm hold";
mobile.this.Immunity = "nonmagic";
mobile.this.Susceptibility = "fire blunt";
mobile.this.Attacks = "drain";

newMobile = LCreateMobile(816, "githzerai prisoner");
mobile.this = newMobile;
mobile.this.ShortDescription = "An imprisoned githzerai";
mobile.this.LongDescription = "A githzerai plots his escape plan.";
mobile.this.Description = [[You see a conniving humanoid, a descendant of a race of humans once enslaved
by the mind flayers and related to the githyanki.  Unfortunately, the two races
are bitter enemies, and this one happened to get caught.  He intends to escape.]]
mobile.this.Race = "magical";
mobile.this.Class = "warrior";
mobile.this.Position = "standing";
mobile.this.DefPosition = "standing";
mobile.this.Gender = "male";
mobile.this.Act = "npc stayarea";
mobile.this.AffectedBy = "infrared";
mobile.this:SetStats1(-250, 10, 8, 8, 0, 6000);
mobile.this:SetStats2(10, 10, 100);
mobile.this:SetStats3(1, 4, 4);
mobile.this:SetStats4(0, 0, 1, 0, 0);
mobile.this.Speaks = "magical";
mobile.this.Speaking = "common";
mobile.this.BodyParts = "head arms legs heart guts hands feet eye";
mobile.this.Attacks = "kick";
mobile.this.Defenses = "didge";

newMobile = LCreateMobile(817, "mind flayer prisoner");
mobile.this = newMobile;
mobile.this.ShortDescription = "a tortured mind flayer";
mobile.this.LongDescription = "A mind flayer lies here, scarred from the torture inflicted by the githyanki.";
mobile.this.Description = [[You see a formerly tall humanoid creature, with four tentacles hanging from the
middle of its head.  It is hunched over, bent and scarred from torture
inflicted by the githyanki.  It cringes as you approach.]]
mobile.this.Race = "magical";
mobile.this.Class = "warrior";
mobile.this.Position = "standing";
mobile.this.DefPosition = "standing";
mobile.this.SpecFun = "spec_cast_mage";
mobile.this.Gender = "neuter";
mobile.this.Act = "npc sentinel";
mobile.this:SetStats1(0, 8, 13, 2, 0, 3000);
mobile.this:SetStats2(8, 8, 80);
mobile.this:SetStats3(3, 4, 5);
mobile.this:SetStats4(0, 0, 2, 0, 0);
mobile.this.Speaks = "magical";
mobile.this.Speaking = "common";
mobile.this.BodyParts = "head arms legs heart guts hands feet eye tentacles";
mobile.this.Immunity = "sleep charm";
mobile.this.Attacks = "drain";

newMobile = LCreateMobile(818, "dragon red slave");
mobile.this = newMobile;
mobile.this.ShortDescription = "The enslaved red dragon";
mobile.this.LongDescription = "A giant red dragon is chained here, his breath heating the furnace.";
mobile.this.Description = [[The mighty scaled beast has been magically bound and subdued by the residents
of the keep.  Nevertheless, he is still an impressive sight, some twenty feet
in length with an even more massive wingspan.  He watches you, smoke drifting
from his open mouth.]]
mobile.this.Race = "dragon";
mobile.this.Class = "warrior";
mobile.this.Position = "standing";
mobile.this.DefPosition = "standing";
mobile.this.SpecFun = "spec_cast_mage";
mobile.this.Gender = "male";
mobile.this.Act = "npc sentinel";
mobile.this.AffectedBy = "detect_invis infrared";
mobile.this:SetStats1(-400, 28, -3, -6, 100000, 3500000);
mobile.this:SetStats2(28, 28, 280);
mobile.this:SetStats3(3, 5, 25);
mobile.this:SetStats4(0, 0, 3, 0, 0);
mobile.this.Speaks = "common elvish dragon magical";
mobile.this.Speaking = "dragon";
mobile.this.BodyParts = "head heart guts feet wings claws horns";
mobile.this.Resistance = "fire magic";
mobile.this.Immunity = "charm";
mobile.this.Susceptibility = "cold";
mobile.this.Attacks = "bite claws tail firebreath";

newMobile = LCreateMobile(819, "fire furnace flame");
mobile.this = newMobile;
mobile.this.ShortDescription = "A massive blast of fire";
mobile.this.LongDescription = "The fires of the furnace burn away at your skin!";
mobile.this.Description = [[The fire feeds on your skin like a living being, licking out at your hair.]]
mobile.this.Race = "magical";
mobile.this.Class = "warrior";
mobile.this.Position = "standing";
mobile.this.DefPosition = "standing";
mobile.this.SpecFun = "spec_breath_fire";
mobile.this.Gender = "neuter";
mobile.this.Act = "npc sentinel aggressive stayarea";
mobile.this.AffectedBy = "infrared fireshield";
mobile.this:SetStats1(0, 13, 5, 5, 0, 10000);
mobile.this:SetStats2(13, 13, 130);
mobile.this:SetStats3(3, 3, 3);
mobile.this:SetStats4(0, 0, 1, 0, 0);
mobile.this.Speaks = "common elvish pixie orcish rodent mammal spiritual magical god ancient clan";
mobile.this.Speaking = "common elvish pixie orcish rodent mammal spiritual magical god ancient clan";
mobile.this.Resistance = "fire";
mobile.this.Immunity = "fire";
mobile.this.Susceptibility = "cold";
mobile.this.Attacks = "fireball";

newMobile = LCreateMobile(899, "border mo");
mobile.this = newMobile;
mobile.this.ShortDescription = "a newly created border mob";
mobile.this.LongDescription = "Some god abandoned a newly created border mob here.";
mobile.this.Race = "human";
mobile.this.Class = "warrior";
mobile.this.Position = "standing";
mobile.this.DefPosition = "standing";
mobile.this.Gender = "neuter";
mobile.this.Act = "npc prototype";
mobile.this:SetStats1(0, 1, 0, 0, 0, 0);
mobile.this.Speaks = "common";
mobile.this.Speaking = "common";

systemLog("=================== AREA 'ASTRAL' - OBJECTS ===================");
newObject = LCreateObject(800, "pearl wand");
object.this = newObject;
object.this.Type = "weapon";
object.this.ShortDescription = "a pearl wand";
object.this.LongDescription = "The ground seems to cradle a pearl wand here.";
object.this.Action = "blast";
object.this.Flags = "magic antigood antievil";
object.this.WearFlags = "take wield";
object.this:SetValues(12, 4, 8, 6, 0, 0);
object.this:AddAffect(-1, -1, 60, 12, 0);
object.this:AddAffect(-1, -1, 20, 13, 0);
object.this:AddAffect(-1, -1, 5, 18, 0);
object.this:AddAffect(-1, -1, 8, 19, 0);
object.this:SetExtraDescription("wand pearl", "An intricate number of nooks have been engraved in the end of the wand, as though it was a key of some sort...");

newObject = LCreateObject(801, "scroll violet");
object.this = newObject;
object.this.Type = "scroll";
object.this.ShortDescription = "a violet scroll";
object.this.LongDescription = "A rolled piece of violet parchment lies on the floor.";
object.this.Flags = "bless";
object.this.WearFlags = "take";
object.this:SetValues(15, -1, -1, -1, 0, 0);
object.this:SetStats(1, 2500, 250, 0, 0);
object.this:AddSpell("armor");
object.this:AddSpell("bless");
object.this:AddSpell("shield");
object.this:SetExtraDescription("scroll violet", "The scroll is written on soft violet parchment that has a pleasing smell to it.");

newObject = LCreateObject(802, "tablet scroll black");
object.this = newObject;
object.this.Type = "scroll";
object.this.ShortDescription = "a black etched tablet";
object.this.LongDescription = "Lying on the ground is a jet black stone tablet.";
object.this.Flags = "evil";
object.this.WearFlags = "take";
object.this:SetValues(30, -1, -1, -1, 0, 0);
object.this:SetStats(5, 15000, 1500, 0, 0);
object.this:AddSpell("blazebane");
object.this:AddSpell("inner warmth");
object.this:AddExtraDescription("tablet scroll black", [[The tablet is an unreflective black rectangular piece of stone, with strange
writings etched deep into its surface.]]);

newObject = LCreateObject(803, "scroll githyanki");
object.this = newObject;
object.this.Type = "scroll";
object.this.ShortDescription = "a scroll with githyanki writings on it";
object.this.LongDescription = "You see a scroll written in an alien tongue.";
object.this.WearFlags = "take";
object.this:SetValues(24, -1, -1, -1, 0, 0);
object.this:SetStats(1, 7500, 750, 0, 0);
object.this:AddSpell("lightning bolt");
object.this:AddExtraDescription("scroll githyanki", "The scroll has writings on it that you cannot comprehend.");


#OBJECT
Vnum     804
Keywords amulet demon~
Type     wand~
Short    a small demon's amulet~
Long     A small amulet lies here in the dirt.~
Flags    evil nodrop~
WFlags   take hold~
Values   25 5 5 -1 0 0
Stats    5 20000 2000 0 0
Affect       -1 -1 -1 3 0
Affect       -1 -1 -1 4 0
Spells   'energy drain'
#EXDESC
ExDescKey    'amulet demon'~
ExDesc       The amulet has been forged of an unknown metal.  Raised inscriptions
and images depict snakes, tortured victims and various unholy rites.
A tingling sensation passes through you when you point the amulet
towards another living being.
~
#ENDEXDESC

#ENDOBJECT

#OBJECT
Vnum     805
Keywords talisman devilish~
Type     staff~
Short    a devilish talisman~
Long     On the ground is a small talisman which radiates a tangible evil.~
Flags    evil nodrop~
WFlags   take hold~
Values   25 5 5 -1 0 0
Stats    5 21000 2100 0 0
Affect       -1 -1 -10 13 0
Spells   'flamestrike'
#EXDESC
ExDescKey    talisman devilish~
ExDesc       The talisman is a red circular stone object hanging from a metallic chain.
It seems to pulse with a life of its own and you feel uncomfortable being
anywhere near it.  Engraved in the center is one word.  Merely reading
it induces chilling fright.
~
#ENDEXDESC

#ENDOBJECT

#OBJECT
Vnum     806
Keywords knife silvery~
Type     weapon~
Short    a silvery knife~
Long     A flat-bladed silvery knife lies here.~
Action   stab~
Flags    magic~
WFlags   take wield~
Values   0 2 4 2 0 0
Stats    3 1000 100 0 0
Affect       -1 -1 1 18 0
Affect       -1 -1 1 19 0
#EXDESC
ExDescKey    knife silvery~
ExDesc       The knife is forged from a strange silvery metal the likes of which you
have never seen in your own world.  It is incredibly sharp and hard, yet
feels almost fluid to the touch.
~
#ENDEXDESC

#ENDOBJECT

#OBJECT
Vnum     807
Keywords dagger silvery~
Type     weapon~
Short    a silvery dagger~
Long     A sharp-looking silvery dagger lies here.~
Action   pierce~
Flags    magic~
WFlags   take wield~
Values   0 3 4 11 0 0
Stats    3 1500 150 0 0
Affect       -1 -1 2 18 0
Affect       -1 -1 1 19 0
#EXDESC
ExDescKey    dagger silvery~
ExDesc       The dagger is forged from a strange silvery metal the likes of which you
have never seen in your own world.  It is incredibly sharp and hard, yet
feels almost fluid to the touch.
~
#ENDEXDESC

#ENDOBJECT

#OBJECT
Vnum     808
Keywords sword thin~
Type     weapon~
Short    an extremely sharp and thin sword~
Long     Light glints off the edge of a razor-sharp sword.~
Action   slice~
Flags    magic~
WFlags   take wield~
Values   0 4 4 1 0 0
Stats    3 2000 200 0 0
Affect       -1 -1 1 2 0
Affect       -1 -1 1 18 0
Affect       -1 -1 3 19 0
#EXDESC
ExDescKey    sword thin~
ExDesc       The sword is forged from a strange silvery metal.  It barely measures an inch
across at its widest point, yet it is totally inflexible and razor-sharp.  It
could probably cut through stone.
~
#ENDEXDESC

#ENDOBJECT

#OBJECT
Vnum     809
Keywords sword thin two two-handed~
Type     weapon~
Short    a thin two-handed sword~
Long     A heavy yet thinly bladed two-handed sword lies here.~
Action   slash~
Flags    magic~
WFlags   take wield~
Values   0 6 4 3 0 0
Stats    16 5000 500 0 0
Affect       -1 -1 2 18 0
Affect       -1 -1 4 19 0
#EXDESC
ExDescKey    sword thin two two-handed~
ExDesc       The sword is forged from a strange silvery metal.  It barely measures two
inches across at its widest point, yet it is totally inflexible and razor
sharp.  It could probably cut through stone.
~
#ENDEXDESC

#ENDOBJECT

#OBJECT
Vnum     810
Keywords sword silvery~
Type     weapon~
Short    a silvery sword~
Long     A silvery sword gleams unnaturally with an alien light.~
Action   slash~
Flags    glow magic~
WFlags   take wield~
Values   0 5 4 3 0 0
Stats    13 10000 1000 0 0
Affect       -1 -1 2 2 0
Affect       -1 -1 4 18 0
Affect       -1 -1 2 19 0
#EXDESC
ExDescKey    sword silvery~
ExDesc       The sword is forged from a strange silvery metal the likes of which you have
never seen in  your own world.  It is incredibly sharp and hard, yet feels
almost fluid to the touch.
~
#ENDEXDESC

#ENDOBJECT

#OBJECT
Vnum     811
Keywords flail barbed iron~
Type     weapon~
Short    an iron barbed flail~
Long     A rusty iron flail with wicked barbs on it tears at the ground.~
Action   tear~
Flags    hum evil~
WFlags   take wield~
Values   0 5 5 5 0 0
Stats    14 20000 2000 0 0
Affect       -1 -1 3 18 0
Affect       -1 -1 7 19 0
#EXDESC
ExDescKey    flail barbed iron~
ExDesc       This hideous weapon has claimed many lives.  You can tell from the bloodstains
dried into the iron and from the residue of flesh that hangs from the many
barbs on it.
~
#ENDEXDESC

#ENDOBJECT

#OBJECT
Vnum     812
Keywords sword gith~
Type     weapon~
Short    the holy sword of the githyanki~
Long     You feel tendrils of soft light emanating from a magnificent blade.~
Action   slash~
Flags    magic bless antievil~
WFlags   take wield~
Values   0 10 3 3 0 0
Stats    25 100000 10000 0 0
Affect       -1 -1 2 1 0
Affect       -1 -1 11 18 0
Affect       -1 -1 4 19 0
Affect       -1 -1 8192 26 0
#EXDESC
ExDescKey    sword gith~
ExDesc       The blade of this sword is made of pure platinum.  It is believed to be
the blade that the Lady Gith used in leading her people to freedom.
~
#ENDEXDESC

#ENDOBJECT

#OBJECT
Vnum     813
Keywords stone flame~
Type     light~
Short    a brightly flaming stone~
Long     The room is lit by a hot blue flame coming from a small stone.~
WFlags   take~
Values   0 0 -1 0 0 0
Stats    1 5000 500 0 0
Affect       -1 -1 -2 17 0
Affect       -1 -1 -5 23 0
Affect       -1 -1 15 13 0
#EXDESC
ExDescKey    stone flame~
ExDesc       This stone burns with an flame that emits great heat, yet it can be held with
ease.  The flame creates a light that allows you to see for miles.
~
#ENDEXDESC

#MUDPROG
Progtype  damage_prog~
Arglist   90~
Comlist   mpoload 813
mpecho As the flaming stone is split in two, one half sputters
mpecho and dies, while the other one begins flaming anew...
drop stone
mpforce $n get stone
mpforce $n wear stone
~
#ENDPROG

#ENDOBJECT

#OBJECT
Vnum     814
Keywords powder astral~
Type     pill~
Short    some astral powder~
Long     Twinkling powder floats before your eyes.~
WFlags   take~
Values   20 -1 -1 -1 0 0
Stats    1 5000 500 0 0
Spells   'refresh' 'NONE' 'NONE'
#EXDESC
ExDescKey    powder astral~
ExDesc       You see the residue of the astral plane.
~
#ENDEXDESC

#ENDOBJECT

#OBJECT
Vnum     815
Keywords vest splint~
Type     armor~
Short    a splint mail vest~
Long     An intricately fashioned splint mail vest lies here.~
WFlags   take body~
Values   7 0 0 0 0 0
Stats    45 5000 500 0 0
#EXDESC
ExDescKey    vest splint~
ExDesc       This splint mail has been expertly and ornately fashioned from the highest
quality of steels.
~
#ENDEXDESC

#ENDOBJECT

#OBJECT
Vnum     816
Keywords skirt splint~
Type     armor~
Short    a splint mail skirt~
Long     An intricately fashioned splint mail skirt lies here.~
WFlags   take legs~
Values   7 0 0 0 0 0
Stats    20 1000 100 0 0
#EXDESC
ExDescKey    skirt splint~
ExDesc       This splint mail has been expertly and ornately fashioned from the highest
quality of steels.
~
#ENDEXDESC

#ENDOBJECT

#OBJECT
Vnum     817
Keywords sleeves splint~
Type     armor~
Short    a pair of splint mail sleeves~
Long     A pair of intricately fashioned splint mail sleeves lie here.~
WFlags   take arms~
Values   7 0 0 0 0 0
Stats    15 1000 100 0 0
#EXDESC
ExDescKey    sleeves splint~
ExDesc       This splint mail has been expertly and ornately fashioned from the highest
quality of steels.
~
#ENDEXDESC

#ENDOBJECT

#OBJECT
Vnum     818
Keywords gauntlets ornate~
Type     armor~
Short    a pair of ornately designed gauntlets~
Long     A pair of painstakingly designed ornate gauntlets lie here.~
WFlags   take hands~
Values   7 0 0 0 0 0
Stats    5 19000 1900 0 0
Affect       -1 -1 2 1 0
Affect       -1 -1 1 18 0
Affect       -1 -1 1 19 0
#EXDESC
ExDescKey    gauntlets ornate~
ExDesc       These gauntlets have been expertly and ornately fashioned from the highest
quality of steels.
~
#ENDEXDESC

#ENDOBJECT

#OBJECT
Vnum     819
Keywords dress silk~
Type     armor~
Short    a dress of black silk~
Long     A dress of black silk has been left lying here.~
Flags    magic~
WFlags   take body~
Values   3 3 0 0 0 0
Stats    5 50 5 0 0
Affect       -1 -1 -6 17 0
Affect       -1 -1 -2 24 0
Affect       -1 -1 3 31 0
Affect       -1 -1 2 27 0
Affect       -1 -1 40 13 0
#EXDESC
ExDescKey    dress black silk~
ExDesc       The dress seems to afford little mundane protection, yet glows with a
strange magical power...
~
#ENDEXDESC

#MUDPROG
Progtype  damage_prog~
Arglist   100~
Comlist   mpecho As the magical dress is damaged, mystic powers are released from
mpecho the black silk and begin swirling about $n's form...
c fires $n
c ices $n
c sanc $n
~
#ENDPROG

#MUDPROG
Progtype  repair_prog~
Arglist   100~
Comlist   mpecho Though the dress has been sewn together, the magical threads which
mpecho gave it life have been severed and it decays to nothing...
mpforce $n drop dress
mpforce $n sac dress
~
#ENDPROG

#ENDOBJECT

#OBJECT
Vnum     820
Keywords potion violet~
Type     potion~
Short    a violet potion~
Long     A tall glass flask holds a translucent violet fluid.~
WFlags   take~
Values   18 -1 -1 -1 0 0
Stats    2 1000 100 0 0
Spells   'invis' 'protection' 'detect evil'
#EXDESC
ExDescKey    potion violet~
ExDesc       The violet fluid inside shifts and swirls.
~
#ENDEXDESC

#ENDOBJECT

#OBJECT
Vnum     821
Keywords potion vial murky~
Type     potion~
Short    a vial of murky fluid~
Long     A small vial contains a murky, sludge-like substance.~
WFlags   take~
Values   15 -1 -1 -1 0 0
Stats    1 750 75 0 0
Spells   'blindness' 'shield' 'NONE'
#EXDESC
ExDescKey    potion vial murky~
ExDesc       The murky liquid inside this vial looks a lot like sewer water.
~
#ENDEXDESC

#ENDOBJECT

#OBJECT
Vnum     822
Keywords potion silvery~
Type     potion~
Short    a silvery-colored potion~
Long     An octagonal vial holds a silvery-colored fluid that reflects light.~
WFlags   take~
Values   20 -1 -1 -1 0 0
Stats    2 1500 150 0 0
Spells   'detect invis' 'detect magic' 'detect poison'
#EXDESC
ExDescKey    potion silvery~
ExDesc       The octagonal vial contains a silvery-colored liquid that shimmers in the light.
~
#ENDEXDESC

#ENDOBJECT

#OBJECT
Vnum     823
Keywords mint chocolate~
Type     pill~
Short    a tasty-looking chocolate mint~
Long     Someone has dropped a delicious-looking chocolate mint here.~
WFlags   take~
Values   35 -1 -1 -1 0 0
Stats    1 1000 100 0 0
Spells   'refresh' 'heal' 'NONE'
#EXDESC
ExDescKey    mint chocolate~
ExDesc       This mint is made from the finest of chocolates, surrounding a cool peppermint
interior that must be savored, not swallowed.
~
#ENDEXDESC

#ENDOBJECT

#OBJECT
Vnum     824
Keywords pentagram~
Type     key~
Short    a black pentagram~
Long     A small black metal pentagram has been left here.~
Flags    invis~
WFlags   take hold~
Values   0 0 0 0 0 0
Stats    10 0 0 0 0
#EXDESC
ExDescKey    pentagram~
ExDesc       The pentagram is a small circular hoop that surrounds a five-pointed star.
Inscribed on its exterior loop are markings in an alien tongue.
~
#ENDEXDESC

#ENDOBJECT

#OBJECT
Vnum     825
Keywords silk pouch~
Type     money~
Short    a pouch of fine silk~
Long     A pouch crafted of the finest silk lies here.~
WFlags   take~
Values   169372 0 0 0 0 0
Stats    100 400000 -25536 0 0
#EXDESC
ExDescKey    pouch rotting~
ExDesc       You think you see glints of metal through the holes...
~
#ENDEXDESC

#ENDOBJECT

#OBJECT
Vnum     826
Keywords vial etherealness~
Type     potion~
Short    a vial of etherealness~
Long     A vial of wispy vapors lies here.~
Flags    glow hum magic~
WFlags   take hold~
Values   45 -1 -1 -1 0 0
Stats    2 22600 2260 0 0
Spells   'pass door' 'restore mana' 'restore mana'
#ENDOBJECT

#OBJECT
Vnum     899
Keywords border object~
Type     trash~
Short    a newly created border object~
Long     Some god dropped a newly created border object here.~
Flags    prototype~
Values   0 0 0 0 0 0
Stats    1 0 0 0 0
#ENDOBJECT

systemLog("=================== AREA 'ASTRAL' - ROOMS ===================");
newRoom = LCreateRoom(800, "The Bottom of the Rainbow");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("air");
room.this.Description = [[You are standing before a shimmering rainbow arching high up into the
sky.  You feel somehow separated from the natural world, even though the city of Darkhaven lies below you.]]
room.this:AddExit("up", 801, "The rainbow extends above you, fading out of existence only meters away.");
room.this:AddExit("down", 21338, "You can see the Temple of Darkhaven below you.");
room.this:AddExtraDescription("rainbow", "The rainbow softly glows with beams of light in colors that defy description.");

newRoom = LCreateRoom(801, "On the Rainbow");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("air");
room.this.Description = [[You are standing on the rainbow, surrounded by many beams of multicolored
light.  Through the bottom of the rainbow you can barely make out the cathedral of Darkhaven.]]
room.this:AddExit("up", 802, "The rainbow extends far above you, slowly fading into darkness.");
room.this:AddExit("down", 800, "The rainbow extends below you towards Darkhaven.");
room.this:AddExtraDescription("rainbow", "The rainbow softly glows with beams of light in colors that defy description.");

newRoom = LCreateRoom(802, "On the Rainbow");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("air");
room.this.Description = [[You are standing on the rainbow, surrounded by many beams of multicolored
light.  Through the bottom of the rainbow you can see the city of Darkhaven and the land surrounding it.  
The vast landscape seems to be leagues below you.]]
room.this:AddExit("up", 803, "The rainbow extends far above you, slowly fading into darkness.");
room.this:AddExit("down", 801, "The rainbow extends below you towards the land you once knew as home.");
room.this:AddExtraDescription("rainbow", "The rainbow softly glows with beams of light in colors that defy description.");

newRoom = LCreateRoom(803, "On the Rainbow");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("air");
room.this.Description = [[You are standing on the rainbow, surrounded by many beams of multicolored
light.  Through the bottom of the rainbow you can see the city of Darkhaven and the land surrounding it.  
The vast landscape seems to be leagues below you.]]
room.this:AddExit("up", 804, "The rainbow extends far above you, slowly fading into darkness.");
room.this:AddExit("down", 802, "The rainbow extends below you towards the land you once knew as home.");
room.this:AddExtraDescription("rainbow", "The rainbow softly glows with beams of light in colors that defy description.");

newRoom = LCreateRoom(804, "On the Rainbow");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("air");
room.this.Description = [[You are standing on the rainbow, surrounded by many beams of multicolored
light.  Through the bottom of the rainbow you can see the city of Darkhaven and the land surrounding it.  
The vast landscape seems to be leagues below you.]]
room.this:AddExit("up", 805, "The rainbow extends far above you, slowly fading into darkness.");
room.this:AddExit("down", 803, "The rainbow extends below you towards the land you once knew as home.");
room.this:AddExtraDescription("rainbow", "The rainbow softly glows with beams of light in colors that defy description.");
room.this:AddExtraDescription("motes star stars dust", [[The star-like motes dance before your eyes.  They are incredibly beautiful yet
intangible -- your hand passes right through them.]]);
room.this:AddExtraDescription("bridge", "The colors of the rainbow seem to converge into a glowing bridge that leads to a massive white gate.");

newRoom = LCreateRoom(805, "The Glowing Bridge at the Rainbow's End");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("city");
room.this.Description = [[After a long climb you have reached the end of the rainbow.  The brilliant
colors have twisted and merged into a beautiful glowing bridge that leads
toward a massive white gate to the north.  All around you is a soft, peaceful 
glow interrupted only by the passing of star-like dust motes before your eyes.]]
room.this:AddExit("north", 806, "The bridge leads on to a massive white gate.  You see a large grey figure standing there.");
room.this:AddExit("down", 804, "The bridge fragments into a beautiful spectrum of colors that drops out of sight below you.");
room.this:AddExtraDescription("rainbow", "The rainbow seems to be forming directly from the light of the bridge.  It drops out of sight below you.");
room.this:AddExtraDescription("motes star stars dust", [[The star-like motes dance before your eyes.  They are incredibly beautiful yet
intangible -- your hand passes right through them.]]);
room.this:AddExtraDescription("bridge", "The bridge glows with an unearthly light.");
room.this:AddExtraDescription("gate", [[The gate towers ten feet above your head. Beyond it is a field of utter
blackness from which the motes of light seem to issue forth.]]);

newRoom = LCreateRoom(806, "The Astral Gate");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("city");
room.this.Description = [[The bridge ends here before a massive, pearly-colored gate beyond which
you can see a field of blackness filled with silvery snake-like tendrils,
stars, and occasional flashes of light.  A slate plaque inset in the wall
next to the gate catches your eye.]]
newExit = LCreateExit("north", 807, "Through the gate you see a massive field of blackness that extends as far as you can see.");
exit.this = newExit;
exit.this.Key = 800;
exit.this.Keywords = "gate";
exit.this.Flags = "isdoor closed locked pickproof nopassdoor";
room.this:AddExit(exit.this);
room.this:AddExit("south", 805, "The bridge extends towards the rainbow, which drops out of sight below you.");
room.this:AddExtraDescription("gate", [[You see a huge, glowing gate wrought from an incredibly hard translucent 
material.  Each of the intricately carved bars of this mighty gate are about 
as thick as a man's wrist, rising from the floor to a height of ten feet. A
plaque is inset into the wall next to the gate.]]
room.this:AddExtraDescription("plaque", [[The plaque reads: 

Let this gate stand to protect mortal man for all time against the perils of 
the outer planes of existence and the creatures fair and foul that dwell
within.

(This zone, the Astral Plane and accompanying githyanki keep, was written by
 Andersen for any standard Merc 2.0 mud.)
 
(This zone was later renovated and spiced up by Lord Rennard for the
Realms of Despair.)]]
room.this:AddReset("mobile", 0, 800, 1, 806);
room.this:AddReset("equipment", 0, 800, 0, 16);
room.this:AddReset("inventory", 1, 826, 1);
room.this:AddReset("door", 0, 806, 0, 2);

newRoom = LCreateRoom(807, "Into the Astral Plane");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("field");
room.this.Flags = "nomob indoors";
room.this.Description = [[You find yourself floating in a field of blackness, filled with tiny tendrils
of silvery light and insubstantial shining motes of dust.  The weightlessness
of this environment is very disorienting.  To the south is a set of pearly
gates: the sole mundane manner of leaving the Astral Plane.]]
room.this:AddExit("north", 808, "The astral field leads off into infinity.");
room.this:AddExit("east", 809, "The astral field leads off into infinity.");

newExit = LCreateExit("south", 806, "The astral gate leads back to your world.");
exit.this = newExit;
exit.this.Key = 800;
exit.this.Keywords = "gate";
exit.this:SetFlags("isdoor closed locked pickproof nopassdoor");
room.this:AddExit(exit.this);
room.this:AddExit("west", 810, "The astral field leads off into infinity.");
room.this:AddExit("up", 811, "The astral field leads off into infinity.");
room.this:AddExit("down", 812, "The astral field leads off into infinity.");
room.this:AddReset("door", 0, 807, 2, 2);
room.this:AddExtraDescription("gate", [[From here the pearly-colored gate seems very small and insubstantial.
Through its bars you can see the astral guardian, protecting Despair from the perils of the Astral Plane.]]


#ROOM
Vnum     808
Name     Exploring the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You find yourself floating in a field of blackness, filled with tiny tendrils
of silvery light and insubstantial shining motes of dust.  The weightlessness
of this environment is very disorienting, and it is difficult to maintain your 
bearings and sense of direction.
~
#EXIT
Direction north~
ToRoom    813
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    807
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    814
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    812
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    810
Desc      The astral field leads off into infinity.
~
#ENDEXIT

Reset R 0 808 6
#ENDROOM

#ROOM
Vnum     809
Name     Exploring the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You find yourself floating in a field of blackness, filled with tiny tendrils
of silvery light and insubstantial shining motes of dust.  The weightlessness
of this environment is very disorienting, and it is difficult to maintain your 
bearings and sense of direction.
~
#EXIT
Direction north~
ToRoom    814
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    816
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    807
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    811
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    810
Desc      The astral field leads off into infinity.
~
#ENDEXIT

Reset R 0 809 6
#ENDROOM

#ROOM
Vnum     810
Name     Exploring the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You find yourself floating in a field of blackness, filled with tiny tendrils
of silvery light and insubstantial shining motes of dust.  The weightlessness
of this environment is very disorienting, and it is difficult to maintain your 
bearings and sense of direction.
~
#EXIT
Direction east~
ToRoom    809
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    813
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    808
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    807
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    815
Desc      The astral field leads off into infinity.
~
#ENDEXIT

Reset R 0 810 6
#ENDROOM

#ROOM
Vnum     811
Name     Exploring the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You find yourself floating in a field of blackness, filled with tiny tendrils
of silvery light and insubstantial shining motes of dust.  The weightlessness
of this environment is very disorienting, and it is difficult to maintain your 
bearings and sense of direction.
~
#EXIT
Direction north~
ToRoom    812
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    809
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    807
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    816
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    817
Desc      The astral field leads off into infinity.
~
#ENDEXIT

Reset R 0 811 6
#ENDROOM

#ROOM
Vnum     812
Name     Exploring the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You find yourself floating in a field of blackness, filled with tiny tendrils
of silvery light and insubstantial shining motes of dust.  The weightlessness
of this environment is very disorienting, and it is difficult to maintain your 
bearings and sense of direction.
~
#EXIT
Direction north~
ToRoom    815
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    817
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    807
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    808
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    811
Desc      The astral field leads off into infinity.
~
#ENDEXIT

Reset R 0 812 6
#ENDROOM

#ROOM
Vnum     813
Name     Roaming the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You find yourself floating in a field of blackness, filled with tiny tendrils
of silvery light and insubstantial shining motes of dust.  The weightlessness
of this environment is very disorienting.  The astral field lies in all
directions.
~
#EXIT
Direction north~
ToRoom    810
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    815
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    814
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    808
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    820
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    818
Desc      The astral field leads off into infinity.
~
#ENDEXIT

Reset R 0 813 6
Reset M 0 804 10 813
#ENDROOM

#ROOM
Vnum     814
Name     Roaming the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You find yourself floating in a field of blackness, filled with tiny tendrils
of silvery light and insubstantial shining motes of dust.  The weightlessness
of this environment is very disorienting.  The vast expanse of the astral
field lies in all directions.
~
#EXIT
Direction north~
ToRoom    808
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    817
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    819
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    813
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    818
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    809
Desc      The astral field leads off into infinity.
~
#ENDEXIT

Reset R 0 814 6
#ENDROOM

#ROOM
Vnum     815
Name     Roaming the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You find yourself floating in a field of blackness, filled with tiny tendrils
of silvery light and insubstantial shining motes of dust.  The weightlessness
of this environment is disorienting, yet your instincts say you are
nearing what you seek.  All exits lead off into the astral field.
~
#EXIT
Direction north~
ToRoom    813
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    812
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    822
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    820
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    816
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    810
Desc      The astral field leads off into infinity.
~
#ENDEXIT

Reset R 0 815 6
#ENDROOM

#ROOM
Vnum     816
Name     Roaming the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You find yourself floating in a field of blackness, filled with tiny tendrils
of silvery light and insubstantial shining motes of dust.  The weightlessness
of this environment is disorienting, yet your instincts say you are nearing
what you seek.  All exits lead off into the astral field.
~
#EXIT
Direction north~
ToRoom    815
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    821
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    809
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    817
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    811
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    819
Desc      The astral field leads off into infinity.
~
#ENDEXIT

Reset R 0 816 6
#ENDROOM

#ROOM
Vnum     817
Name     Roaming the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You find yourself floating in a field of blackness, filled with tiny tendrils
of silvery light and insubstantial shining motes of dust.  The weightlessness
of this environment is disorienting, yet your instincts say you are nearing
what you seek.  All exits lead off into the astral plane.
~
#EXIT
Direction north~
ToRoom    821
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    816
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    812
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    814
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    822
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    811
Desc      The astral field leads off into infinity.
~
#ENDEXIT

Reset R 0 817 6
#ENDROOM

#ROOM
Vnum     818
Name     Wandering the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You find yourself floating in a field of blackness, filled with tiny tendrils
of silvery light and insubstantial shining motes of dust.  The weightlessness
of this environment is highly disorienting.  Nearby you see a large silvery
strand of light.  All but one exit leads off into the astral field.
~
#EXIT
Direction north~
ToRoom    809
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    819
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    833
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    836
Desc      You see a tangible strand of silvery light.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    813
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    814
Desc      The astral field leads off into infinity.
~
#ENDEXIT

Reset R 0 818 6
Reset M 0 805 8 818
  Reset E 0 807 0 16
#ENDROOM

#ROOM
Vnum     819
Name     Wandering the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You find yourself floating in a field of blackness, filled with tiny tendrils
of silvery light and insubstantial shining motes of dust.  The weightlessness
of this environment is highly disorienting.  Nearby you see a large silvery
strand of light.  All but one exit leads off into the astral field.
~
#EXIT
Direction north~
ToRoom    814
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    816
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    834
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    810
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    818
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    838
Desc      You see a tangible strand of silvery light.
~
#ENDEXIT

Reset R 0 819 6
#ENDROOM

#ROOM
Vnum     820
Name     Wandering the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You find yourself floating in a field of blackness, filled with tiny tendrils
of silvery light and insubstantial shining motes of dust.  The weightlessness
of this environment is highly disorienting.  Nearby you see a large silvery
strand of light.  All but one exit leads off into the astral field.
~
#EXIT
Direction north~
ToRoom    815
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    840
Desc      You see a tangible strand of silvery light.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    821
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    813
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    835
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    811
Desc      The astral field leads off into infinity.
~
#ENDEXIT

Reset R 0 820 6
Reset M 0 803 4 820
  Reset G 0 814 0
Reset M 0 805 8 820
  Reset E 0 807 0 16
#ENDROOM

#ROOM
Vnum     821
Name     Wandering the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You find yourself floating in a field of blackness, filled with tiny tendrils
of silvery light and insubstantial shining motes of dust.  The weightlessness
of this environment is highly disorienting.  Nearby you see a large silvery
strand of light.  All but one exit leads off into the astral field.
~
#EXIT
Direction north~
ToRoom    842
Desc      You see a tangible strand of silvery light.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    816
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    820
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    833
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    817
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    812
Desc      The astral field leads off into infinity.
~
#ENDEXIT

Reset R 0 821 6
#ENDROOM

#ROOM
Vnum     822
Name     Wandering the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You find yourself floating in a field of blackness, filled with tiny tendrils
of silvery light and insubstantial shining motes of dust.  The weightlessness
of this environment is highly disorienting.  Nearby you see a large silvery
strand of light.  All but one exit leads off into the astral field.
~
#EXIT
Direction north~
ToRoom    834
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    844
Desc      You see a tangible strand of silvery light.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    817
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    812
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    815
Desc      The astral field leads off into infinity.
~
#ENDEXIT

Reset R 0 822 6
Reset M 0 805 8 822
  Reset E 0 807 0 16
#ENDROOM

#ROOM
Vnum     823
Name     Travelling the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You find yourself floating in a field of blackness, filled with tiny tendrils
of silvery light and insubstantial shining motes of dust.  The weightlessness
of this environment is extremely disorienting.  Nearby you see a large silvery
strand of light.  All but one exit leads off into the astral field.
~
#EXIT
Direction north~
ToRoom    826
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    832
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    828
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    835
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    837
Desc      You see a tangible strand of silvery light.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    824
Desc      The astral field leads off into infinity.
~
#ENDEXIT

Reset R 0 823 6
Reset M 0 806 6 823
  Reset E 0 809 0 16
  Reset E 0 815 0 5
#ENDROOM

#ROOM
Vnum     824
Name     Travelling the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You find yourself floating in a field of blackness, filled with tiny tendrils
of silvery light and insubstantial shining motes of dust.  The weightlessness
of this environment is extremely disorienting.  Nearby you see a large silvery
strand of light.  All but one exit leads off into the astral field.
~
#EXIT
Direction north~
ToRoom    839
Desc      You see a tangible strand of silvery light.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    833
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    829
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    823
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    825
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    830
Desc      The astral field leads off into infinity.
~
#ENDEXIT

Reset R 0 824 6
Reset M 0 803 4 824
  Reset G 0 814 0
Reset M 0 805 8 824
  Reset E 0 807 0 16
#ENDROOM

#ROOM
Vnum     825
Name     Travelling the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You find yourself floating in a field of blackness, filled with tiny tendrils
of silvery light and insubstantial shining motes of dust.  The weightlessness
of this environment is extremely disorienting.  Nearby you see a large silvery
strand of light.  All but one exit leads off into the astral field.
~
#EXIT
Direction north~
ToRoom    827
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    830
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    828
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    841
Desc      You see a tangible strand of silvery light.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    824
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    834
Desc      The astral field leads off into infinity.
~
#ENDEXIT

Reset R 0 825 6
#ENDROOM

#ROOM
Vnum     826
Name     Travelling the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You find yourself floating in a field of blackness, filled with tiny tendrils
of silvery light and insubstantial shining motes of dust.  The weightlessness
of this environment is extremely disorienting.  Nearby you see a large silvery
strand of light.  All but one exit leads off into the astral field.
~
#EXIT
Direction north~
ToRoom    827
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    843
Desc      You see a tangible strand of silvery light.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    829
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    835
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    823
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    831
Desc      The astral field leads off into infinity.
~
#ENDEXIT

Reset R 0 826 6
Reset M 0 803 4 826
  Reset G 0 814 0
#ENDROOM

#ROOM
Vnum     827
Name     Travelling the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You find yourself floating in a field of blackness, filled with tiny tendrils
of silvery light and insubstantial shining motes of dust.  The weightlessness
of this environment is extremely disorienting.  Nearby you see a large silvery
strand of light.  All but one exit leads off into the astral field.
~
#EXIT
Direction north~
ToRoom    825
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    831
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    844
Desc      You see a tangible strand of silvery light.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    832
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    833
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    826
Desc      The astral field leads off into infinity.
~
#ENDEXIT

Reset R 0 827 6
Reset M 0 806 6 827
  Reset E 0 809 0 16
  Reset E 0 815 0 5
#ENDROOM

#ROOM
Vnum     828
Name     Traversing the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You find yourself deep within in a dark field filled with tiny tendrils
of silvery light and insubstantial shining motes of dust.  The weightlessness
of this environment is extremely disorienting, and you are having trouble 
keeping your sense of direction.  All exits lead off into the astral field.
~
#EXIT
Direction north~
ToRoom    809
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    845
Desc      The astral field leads off into the depths.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    847
Desc      The astral field leads off into the depths.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    834
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    825
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    823
Desc      The astral field leads off into infinity.
~
#ENDEXIT

Reset R 0 828 6
#ENDROOM

#ROOM
Vnum     829
Name     Traversing the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You find yourself deep within in a dark field filled with tiny tendrils
of silvery light and insubstantial shining motes of dust.  The weightlessness
of this environment is extremely disorienting, and you are having trouble 
keeping your sense of direction.  All exits lead off into the astral field.
~
#EXIT
Direction north~
ToRoom    826
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    849
Desc      The astral field leads off into the depths.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    835
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    846
Desc      The astral field leads off into the depths.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    824
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    830
Desc      The astral field leads off into infinity.
~
#ENDEXIT

Reset R 0 829 6
Reset M 0 810 6 829
  Reset E 0 806 0 16
#ENDROOM

#ROOM
Vnum     830
Name     Traversing the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You find yourself deep within in a dark field filled with tiny tendrils
of silvery light and insubstantial shining motes of dust.  The weightlessness
of this environment is extremely disorienting, and you are having trouble 
keeping your sense of direction.  All exits lead off into the astral field.
~
#EXIT
Direction north~
ToRoom    833
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    845
Desc      The astral field leads off into the depths.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    825
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    847
Desc      The astral field leads off into the depths.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    824
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    829
Desc      The astral field leads off into infinity.
~
#ENDEXIT

Reset R 0 830 6
Reset M 0 803 4 830
  Reset G 0 814 0
Reset M 0 808 3 830
  Reset E 0 809 0 16
  Reset E 0 815 0 5
  Reset E 0 816 0 7
  Reset E 0 817 0 10
#ENDROOM

#ROOM
Vnum     831
Name     Traversing the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You find yourself deep within in a dark field filled with tiny tendrils
of silvery light and insubstantial shining motes of dust.  The weightlessness
of this environment is extremely disorienting, and you are having trouble 
keeping your sense of direction.  All exits lead off into the astral field.
~
#EXIT
Direction north~
ToRoom    826
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    846
Desc      The astral field leads off into the depths.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    834
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    832
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    827
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    848
Desc      The astral field leads off into the depths.
~
#ENDEXIT

Reset R 0 831 6
Reset M 0 810 6 831
  Reset E 0 806 0 16
#ENDROOM

#ROOM
Vnum     832
Name     Traversing the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You find yourself deep within in a dark field filled with tiny tendrils
of silvery light and insubstantial shining motes of dust.  The weightlessness
of this environment is extremely disorienting, and you are having trouble 
keeping your sense of direction.  All exits lead off into the astral field.
~
#EXIT
Direction north~
ToRoom    835
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    849
Desc      The astral field leads off into the depths.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    823
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    831
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    848
Desc      The astral field leads off into the depths.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    827
Desc      The astral field leads off into infinity.
~
#ENDEXIT

Reset R 0 832 6
#ENDROOM

#ROOM
Vnum     833
Name     Lost in the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You find yourself in in a field of blackness, filled with tiny tendrils
of silvery light and insubstantial shining motes of dust.  The weightlessness
of this environment is incredibly disorienting, and you are having trouble 
keeping your senses straight.  All exits lead off into the astral field.
~
#EXIT
Direction east~
ToRoom    818
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    821
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    830
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    827
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    824
Desc      The astral field leads off into infinity.
~
#ENDEXIT

Reset R 0 833 6
Reset M 0 801 2 833
  Reset G 0 804 0
#ENDROOM

#ROOM
Vnum     834
Name     Lost in the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You find yourself in in a field of blackness, filled with tiny tendrils
of silvery light and insubstantial shining motes of dust.  The weightlessness
of this environment is incredibly disorienting, and you are having trouble 
keeping your senses straight.  All exits lead off into the astral plane.
~
#EXIT
Direction east~
ToRoom    828
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    825
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    831
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    822
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    819
Desc      The astral field leads off into infinity.
~
#ENDEXIT

Reset R 0 834 6
Reset M 0 801 2 834
  Reset G 0 804 0
#ENDROOM

#ROOM
Vnum     835
Name     Lost in the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You find yourself in in a field of blackness, filled with tiny tendrils
of silvery light and insubstantial shining motes of dust.  The weightlessness
of this environment is incredibly disorienting, and you are having trouble 
keeping your senses straight.  All exits lead off into the astral field.
~
#EXIT
Direction north~
ToRoom    829
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    820
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    823
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    832
Desc      The astral field leads off into infinity.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    826
Desc      The astral field leads off into infinity.
~
#ENDEXIT

Reset R 0 835 6
#ENDROOM

#ROOM
Vnum     836
Name     Walking the Silvery Strand~
Sector   field~
Flags    indoors~
Desc     You are standing atop a silvery strand of light that twists and turns
through the astral field.
~
#EXIT
Direction south~
ToRoom    837
Desc      The strand of light twists and turns out of sight.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    818
Desc      You see the infinite expanse of the astral field.
~
#ENDEXIT

Reset R 0 836 6
#ENDROOM

#ROOM
Vnum     837
Name     Walking the Silvery Strand~
Sector   field~
Flags    indoors~
Desc     You are standing atop a silvery strand of light that twists and turns
through the astral field.
~
#EXIT
Direction up~
ToRoom    823
Desc      You see the infinite expanse of the astral field.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    836
Desc      The strand of light twists and turns out of sight.
~
#ENDEXIT

Reset R 0 837 6
#ENDROOM

#ROOM
Vnum     838
Name     Walking the Silvery Strand~
Sector   field~
Flags    indoors~
Desc     You are standing atop a silvery strand of light that twists and turns
through the astral field.
~
#EXIT
Direction west~
ToRoom    819
Desc      You see the infinite expanse of the astral field.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    839
Desc      The strand of light twists and turns out of sight.
~
#ENDEXIT

Reset R 0 838 6
#ENDROOM

#ROOM
Vnum     839
Name     Walking the Silvery Strand~
Sector   field~
Flags    indoors~
Desc     You are standing atop a silvery strand of light that twists and turns
through the astral field.
~
#EXIT
Direction west~
ToRoom    824
Desc      You see the infinite expanse of the astral field.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    838
Desc      The strand of light twists and turns out of sight.
~
#ENDEXIT

Reset R 0 839 6
#ENDROOM

#ROOM
Vnum     840
Name     Walking the Silvery Strand~
Sector   field~
Flags    indoors~
Desc     You are standing atop a silvery strand of light that twists and turns
through the astral field.
~
#EXIT
Direction up~
ToRoom    841
Desc      The strand of light twists and turns out of sight.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    820
Desc      You see the infinite expanse of the astral field.
~
#ENDEXIT

Reset R 0 840 6
#ENDROOM

#ROOM
Vnum     841
Name     Walking the Silvery Strand~
Sector   field~
Flags    indoors~
Desc     You are standing atop a silvery strand of light that twists and turns
through the astral field.
~
#EXIT
Direction south~
ToRoom    825
Desc      You see the infinite expanse of the astral field.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    840
Desc      The strand of light twists and turns out of sight.
~
#ENDEXIT

Reset R 0 841 6
#ENDROOM

#ROOM
Vnum     842
Name     Walking the Silvery Strand~
Sector   field~
Flags    indoors~
Desc     You are standing atop a silvery strand of light that twists and turns
through the astral field.
~
#EXIT
Direction up~
ToRoom    821
Desc      You see the infinite expanse of the astral field.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    843
Desc      The strand of light twists and turns out of sight.
~
#ENDEXIT

Reset R 0 842 6
#ENDROOM

#ROOM
Vnum     843
Name     Walking the Silvery Strand~
Sector   field~
Flags    indoors~
Desc     You are standing atop a silvery strand of light that twists and turns
through the astral field.
~
#EXIT
Direction north~
ToRoom    842
Desc      The strand of light twists and turns out of sight.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    826
Desc      You see the infinite expanse of the astral field.
~
#ENDEXIT

Reset R 0 843 6
#ENDROOM

#ROOM
Vnum     844
Name     Walking the Silvery Strand~
Sector   field~
Flags    indoors~
Desc     You are standing atop a silvery strand of light that twists and turns
through the astral field.
~
#EXIT
Direction east~
ToRoom    822
Desc      You see the infinite expanse of the astral field.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    827
Desc      You see the endless expanse of the astral field.
~
#ENDEXIT

Reset R 0 844 6
#ENDROOM

#ROOM
Vnum     845
Name     Deep Within the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You have travelled deep inside the astral field.  Here the fabric of the
field has become twisted and warped due to the proximity of another plane of
existence.  Some of the tendrils of light that line the astral field have been
warped and twisted into portals to other worlds.  However, all but one of these
appears to be sealed by powerful magics.
~
#EXIT
Direction north~
ToRoom    846
Desc      The astral field leads on into the depths.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    830
Desc      The astral field leads on into infinity.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    849
Desc      The astral field leads on into the depths.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    850
Desc      Through the nexial link you can see a stony keep.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    828
Desc      The astral field leads on into infinity.
~
#ENDEXIT

Reset M 0 815 3 845
  Reset G 0 821 0
#ENDROOM

#ROOM
Vnum     846
Name     Deep Within the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You have travelled deep inside the astral field.  Here the fabric of the
field has become twisted and warped due to the proximity of another plane of
existence.  Some of the tendrils of light that line the astral field have been
warped and twisted into portals to other worlds.  However, these appear to be
forever sealed by powerful magics.
~
#EXIT
Direction north~
ToRoom    833
Desc      The astral field leads on into infinity.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    845
Desc      The astral field leads on into the depths.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    847
Desc      The astral field leads on into the depths.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    829
Desc      The astral field leads on into infinity.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    831
Desc      The astral field leads on into infinity.
~
#ENDEXIT

Reset M 0 802 1 846
  Reset G 0 805 0
#ENDROOM

#ROOM
Vnum     847
Name     Deep Within the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You have travelled deep inside the astral field.  Here the fabric of the
field has become twisted and warped due to the proximity of another plane of
existence.  Some of the tendrils of light that line the astral field have been
warped and twisted into portals to other worlds.  However, these appear to be
forever sealed by powerful magics.
~
#EXIT
Direction north~
ToRoom    834
Desc      The astral field leads on into infinity.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    846
Desc      The astral field leads on into the depths.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    830
Desc      The astral field leads on into infinity.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    828
Desc      The astral field leads on into infinity.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    848
Desc      The astral field leads on into the depths.
~
#ENDEXIT

Reset M 0 815 3 847
  Reset G 0 821 0
#ENDROOM

#ROOM
Vnum     848
Name     Deep Within the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You have travelled deep inside the astral field.  Here the fabric of the
field has become twisted and warped due to the proximity of another plane of
existence.  Some of the tendrils of light that line the astral field have been
warped and twisted into portals to other worlds.  However these appear to be
sealed forever by powerful magics.
~
#EXIT
Direction north~
ToRoom    835
Desc      The astral field leads on into infinity.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    831
Desc      The astral field leads on into infinity.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    849
Desc      The astral field leads on into the depths.
~
#ENDEXIT

#EXIT
Direction up~
ToRoom    847
Desc      The astral field leads on into the depths.
~
#ENDEXIT

#EXIT
Direction down~
ToRoom    832
Desc      The astral field leads on into infinity.
~
#ENDEXIT

#ENDROOM

#ROOM
Vnum     849
Name     Deep Within the Astral Plane~
Sector   field~
Flags    indoors~
Desc     You have travelled deep inside the astral field.  Here the fabric of the
field has become twisted and warped due to the proximity of another plane of
existence.  Some of the tendrils of light that line the astral field have been
warped and twisted into portals to other worlds.  However, these appear to be
forever sealed by powerful magics.
~
#EXIT
Direction north~
ToRoom    848
Desc      The astral field leads on into the depths.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    845
Desc      The astral field leads on into the depths.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    832
Desc      The astral field leads on into the depths.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    829
Desc      The astral field leads on into infinity.
~
#ENDEXIT

Reset M 0 815 3 849
  Reset G 0 821 0
#ENDROOM

#ROOM
Vnum     850
Name     Inside the Githyanki Keep~
Sector   inside~
Flags    nomob indoors~
Desc     You are standing just inside a nexial portal that leads into the
Astral Plane.  It seems that you have found the lair of the Githyanki,
the once-human followers of Lady Gith.  Ahead of you stands a steel door
leading into the keep hewn from stone.
~
#EXIT
Direction north~
ToRoom    851
Desc      You see a rusty steel door which leads into the keep.
~
Keywords  steel~
Flags     isdoor closed~
#ENDEXIT

#EXIT
Direction down~
ToRoom    845
Desc      Below you is the nexial portal that leads into the astral plane.
~
#ENDEXIT

Reset D 0 850 0 1
#EXDESC
ExDescKey    steel~
ExDesc       The door is made from a set of rusting steel plates set deep into the stone.
~
#ENDEXDESC

#ENDROOM

#ROOM
Vnum     851
Name     A guard station~
Sector   inside~
Flags    indoors~
Desc     This is a small building just inside the keep.  Doors lead out to the east
and west.  Some small weapons hang on hooks on the walls, for use in defense
of the keep.  Doors stand in all directions save north.
~
#EXIT
Direction east~
ToRoom    861
Desc      You see an iron door which leads out onto a grey stone road.
~
Keywords  door iron~
Flags     isdoor closed~
#ENDEXIT

#EXIT
Direction south~
ToRoom    850
Desc      You see a rusty steel door which leads out to the nexial link.
~
Keywords  steel~
Flags     isdoor closed~
#ENDEXIT

#EXIT
Direction west~
ToRoom    852
Desc      You see an iron door which leads out onto a grey stone road.
~
Keywords  door iron~
Flags     isdoor closed~
#ENDEXIT

Reset D 0 851 2 1
Reset D 0 851 1 1
Reset D 0 851 3 1
Reset M 0 807 8 851
  Reset E 0 808 0 16
  Reset E 0 815 0 5
Reset M 0 807 8 851
  Reset E 0 808 0 16
  Reset E 0 815 0 5
#EXDESC
ExDescKey    steel~
ExDesc       The door is made from a set of rusting steel plates set deep into the stone.
~
#ENDEXDESC

#EXDESC
ExDescKey    door iron~
ExDesc       The door is made from a set of iron plates set deep into the stone.
~
#ENDEXDESC

#ENDROOM

#ROOM
Vnum     852
Name     A grey road~
Sector   city~
Flags    indoors~
Desc     The road beneath your feet is of plain grey cobblestone.  A dull red
sun hangs in the orange sky above your head, strangely still.  You hear
none of the sounds you would commonly associate with a normal city.  The
cobblestone road turns to the north, while to the east is a strong iron door.
~
#EXIT
Direction north~
ToRoom    853
Desc      The grey cobblestones continue.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    851
Desc      You see an iron door which leads into a small stone building set into the wall
of the keep.
~
Keywords  door iron~
Flags     isdoor closed~
#ENDEXIT

Reset D 0 852 1 1
#EXDESC
ExDescKey    door iron~
ExDesc       The door is made from a set of iron plates set deep into the stone.
~
#ENDEXDESC

#ENDROOM

#ROOM
Vnum     853
Name     A grey road~
Sector   city~
Flags    indoors~
Desc     The road beneath your feet is of plain grey cobblestone.  A dull red sun
hangs in the orange sky above your head, strangely still.  You hear none
of the sounds you would commonly associate with a normal city.  The 
cobblestone road continues to the north and south, while doors to the east
and west allow you to enter two stone buildings.
~
#EXIT
Direction north~
ToRoom    854
Desc      The grey cobblestones continue.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    862
Desc      You see a small stone building with a wooden door.
~
Keywords  door wooden~
Flags     isdoor closed~
#ENDEXIT

#EXIT
Direction south~
ToRoom    852
Desc      The grey cobblestones continue.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    869
Desc      An iron door blocks your entrance into a large stone building.
~
Keywords  door iron~
Flags     isdoor closed~
#ENDEXIT

Reset D 0 853 1 1
Reset D 0 853 3 1
Reset M 0 811 6 853
  Reset E 0 806 0 16
#EXDESC
ExDescKey    door wooden~
ExDesc       The door is thick, made from a knotted and twisted greyish wood.
~
#ENDEXDESC

#EXDESC
ExDescKey    door iron~
ExDesc       The door is fashioned from many iron plates and set deep into the stone wall.
~
#ENDEXDESC

#ENDROOM

#ROOM
Vnum     854
Name     A grey road~
Sector   city~
Flags    indoors~
Desc     The road beneath your feet is of plain grey cobblestone.  A dull red sun
hangs in the orange sky above your head, strangely still.  You hear none
of the sounds you would commonly associate with a normal city.  The
cobblestone road extends to the east and south.
~
#EXIT
Direction east~
ToRoom    855
Desc      The grey cobblestones continue.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    853
Desc      The grey cobblestones continue.
~
#ENDEXIT

#ENDROOM

#ROOM
Vnum     855
Name     A grey road~
Sector   city~
Flags    indoors~
Desc     The road beneath your feet is of plain grey cobblestone.  A dull red sun
hangs in the orange sky above your head, strangely still.  You hear none
of the sounds you would commonly associate with a normal city.  The 
cobblestone extends to the east and west, while two double doors to the
north allow you to enter an enormous building.
~
#EXIT
Direction north~
ToRoom    874
Desc      Looming before you is a forbidding pair of metal doors.
~
Keywords  doors metal~
Flags     isdoor closed~
#ENDEXIT

#EXIT
Direction east~
ToRoom    856
Desc      The grey cobblestones continue.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    854
Desc      The grey cobblestones continue.
~
#ENDEXIT

Reset D 0 855 0 1
Reset M 0 808 3 855
  Reset E 0 809 0 16
  Reset E 0 815 0 5
  Reset E 0 816 0 7
  Reset E 0 817 0 10
#EXDESC
ExDescKey    doors metal~
ExDesc       The doors are formed from a silvery substance that feels very hard yet almost
slippery to the touch.  They are covered in images in bas-relief, depicting
scenes of combat between humans and strange tentacle-mouthed humanoids.
~
#ENDEXDESC

#ENDROOM

#ROOM
Vnum     856
Name     A grey road~
Sector   city~
Flags    indoors~
Desc     The ground beneath your feet is of plain grey cobblestone.  A dull red sun
hangs in the orange sky above your head, strangely still.  You hear none
of the sounds you would commonly associate with a normal city.  The
cobblestone road ranges to the east and west, while a door to the south
leads into a tall stone building.
~
#EXIT
Direction east~
ToRoom    857
Desc      The grey cobblestones continue.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    864
Desc      You see a tall stone building with a wooden door.
~
Keywords  door wooden~
Flags     isdoor closed~
#ENDEXIT

#EXIT
Direction west~
ToRoom    855
Desc      The grey cobblestones continue.
~
#ENDEXIT

Reset D 0 856 2 1
#EXDESC
ExDescKey    door wooden~
ExDesc       The door is thick, made from a knotted and twisted greyish wood.
~
#ENDEXDESC

#ENDROOM

#ROOM
Vnum     857
Name     A grey road~
Sector   city~
Flags    indoors~
Desc     The ground beneath your feet is of plain grey cobblestone.  A dull red sun
hangs in the orange sky above your head, strangely still.  You hear none
of the sounds you would commonly associate with a normal city.  The
cobblestone road lies to the south and west, while a door to the north leads
into a tall stone building.
~
#EXIT
Direction north~
ToRoom    873
Desc      You see a tall stone building with a wooden door.
~
Keywords  door wooden~
Flags     isdoor closed~
#ENDEXIT

#EXIT
Direction south~
ToRoom    858
Desc      The grey cobblestones continue.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    856
Desc      The grey cobblestones continue.
~
#ENDEXIT

Reset D 0 857 0 1
#EXDESC
ExDescKey    door wooden~
ExDesc       The door is thick, made from a knotted and twisted greyish wood.
~
#ENDEXDESC

#ENDROOM

#ROOM
Vnum     858
Name     A grey road~
Sector   city~
Flags    indoors~
Desc     The ground beneath your feet is of plain grey cobblestone.  A dull red sun
hangs in the orange sky above your head, strangely still.  You hear none 
of the sounds you would commonly associate with a normal city.  The
cobblestone road lies to the north and south, while a door is set in a large
stone building to the east.
~
#EXIT
Direction north~
ToRoom    857
Desc      The grey cobblestones continue.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    867
Desc      An iron door blocks your entrance into a large stone building.
~
Keywords  door iron~
Flags     isdoor closed~
#ENDEXIT

#EXIT
Direction south~
ToRoom    859
Desc      The grey cobblestones continue.
~
#ENDEXIT

Reset D 0 858 1 1
#EXDESC
ExDescKey    door iron~
ExDesc       The door is made from many hammered iron plates, set deep into the stone wall.
~
#ENDEXDESC

#ENDROOM

#ROOM
Vnum     859
Name     A grey road~
Sector   city~
Flags    indoors~
Desc     The road beneath your feet is of plain grey cobblestone.  A dull red sun
hangs in the orange sky above your head, strangely still.  You hear none
of the sounds you would commonly associate with a normal city.  The
cobblestone road continues to the north and south.
~
#EXIT
Direction north~
ToRoom    858
Desc      The grey cobblestones continue.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    860
Desc      The grey cobblestones continue.
~
#ENDEXIT

Reset M 0 811 6 859
  Reset E 0 806 0 16
#ENDROOM

#ROOM
Vnum     860
Name     A grey road~
Sector   city~
Flags    indoors~
Desc     The road beneath your feet is of plain grey cobblestone.  A dull red sun
hangs in the orange sky above your head, strangely still.  You hear none
of the sounds you would commonly associate with a normal city.  The
cobblestone road lies to the north and west, while to the east is a door
which leads into a large stone building.
~
#EXIT
Direction north~
ToRoom    859
Desc      The grey cobblestones continue.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    865
Desc      An iron door blocks your entrance into a large stone building.
~
Keywords  door iron~
Flags     isdoor closed~
#ENDEXIT

#EXIT
Direction west~
ToRoom    861
Desc      The grey cobblestones continue.
~
#ENDEXIT

Reset D 0 860 1 1
#EXDESC
ExDescKey    door iron~
ExDesc       The door is made from many hammered iron plates, set deep into the stone wall.
~
#ENDEXDESC

#ENDROOM

#ROOM
Vnum     861
Name     A grey road~
Sector   city~
Flags    indoors~
Desc     The road beneath your feet is of plain grey cobblestone.  A dull red sun
hangs in the orange sky above your head, strangely still.  You hear none
of the sounds you would commonly associate with a normal city.  The
cobblestone road stretches eastward, while doors to the north and west
allow entrance into two stone buildings.
~
#EXIT
Direction north~
ToRoom    863
Desc      You see a small stone building with a wooden door.
~
Keywords  door wooden~
Flags     isdoor closed~
#ENDEXIT

#EXIT
Direction east~
ToRoom    860
Desc      The grey cobblestones continue.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    851
Desc      You see an iron door which leads into a small stone building set into the wall
of the keep.
~
Keywords  door iron~
Flags     isdoor closed~
#ENDEXIT

Reset D 0 861 0 1
Reset D 0 861 3 1
#EXDESC
ExDescKey    door wooden~
ExDesc       The door is thick, made from a knotted and twisted greyish wood.
~
#ENDEXDESC

#EXDESC
ExDescKey    door iron~
ExDesc       The door is made from a set of iron plates set deep into the stone.
~
#ENDEXDESC

#ENDROOM

#ROOM
Vnum     862
Name     A small building~
Sector   inside~
Flags    indoors~
Desc     You have stepped inside a dimly lit building, which apparently serves as
living quarters for several githyanki gish.  The walls are heavy stone blocks, 
covered with panels of strangely knotted and twisted wood.  A couple of candles
burn dimly in sconces here and there.
~
#EXIT
Direction west~
ToRoom    853
Desc      The wooden door leads back out onto the grey cobblestone road.
~
Keywords  door wooden~
Flags     isdoor closed~
#ENDEXIT

Reset D 0 862 3 1
Reset M 0 811 6 862
  Reset E 0 806 0 16
Reset M 0 811 6 862
  Reset E 0 806 0 16
  Reset G 0 822 0
#EXDESC
ExDescKey    door wooden~
ExDesc       The door is thick, made from a knotted and twisted greyish wood.
~
#ENDEXDESC

#ENDROOM

#ROOM
Vnum     863
Name     A small building~
Sector   inside~
Flags    indoors~
Desc     You have stepped inside a dimly lit building, which apparently serves as
living quarters for several githyanki gish.  The walls are heavy stone blocks, 
covered with panels of strangely knotted and twisted wood.  A couple of candles
burn dimly in sconces here and there.
~
#EXIT
Direction south~
ToRoom    861
Desc      The wooden door leads back out onto the grey cobblestone road.
~
Keywords  door wooden~
Flags     isdoor closed~
#ENDEXIT

Reset D 0 863 2 1
Reset M 0 811 6 863
  Reset E 0 806 0 16
Reset M 0 811 6 863
  Reset E 0 806 0 16
  Reset G 0 822 0
#EXDESC
ExDescKey    door wooden~
ExDesc       The door is thick, made from a knotted and twisted greyish wood.
~
#ENDEXDESC

#ENDROOM

#ROOM
Vnum     864
Name     A large dwelling~
Sector   inside~
Flags    indoors~
Desc     You have stepped inside a dimly lit building with vaulted ceilings and high
windows.  The walls are heavy stone blocks that rise tens of feet above your
head, covered with panels of strangely knotted and twisted wood.  On the floor
are charcoal stencils of pentagrams and various other ominous sigils.
~
#EXIT
Direction north~
ToRoom    856
Desc      The wooden door leads back out onto the grey cobblestone road.
~
Keywords  door wooden~
Flags     isdoor closed~
#ENDEXIT

Reset D 0 864 0 1
Reset M 0 812 4 864
  Reset E 0 810 0 16
  Reset G 0 803 0
#EXDESC
ExDescKey    door wooden~
ExDesc       The door is thick, made from a knotted and twisted greyish wood.
~
#ENDEXDESC

#ENDROOM

#ROOM
Vnum     865
Name     The Githyanki Weaponry~
Sector   inside~
Flags    indoors~
Desc     You have entered a large chamber full of weapons in various states of
completion.  Strange glowing blocks of silvery metal are being pounded and
rolled out into deadly razor-sharp knives, daggers and swords.  The sounds 
of hammering and fierce waves of heat emerge from the archway to the north.
To the west is an iron door which leads back to the sole road of the githyanki 
keep.
~
#EXIT
Direction north~
ToRoom    866
Desc      You see a massive forgeroom, lit by a white-hot furnace.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    860
Desc      The iron door leads back out onto the grey cobblestone road.
~
Keywords  door iron~
Flags     isdoor closed~
#ENDEXIT

Reset D 0 865 3 1
Reset M 0 807 8 865
  Reset E 0 808 0 16
  Reset E 0 815 0 5
Reset M 0 807 8 865
  Reset E 0 808 0 16
  Reset E 0 815 0 5
#EXDESC
ExDescKey    door iron~
ExDesc       The door is made from many hammered iron plates, set deep into the stone wall.
~
#ENDEXDESC

#ENDROOM

#ROOM
Vnum     866
Name     The Forge~
Sector   inside~
Flags    indoors~
Desc     In the center of the room is a massive furnace which emits great gouts of
heat and a strange smell which comes from the melting of the silvery metal used
by the githyanki.  A large trough of icy water stands to the west, used for 
quenching hot metal.  Archways lie to the north and south, while the door to
the furnace lies to the east.
~
#EXIT
Direction north~
ToRoom    867
Desc      You see the armoury.
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    868
Desc      Through the grille in the furnace door you see only flames.
~
Keywords  door furnace~
Flags     isdoor closed~
#ENDEXIT

#EXIT
Direction south~
ToRoom    865
Desc      You see the weaponry.
~
#ENDEXIT

Reset D 0 866 1 1
#EXDESC
ExDescKey    door furnace~
ExDesc       The entire furnace is glowing white-hot to the touch.
The furnace door looks too hot to even attempt to open.
~
#ENDEXDESC

#ENDROOM

#ROOM
Vnum     867
Name     The Githyanki Armoury~
Sector   inside~
Flags    indoors~
Desc     You have entered a large chamber filled with suits of armor in various
states of completion.  In this chamber the hard steels of the githyanki are
shaped into ornately fashioned suits of mail.  Sounds of hammering and
waves of heat emerge from the archway to the south, while a door to the
west leads back out to the cobblestone road.
~
#EXIT
Direction south~
ToRoom    866
Desc      You see a massive forgeroom, lit by a white-hot furnace.
~
#ENDEXIT

#EXIT
Direction west~
ToRoom    858
Desc      The iron door leads back out onto the grey cobblestone road.
~
Keywords  door iron~
Flags     isdoor closed~
#ENDEXIT

Reset D 0 867 3 1
Reset M 0 807 8 867
  Reset E 0 808 0 16
  Reset E 0 815 0 5
Reset M 0 807 8 867
  Reset E 0 808 0 16
  Reset E 0 815 0 5
#EXDESC
ExDescKey    door iron~
ExDesc       The door is made from many hammered iron plates, set deep into the stone wall.
~
#ENDEXDESC

#ENDROOM

#ROOM
Vnum     868
Name     Inside the furnace~
Sector   inside~
Flags    indoors noastral~
Desc     You gasp for breath and choke on toxic sulphur and brimstone.  The sweat
on your forehead is instantly vaporized by the flames that surround you.  You
begin to feel faint and dizzy from the heat...
~
#EXIT
Direction west~
ToRoom    866
Desc      The flames blind you and obscure your vision.
~
Keywords  door furnace~
Flags     isdoor closed~
#ENDEXIT

Reset D 0 868 3 1
Reset M 0 818 1 868
  Reset E 0 813 0 0
Reset M 0 819 6 868
Reset M 0 819 6 868
Reset M 0 819 6 868
Reset M 0 819 6 868
Reset M 0 819 6 868
Reset M 0 819 6 868
#EXDESC
ExDescKey    e east~
ExDesc       The flames blind you and obscure your vision.
~
#ENDEXDESC

#ENDROOM

#ROOM
Vnum     869
Name     A guard station~
Sector   inside~
Flags    indoors~
Desc     You are standing in a small, fairly empty building that serves as the prison
for the keep.  The building is small because the githyanki do not make a habit 
of taking prisoners and those that they do capture do not last long.  The
cells lie to the west, while the grey cobblestone road lies beyond a door
to the east.
~
#EXIT
Direction east~
ToRoom    853
Desc      You see an iron door which leads back out onto the grey cobblestone road.
~
Keywords  door iron~
Flags     isdoor closed~
#ENDEXIT

#EXIT
Direction west~
ToRoom    870
Desc      You see the cellblock.
~
#ENDEXIT

Reset D 0 869 1 1
Reset M 0 807 8 869
  Reset E 0 808 0 16
  Reset E 0 815 0 5
Reset M 0 807 8 869
  Reset E 0 808 0 16
  Reset E 0 815 0 5
#EXDESC
ExDescKey    door iron~
ExDesc       The door is fashioned from many iron plates and set deep into the stone wall.
~
#ENDEXDESC

#ENDROOM

#ROOM
Vnum     870
Name     The cellblock~
Sector   inside~
Flags    indoors~
Desc     A torch firmly held in a sconce on the western wall illuminates this dim
corridor.  Recently used whips, manacles and chains hang on the walls, soaked
with many different types of blood.  Cell doors stand to the north and south,
while the guard station lies to the east.
~
#EXIT
Direction north~
ToRoom    871
Desc      You see a small cell.
~
Keywords  door cell~
Flags     isdoor closed~
#ENDEXIT

#EXIT
Direction east~
ToRoom    869
Desc      You see the guard station.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    872
Desc      You see a small cell.
~
Keywords  door cell~
Flags     isdoor closed~
#ENDEXIT

Reset D 0 870 0 1
Reset D 0 870 2 1
#EXDESC
ExDescKey    door cell~
ExDesc       The cell door is wooden and about 4' high, with a slot in the bottom to pass
food underneath to the prisoner.
~
#ENDEXDESC

#ENDROOM

#ROOM
Vnum     871
Name     A cramped cell~
Sector   inside~
Flags    indoors~
Desc     You are in a tiny, dank cell.  The chill air lashes at your flesh and
you wonder how long the githyanki expect the cell's occupant to
survive.  The only exit is back through the door to the south.
~
#EXIT
Direction south~
ToRoom    870
Desc      You see the cellblock.
~
Keywords  door cell~
Flags     isdoor closed~
#ENDEXIT

Reset D 0 871 2 1
Reset M 0 817 1 871
#EXDESC
ExDescKey    door cell~
ExDesc       The cell door is wooden and about 4' high, with a slot in the bottom to pass
food underneath to the prisoner.
~
#ENDEXDESC

#ENDROOM

#ROOM
Vnum     872
Name     A tiny cell~
Sector   inside~
Flags    indoors~
Desc     You are in a tiny, dank cell.  The chill air lashes at your flesh and
you wonder how long the githyanki expect the cell's occupant to
survive.  The only exit is back through the door to the north.
~
#EXIT
Direction north~
ToRoom    870
Desc      You see the cellblock.
~
Keywords  door cell~
Flags     isdoor closed~
#ENDEXIT

Reset D 0 872 0 1
Reset M 0 816 1 872
#EXDESC
ExDescKey    door cell~
ExDesc       The cell door is wooden and about 4' high, with a slot in the bottom to pass
food underneath to the prisoner.
~
#ENDEXDESC

#ENDROOM

#ROOM
Vnum     873
Name     A large dwelling~
Sector   inside~
Flags    indoors~
Desc     You have stepped inside a dimly lit building with vaulted ceilings and high
windows.  The walls are heavy stone blocks that rise tens of feet above your
head, covered with panels of strangely knotted and twisted wood.  On the floor
are charcoal stencils of pentagrams and various other ominous sigils.
~
#EXIT
Direction south~
ToRoom    857
Desc      The wooden door leads back out onto the grey cobblestone road.
~
Keywords  door wooden~
Flags     isdoor closed~
#ENDEXIT

Reset D 0 873 2 1
Reset M 0 812 4 873
  Reset E 0 810 0 16
  Reset G 0 803 0
#EXDESC
ExDescKey    door wooden~
ExDesc       The door is thick, made from a knotted and twisted greyish wood.
~
#ENDEXDESC

#ENDROOM

#ROOM
Vnum     874
Name     South End of the Hall~
Sector   inside~
Flags    indoors~
Desc     This is a massive stone hall, lit by flaming blue stones set into the walls.
Massive suits of armor line the walls, holding weapons fashioned from a strange
silvery metal.  The great hall continues to the north and ends in a pair
of huge metal doors to the south.
~
#EXIT
Direction north~
ToRoom    875
Desc      The great hall continues to the north.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    855
Desc      The metal doors lead out onto the grey cobblestone road.
~
Keywords  doors metal~
Flags     isdoor closed~
#ENDEXIT

Reset D 0 874 2 1
Reset M 0 812 4 874
  Reset E 0 810 0 16
  Reset G 0 803 0
Reset M 0 812 4 874
  Reset E 0 810 0 16
  Reset G 0 803 0
#EXDESC
ExDescKey    armor suits~
ExDesc       The suits of armor are very intricately and ornately fashioned splint mail.
~
#ENDEXDESC

#EXDESC
ExDescKey    weapons~
ExDesc       Massive pole arms, spears, mighty swords, all kinds of death-dealing devices.
~
#ENDEXDESC

#EXDESC
ExDescKey    doors metal~
ExDesc       The doors are formed from a silvery substance that feels very hard yet almost
slippery to the touch.  They are covered in images in bas-relief, depicting
scenes of combat between humans and strange tentacle-mouthed humanoids.
~
#ENDEXDESC

#ENDROOM

#ROOM
Vnum     875
Name     North End of the Hall~
Sector   inside~
Flags    indoors~
Desc     This is a massive stone hall, lit by flaming blue stones set into the walls.
Massive suits of armor line the walls, holding weapons fashioned from a strange
silvery metal.  The great hall continues to the south, while thick mists
to the north prevent further passage.
~
#EXIT
Direction north~
ToRoom    876
Desc      You think you can see some steps leading up to an altar...
~
Keywords  mist door~
Flags     isdoor closed~
#ENDEXIT

#EXIT
Direction south~
ToRoom    874
Desc      The great hall continues to the south.
~
#ENDEXIT

Reset D 0 875 0 1
#EXDESC
ExDescKey    armor suits~
ExDesc       The suits of armor are very intricately and ornately fashioned splint mail.
~
#ENDEXDESC

#EXDESC
ExDescKey    weapons~
ExDesc       Massive pole arms, spears, mighty swords, all kinds of death-dealing devices.
~
#ENDEXDESC

#EXDESC
ExDescKey    arch archway door~
ExDesc       The archway is shrouded in a strange black mist that obscures your vision.
~
#ENDEXDESC

#ENDROOM

#ROOM
Vnum     876
Name     The Altar Chamber~
Sector   inside~
Flags    dark indoors~
Desc     Your eyes have trouble adjusting to the dim lighting.  All around you are
statues of githyanki turned towards the altar which stands at the top of a
short flight of steps to the north.
~
#EXIT
Direction north~
ToRoom    877
Desc      The steps lead north and up to a massive sacrificial altar.
~
#ENDEXIT

#EXIT
Direction south~
ToRoom    875
Desc      You think you can see the hall.
~
Keywords  mist door~
Flags     isdoor closed~
#ENDEXIT

Reset D 0 876 2 1
Reset M 0 809 2 876
  Reset E 0 809 0 16
  Reset E 0 815 0 5
  Reset E 0 816 0 7
  Reset E 0 817 0 10
  Reset E 0 818 0 9
Reset M 0 809 2 876
  Reset E 0 809 0 16
  Reset E 0 815 0 5
  Reset E 0 816 0 7
  Reset E 0 817 0 10
  Reset E 0 818 0 9
#EXDESC
ExDescKey    arch archway door~
ExDesc       The archway is shrouded in a strange black mist that obscures your vision.
~
#ENDEXDESC

#ENDROOM

#ROOM
Vnum     877
Name     Before the Altar~
Sector   inside~
Flags    dark indoors nosummon noastral~
Desc     You have climbed the mighty steps of the altar chamber and stand before a
huge altar used to pay tribute to Lady Gith, the long-dead ruler of the
githyanki.  The altar is covered in blood and radiates an unspeakable
power that is absorbed by the horrible presence which stands before you.
~
#EXIT
Direction south~
ToRoom    876
Desc      The steps lie to the south, leading back down to the altar chamber floor.
~
#ENDEXIT

Reset M 0 813 1 877
  Reset E 0 811 0 16
  Reset G 1 824 1
  Reset G 0 802 0
#EXDESC
ExDescKey    altar~
ExDesc       The altar is a huge sacrificial altar covered in bloodstains.
~
#ENDEXDESC

#EXDESC
ExDescKey    blood bloodstains stains~
ExDesc       The stains of blood run down off the altar towards your feet, oozing quietly.
~
#ENDEXDESC

#EXDESC
ExDescKey    hole holes~
ExDesc       The holes have passed countless amounts of blood down below the altar.
~
#ENDEXDESC

#ENDROOM

#ROOM
Vnum     878
Name     A Gateway to the Underworld~
Sector   city~
Flags    dark indoors noastral~
Desc     You step through the tiny hole in the altar, and stand before a swirling
field of blackness.  Through the field you can hear the howls of lost souls
and long-dead beings.
~
#EXIT
Direction down~
ToRoom    879
Key       824
Desc      You look down into the field of blackness, and feel a tangible wave of evil wash
over you.  Stepping into the field would be a bad idea.
~
Keywords  gateway~
Flags     isdoor closed locked nopassdoor~
#ENDEXIT

Reset D 0 878 5 2
#EXDESC
ExDescKey    blood~
ExDesc       The blood drips from above your head and falls through the field...
~
#ENDEXDESC

#EXDESC
ExDescKey    field gateway~
ExDesc       The field pulses and feeds on the blood dripping from above.

A tangible wave of hatred washes over you, striking fear into your very soul.
~
#ENDEXDESC

#ENDROOM

#ROOM
Vnum     879
Name     Utter Darkness and Despair~
Sector   air~
Flags    dark nomob indoors norecall noastral~
Desc     A heavy darkness weighs upon this vast territory, reducing your vision to
mere feet.  Flittering shapes seem to emerge from the blackness then return
to the shadows in fear of your light.  Though there is no apparent danger,
waves of fear pass through your body and you remain on your guard...
~
Reset M 0 814 1 879
  Reset E 0 812 0 16
  Reset E 0 819 0 5
  Reset G 0 825 0
#ENDROOM

#ROOM
Vnum     880
Name     Entrance to Limbo~
Sector   city~
#EXIT
Direction west~
ToRoom    881
#ENDEXIT

#ENDROOM

#ROOM
Vnum     881
Name     Door to Githzerai Keep~
Sector   city~
#EXIT
Direction east~
ToRoom    880
#ENDEXIT

#EXIT
Direction northwest~
ToRoom    882
#ENDEXIT

#EXIT
Direction southwest~
ToRoom    898
#ENDEXIT

#ENDROOM

#ROOM
Vnum     882
Name     Path Skirting Keep~
Sector   city~
#EXIT
Direction north~
ToRoom    883
#ENDEXIT

#EXIT
Direction southeast~
ToRoom    881
#ENDEXIT

#ENDROOM

#ROOM
Vnum     883
Name     path~
Sector   city~
#EXIT
Direction south~
ToRoom    882
#ENDEXIT

#EXIT
Direction northwest~
ToRoom    884
#ENDEXIT

#ENDROOM

#ROOM
Vnum     884
Name     path~
Sector   city~
#EXIT
Direction northwest~
ToRoom    885
#ENDEXIT

#EXIT
Direction southeast~
ToRoom    883
#ENDEXIT

#ENDROOM

#ROOM
Vnum     885
Name     path~
Sector   city~
#EXIT
Direction west~
ToRoom    886
#ENDEXIT

#EXIT
Direction southeast~
ToRoom    884
#ENDEXIT

#ENDROOM

#ROOM
Vnum     886
Name     path~
Sector   city~
#EXIT
Direction east~
ToRoom    885
#ENDEXIT

#EXIT
Direction southwest~
ToRoom    887
#ENDEXIT

#ENDROOM

#ROOM
Vnum     887
Name     path~
Sector   city~
#EXIT
Direction northeast~
ToRoom    886
#ENDEXIT

#EXIT
Direction southwest~
ToRoom    888
#ENDEXIT

#ENDROOM

#ROOM
Vnum     888
Name     path~
Sector   city~
#EXIT
Direction south~
ToRoom    889
#ENDEXIT

#EXIT
Direction northeast~
ToRoom    887
#ENDEXIT

#ENDROOM

#ROOM
Vnum     889
Name     path~
Sector   city~
#EXIT
Direction north~
ToRoom    888
#ENDEXIT

#EXIT
Direction southwest~
ToRoom    890
#ENDEXIT

#ENDROOM

#ROOM
Vnum     890
Name     path~
Sector   city~
#EXIT
Direction northeast~
ToRoom    889
#ENDEXIT

#EXIT
Direction southeast~
ToRoom    891
#ENDEXIT

#ENDROOM

#ROOM
Vnum     891
Name     path~
Sector   city~
#EXIT
Direction south~
ToRoom    892
#ENDEXIT

#EXIT
Direction northwest~
ToRoom    890
#ENDEXIT

#ENDROOM

#ROOM
Vnum     892
Name     path~
Sector   city~
#EXIT
Direction north~
ToRoom    891
#ENDEXIT

#EXIT
Direction southeast~
ToRoom    893
#ENDEXIT

#ENDROOM

#ROOM
Vnum     893
Name     path~
Sector   city~
#EXIT
Direction northwest~
ToRoom    892
#ENDEXIT

#EXIT
Direction southeast~
ToRoom    894
#ENDEXIT

#ENDROOM

#ROOM
Vnum     894
Name     path~
Sector   city~
#EXIT
Direction east~
ToRoom    895
#ENDEXIT

#EXIT
Direction northwest~
ToRoom    893
#ENDEXIT

#ENDROOM

#ROOM
Vnum     895
Name     path~
Sector   city~
#EXIT
Direction west~
ToRoom    894
#ENDEXIT

#EXIT
Direction northeast~
ToRoom    896
#ENDEXIT

#ENDROOM

#ROOM
Vnum     896
Name     path~
Sector   city~
#EXIT
Direction northeast~
ToRoom    897
#ENDEXIT

#EXIT
Direction southwest~
ToRoom    895
#ENDEXIT

#ENDROOM

#ROOM
Vnum     897
Name     path~
Sector   city~
#EXIT
Direction north~
ToRoom    898
#ENDEXIT

#EXIT
Direction southwest~
ToRoom    896
#ENDEXIT

#ENDROOM

#ROOM
Vnum     898
Name     path~
Sector   city~
#EXIT
Direction south~
ToRoom    897
#ENDEXIT

#EXIT
Direction northeast~
ToRoom    881
#ENDEXIT

#ENDROOM

#ROOM
Vnum     899
Name     Floating in a void~
Sector   city~
Flags    nomob~
#ENDROOM

#ENDAREA
