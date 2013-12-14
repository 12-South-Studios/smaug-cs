-- THEVOID.LUA
-- This is the zone-file for The Void
-- Revised: 2013.11.23
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_area.lua")();

function LoadArea()
	LBootLog("=================== AREA 'THE VOID' INITIALIZING ===================");
	newArea = LCreateArea(1, "The Void");
	area.this = newArea;
	area.this.Author = "RoD";
	area.this.HighSoftRange = 60;
	area.this.HighHardRange = 60;
	area.this.HighEconomy = 33928447;
	area.this.ResetMessage = "A dim pulse of light filters through the swirling mists.";
	area.this.ResetFrequency = 60;
	area.this:SetFlags("noteleport");
	
	Mobs();
	Objects();
	SystemRooms();
	Arena();
	OtherRooms();
	
	LBootLog("=================== AREA 'THE VOID' - COMPLETED ================");
end

function Mobs()
	LBootLog("=================== AREA 'THE VOID' - MOBS ===================");
	mobile = CreateMobile(1, "translucent figure", "A translucent figure");
	mobile.LongDescription = "A translucent figure is here, contemplating a higher reality.";
	mobile.Class = "mage";
	mobile.ActFlags = "npc sentinel noattack";
	mobile.AffectedBy = "detect_invis detect_hidden sanctuary infrared protect truesight";
	mobile:SetStats1(1000, 50, 20, -30, 10000, 1000);
	mobile:SetStats2(5, 10, 30550);
	mobile:SetStats3(4, 10, 200);
	mobile.Speaks = "all";
	mobile.Speaking = "all";
	mobile:AddMudProg(CreateMudProg("rand_prog", "50", 
	[[
		MPAt(4, "MPPurge");
	]]));
	mobile:AddMudProg(CreateMudProg("rand_prog", "5", 
	[[ 
		MPE("To escape the Void, type a command or say 'return' ...");
	]]));
	
	mobile = CreateMobile(2, "demon imp", "A demon imp");
	mobile.LongDescription = "A demon imp hovers nearby...drooling constantly with a fiendish grin.";
	mobile.Description = [[This demon is clearly something that you don't want to mess with...
		It appears to be very agile, and very strong.]];
	mobile.Gender = "neuter";
	mobile.ActFlags = "detect_invis detect_hidden";
	mobile:SetStats1(-1000, 50, 1, -300, 10000, 155000);
	mobile:SetStats2(5, 10, 31550);
	mobile:SetStats3(1, 2, 2);
	mobile:AddMudProg(CreateMudProg("act_prog", "p disappears in a column of divine power", 
	[[ 
		local ch = LGetCurrentCharacter();
		if (LIsInRoom(ch, 6)) then
			MPTransfer(ch, 6);
			MPForce(ch, "look");
		else 
			MPTransfer(ch, 8);
		end
	]]));
	mobile:AddMudProg(CreateMudProg("act_prog", "p has entered the game", 
	[[
		local ch = LGetCurrentCharacter();
		if (not LIsInRoom(ch, 8) and not LIsImmortal(ch)) then
			MEA(ch, "_yel Your time in hell has expired, and you have been released.");
			if (LIsPKill(ch)) then
				MPTransfer(ch, 3001);
			else 
				MPTransfer(ch, 21000);
			end
			MPAt(ch, "MPForce(ch, \"look\");");
		end
	]]));
	mobile:AddMudProg(CreateMudProg("act_prog", "p is incapacitated", 
	[[
		MPRestore(LGetCurrentCharacter(), 1000);
	]]));
	mobile:AddMudProg(CreateMudProg("act_prog", "p is mortally wounded", 
	[[ 
		MPRestore(LGetCurrentCharacter(), 1000);
	]]));
	mobile:AddMudProg(CreateMudProg("rand_prog", "1", 
	[[ 
		local ch = LGetCurrentCharacter();
		if (not LIsImmortal(ch) and LIsClass(ch, "vampire")) then
			MPForce(ch, "drink blood");
			MPRestore(ch, 100);
		end
	]]));
	mobile:AddMudProg(CreateMudProg("act_prog", "p is suffering from lack of blood", 
	[[
		local ch = LGetCurrentCharacter();
		MPForce(ch, "drink blood");
		MPRestore(ch, 500);
	]]));
	mobile:AddMudProg(CreateMudProg("act_prog", "p is DEAD", 
	[[ 
		local ch = LGetCurrentCharacter();
		if (LIsInRoom(ch, 6)) then
			MPTransfer(ch, 6);
		else 
			MPTransfer(ch, 8);
		end
	]]));
	mobile:AddMudProg(CreateMudProg("act_prog", "p wields", 
	[[
		MPRestore(LGetCurrentCharacter(), 500);
	]]));
	mobile:AddMudProg(CreateMudProg("act_prog", "p wears", 
	[[
		MPRestore(LGetCurrentCharacter(), 500);
	]]));
	mobile:AddMudProg(CreateMudProg("act_prog", "p shivers and", 
	[[
		local ch = LGetCurrentCharacter();
		LMobCastSpell("cure poison", ch);
		MPRestore(ch, 500);
	]]));
	mobile:AddMudProg(CreateMudProg("act_prog", "p bashes against", 
	[[
		MPRestore(LGetCurrentCharacter(), 500);
	]]));
	mobile:AddMudProg(CreateMudProg("act_prog", "p is starved", 
	[[
		local ch = LGetCurrentCharacter();
		local obj = MPOLoad(20);
		
		LMobCommand("give mushroom $n", ch);
		MPForce(ch, "eat mushroom");
		MPRestore(ch, 500);
	]]));
	
	mobile = CreateMobile(3, "supermob", "the supermob");
	mobile.LongDescription = "The supermob is here.  He looks busy as hell.";
	mobile.Description = "How clever he looks!";
	mobile.Gender = "neuter";
	mobile.ActFlags = "npc polyself secretive mobinvis prototype";
	mobile.AffectedBy = "invisible detect_invis detect_hidden hide truesight";
	mobile:SetStats1(-1000, 56, 1, -300, 10000, 155000);
	mobile:SetStats2(1, 2, 2);
	mobile:SetAttributes(13, 13, 13, 13, 13, 13, 25);
	mobile.Immunity = "blunt pierce slash sleep charm nonmagic paralysis";
	mobile.Defenses = "parry dodge";
	mobile:AddMudProg(CreateMudProg("rand_prog", "10", 
	[[
		MPInvis(51);
		if (not LIsMobInvisible(LGetMobile(3)) then
			MPInvis();
		end
	]]));
	
--[[
#MOBILE
Vnum       4
Keywords   fharlangan god traveler~
Short      Fharlangan~
Long       A man desiring nothing more than to be allowed to travel is here.
~
Desc       The eternal wanderer is before you.  His clothes are threadbare and
his boots weathered, yet his eyes possess the light of youth.  He
goes throughout the world now always wandering, never tarrying.
~
Race       god~
Class      mage~
Position   standing~
DefPos     standing~
Gender     male~
Actflags   npc scavenger~
Affected   detect_invis detect_hidden sanctuary infrared protect truesight~
Stats1     600 50 0 -200 150000 0
Stats2     100 50 3500
Stats3     4 10 14
Stats4     0 0 0 0 0
Attribs    25 20 20 18 17 13 25
Saves      0 0 0 0 0
Speaks     common halfling~
Speaking   common halfling~
Resist     pierce slash~
Immune     fire cold electricity sleep charm paralysis~
Suscept    blunt~
Attacks    punch kick trip harm fireball~
Defenses   dodge~
#MUDPROG
Progtype  rand_prog~
Arglist   3~
Comlist   mpat 3 c heal
, mumbles and runs his hands over his arms.
~
#ENDPROG

#MUDPROG
Progtype  rand_prog~
Arglist   10~
Comlist   wear all
drop all.ball
drop all.mushroom
~
#ENDPROG

#MUDPROG
Progtype  fight_prog~
Arglist   15~
Comlist   mpasound The nearby sound of a god doing battle can be heard clearly.
mpe $I gathers his godly might into one blow and strikes out!
mpe $I's blast of magic rips through the air!
mpdamage $n 500
~
#ENDPROG

#MUDPROG
Progtype  rand_prog~
Arglist   5~
Comlist   c teleport
mpasound You feel the power of an immortal close by...
mpe With a clap of thunder, Fharlangan strides out of a cloud of lightning.
if level($r) == 2
  mpadvance $r 3
  mea $r You have been chosen by me to carry my word throughout the Realms!
  mer $r $I points an aged finger at $r, who suddenly looks different!
  c bless $n
  c refresh $n
  c 'heal' $n
  c sanc $n
endif
~
#ENDPROG

#ENDMOBILE

#MOBILE
Vnum       5
Keywords   undead animated corpse~
Short      an animated corpse~
Long       An animated corpse struggles with the horror of its undeath.
~
Desc       Denied its rightful death, this animated corpse has been infused with the
powerful energies of its master.  It exists on the precipice between life
and unlife, even as its physical body rots and decays under the strain of
its tasks.
~
Race       human~
Class      warrior~
Position   standing~
DefPos     standing~
Gender     neuter~
Actflags   npc~
Stats1     0 1 0 0 0 0
Stats2     10 0 2
Stats3     0 0 0
Stats4     0 0 0 0 0
Attribs    13 13 13 13 13 13 13
Saves      0 0 0 0 0
Speaks     common~
Speaking   common~
Bodyparts  head arms legs~
Defenses   dodge~
#ENDMOBILE

#MOBILE
Vnum       10
Keywords   wolf~
Short      a deadly wolf~
Long       A deadly wolf prowls around you with a vengeance
~
Desc       While this large beast circles you, you have a chance to see its large          
fangs and its sharp claws.  The wolf's eyes are small, black and have           
the aura of a deadly intent.  Its raggedy coat tells of a recent battle         
with something.
~
Race       vampire~
Class      vampire~
Position   standing~
DefPos     standing~
Gender     neuter~
Actflags   npc prototype~
Stats1     0 1 0 0 0 0
Stats2     0 0 0
Stats3     0 0 0
Stats4     0 0 2 0 0
Attribs    13 13 13 13 13 13 13
Saves      0 0 0 0 0
Speaks     common~
Speaking   common~
Attacks    bite claws~
#ENDMOBILE

#MOBILE
Vnum       11
Keywords   mist~
Short      mist~
Long       A thick mist plays with your senses.
~
Desc       Is it the fog you see?  Or are your eyes playing deadly tricks on you.  The
mist swirls around you and then quickly disappears before you can blink.  You
suddenly find yourself overcome with an unknown power.
~
Race       magical~
Class      warrior~
Position   standing~
DefPos     standing~
Gender     neuter~
Actflags   npc prototype~
Affected   sneak floating~
Stats1     0 1 0 0 0 0
Stats2     0 0 0
Stats3     0 0 0
Stats4     0 0 2 0 0
Attribs    13 13 13 13 13 13 13
Saves      0 0 0 0 0
Speaks     common~
Speaking   common~
Attacks    blindness curse~
#ENDMOBILE

#MOBILE
Vnum       12
Keywords   bat~
Short      bat~
Long       A bat hovers nearby with bloody fangs.
~
Desc       As this bloodsucking creature flies past you, you notice the tiny black
eyes that stare at your jugular vein.  This little black bat has the
intentions of drinking your blood until there is none left in your body.
~
Race       bat~
Class      warrior~
Position   standing~
DefPos     standing~
Gender     neuter~
Actflags   npc prototype~
Affected   flying~
Stats1     0 1 0 0 0 0
Stats2     0 0 0
Stats3     0 0 0
Stats4     0 0 2 0 0
Attribs    13 13 13 13 13 13 13
Saves      0 0 0 0 0
Speaks     common~
Speaking   common~
Attacks    bite claws~
#ENDMOBILE

#MOBILE
Vnum       13
Keywords   hawk~
Short      hawk~
Long       A hawk watches you with predatory eyes from its perch.
~
Desc       This proud creature stands upon its perch watching you with unblinking
eyes.  Any movement made by you could make it flare its wings in disapproval
and open its beak to scream its intent.  Its long beak open and before you
know what has happened, it comes flying towards you.
~
Race       avis~
Class      warrior~
Position   standing~
DefPos     standing~
Gender     neuter~
Actflags   npc prototype~
Affected   flying~
Stats1     0 1 0 0 0 0
Stats2     0 0 0
Stats3     0 0 0
Stats4     0 0 2 0 0
Attribs    13 13 13 13 13 13 13
Saves      0 0 0 0 0
Speaks     common~
Speaking   common~
Attacks    claws~
#ENDMOBILE

#MOBILE
Vnum       14
Keywords   black cat~
Short      black cat~
Long       A black cat has crossed your path with deadly intent.
~
Desc       With a furry black coat, and big yellow eyes, this cat gracefully walks
across your path, bringing you several years of bad luck.  Its tail moves
slowly to tell you that it is laughing at your misfortune.
~
Race       cat~
Class      warrior~
Position   standing~
DefPos     standing~
Gender     neuter~
Actflags   npc prototype~
Affected   detect_invis~
Stats1     0 1 0 0 0 0
Stats2     0 0 0
Stats3     0 0 0
Stats4     0 0 2 0 0
Attribs    13 13 13 13 13 13 13
Saves      0 0 0 0 0
Speaks     common~
Speaking   common~
Attacks    bite claws~
#ENDMOBILE

#MOBILE
Vnum       15
Keywords   dove~
Short      dove~
Long       A beautiful dove sings a melancholy melody from its nest.
~
Desc       With feathers the colour of soft grey and soft loving black eyes, this small
delicate creature sings you a tune full of romance.  The tune it sings comes
from the small breast that rises and falls with every chorus it sings.  You
are at peace as you listen to this lovable creature.
~
Race       avis~
Class      warrior~
Position   standing~
DefPos     standing~
Gender     neuter~
Actflags   npc prototype~
Affected   flying~
Stats1     0 1 0 0 0 0
Stats2     0 0 0
Stats3     0 0 0
Stats4     0 0 1 0 0
Attribs    13 13 13 13 13 13 13
Saves      0 0 0 0 0
Speaks     common~
Speaking   common~
Attacks    claws~
#ENDMOBILE

#MOBILE
Vnum       16
Keywords   fish~
Short      fish~
Long       A fish blows large bubbles as it quickly swims past you.
~
Desc       With fins, teeth and gills, this slippery little thing is hard to catch.
It swims with a speed no beast, human or animal can compete with.  It's
tiny eyes, move with every breath.  It blows several large bubbles as it
quickly swims past you.
~
Race       fish~
Class      warrior~
Position   standing~
DefPos     standing~
Gender     neuter~
Actflags   npc prototype~
Affected   aqua_breath~
Stats1     0 1 0 0 0 0
Stats2     0 0 0
Stats3     0 0 0
Stats4     0 0 1 0 0
Attribs    13 13 13 13 13 13 13
Saves      0 0 0 0 0
Speaks     common~
Speaking   common~
#ENDMOBILE

#MOBILE
Vnum       17
Keywords   avatar~
Short      the avatar of %s~
Long       The daunting and powerful avatar of %s occupies the ground here.
~
Desc       The avatar of %s appears calm and cool, ready for whatever might come.
~
Race       humanoid~
Class      warrior~
Position   standing~
DefPos     standing~
Gender     neuter~
Actflags   npc sentinel noassist~
Stats1     0 1 0 0 0 0
Stats2     5 5 25
Stats3     5 5 26
Stats4     0 0 0 20 0
Attribs    25 25 25 25 25 25 25
Saves      0 0 0 0 0
Speaks     common~
Speaking   common~
Bodyparts  head arms legs heart brains guts hands feet fingers ear eye~
Immune     sleep~
#MUDPROG
Progtype  speech_prog~
Arglist   depart~
Comlist   bow
, disappears in a puff of smoke.
mpgoto 4
~
#ENDPROG

#MUDPROG
Progtype  rand_prog~
Arglist   5~
Comlist   mpecho With its task complete, the avatar departs in a puff of smoke.
mpgoto 4
~
#ENDPROG

#MUDPROG
Progtype  death_prog~
Arglist   100~
Comlist   mpecho The spirit of the avatar rises skyward to be joined with its master,
mpecho while its corporeal form falls lifelessly to the ground.
~
#ENDPROG

#ENDMOBILE

#MOBILE
Vnum       70
Keywords   chateau jules host~
Short      Jules~
Long       The executive of the Chateau stands here, looking very pompous.
~
Race       neanderthal~
Class      warrior~
Position   standing~
DefPos     standing~
Gender     male~
Actflags   npc sentinel stayarea immortal noassist pacifist~
Affected   detect_evil detect_invis detect_magic detect_hidden sanctuary~
Stats1     0 50 0 -300 0 0
Stats2     1 1 30000
Stats3     50 100 50
Stats4     0 0 5 60 60
Attribs    25 25 25 25 25 25 25
Saves      -30 -30 -30 -30 -30
Speaks     common elvish dwarven~
Speaking   elvish dwarven~
Immune     sleep charm paralysis~
Defenses   parry dodge~
#MUDPROG
Progtype  speech_prog~
Arglist   p i wish to dine~
Comlist   if sex($n) == 1
say Well then, right this way monsieur.
mpopenpassage 72 73 7
mea 0.$n $I escorts you into the dining room.
mer 0.$n $I escorts $n into the dining room.
mpforce $n nw
mpclosepassage 72 7
mpat 73 say Ah, here is a table for two.
mpat 73 say Enjoy your dinner, sir.
else
if sex($n) == 2
smile
say Right this way madame $n.
mpopenpassage 72 73 7
mea 0.$n $I escorts you into the dining room.
mer 0.$n $I escorts $n into the dining room.
mpforce $n nw
mpclosepassage 72 7
mpat 73 say Ah, here is a table for two.
mpat 73 say Enjoy your dinner madame.
endif
endif
~
#ENDPROG

#MUDPROG
Progtype  all_greet_prog~
Arglist   100~
Comlist   if sex($n) == 1
say Ah, bonjour monsieur $n, welcome to the Chateau L'amour!
wink
say If you wish to dine, please speak up sir.
endif
if sex($n) == 2
say ooOOooOOooOOoo la la! Welcome madame $n!
hand $n
say If you wish to dine madame, just say so.
endif
~
#ENDPROG

#ENDMOBILE

#MOBILE
Vnum       71
Keywords   chateau pierre waiter~
Short      Pierre~
Long       A Chateau waiter stands here, looking very preoccupied.
~
Race       halfling~
Class      warrior~
Position   standing~
DefPos     standing~
Gender     male~
Actflags   npc sentinel stayarea immortal noassist pacifist~
Affected   detect_evil detect_invis detect_magic detect_hidden sanctuary infrared floating~
Stats1     0 50 0 -300 0 0
Stats2     1 1 30000
Stats3     50 100 50
Stats4     0 0 5 60 60
Attribs    25 25 25 25 25 25 25
Saves      -30 -30 -30 -30 -30
Speaks     common halfling~
Speaking   halfling~
Immune     sleep charm paralysis~
Defenses   parry dodge~
#MUDPROG
Progtype  speech_prog~
Arglist   p I wish to go to the room~
Comlist   if ispc($n)
ooo
say Ah yes, right this way.
mpopenpassage 73 74 4
mpforce $n u
mpclosepassage 73 4
mpat 74 say Enjoy!
endif
~
#ENDPROG

#MUDPROG
Progtype  speech_prog~
Arglist   p I will have the fillet mignon~
Comlist   say Ah excellent!
emote dashes to the kitchen and comes back holding plates.
mpoload 70
give fillet-mignon-steak $n
mpoload 71
give glass-sherry-wine $n
say If you wish to go to your room after you've eaten, just say so.
wink
~
#ENDPROG

#MUDPROG
Progtype  speech_prog~
Arglist   waiter!~
Comlist   if ispc($n)
say Ah bonjour! welcome to the Chateau "Cabaret"
say Our special this evening is fillet mignon...
say If you want this delectable entree, just say so.
smile
endif
~
#ENDPROG

#ENDMOBILE

#MOBILE
Vnum       72
Keywords   albatross~
Short      An Albatross~
Long       An Albatross lurkes in the darkness, here.
~
Race       human~
Class      warrior~
Position   standing~
DefPos     standing~
Gender     neuter~
Actflags   npc sentinel~
Affected   detect_invis detect_hidden sanctuary truesight~
Stats1     0 51 0 0 0 0
Stats2     500 0 30000
Stats3     100 0 1000
Stats4     0 0 0 0 0
Attribs    13 13 13 13 13 13 13
Saves      0 0 0 0 0
Speaks     common~
Speaking   common~
#MUDPROG
Progtype  speech_prog~
Arglist   p get mail~
Comlist   mpe _yel The Albatross runs to the post office to fetch the mail.
mpoload 21048
~
#ENDPROG

#MUDPROG
Progtype  speech_prog~
Arglist   p get paper~
Comlist   mpe _yel The Albatross rushes quickly to fetch the paper.
mpoload 31985
~
#ENDPROG

#ENDMOBILE

#MOBILE
Vnum       80
Keywords   guardian vampire~
Short      a guardian vampire~
Long       A vampire is here hiding his face from your light source.
~
Desc       Before you stands a guardian vampire, looking quite evil to say the least.
The hair on your arms is raised by its presence alone, and it seems to you
that he appears too formidable to be a minor minion.
~
Race       undead~
Class      warrior~
Position   standing~
DefPos     standing~
Gender     male~
Actflags   npc sentinel aggressive~
Affected   detect_invis detect_hidden~
Stats1     -1000 25 0 -4 0 105000
Stats2     1 1 385
Stats3     4 5 15
Stats4     0 0 0 0 0
Attribs    13 13 13 13 13 13 13
Saves      0 0 0 0 0
Speaks     common~
Speaking   common~
Bodyparts  head arms legs hands feet ear eye~
Attacks    drain curse~
#MUDPROG
Progtype  fight_prog~
Arglist   60~
Comlist   if isnpc($n)
  , shrieks in rage and vanishes ...
  mpgoto 4
endif
~
#ENDPROG

#ENDMOBILE

#MOBILE
Vnum       89
Keywords   Owl Hooters~
Short      Hooters the Owl~
Long       An owl with large wide eyes rests on his perch, hooting madly.
~
Race       human~
Class      warrior~
Position   standing~
DefPos     standing~
Gender     neuter~
Actflags   npc~
Stats1     0 1 0 0 0 0
Stats2     10 10 30000
Stats3     40 40 1000
Stats4     0 0 4 0 0
Attribs    13 13 13 13 13 13 13
Saves      0 0 0 0 0
Speaks     common~
Speaking   common~
#ENDMOBILE

#MOBILE
Vnum       99
Keywords   final mob~
Short      a newly created final mob~
Long       Some god abandoned a newly created final mob here.
~
Race       human~
Class      warrior~
Position   standing~
DefPos     standing~
Gender     neuter~
Actflags   npc prototype~
Stats1     0 1 0 0 0 0
Stats2     0 0 0
Stats3     0 0 0
Stats4     0 0 0 0 0
Attribs    13 13 13 13 13 13 13
Saves      0 0 0 0 0
Speaks     common~
Speaking   common~
#ENDMOBILE
--]]
end

function Objects()
	LBootLog("=================== AREA 'THE VOID' - COMMON OBJECTS ===================");
	object = CreateObject(2, "coin gold", "money");
	object.ShortDescription = "a gold coin";
	object.LongDescription = "One miserable gold coin.";
	object:SetStats(1, 0, 0, 0, 0);
	object:SetValues(1, 0, 0, 0, 0);
	
	object = CreateObject(3, "coins gold", "money");
	object.ShortDescription = "%d gold coins";
	object.LongDescription = "A pile of gold coins.";
	object:SetStats(1, 0, 0, 0, 0);
	
	object = CreateObject(7, "board marks", "trash");
	object.ShortDescription = "the Marks Board";
	object.LongDescription = "A large bullseye riddled with arrows is here.";
	object:SetStats(1, 0, 0, 0, 0);
	
	object = CreateObject(8, "board vnum area", "trash");
	object.ShortDescription = "the Vnum/Area Board";
	object.LongDescription = "The Vnum/Area Board is hanging on the wall here.";
	object.Flags = "prototype";
	object:SetStats(1, 0, 0, 0, 0);
	
	object = CreateObject(9, "board vnum", "trash");
	object.ShortDescription = "the Vnum Board";
	object.LongDescription = "A small bulletin board is here.";
	object:SetStats(1, 0, 0, 0, 0);
	
	object = CreateObject(10, "corpse", "corpse");
	object.ShortDescription = "the corpse of %s";
	object.LongDescription = "The corpse of %s lies here.";
	object:SetValues(0, 0, 0, 1, 0, 0);
	object:SetStats(100, 0, 0, 0, 0);
	
	object = CreateObject(11, "corpse", "corpse_pc");
	object.ShortDescription = "the corpse of %s";
	object.LongDescription = "The corpse of %s lies here.";
	object:SetValues(0, 0, 0, 1, 0, 0);
	object:SetStats(100, 0, 0, 0, 0);
	
	object = CreateObject(12, "head", "cook");
	object.ShortDescription = "the decapitated head of %s";
	object.LongDescription = "The head of %s lies here, a vacant stare of shock on its face.";
	object.Action = "%s gobble$q down $p with gusto... disgusting!";
	object:SetValues(10, 0, 0, 0, 0, 0);
	object:SetStats(5, 0, 0, 0, 0);
	
	object = CreateObject(13, "heart", "cook");
	object.ShortDescription = "the torn-out heart of %s";
	object.LongDescription = "The torn-out heart of %s lies here, no longer beating with life.";
	object.Action = "%s savagely devour$q $p!";
	object:SetValues(16, 0, 0, 0, 0, 0);
	object:SetStats(2, 0, 0, 0, 0);
	
	object = CreateObject(14, "arm", "cook");
	object.ShortDescription = "the mangled arm of %s";
	object.LongDescription = "A writhing arm lies torn from the body of %s.";
	object.Action = "%s chomp$q on $p.";
	object:SetValues(30, 0, 0, 0, 0, 0);
	object:SetStats(5, 0, 0, 0, 0);
	
	object = CreateObject(15, "leg", "cook");
	object.ShortDescription = "the dismembered leg of %s";
	object.LongDescription = "Still twitching as if to kick you, the leg of %s lies here.";
	object.Action = "%s chomp$q on $p.";
	object:SetValues(40, 0, 0, 0, 0, 0);
	object:SetStats(5, 0, 0, 0, 0);
	
	object = CreateObject(16, "guts", "cook");
	object.ShortDescription = "the spilled guts of %s";
	object.LongDescription = "The disemboweled guts of %s swarm with maggots.";
	object:SetValues(5, 0, 0, 1, 0, 0);
	object:SetStats(1, 0, 0, 0, 0);
	
	object = CreateObject(17, "blood", "blood");
	object.ShortDescription = "the spilled blood";
	object.LongDescription = "A pool of spilled blood is here.";
	object:SetValues(5, 0, 0, 1, 0, 0);
	object:SetStats(1, 0, 0, 0, 0);
	
	object = CreateObject(18, "bloodstain", "bloodstain");
	object.ShortDescription = "the bloodstain";
	object.LongDescription = "Blood stains the ground.";
	object:SetValues(5, 0, 0, 1, 0, 0);
	object:SetStats(1, 0, 0, 0, 0);
	
	object = CreateObject(19, "scraps remnants", "scraps");
	object.ShortDescription = "the remnants of %s";
	object.LongDescription = "The remnants of %s are strewn about.";
	object:SetValues(5, 0, 0, 1, 0, 0);
	object:SetStats(1, 0, 0, 0, 0);
	
	object = CreateObject(20, "magic mushroom", "food");
	object.ShortDescription = "a magic mushroom";
	object.LongDescription = "A magic mushroom appears to have been left here.";
	object.Action = "%s enjoy$q $p.";
	object:SetValues(5, 0, 0, 0, 0, 0);
	object:SetStats(1, 10, 1, 0, 0);
	
	object = CreateObject(21, "ball light", "light");
	object.ShortDescription = "a ball of light";
	object.LongDescription = "A ball of light.";
	object.Flags = "glow magic";
	object:SetValues(0, 0, -1, 0, 0, 0);
	object:SetStats(1, 0, 0, 0, 0);
	
	object = CreateObject(22, "mystical spring flowing", "fountain");
	object.ShortDescription = "a mystical spring";
	object.LongDescription = "A mystical spring flows majestically from a glowing circle of blue.";
	object.Flags = "magic";
	object:SetValues(0, 0, -1, 0, 0, 0);
	object:SetStats(1, 0, 0, 0, 0);
	object:AddMudProg(CreateMudProg("use_prog", "100", 
	[[
		local ch = LGetCurrentCharacter();
		MPEchoAt(ch, "You drink deeply from the flow of mystical water.");
		MPEchoAround(ch, "$n drinks deeply from the flow of mystical water.");
	]]));
	
	object = CreateObject(23, "skin", "treasure");
	object.ShortDescription = "the skin of %s";
	object.LongDescription = "The skin of %s lies here.";
	object:SetStats(1, 0, 0, 0, 0);
	
	object = CreateObject(24, "meat fresh slice", "cook");
	object.ShortDescription = "a slice of raw meat from %s";
	object.LongDescription = "A slice of raw meat from %s is here.";
	object.Flags = "organic";
	object:SetValues(15, 0, 0, 0, 0, 0);
	object:SetStats(2, 0, 0, 0, 0);
	object:AddMudProg(CreateMudProg("use_prog", "100", 
	[[
		local ch = LGetCurrentCharacter();
		MPEchoAt(ch, "Your mouth waters in delight as you enjoy the fresh meat.");
		MPEchoAround(ch, "$n chews contentedly, enjoying the flavor of the fresh meat.");
	]]));
	
	object = CreateObject(25, "shopping bag", "container");
	object.ShortDescription = "a bag";
	object.LongDescription = "A shopping bag is here.";
	object.Flags = "groundrot";
	object:SetValues(50, 0, 0, 0, 0, 0);
	object:SetStats(2, 20, 2, 0, 0);
	
	object = CreateObject(26, "blood pool spill bloodlet", "blood");
	object.ShortDescription = "a quantity of let blood";
	object.LongDescription = "A pool of let blood glistens here.";
	object:SetStats(1, 0, 0, 0, 0);
	
	object = CreateObject(30, "fire flame cloud", "fire");
	object.ShortDescription = "a cloud of vaporous flame";
	object.LongDescription = "A cloud of vaporous flame blazes here, defying the elements.";
	object.Flags = "magic";
	object:SetStats(10, 10, 1, 0, 0);
	
	object = CreateObject(31, "trap", "trap");
	object.ShortDescription = "a trap";
	object.LongDescription = "You detect a trap.";
	object:SetStats(100, 10, 1, 0, 0);
	
	object = CreateObject(32, "portal whirling", "portal");
	object.ShortDescription = "a whirling portal";
	object.LongDescription = "A whirling portal of energy turns slowly, beckoning to the unknown.";
	object.Flags = "magic";
	object:SetStats(100, 10, 1, 0, 0);
	
	object = CreateObject(33, "black poison powder", "trash");
	object.ShortDescription = "black poisoning powder";
	object.LongDescription = "A small container filled with black powder is here.";
	object.WearFlags = "take hold";
	object:SetStats(1, 48000, 4800, 0, 0);
	
	object = CreateObject(34, "scroll scribing blank", "scroll");
	object.ShortDescription = "a blank scroll";
	object.LongDescription = "A blank scroll is here.";
	object.WearFlags = "take hold";
	object:SetValues(0, -1, -1, -1, 0, 0);
	object:SetStats(2, 10000, 1000, 0, 0);
	
	object = CreateObject(35, "flask empty", "potion");
	object.ShortDescription = "an empty flask";
	object.LongDescription = "An empty flask is here.";
	object.WearFlags = "take hold";
	object:SetValues(1, -1, -1, -1, 0, 0);
	object:SetStats(1, 15000, 1500, 0, 0);
	
	object = CreateObject(36, "parchment paper note", "paper");
	object.ShortDescription = "a note";
	object.LongDescription = "A note is here.";
	object.WearFlags = "take hold";
	object:SetStats(1, 1500, 150, 0, 0);
	
	object = CreateObject(37, "quill pen", "pen");
	object.ShortDescription = "a quill";
	object.LongDescription = "A feather used for writing is here.";
	object:SetValues(15, 15, 0, 0, 0, 0);
	object:SetStats(2, 2000, 200, 0, 0);
	
--[[
#OBJECT
Vnum     38
Keywords boots travel traveling~
Type     armor~
Short    weathered boots~
Long     A pair of weathered traveling boots lie here.~
Flags    glow hum magic antievil antineutral antivampire~
WFlags   take feet~
Values   11 11 0 0 0 0
Stats    2 325400 32540 0 0
Affect       -1 -1 5 19 0
Affect       -1 -1 1 18 0
Affect       -1 -1 100 14 0
Affect       -1 -1 1 2 0
#ENDOBJECT

#OBJECT
Vnum     39
Keywords key gate~
Type     key~
Short    a gate key~
Long     A largish key lies here.~
Flags    metal~
WFlags   take~
Values   0 0 0 0 0 0
Stats    1 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     41
Keywords orb~
Type     lever~
Short    the orb~
Long     A small orb tops a slender pedestal of glowing crystal.~
Values   0 0 0 0 0 0
Stats    1 0 0 0 0
#EXDESC
ExDescKey    orb~
ExDesc       Pulsing atop the crystal column, it seems as though it could be pushed to
turn it ever so slightly.
~
#ENDEXDESC

#MUDPROG
Progtype  push_prog~
Arglist   100~
Comlist   mpechoat $n The instant your hand touches the orb, your surroundings change.
mpechoat $n The sights and busy sounds of Darkhaven surround you...
if ispkill($n)
  mptrans 0.$n 3009
else
  mptrans $n 21001
  mpat 21001 mer 0.$n $n materializes in the center of the great rune.
endif
mpat 99 pull orb
~
#ENDPROG

#ENDOBJECT

#OBJECT
Vnum     42
Keywords orb~
Type     staff~
Short    a pulsing orb~
Long     A pulsing orb lies here, probably lost.~
WFlags   take hold~
Values   25 1 1 -1 0 0
Stats    1 1 0 0 0
Spells   'heal'
#MUDPROG
Progtype  use_prog~
Arglist   100~
Comlist   mpecho $n blows sweet dragon's breath over you.
~
#ENDPROG

#ENDOBJECT

#OBJECT
Vnum     43
Keywords holy symbol faith~
Type     armor~
Short    a symbol of faith~
Long     A holy symbol lies here, shining.~
Flags    nolocate~
WFlags   take hold~
Values   5 5 0 0 0 0
Stats    5 0 0 0 0
Affect       -1 -1 2 4 0
#ENDOBJECT

#OBJECT
Vnum     44
Keywords chunk chunks brain brains~
Type     food~
Short    the splattered brains of %s~
Long     Grey chunks of the brain of %s lie here attracting flies.~
Action   %s savagely devour$q $p!~
WFlags   take~
Values   16 0 0 0 0 0
Stats    2 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     45
Keywords hand~
Type     cook~
Short    the severed hand of %s~
Long     The severed hand of %s clenches in a death spasm.~
Action   %s savagely devour$q $p!~
WFlags   take~
Values   12 0 0 0 0 0
Stats    2 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     46
Keywords foot~
Type     cook~
Short    the twisted foot of %s~
Long     The foot of %s lies in a pool of coagulated blood.~
Action   %s savagely devour$q $p!~
WFlags   take~
Values   12 0 0 0 0 0
Stats    2 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     47
Keywords finger~
Type     cook~
Short    the wriggling finger of %s~
Long     Ripped from the hand of %s, a finger lies wriggling here.~
Action   %s savagely devour$q $p!~
WFlags   take~
Values   5 0 0 0 0 0
Stats    1 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     48
Keywords ear~
Type     cook~
Short    the torn ear of %s~
Long     The sliced-off ear of %s lies here, never again to hear battle.~
Action   %s savagely devour$q $p!~
WFlags   take~
Values   5 0 0 0 0 0
Stats    1 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     49
Keywords eye~
Type     cook~
Short    the gouged-out eye of %s~
Long     A gouged-out eye forever envisions the gruesome death of %s.~
Action   %s savagely devour$q $p!~
WFlags   take~
Values   5 0 0 0 0 0
Stats    1 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     50
Keywords long-tongue~
Type     cook~
Short    the long tongue of %s~
Long     Twisting as if to taste you, the writhing tongue of %s lies here.~
Action   %s savagely devour$q $p!~
WFlags   take~
Values   14 0 0 0 0 0
Stats    2 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     51
Keywords eyestalk~
Type     cook~
Short    the eyestalk of %s~
Long     Visions of death fill your mind as you notice the eyestalks of %s.~
Action   %s savagely devour$q $p!~
WFlags   take~
Values   10 0 0 0 0 0
Stats    1 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     52
Keywords tentacle~
Type     cook~
Short    the slimy tentacle of %s~
Long     A tentacle of %s thrashes wildly in a feeble attempt to regain life.~
Action   %s savagely devour$q $p!~
WFlags   take~
Values   25 0 0 0 0 0
Stats    4 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     53
Keywords fin~
Type     trash~
Short    the mutilated fin of %s~
Long     The mutilated fin of %s lies here, smelling of rot and decay.~
WFlags   take~
Values   0 0 0 0 0 0
Stats    2 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     54
Keywords wing~
Type     trash~
Short    the wing of %s~
Long     The mangled wing of %s thrashes in final throes.~
Values   0 0 0 0 0 0
Stats    4 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     55
Keywords tail~
Type     trash~
Short    the thrashing tail of %s~
Long     The tail of %s lies here in a heap of morbid decay.~
WFlags   take~
Values   0 0 0 0 0 0
Stats    5 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     56
Keywords scale~
Type     trash~
Short    a scale from %s~
Long     A scale from %s lies here, caked thickly in blood.~
WFlags   take~
Values   0 0 0 0 0 0
Stats    3 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     57
Keywords tusk~
Type     trash~
Short    the broken tusk of %s~
Long     The broken tusk of %s has dropped here.~
WFlags   take~
Values   0 0 0 0 0 0
Stats    5 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     58
Keywords horn~
Type     trash~
Short    the cracked horn of %s~
Long     The cracked horn of %s lies dislodged here.~
WFlags   take~
Values   0 0 0 0 0 0
Stats    6 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     59
Keywords claw~
Type     trash~
Short    the severed claw of %s~
Long     The severed claw of %s lies mangled here.~
WFlags   take~
Values   0 0 0 0 0 0
Stats    5 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     60
Keywords blood fountain~
Type     blood~
Short    a fountain of blood~
Long     A fountain of fresh blood flows in the corner.~
Values   99999 99999 0 0 0 0
Stats    1 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     61
Keywords spring matrimony~
Type     fountain~
Short    the Spring of Matrimony~
Long     A shimmering spring flows quietly through.~
Flags    magic~
Values   30000 30000 0 0 0 0
Stats    1 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     62
Keywords ceremonial lever griffon~
Type     lever~
Short    A shiny white lever ~
Long     A white lever is here for your transportation needs.~
Values   1 0 0 0 0 0
Stats    1000 0 0 0 0
#EXDESC
ExDescKey    lever ceremonial~
ExDesc       Pull this lever for magical transportation to the wedding.
Pull the lever to call your limousine.
~
#ENDEXDESC

#EXDESC
ExDescKey    griffon lever~
ExDesc       &w       _____,    _..-=-=-=-=-====--,
        _.'a   /  .-',___,..=--=--==-'`
       ( _     \ /  //___/-=---=----'
        ` `\    /  //---/--==----=-'
     ,-.    | / \_//-_.'==-==---='
    (.-.`\  | |'../-'=-=-=-=--'
     (' `\`\| //_|-\.`;--````-,        _ 
          \ | \_,_,_\.'        \     .'_`\
           `\            ,    , \    || `\\
             \    /   _.--\    \ '._.'/  / |
             /  /`---'   \ \   |`'---'   \/
            / /'          \ ;-. \
         __/ /           __) \ ) `|
       ((='--;)         (,___/(,_/ 
A Majestic Griffon awaits to transport you to the wedding of
Mokshonian and Delphya. 
~
#ENDEXDESC

#MUDPROG
Progtype  use_prog~
Arglist   100~
Comlist   if ispc($n)
or isimmort($n)
mea 0.$n A stretch white limousine pulls up and the chauffeur urges you in.
mptrans 0.$n Harakiem
mpat 0.$n mer $n arrives in a stretch white limousine.
mpat 0.$n mer $n $n appears from a cloud of swirling mists.
endif
~
#ENDPROG

#ENDOBJECT

#OBJECT
Vnum     63
Keywords extradimensional portal~
Type     container~
Short    an extradimensional portal~
Long     Some wizard left an extradimensional portal laying here.~
Flags    bless antithief antiwarrior anticleric antivampire~
WFlags   take~
Values   500 0 0 0 0 0
Stats    4 1000 100 0 0
#ENDOBJECT

#OBJECT
Vnum     64
Keywords sigil~
Type     light~
Short    the sigil of %s~
Long     The sigil of %s lies here, abandoned.~
WFlags   take~
Values   200 0 -1 0 0 0
Stats    1 0 0 0 0
Affect       -1 -1 20 13 0
Affect       -1 -1 20 12 0
Affect       -1 -1 20 14 0
Affect       -1 -1 -20 17 0
#EXDESC
ExDescKey    sigil~
ExDesc       A floating, opaque candle spreads the aura of a deity about the room.
~
#ENDEXDESC

#MUDPROG
Progtype  wear_prog~
Arglist   100~
Comlist   if favor($n) < 300
mpforce 0.$n remove sigil
mea 0.$n Due to your low favor with your deity, you cannot brandish the sigil.
endif
~
#ENDPROG

#ENDOBJECT

#OBJECT
Vnum     65
Keywords solomonic cross crucifix symbol~
Type     armor~
Short    a solomonic crucifix~
Long     An elegant crucifix radiates a glow of warmth and piety.~
Flags    glow hum bless antievil antineutral antimage antithief antivampire antidruid~
WFlags   take hold~
Values   10 10 0 0 0 0
Stats    4 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     66
Keywords sign~
Type     trash~
Short    A Sign~
Long     A sign providing you with information stands here.~
Values   0 0 0 0 0 0
Stats    1 0 0 0 0
#EXDESC
ExDescKey    sign~
ExDesc       If you feel the punishment you have received is unfair you may appeal
to a higher level immortal by following the procedure in the Help Appeals
file.  If you do not follow this procedure your appeal may not be dealt
with in a timely fashion.  Frivolous appeals may not result in a 
reduction of sentence BUT an increase in time.  
Please read the following help files:  help appeals and help the_panel.
~
#ENDEXDESC

#ENDOBJECT

#OBJECT
Vnum     70
Keywords fillet mignon steak~
Type     food~
Short    a delicate fillet mignon~
Long     A delicate fillet mignon lies here on a plate, still sizzling.~
WFlags   take~
Values   0 0 0 0 0 0
Stats    1 50 5 0 0
#MUDPROG
Progtype  use_prog~
Arglist   100~
Comlist   mea 0.$n The delectable and juicy steak melts in your mouth...
mer 0.$n As $n eats the delectable and juicy steak, it melts in $s mouth...
~
#ENDPROG

#ENDOBJECT

#OBJECT
Vnum     71
Keywords glass sherry wine~
Type     drinkcon~
Short    a fine glass of sherry~
Long     A wine glass full of sherry drips here on the ground.~
WFlags   take hold~
Values   6 6 2 0 0 0
Stats    1 250 25 0 0
#ENDOBJECT

#OBJECT
Vnum     72
Keywords honeymoon plaque sign~
Type     furniture~
Short    a honeymoon plaque~
Long     A golden plaque gleams in the sunlight, crying out for your attention.~
Values   0 0 0 0 0 0
Stats    1500 0 0 0 0
#EXDESC
ExDescKey    honeymoon plaque sign~
ExDesc       These few but illustrious rooms are dedicated to Haus and Moonbeam, on 
their wedding day, from Telemachus and the whole Imm gang, may it be a 
fruitful and eternal wedlock =)
 
--Telemachus
12/15/97
~
#ENDEXDESC

#ENDOBJECT

#OBJECT
Vnum     73
Keywords french lingerie lace~
Type     armor~
Short    French-lace lingerie~
Long     Some white french-lace lingerie lies here, hmmmm.~
WFlags   take body~
Values   12 12 0 0 0 0
Stats    1 500 50 0 0
#MUDPROG
Progtype  use_prog~
Arglist   100~
Comlist   mea 0.$n You put on the french-lace lingerie, OOoOoo la la!
mer 0.$n $n puts on some french-lace lingerie, OOoOoo la la!
~
#ENDPROG

#ENDOBJECT

#OBJECT
Vnum     74
Keywords french boxers chateau~
Type     armor~
Short    French silk boxers~
Long     Some french silk boxers lie here, from some type of chateau.~
WFlags   take about~
Values   12 12 0 0 0 0
Stats    1 100 10 0 0
#MUDPROG
Progtype  use_prog~
Arglist   100~
Comlist   mea 0.$n You slip into some french silk boxers.
mer 0.$n $n slips into some french silk boxers.
mpforce $n flex
~
#ENDPROG

#ENDOBJECT

#OBJECT
Vnum     75
Keywords french bed heart~
Type     furniture~
Short    a French heart-shaped bed~
Long     A french heart-shaped bed is here, taking up most of the room.~
Values   0 0 0 0 0 0
Stats    5000 5000 500 0 0
#ENDOBJECT

#OBJECT
Vnum     80
Keywords feather~
Type     trash~
Short    a feather from %s~
Long     A feather from %s floats about here.~
WFlags   take~
Values   0 0 0 0 0 0
Stats    1 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     81
Keywords forelegs~
Type     trash~
Short    the severed foreleg of %s~
Long     The severed foreleg of %s lies here.~
WFlags   take~
Values   0 0 0 0 0 0
Stats    1 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     82
Keywords paw~
Type     trash~
Short    %s's severed paw~
Long     The severed paw of %s lies here.~
WFlags   take~
Values   0 0 0 0 0 0
Stats    1 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     83
Keywords hoof~
Type     trash~
Short    the cloven hoof of %s~
Long     The cloven hoof of %s lies here.~
WFlags   take~
Values   0 0 0 0 0 0
Stats    2 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     84
Keywords beak~
Type     trash~
Short    the beak of %s~
Long     The beak of %s lies here.~
WFlags   take~
Values   0 0 0 0 0 0
Stats    1 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     85
Keywords sharpscale~
Type     trash~
Short    a sharp scale from %s~
Long     A sharp scale from %s lies here.~
WFlags   take~
Values   0 0 0 0 0 0
Stats    1 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     86
Keywords haunch~
Type     food~
Short    the haunch of %s~
Long     The great haunch of %s lies here.~
WFlags   take~
Values   25 0 0 0 0 0
Stats    4 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     87
Keywords fang~
Type     trash~
Short    the fang of %s~
Long     The fang of %s lies here.~
WFlags   take~
Values   0 0 0 0 0 0
Stats    1 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     88
Keywords deck~
Type     furniture~
Short    A large deck chair~
Long     A large deck chair lies here awaiting Mystaric.~
Values   0 0 0 0 0 0
Stats    1 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     89
Keywords swirling tide pool~
Type     fountain~
Short    a Mystic tide pool~
Long     A Mystic tide pool is here, the water swirling hypnotically.~
Values   0 9999 0 0 0 0
Stats    1 0 0 0 0
#ENDOBJECT

#OBJECT
Vnum     90
Keywords arena sign banner~
Type     furniture~
Short    a floating banner~
Long     A big banner floats here.  Read it.~
Flags    glow~
Values   0 0 0 0 0 0
Stats    1 0 0 0 0
#EXDESC
ExDescKey    arena sign banner~
ExDesc       Use the arena at your own risk.
 
Equipment loss for _ANY_ reason will _NOT_ be reimbursed.
 
Deadlies will be able to loot and be looted by other deadlies.
Otherwise no looting is possible.

To return to Darkhaven, say "return" in the center of the arena.
You cannot leave if engaged in combat.
~
#ENDEXDESC

#ENDOBJECT

#OBJECT
Vnum     98
Keywords A slice of wedding cake~
Type     food~
Short    A slice of wedding cake~
Long     A slice of heavenly wedding cake sits on a garden table.~
Action   %s look$q around suspiciously, then scarf$q down $p.~
WFlags   take~
Values   40 0 0 0 0 0
Stats    2 75 7 0 0
#EXDESC
ExDescKey    angel food angelfood cake~
ExDesc       This cake appears as if it might just float away...unless you eat it!
~
#ENDEXDESC

#ENDOBJECT

#OBJECT
Vnum     99
Keywords final object~
Type     trash~
Short    a newly created final object~
Long     Some god dropped a newly created final object here.~
Flags    prototype~
Values   0 0 0 0 0 0
Stats    1 0 0 0 0
#ENDOBJECT
--]]
end

function SystemRooms()
	LBootLog("=================== AREA 'THE VOID' - ROOMS ====================");
	room = CreateRoom(2, "Limbo", "city", area.this);
	room.Description = [[You float in a formless void, detached from all sensation of physical
		matter, surrounded by swirling glowing light which fades into the
		relative darkness around you without any trace of edge or shadow.]];
	room:SetFlags("nomob indoors safe norecall nosummon noastral");
	room:AddExit("north", 2100, "north");
	AddMobileToRoom(room, 1, 2, 1);			-- Puff the Dragon
	AddObjectToRoom(room, 22, 2, 1);		-- Mystical Spring
	AddObjectToRoom(room, 60, 2, 1);		-- Fountain of Blood
	room:AddMudProg(CreateMudProg("speech_prog", "p return", 
	[[ 
		local ch = LGetCurrentCharacter();
		if (ch.Level > 1) then
			MPTrans(ch, 21001);
		end
	]]));
	room:AddMudProg(CreateMudProg("act_prog", "p has lost", 
	[[
		local ch = LGetCurrentCharacter();
		MPMSet(ch, "full 50");
		MPMSet(ch, "thirst 100");
		MPRestore(ch, 2);
	]]));
	room:AddMudProg(CreateMudPRog("act_prog", "is", 
	[[
		local ch = LGetCurrentCharacter();
		MPMSet(ch, "full 50");
		MPMSet(ch, "thirst 100");
		MPRestore(ch, 2);
	]]));
	
	room = CreateRoom(3, "Storage", "city", area.this);
	room.Description = [[This room is reserved for storage of polymorphed players.]];
	room:SetFlags("death nomob private solitary nosummon noastral");
	
	room = CreateRoom(4, "Deity Purge Room", "city", area.this);
	room.Description = [[This room handles the purging of unused deities.]];
	room:SetFlags("nomob indoors nosummon noastral");
	
	room = CreateRoom(6, "Hell", "city", area.this);
	room.Description = [[As if picked up by the scruff of your neck by a mighty hand, you find
		yourself unceremoniously dumped at a strange gateway.  Here is the
		place which will determine your fate.  Whether you will be sent back
		to life as you once knew it, or proceed onto a far yet bleaker pathway.
		The time has come for you to plead your case and await judgement for the
		crimes that have been placed upon your head.  Speak wisely and choose
		your words carefully, for your testimony will be written in the ledgers
		of the Gods, and will determine the path you will ultimately travel.]];
	room:SetFlags("nomob indoors chaotic safe norecall logspeech nosummon noastral nosupplicate");
	AddObjectToRoom(room, 22, 6, 1);		-- Mystical Spring
	AddObjectToRoom(room, 60, 6, 1);		-- Fountain of Blood		
	AddMobileToRoom(room, 2, 6, 2);			-- demon imp
	
	room = CreateRoom(8, "Hell", "city", area.this);
	room.Description = [[As if picked up by the scruff of your neck by a mighty hand, you find
		yourself unceremoniously dumped at a strange gateway.  Here is the
		place which will determine your fate.  Whether you will be sent back
		to life as you once knew it, or proceed onto a far yet bleaker pathway.
		The time has come for you to plead your case and await judgement for the
		crimes that have been placed upon your head.  Speak wisely and choose
		your words carefully, for your testimony will be written in the ledgers
		of the Gods, and will determine the path you will ultimately travel.
						L|J(_)
					)    | (")      (
					,(.  |`/ \- y  (,`)
				   )' (' | \ /\/  ) (.
				  (' ),) | _W_   (,)' ).]];
	room:SetFlags("nomob indoors chaotic nomagic safe norecall logspeech nosummon noastral nosupplicate");
	AddObjectToRoom(room, 22, 8, 1);		-- Mystical Spring
	AddObjectToRoom(room, 60, 8, 1);		-- Fountain of Blood
	AddMobileToRoom(room, 2, 8, 2);			-- demon imp
	room:AddMudProg(CreateMudProg("death_prog", "100", 
	[[
		local ch = LGetCurrentCharacter();
		if (ch.IsNpc()) then
			MPTransfer(ch, 8);
			MPForce(ch, "drink blood");
			MPForce(ch, "drink blood");
			MPForce(ch, "drink blood");
			MPForce(ch, "drink blood");
			MPForce(ch, "drink blood");
			MPForce(ch, "drink water");
			MPForce(ch, "drink water");
			MPForce(ch, "drink water");
			MPForce(ch, "drink water");
			MPForce(ch, "drink water");
		end
	]]));
	
	room = CreateRoom(9, "Task Room", "city", area.this);
	room.Description = "Any mob here is probably performing tasks.  Mess with it and I will kill your dog.  -- Blodkai";
	room:SetFlags("nomob nodrop nosummon noastral");
	room:AddExit("down", 10300, "The Academy of Darkhaven");
	AddExitToRoom(room, "somewhere", 10300, "vnums", "hidden auto");
end

function Arena()
--[[
#ROOM
Vnum     29
Name     The Arena~
Sector   city~
Flags    nomob indoors norecall nosummon noastral~
Desc     You find yourself on the Arena floor.  The roar of the crowd above you
is overwhelming, while around you ... your opponent awaits your coming.
~
#EXIT
Direction north~
ToRoom    30
#ENDEXIT

#EXIT
Direction east~
ToRoom    37
#ENDEXIT

#EXIT
Direction south~
ToRoom    36
#ENDEXIT

#MUDPROG
Progtype  rand_prog~
Arglist   15~
Comlist   mpe A slow chant floats down to you: "Two men enter ... one man leaves."
~
#ENDPROG

#MUDPROG
Progtype  death_prog~
Arglist   100~
Comlist   mpecho A deafening clamor of gleeful shouts rain down from above!
mpechoaround $n Trinkets, spittle and half-eaten food pelt down on your head.
mpechoaround $n Tokens of favor from an appreciative crowd.
~
#ENDPROG

#ENDROOM

#ROOM
Vnum     30
Name     The Arena~
Sector   city~
Flags    nomob indoors norecall nosummon noastral~
Desc     You find yourself on the Arena floor.  The roar of the crowd above you
is overwhelming, while around you ... your opponent awaits your coming.
~
#EXIT
Direction east~
ToRoom    31
#ENDEXIT

#EXIT
Direction south~
ToRoom    29
#ENDEXIT

#EXIT
Direction down~
ToRoom    34
Flags     climb~
#ENDEXIT

#EXIT
Direction southeast~
ToRoom    37
#ENDEXIT

#MUDPROG
Progtype  rand_prog~
Arglist   15~
Comlist   mpe A slow chant floats down to you: "Two men enter ... one man leaves."
~
#ENDPROG

#MUDPROG
Progtype  death_prog~
Arglist   100~
Comlist   mpecho A deafening clamor of gleeful shouts rain down from above!
mpechoaround $n Trinkets, spittle and half-eaten food pelt down on your head.
mpechoaround $n Tokens of favor from an appreciative crowd.
~
#ENDPROG

#ENDROOM

#ROOM
Vnum     31
Name     The Arena~
Sector   city~
Flags    nomob indoors norecall nosummon noastral~
Desc     You find yourself on the Arena floor.  The roar of the crowd above you
is overwhelming, while around you ... your opponent awaits your coming.
~
#EXIT
Direction east~
ToRoom    32
#ENDEXIT

#EXIT
Direction south~
ToRoom    37
#ENDEXIT

#EXIT
Direction west~
ToRoom    30
#ENDEXIT

#MUDPROG
Progtype  rand_prog~
Arglist   15~
Comlist   mpe A slow chant floats down to you: "Two men enter ... one man leaves."
~
#ENDPROG

#MUDPROG
Progtype  death_prog~
Arglist   100~
Comlist   mpecho A deafening clamor of gleeful shouts rain down from above!
mpechoaround $n Trinkets, spittle and half-eaten food pelt down on your head.
mpechoaround $n Tokens of favor from an appreciative crowd.
~
#ENDPROG

#ENDROOM

#ROOM
Vnum     32
Name     The Arena~
Sector   city~
Flags    nomob indoors norecall nosummon noastral~
Desc     You find yourself on the Arena floor.  The roar of the crowd above you
is overwhelming, while around you ... your opponent awaits your coming.
~
#EXIT
Direction south~
ToRoom    33
#ENDEXIT

#EXIT
Direction west~
ToRoom    31
#ENDEXIT

#EXIT
Direction down~
ToRoom    36
Flags     climb~
#ENDEXIT

#EXIT
Direction southwest~
ToRoom    37
#ENDEXIT

#MUDPROG
Progtype  rand_prog~
Arglist   15~
Comlist   mpe A slow chant floats down to you: "Two men enter ... one man leaves."
~
#ENDPROG

#MUDPROG
Progtype  death_prog~
Arglist   100~
Comlist   mpecho A deafening clamor of gleeful shouts rain down from above!
mpechoaround $n Trinkets, spittle and half-eaten food pelt down on your head.
mpechoaround $n Tokens of favor from an appreciative crowd.
~
#ENDPROG

#ENDROOM

#ROOM
Vnum     33
Name     The Arena~
Sector   city~
Flags    nomob indoors norecall nosummon noastral~
Desc     You find yourself on the Arena floor.  The roar of the crowd above you
is overwhelming, while around you ... your opponent awaits your coming.
~
#EXIT
Direction north~
ToRoom    32
#ENDEXIT

#EXIT
Direction south~
ToRoom    34
#ENDEXIT

#EXIT
Direction west~
ToRoom    37
#ENDEXIT

#MUDPROG
Progtype  rand_prog~
Arglist   15~
Comlist   mpe A slow chant floats down to you: "Two men enter ... one man leaves."
~
#ENDPROG

#MUDPROG
Progtype  death_prog~
Arglist   100~
Comlist   mpecho A deafening clamor of gleeful shouts rain down from above!
mpechoaround $n Trinkets, spittle and half-eaten food pelt down on your head.
mpechoaround $n Tokens of favor from an appreciative crowd.
~
#ENDPROG

#ENDROOM

#ROOM
Vnum     34
Name     The Arena~
Sector   city~
Flags    nomob indoors norecall nosummon noastral~
Desc     You find yourself on the Arena floor.  The roar of the crowd above you
is overwhelming, while around you ... your opponent awaits your coming.
~
#EXIT
Direction north~
ToRoom    33
#ENDEXIT

#EXIT
Direction west~
ToRoom    35
#ENDEXIT

#EXIT
Direction up~
ToRoom    30
Flags     climb~
#ENDEXIT

#EXIT
Direction northwest~
ToRoom    37
#ENDEXIT

#MUDPROG
Progtype  rand_prog~
Arglist   15~
Comlist   mpe A slow chant floats down to you: "Two men enter ... one man leaves."
~
#ENDPROG

#MUDPROG
Progtype  death_prog~
Arglist   100~
Comlist   mpecho A deafening clamor of gleeful shouts rain down from above!
mpechoaround $n Trinkets, spittle and half-eaten food pelt down on your head.
mpechoaround $n Tokens of favor from an appreciative crowd.
~
#ENDPROG

#ENDROOM

#ROOM
Vnum     35
Name     The Arena~
Sector   city~
Flags    nomob indoors norecall nosummon noastral~
Desc     You find yourself on the Arena floor.  The roar of the crowd above you
is overwhelming, while around you ... your opponent awaits your coming.
~
#EXIT
Direction north~
ToRoom    37
#ENDEXIT

#EXIT
Direction east~
ToRoom    34
#ENDEXIT

#EXIT
Direction west~
ToRoom    36
#ENDEXIT

#MUDPROG
Progtype  rand_prog~
Arglist   15~
Comlist   mpe A slow chant floats down to you: "Two men enter ... one man leaves."
~
#ENDPROG

#MUDPROG
Progtype  death_prog~
Arglist   100~
Comlist   mpecho A deafening clamor of gleeful shouts rain down from above!
mpechoaround $n Trinkets, spittle and half-eaten food pelt down on your head.
mpechoaround $n Tokens of favor from an appreciative crowd.
~
#ENDPROG

#ENDROOM

#ROOM
Vnum     36
Name     The Arena~
Sector   city~
Flags    nomob indoors norecall nosummon noastral~
Desc     You find yourself on the Arena floor.  The roar of the crowd above you
is overwhelming, while around you ... your opponent awaits your coming.
~
#EXIT
Direction north~
ToRoom    29
#ENDEXIT

#EXIT
Direction east~
ToRoom    35
#ENDEXIT

#EXIT
Direction up~
ToRoom    32
Flags     climb~
#ENDEXIT

#EXIT
Direction northeast~
ToRoom    37
#ENDEXIT

#MUDPROG
Progtype  rand_prog~
Arglist   15~
Comlist   mpe A slow chant floats down to you: "Two men enter ... one man leaves."
~
#ENDPROG

#MUDPROG
Progtype  death_prog~
Arglist   100~
Comlist   mpecho A deafening clamor of gleeful shouts rain down from above!
mpechoaround $n Trinkets, spittle and half-eaten food pelt down on your head.
mpechoaround $n Tokens of favor from an appreciative crowd.
~
#ENDPROG

#ENDROOM

#ROOM
Vnum     37
Name     The Arena~
Sector   city~
Flags    nomob indoors norecall nosummon noastral~
Desc     You stand in the direct center of the Arena.  Blood pools around your
feet, and you almost slip in this morbid mess.  The stench of death
surrounds you, momentarily blocking from your mind the realization
that you could be attacked from any conceivable direction.  Spinning
slowly about, you realize the poor tactical position you hold.
~
#EXIT
Direction north~
ToRoom    31
#ENDEXIT

#EXIT
Direction east~
ToRoom    33
#ENDEXIT

#EXIT
Direction south~
ToRoom    35
#ENDEXIT

#EXIT
Direction west~
ToRoom    29
#ENDEXIT

#EXIT
Direction up~
ToRoom    39
Flags     climb~
#ENDEXIT

#EXIT
Direction down~
ToRoom    38
Flags     climb~
#ENDEXIT

#EXIT
Direction northeast~
ToRoom    32
#ENDEXIT

#EXIT
Direction northwest~
ToRoom    30
#ENDEXIT

#EXIT
Direction southeast~
ToRoom    34
#ENDEXIT

#EXIT
Direction southwest~
ToRoom    36
#ENDEXIT

#MUDPROG
Progtype  speech_prog~
Arglist   p return~
Comlist   if isfight($n)
else
  mptrans $n 21001
endif
~
#ENDPROG

#MUDPROG
Progtype  rand_prog~
Arglist   15~
Comlist   mpe A slow chant floats down to you: "Two men enter ... one man leaves."
~
#ENDPROG

#MUDPROG
Progtype  death_prog~
Arglist   100~
Comlist   mpecho A deafening clamor of gleeful shouts rain down from above!
mpechoaround $n Trinkets, spittle and half-eaten food pelt down on your head.
mpechoaround $n Tokens of favor from an appreciative crowd.
~
#ENDPROG

#ENDROOM

#ROOM
Vnum     38
Name     The Arena~
Sector   city~
Flags    nomob indoors norecall nosummon noastral~
Desc     You find yourself beneath the floor of the great Arena.  The crowd's roar
above is overwhelming, while somewhere ahead ... your opponent awaits.
~
#EXIT
Direction north~
ToRoom    40
#ENDEXIT

#EXIT
Direction up~
ToRoom    37
Flags     climb~
#ENDEXIT

#MUDPROG
Progtype  rand_prog~
Arglist   15~
Comlist   mpe A slow chant floats down to you: "Two men enter ... one man leaves."
~
#ENDPROG

#MUDPROG
Progtype  death_prog~
Arglist   100~
Comlist   mpecho A deafening clamor of gleeful shouts rain down from above!
mpechoaround $n Trinkets, spittle and half-eaten food pelt down on your head.
mpechoaround $n Tokens of favor from an appreciative crowd.
~
#ENDPROG

#ENDROOM

#ROOM
Vnum     39
Name     The Arena~
Sector   city~
Flags    nomob indoors norecall nosummon noastral~
Desc     You find yourself above the floor of the Arena.  The roar of the crowd
is overwhelming, while somewhere ahead of you ... your opponent awaits.
~
#EXIT
Direction south~
ToRoom    40
#ENDEXIT

#EXIT
Direction down~
ToRoom    37
Flags     climb~
#ENDEXIT

#MUDPROG
Progtype  rand_prog~
Arglist   15~
Comlist   mpe A slow chant floats down to you: "Two men enter ... one man leaves."
~
#ENDPROG

#MUDPROG
Progtype  death_prog~
Arglist   100~
Comlist   mpecho A deafening clamor of gleeful shouts rain down from above!
mpechoaround $n Trinkets, spittle and half-eaten food pelt down on your head.
mpechoaround $n Tokens of favor from an appreciative crowd.
~
#ENDPROG

#ENDROOM

#ROOM
Vnum     40
Name     The Arena~
Sector   city~
Flags    nomob indoors norecall nosummon noastral~
Desc     You are somewhere above or below the arena floor.  The sound of the crowd
around you is overwhelming, while somewhere ... your opponent awaits.
~
#EXIT
Direction north~
ToRoom    39
#ENDEXIT

#EXIT
Direction south~
ToRoom    38
#ENDEXIT

#MUDPROG
Progtype  rand_prog~
Arglist   15~
Comlist   mpe A slow chant floats down to you: "Two men enter ... one man leaves."
~
#ENDPROG

#MUDPROG
Progtype  death_prog~
Arglist   100~
Comlist   mpecho A deafening clamor of gleeful shouts rain down from above!
mpechoaround $n Trinkets, spittle and half-eaten food pelt down on your head.
mpechoaround $n Tokens of favor from an appreciative crowd.
~
#ENDPROG

#ENDROOM

#ROOM
Vnum     41
Name     The Halls of Combat~
Sector   city~
Flags    indoors safe norecall nosummon noastral~
Desc     You stand in a massive hallway before the great Arena.  Cheers and jeers
from the assembled throngs rise and fall through the corridors.  Lounging
around the hall with you are several other individuals awaiting their own
chance at glory, honor, and spoils beyond the great doors.  The Arena is
dangerous, though, and battles are often to the death.  Once you leave
this room, there is no turning back.....
 
If you have not fought in the Arena before, you should type "look rules".
When ready, wait for the Immortal to assign you an exit.
~
#EXIT
Direction up~
ToRoom    42
#ENDEXIT

#EXIT
Direction down~
ToRoom    43
#ENDEXIT

#EXDESC
ExDescKey    rules~
ExDesc       Arena Combat Rules
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
1)  All duels between peaceful characters must be run by an immortal of
    Acolyte or higher.
 
2)  All duels between deadly characters must be run by an immortal of
    Acolyte or higher.
 
2)  Combat must be agreed upon by both parties.
 
3)  Any looting must be agreed upon before-hand by both parties.
    (The overseeing immortal will enforce this)
 
4)  Once you have entered your combatant room, there is no restarting.
 
5)  Any rules on the fight must be stated clearly by both parties prior
    to the commencement of battle.
 
6)  The decision of the immortal overseeing the duel is final.
 
Any complaints should be directed to the Pkill Conclave; you should include
who you dueled, who ran the duel, what occured, and why you have a problem.
Logs are extremely helpful.
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
~
#ENDEXDESC

#MUDPROG
Progtype  speech_prog~
Arglist   p I wish to go~
Comlist   mptransfer $n 21000
mpat 21000 mpforce $n look
~
#ENDPROG

#MUDPROG
Progtype  entry_prog~
Arglist   100~
Comlist   if ispkill($n)
or isnpc($n)
mea $n You are not allowed here today!
mptransfer $n 21000
mpat 21000 mpforce $n look
else
mea $n _red By entering the arena you abide to its terms and conditions.
mea $n _red Disarming will result in helling, do not do it.
mea $n _cya No one will be available to help you with your corpse.
mea $n _cya If you do not accept these terms, just say 'I wish to go'
mea $n _cya else whatever happens from here on in is your responsibility.
mea $n _yel There will be no reimbursements from this event.
endif
~
#ENDPROG

#MUDPROG
Progtype  rand_prog~
Arglist   20~
Comlist   mpe A slow chant floats down to you: "Two men enter ... one man leaves."
~
#ENDPROG

#ENDROOM

#ROOM
Vnum     42
Name     First Combatant's Chamber~
Sector   city~
Flags    indoors safe norecall nosummon noastral~
Desc     You stand in a long hallway leading out into the Arena proper.  About
you are stacked all manner of weaponry and armor.  Choose your armament
properly, as it will be all that stands between you and death.
~
#EXIT
Direction north~
ToRoom    30
#ENDEXIT

#EXIT
Direction east~
ToRoom    32
#ENDEXIT

#EXIT
Direction south~
ToRoom    34
#ENDEXIT

#EXIT
Direction west~
ToRoom    36
#ENDEXIT

#MUDPROG
Progtype  leave_prog~
Arglist   100~
Comlist   if isimmort($n)
or isnpc($n)
else
  mpecho A rousing chorus of cheers explodes in your ears!
  mpechoat $n You bow deeply to the dangerous crowd.
  mpechoaround $n $n bows deeply to the assembled throng.
  if class($n) == warrior
    mpforce $n yell Your blood will flow upon the cold stone!
  endif
  if class($n) == thief
    mpforce $n yell Soon will my blade find its mark!
  endif
  if class($n) == mage
    mpforce $n yell Now will the spellfire swell within and consume thee!
  endif
  if class($n) == druid
    mpforce $n yell Let the melee begin!
  endif
  if class($n) == ranger
    mpforce $n yell Let the melee begin!
  endif
  if class($n) == vampire
    mpforce $n yell Bring your damnable hunt!  We shall see who survives!
  endif
  if class($n) == cleric
    mpforce $n yell Let the melee begin!
  endif
endif
~
#ENDPROG

#MUDPROG
Progtype  entry_prog~
Arglist   100~
Comlist   if isimmort($n)
or isnpc($n)
else
  mpforce $n config -autoloot
  mpforce $n channel +yell
  mpechoat $n Do not alter your autoloot configuration until duel is complete.
endif
~
#ENDPROG

#ENDROOM

#ROOM
Vnum     43
Name     Combatant's Chamber~
Sector   city~
Flags    indoors safe norecall nosummon noastral~
Desc     You stand in a long hallway leading out into the Arena proper.  About
you are stacked all manner of weaponry and armor.  Choose your armament
properly, as it will be all that stands between you and death.
~
#EXIT
Direction north~
ToRoom    31
#ENDEXIT

#EXIT
Direction east~
ToRoom    33
#ENDEXIT

#EXIT
Direction south~
ToRoom    35
#ENDEXIT

#EXIT
Direction west~
ToRoom    29
#ENDEXIT

#MUDPROG
Progtype  leave_prog~
Arglist   100~
Comlist   if isimmort($n)
or isnpc($n)
else
  mpecho A rousing chorus of cheers explodes in your ears!
  mpechoat $n You bow deeply to the dangerous crowd.
  mpechoaround $n $n bows deeply to the assembled throng.
  if class($n) == warrior
    mpforce $n yell Your blood will flow upon the cold stone!
  endif
  if class($n) == thief
    mpforce $n yell Soon will my blade find its mark!
  endif
  if class($n) == mage
    mpforce $n yell Now will the spellfire swell within and consume thee!
  endif
  if class($n) == druid
    mpforce $n yell Let the melee begin!
  endif
  if class($n) == ranger
    mpforce $n yell Let the melee begin!
  endif
  if class($n) == vampire
    mpforce $n yell Bring your damnable hunt!  We shall see who survives!
  endif
  if class($n) == cleric
    mpforce $n yell Let the melee begin!
  endif
endif
~
#ENDPROG

#MUDPROG
Progtype  entry_prog~
Arglist   100~
Comlist   if isimmort($n)
or isnpc($n)
else
  mpforce $n config -autoloot
  mpforce $n channel +yell
  mpechoat $n Do not alter your autoloot configuration until duel is complete.
endif
~
#ENDPROG

#ENDROOM
--]]
end

-- These may be obsolete, appears to be an intro area, why is it part of Limbo?
function OtherRooms()
--[[
#ROOM
Vnum     44
Name     The Shores of The Dragon Sea~
Sector   water_swim~
Flags    norecall noastral~
Desc     You have come to these shores in an attempt to aid others, your very 
blood may soon cover this beach though.
~
#EXIT
Direction northeast~
ToRoom    45
#ENDEXIT

#ENDROOM

#ROOM
Vnum     45
Name     The Path to Caermont~
Sector   field~
Flags    norecall noastral~
Desc     The path you travel now is long and arduous, perhaps soon you will
find a place to rest, or a battle to fight.
~
#EXIT
Direction east~
ToRoom    46
#ENDEXIT

#EXIT
Direction south~
ToRoom    47
#ENDEXIT

#EXIT
Direction southeast~
ToRoom    65
#ENDEXIT

#EXIT
Direction southwest~
ToRoom    44
#ENDEXIT

#ENDROOM

#ROOM
Vnum     46
Name     The Gates of Caermont~
Sector   field~
Flags    norecall noastral~
Desc     You stand before the battered walls of Caermont.  The walls have
withstood the assualts so far but how long this will last is unknown.
~
#EXIT
Direction north~
ToRoom    48
Key       39
Keywords  gates gate~
Flags     isdoor closed nopassdoor~
#ENDEXIT

#EXIT
Direction south~
ToRoom    65
#ENDEXIT

#EXIT
Direction west~
ToRoom    45
#ENDEXIT

#EXIT
Direction southwest~
ToRoom    47
#ENDEXIT

Reset D 0 46 0 1
#ENDROOM

#ROOM
Vnum     47
Name     The Trail of Blood~
Sector   field~
Flags    norecall noastral~
Desc     Most of the battles have been fought along this trail.  The mud which 
makes up the trai is a deep red (almost brown) in color, and the stench
of the dead pervades the area.
~
#EXIT
Direction north~
ToRoom    45
#ENDEXIT

#EXIT
Direction east~
ToRoom    65
#ENDEXIT

#EXIT
Direction northeast~
ToRoom    46
#ENDEXIT

#EXIT
Direction southeast~
ToRoom    66
#ENDEXIT

#ENDROOM

#ROOM
Vnum     48
Name     The Courtyard of Caermont~
Sector   field~
Flags    norecall noastral~
Desc     Men run around rebuilding walls, filling buckets, moving stores to
and fro.  This keep is under siege and it shows.  With little doubt
you think these people can not last much longer.
~
#EXIT
Direction north~
ToRoom    50
#ENDEXIT

#EXIT
Direction east~
ToRoom    51
#ENDEXIT

#EXIT
Direction south~
ToRoom    46
Key       39
Keywords  gates gate~
Flags     isdoor closed nopassdoor~
#ENDEXIT

#EXIT
Direction west~
ToRoom    49
#ENDEXIT

Reset D 0 48 2 1
#ENDROOM

#ROOM
Vnum     49
Name     The Stables~
Sector   inside~
Flags    indoors norecall noastral~
Desc     These stables are empty right now.  All the mounts which were at one
time stabled here have been either slain or captured.
~
#EXIT
Direction east~
ToRoom    48
#ENDEXIT

#ENDROOM

#ROOM
Vnum     50
Name     Gardens of the Gods~
Sector   forest~
Desc     High in the hills of Darkhaven, you have found a mystical garden overlooking
the busy valley below.  The seascape is visible to your east, and a breeze
blows gently over you.  Each inch of the grounds has been painstakingly
manicured by the local monks.  Unique and colorful flowers adorn each inch
of the garden.  This place is used for the holiest of ceremonies as it is
rumoured to have mystical qualities.  Pale moonlight enshrouds the area in a
veil of blue light.  The Holy Cathedral's entrance looms just to the west...
~
#EXIT
Direction west~
ToRoom    51
#ENDEXIT

#EXIT
Direction down~
ToRoom    52
#ENDEXIT

Reset O 0 61 1 50
Reset O 0 98 1 50
#EXDESC
ExDescKey    stone~
ExDesc       The stones are polished to perfection and placed neatly to provide a
spectator area.
~
#ENDEXDESC

#EXDESC
ExDescKey    flower flowers~
ExDesc       The most exotic of flowers adorn each inch of the garden.  Thousands of
brilliant deep red roses, poppies, orchids and lilies.
~
#ENDEXDESC

#ENDROOM

#ROOM
Vnum     51
Name     The Holy Cathedral~
Sector   inside~
Flags    indoors noastral~
Desc     The joyous music of the choir fills the room, masking the sounds of your
footsteps as you step across the wooden floors polished to a brilliant
shine within the cathedral.  Finding your seat with the oak pews in
this majestic room, your eyes bathe in the beauty of intricate paintings
covering the large domed ceiling.  At the front of the cathedral, a large
crucifix is mounted just below the balcony where the choir performs their
songs of praise.  The harmonious sounds are accompanied by a large pipe
organ built into the west wall.
~
#EXIT
Direction east~
ToRoom    50
#ENDEXIT

#EXDESC
ExDescKey    balcony~
ExDesc       Glancing high above the crucifix behind the altar, you see a balcony
filled with several angels singing joyously in praise for the event
that is about to occur.
~
#ENDEXDESC

#EXDESC
ExDescKey    organ~
ExDesc       A large pipe organ stands along the east wall, filling the room with
beautiful orchestral sounds spilling from the large pipes that extend
completely to the ceiling.
~
#ENDEXDESC

#EXDESC
ExDescKey    floor~
ExDesc       The old wooden floors, carefully polished to a brilliant shine, show
no wear despite the large numbers of footsteps that have made this same
path.  Your steps reverberate loudly throughout the cathedral.
~
#ENDEXDESC

#EXDESC
ExDescKey    couple~
ExDesc       /:""|                 .****,
             (\/)   |:`66|_                @@@@@\ `,
              \/    C`    _)               aa`@@@\  \
                     \ ._|        _ _     (_   ?@@|  \
                      )_/        ( Y )     =' @@@@|   |
                     /`\8\        \ /       \ (``/    |
                    || |8|      ___Y___     /^^\ |    /
                    || |8|     {-@- -@-}   /\::/||    |  (\/)
                    || |8|     {_______}   \ | |||    \   \/
                    || |-|    ____)_(____   \| |||     \
     (\/)           :| |=:   {-@- -@- -@-}   |:|\\.:.:.::.
      \/      _____ ||_|,|   {___________}   |:| \ ':':':`
             { -@- } ))) |_______)___(______/(((  \{ -@- }
             {_____} |   {-@- -@- -@- -@- -@-}     {_____}
~
#ENDEXDESC

#MUDPROG
Progtype  speech_prog~
Arglist   p prepare wedding~
Comlist   if isimmort($n)
  mpat 21000 mpoload 62
  mpat 3001 mpoload 62
endif
~
#ENDPROG

#MUDPROG
Progtype  speech_prog~
Arglist   p I do~
Comlist   mpe _whi Songs of rejoice fill the air as the choir lifts its voice in praise.
~
#ENDPROG

#MUDPROG
Progtype  act_prog~
Arglist   p is dying of thirst!~
Comlist   mpoload 1699
mea 0.$n The waiter pours you a glass of water and returns to the back of
mea 0.$n the cathedral.
give water 0.$n
mpforce 0.$n drink water
~
#ENDPROG

#MUDPROG
Progtype  act_prog~
Arglist   p is starved~
Comlist   mpoload 24854
mea 0.$n The caterer quietly brings you a piece of cake and retires to the
mea 0.$n back of the cathedral.
give cake 0.$n
mpforce 0.$n eat cake
~
#ENDPROG

#MUDPROG
Progtype  speech_prog~
Arglist   p Please be seated.~
Comlist   if isimmort($n)
  mpforce all sit
  mpat 3001 mppurge lever
  mpat 21000 mppurge lever
endif
~
#ENDPROG

#ENDROOM

#ROOM
Vnum     52
Name     The Reception Line~
Sector   inside~
Desc     Well wishers gather here to bid farewell to the newly married couples.
There are rose petals strewn around the path.  Just up from here is
the mystical Garden of the Gods.
~
#EXIT
Direction up~
ToRoom    50
#ENDEXIT

#MUDPROG
Progtype  entry_prog~
Arglist   100~
Comlist   if isimmort($n)
else
  if ispkill($n)
    mptrans $n 3001
    mpat 0.$n mpforce $n look
  else
    mptrans $n 21001
    mpat 0.$n mpforce $n look
  endif
endif
~
#ENDPROG

#ENDROOM

#ROOM
Vnum     53
Name     A hallway~
Sector   inside~
Flags    indoors norecall noastral~
Desc     This hallway continues both north and east.
~
#EXIT
Direction west~
ToRoom    56
Flags     isdoor closed~
#ENDEXIT

Reset D 0 53 3 1
#ENDROOM

#ROOM
Vnum     54
Name     The Throne Room~
Sector   inside~
Flags    indoors norecall noastral~
Desc     This is the throne room of the keep.  Once ornate and lovely, it has
fallen into disrepair now with the ongoing war.
~
#EXIT
Direction east~
ToRoom    59
Flags     isdoor closed~
#ENDEXIT

#EXIT
Direction west~
ToRoom    57
Flags     isdoor closed~
#ENDEXIT

Reset D 0 54 1 1
Reset D 0 54 3 1
#ENDROOM

#ROOM
Vnum     55
Name     A hallway~
Sector   inside~
Flags    indoors norecall noastral~
Desc     This hall continues both north and west.
~
#EXIT
Direction north~
ToRoom    59
#ENDEXIT

#EXIT
Direction east~
ToRoom    67
Flags     isdoor closed~
#ENDEXIT

Reset D 0 55 1 1
#ENDROOM

#ROOM
Vnum     56
Name     A small room~
Sector   inside~
Flags    indoors norecall noastral~
Desc     This is a small bedroom.
~
#EXIT
Direction east~
ToRoom    53
Flags     isdoor closed~
#ENDEXIT

Reset D 0 56 1 1
#ENDROOM

#ROOM
Vnum     57
Name     A hallway~
Sector   inside~
Flags    indoors norecall noastral~
Desc     This hall continues to the south and northeast.
~
#EXIT
Direction east~
ToRoom    54
Flags     isdoor closed~
#ENDEXIT

#EXIT
Direction south~
ToRoom    53
#ENDEXIT

#EXIT
Direction northeast~
ToRoom    58
#ENDEXIT

Reset D 0 57 1 1
#ENDROOM

#ROOM
Vnum     58
Name     Behind the Throne~
Sector   inside~
Flags    indoors norecall noastral~
Desc     This small passage runs behind the throne room and allows for the
courtiers to move about without drawing the notice of the court.
~
#EXIT
Direction down~
ToRoom    60
Key       39
Keywords  trapdoor~
Flags     isdoor closed locked secret~
#ENDEXIT

#EXIT
Direction southeast~
ToRoom    59
#ENDEXIT

#EXIT
Direction southwest~
ToRoom    57
#ENDEXIT

Reset D 0 58 5 2
#ENDROOM

#ROOM
Vnum     59
Name     A hallway~
Sector   inside~
Flags    indoors norecall noastral~
Desc     This hall continues to the south and northwest.
~
#EXIT
Direction south~
ToRoom    55
#ENDEXIT

#EXIT
Direction west~
ToRoom    54
Flags     isdoor closed~
#ENDEXIT

#EXIT
Direction northwest~
ToRoom    58
#ENDEXIT

Reset D 0 59 3 1
#ENDROOM

#ROOM
Vnum     60
Name     The Secret Passage~
Sector   inside~
Flags    dark indoors norecall nosummon~
Desc     You are in a dank and dark tunnel.  It was apparently made recently
as tools still lie about in disarray.  Where this might lead is unknown
though.
~
#EXIT
Direction south~
ToRoom    61
#ENDEXIT

#EXIT
Direction up~
ToRoom    58
Key       7
Flags     isdoor closed locked pickproof~
#ENDEXIT

Reset D 0 60 4 2
#ENDROOM

#ROOM
Vnum     61
Name     The Secret Passage~
Sector   inside~
Flags    dark indoors norecall nosummon~
Desc     This secret passage continues to the north and south.
~
#EXIT
Direction north~
ToRoom    60
#ENDEXIT

#EXIT
Direction south~
ToRoom    62
#ENDEXIT

#ENDROOM

#ROOM
Vnum     62
Name     The Secret Passage~
Sector   inside~
Flags    dark indoors norecall nosummon~
Desc     This secret passage continues to the north and south.
~
#EXIT
Direction north~
ToRoom    61
#ENDEXIT

#EXIT
Direction south~
ToRoom    63
#ENDEXIT

#ENDROOM

#ROOM
Vnum     63
Name     The Secret Passage~
Sector   inside~
Flags    dark indoors norecall nosummon~
Desc     This secret passage continues to the north and south.
~
#EXIT
Direction north~
ToRoom    62
#ENDEXIT

#EXIT
Direction south~
ToRoom    64
#ENDEXIT

#ENDROOM

#ROOM
Vnum     64
Name     The Secret Passage~
Sector   inside~
Flags    dark indoors norecall nosummon~
Desc     This passage continues to the north from here.
~
#EXIT
Direction north~
ToRoom    63
#ENDEXIT

#EXIT
Direction up~
ToRoom    65
Flags     isdoor closed~
#ENDEXIT

#ENDROOM

#ROOM
Vnum     65
Name     The Hill of Bones~
Sector   hills~
Flags    norecall nosummon~
Desc     You stand atop a hill made of the bodies and corpses of the fallen.
Broken limbs, mangled bodies, shattered weapons, and rusted armor all
lie about here in a meangerie of destruction.  This hill has seen 
more death in the last five years of war, than any other land has seen
in over a century.
~
#EXIT
Direction north~
ToRoom    46
#ENDEXIT

#EXIT
Direction south~
ToRoom    66
#ENDEXIT

#EXIT
Direction west~
ToRoom    47
#ENDEXIT

#EXIT
Direction down~
ToRoom    64
Keywords  bones hole~
Flags     isdoor closed secret~
#ENDEXIT

#EXIT
Direction northwest~
ToRoom    45
#ENDEXIT

#EXIT
Direction southeast~
ToRoom    68
#ENDEXIT

#ENDROOM

#ROOM
Vnum     66
Name     The Trail of Blood~
Sector   field~
Flags    norecall nosummon~
#EXIT
Direction north~
ToRoom    65
#ENDEXIT

#EXIT
Direction east~
ToRoom    68
#ENDEXIT

#EXIT
Direction south~
ToRoom    70
#ENDEXIT

#EXIT
Direction northwest~
ToRoom    47
#ENDEXIT

#ENDROOM

#ROOM
Vnum     67
Name     A Small Room~
Sector   city~
Flags    nomob norecall nosummon~
Desc     This is a small bedroom.
~
#EXIT
Direction west~
ToRoom    55
Flags     isdoor closed~
#ENDEXIT

Reset D 0 67 3 1
#ENDROOM

#ROOM
Vnum     68
Name     The Trail of Blood~
Sector   field~
Flags    norecall nosummon~
#EXIT
Direction west~
ToRoom    66
#ENDEXIT

#EXIT
Direction northwest~
ToRoom    65
#ENDEXIT

#EXIT
Direction southeast~
ToRoom    69
#ENDEXIT

#ENDROOM

#ROOM
Vnum     69
Name     The Trail of Blood~
Sector   field~
Flags    norecall nosummon~
#EXIT
Direction east~
ToRoom    71
#ENDEXIT

#EXIT
Direction northwest~
ToRoom    68
#ENDEXIT

#ENDROOM

#ROOM
Vnum     70
Name     An Exquisite Garden~
Sector   city~
Flags    tunnel safe nosummon noastral~
Stats    0 0 2
Desc     Standing in a ravishing broad avenue within this garden, it is found to 
be lined with epicurean emerald trees, shrubbery, and groups of charming 
sculpture.  Alas, this illustrious locale reminds you of the 
romanticizing Versailles.  Coming here with one who belongs to your heart 
is definitely encouraged, as this social milieu relaxes your senses, 
setting the impeccable scene for a honeymoon of sorts.  Secluded groves 
lie unmistakably to the east, many occupied by lovers flaunting and 
flirting with ecstasy.  Nightfall has almost commenced, and the period 
to dine, dance, and check into a local chateau approaches.
~
#EXIT
Direction north~
ToRoom    72
Desc      The Chateau L'amour...ooOOooOOooOOoo la la!
~
#ENDEXIT

#EXIT
Direction east~
ToRoom    71
Desc      An extravagant grove awaits two lovers...
~
#ENDEXIT

Reset O 0 72 1 70
#MUDPROG
Progtype  entry_prog~
Arglist   100~
Comlist   mea 0.$n _lbl The ballad of chirping birds reverberates throughout your ears...
~
#ENDPROG

#MUDPROG
Progtype  rand_prog~
Arglist   70~
Comlist   mpe _lbl The ballad of chirping birds reverberates throughout your ears...
~
#ENDPROG

#ENDROOM

#ROOM
Vnum     71
Name     An Exalted Grove~
Sector   hills~
Flags    tunnel safe nosummon noastral~
Stats    0 0 2
Desc     You sit in an attractive, but private grove, the renowned garden off in 
the distant background.  Intricate and flamboyant furnishings of 
dazzling white marble encircle the grove, while love's innocent laughter 
in the air drifts into your ears, delivering much delight.  The 
omnipotent god of passion has certainly paid many a visit here, to 
consolidate two into infinite rapture.
~
#EXIT
Direction west~
ToRoom    70
Desc      Back to the illustrious garden...
~
#ENDEXIT

#ENDROOM

#ROOM
Vnum     72
Name     Chateau L'amour~
Sector   inside~
Flags    indoors tunnel safe nosummon noastral~
Stats    0 0 2
Desc     You stand in the lobby of the famed and renowned Chateau Lamour, the 
prime candidate for a honeymoon escapade.  Several effulgent gold-plated 
dais layer on top of each other until finally reaching the dark chestnut 
counter.  A slender and distinguished looking man with a thin moustache 
curled flawlessly, and a small monocle awaits your gesture.  To the west 
a grand hall covered with enormous looking-glasses and kaleidoscopes 
brings much attention, while a symphony of fervent classical music fills 
your ears, bringing calmness and serenity to your soul.
~
#EXIT
Direction south~
ToRoom    70
Desc      Returning to the Garden...
~
#ENDEXIT

Reset M 0 70 1 72
#EXDESC
ExDescKey    painting louis XIV~
ExDesc       An antiquated oil painting of Louis XIV is mounted here in a frilled 
golden border--the word "pretentious" is scrawled into the bottom.
~
#ENDEXDESC

#ENDROOM

#ROOM
Vnum     73
Name     The Chateau "Cabaret" ~
Sector   inside~
Flags    indoors tunnel safe nosummon noastral~
Stats    0 0 2
Desc     This room is dimly lit, with an atmosphere full of passion and 
merriment.  The musical interlude filling your ears brings enchantment 
and rapture, as this is the perfect diner to eat with one you take 
affection to.  The occasional clinking and clanging of dishes and soft 
clamoring from the kitchen assures you they are hard at work, preparing 
delectable entrees.  Evidently the waiter must be called out to, as the 
people here are extremely occupied.
~
#EXIT
Direction southwest~
ToRoom    72
#ENDEXIT

Reset M 0 71 1 73
#MUDPROG
Progtype  rand_prog~
Arglist   60~
Comlist   mpe _whi The candle at your table burns slowly, flickering slightly...
~
#ENDPROG

#ENDROOM

#ROOM
Vnum     74
Name     The L'amour Suite~
Sector   inside~
Flags    nomob indoors tunnel safe nosummon noastral~
Stats    0 0 2
Desc     Ethereal furnishings and luxuriant carpets astound you upon entering 
this entourage, giving a very fervent and passionate sensation.  The 
primary component being the abundant intricately designed heart-shaped 
bed, taking more than two thirds of the room up, as you notice some 
extremely arousing french-lace lingerie, as well as a pair of silk 
boxers laid out on it.  The humming and effervescing sound of a sensual 
jacuzzi brings ecstasy to your senses, as this whole room is perfect
for.....discussing your financial terms.
~
Reset O 0 75 1 74
#MUDPROG
Progtype  speech_prog~
Arglist   return~
Comlist   if ispc($n)
mptrans $n 21001
endif
~
#ENDPROG

#MUDPROG
Progtype  entry_prog~
Arglist   100~
Comlist   if sex($n) == 2
mpforce $n rem all
mpoload 73
mpat 74 drop lingerie
mpforce $n get lingerie
mpforce $n wear lingerie
endif
if sex($n) == 1
mpforce $n rem all
mpoload 74
mpat 74 drop boxers
mpforce $n get boxers
mpforce $n wear boxers
endif
~
#ENDPROG

#ENDROOM

#ROOM
Vnum     75
Name     The Swamp of Gore~
Sector   forest~
Flags    nomob norecall noastral~
#EXIT
Direction north~
ToRoom    77
#ENDEXIT

#EXIT
Direction east~
ToRoom    74
#ENDEXIT

#EXIT
Direction south~
ToRoom    76
#ENDEXIT

#EXIT
Direction west~
ToRoom    74
#ENDEXIT

#EXIT
Direction northeast~
ToRoom    78
#ENDEXIT

#EXIT
Direction northwest~
ToRoom    72
#ENDEXIT

#EXIT
Direction southeast~
ToRoom    73
#ENDEXIT

#EXIT
Direction southwest~
ToRoom    73
#ENDEXIT

#ENDROOM

#ROOM
Vnum     76
Name     The Swamp of Gore~
Sector   forest~
Flags    nomob norecall noastral~
#EXIT
Direction north~
ToRoom    75
#ENDEXIT

#EXIT
Direction east~
ToRoom    73
#ENDEXIT

#EXIT
Direction south~
ToRoom    77
#ENDEXIT

#EXIT
Direction west~
ToRoom    73
#ENDEXIT

#EXIT
Direction northeast~
ToRoom    74
#ENDEXIT

#EXIT
Direction northwest~
ToRoom    74
#ENDEXIT

#EXIT
Direction southeast~
ToRoom    72
#ENDEXIT

#EXIT
Direction southwest~
ToRoom    72
#ENDEXIT

#ENDROOM

#ROOM
Vnum     77
Name     The Swamp of Gore~
Sector   forest~
Flags    nomob norecall noastral~
#EXIT
Direction north~
ToRoom    76
#ENDEXIT

#EXIT
Direction east~
ToRoom    72
#ENDEXIT

#EXIT
Direction south~
ToRoom    74
#ENDEXIT

#EXIT
Direction west~
ToRoom    72
#ENDEXIT

#EXIT
Direction northeast~
ToRoom    73
#ENDEXIT

#EXIT
Direction northwest~
ToRoom    73
#ENDEXIT

#EXIT
Direction southwest~
ToRoom    71
#ENDEXIT

#ENDROOM

#ROOM
Vnum     78
Name     The Road of Broken Bones~
Sector   field~
Flags    nomob norecall noastral~
#EXIT
Direction northeast~
ToRoom    92
#ENDEXIT

#EXIT
Direction southwest~
ToRoom    75
#ENDEXIT

#ENDROOM

#ROOM
Vnum     79
Name     The Dusty Trail~
Sector   field~
Flags    nomob norecall noastral~
#EXIT
Direction north~
ToRoom    71
#ENDEXIT

#EXIT
Direction southeast~
ToRoom    80
#ENDEXIT

#ENDROOM

#ROOM
Vnum     80
Name     The Dusty Trail~
Sector   field~
Flags    nomob norecall noastral~
#EXIT
Direction east~
ToRoom    87
#ENDEXIT

#EXIT
Direction south~
ToRoom    81
#ENDEXIT

#EXIT
Direction west~
ToRoom    86
#ENDEXIT

#EXIT
Direction northwest~
ToRoom    79
#ENDEXIT

#ENDROOM

#ROOM
Vnum     81
Name     The Promontory~
Sector   hills~
Flags    nomob norecall noastral~
#EXIT
Direction north~
ToRoom    80
#ENDEXIT

#ENDROOM

#ROOM
Vnum     82
Name     The Cliffs on the Sea~
Sector   mountain~
Flags    nomob norecall noastral~
#EXIT
Direction up~
ToRoom    70
#ENDEXIT

#ENDROOM

#ROOM
Vnum     83
Name     Above the Sea~
Sector   hills~
Flags    nomob norecall nosummon noastral~
#EXIT
Direction northwest~
ToRoom    70
#ENDEXIT

#EXIT
Direction southeast~
ToRoom    84
#ENDEXIT

#ENDROOM

#ROOM
Vnum     84
Name     Above the Sea~
Sector   hills~
Flags    nomob norecall noastral~
#EXIT
Direction northwest~
ToRoom    83
#ENDEXIT

#EXIT
Direction southeast~
ToRoom    85
#ENDEXIT

#ENDROOM

#ROOM
Vnum     85
Name     Above the Sea~
Sector   hills~
Flags    nomob norecall noastral~
#EXIT
Direction east~
ToRoom    86
#ENDEXIT

#EXIT
Direction northwest~
ToRoom    84
#ENDEXIT

#ENDROOM

#ROOM
Vnum     86
Name     Above the Sea~
Sector   hills~
Flags    nomob norecall nosummon noastral~
#EXIT
Direction east~
ToRoom    80
#ENDEXIT

#EXIT
Direction west~
ToRoom    85
#ENDEXIT

#ENDROOM

#ROOM
Vnum     87
Name     The Sea Cliff~
Sector   hills~
Flags    nomob norecall noastral~
#EXIT
Direction east~
ToRoom    88
#ENDEXIT

#EXIT
Direction west~
ToRoom    80
#ENDEXIT

#ENDROOM

#ROOM
Vnum     88
Name     The Mountain Foot~
Sector   mountain~
Flags    nomob norecall noastral~
#EXIT
Direction west~
ToRoom    87
#ENDEXIT

#EXIT
Direction up~
ToRoom    90
#ENDEXIT

#ENDROOM

#ROOM
Vnum     90
Name     The Mesa~
Sector   mountain~
Flags    nomob norecall noastral~
#EXIT
Direction north~
ToRoom    91
#ENDEXIT

#EXIT
Direction down~
ToRoom    88
#ENDEXIT

#ENDROOM

#ROOM
Vnum     91
Name     The Mesa~
Sector   mountain~
Flags    nomob norecall noastral nofloor~
#EXIT
Direction south~
ToRoom    90
#ENDEXIT

#EXIT
Direction down~
ToRoom    78
#ENDEXIT

#ENDROOM

#ROOM
Vnum     92
Name     Before the Keep~
Sector   city~
Flags    nomob norecall noastral~
#EXIT
Direction east~
ToRoom    93
Keywords  gates gate portcullis~
Flags     isdoor closed locked~
#ENDEXIT

#EXIT
Direction southwest~
ToRoom    78
#ENDEXIT

Reset D 0 92 1 2
#ENDROOM

#ROOM
Vnum     93
Name     The Inner Keep~
Sector   inside~
Flags    dark nomob indoors norecall noastral~
#EXIT
Direction north~
ToRoom    94
#ENDEXIT

#EXIT
Direction east~
ToRoom    96
#ENDEXIT

#EXIT
Direction south~
ToRoom    95
#ENDEXIT

#EXIT
Direction west~
ToRoom    92
Keywords  gates gate portcullis~
Flags     isdoor closed locked~
#ENDEXIT

Reset D 0 93 3 2
#ENDROOM

#ROOM
Vnum     94
Name     The Barracks~
Sector   inside~
Flags    dark nomob indoors norecall noastral~
#EXIT
Direction south~
ToRoom    93
#ENDEXIT

#ENDROOM

#ROOM
Vnum     95
Name     The Barracks~
Sector   inside~
Flags    dark nomob indoors norecall noastral~
#EXIT
Direction north~
ToRoom    93
#ENDEXIT

#ENDROOM

#ROOM
Vnum     96
Name     The Entry Hall~
Sector   inside~
Flags    dark nomob indoors norecall noastral~
#EXIT
Direction west~
ToRoom    93
#ENDEXIT

#EXIT
Direction up~
ToRoom    97
#ENDEXIT

#ENDROOM

#ROOM
Vnum     97
Name     Before the Throne~
Sector   inside~
Flags    dark nomob indoors norecall noastral~
#EXIT
Direction north~
ToRoom    98
#ENDEXIT

#EXIT
Direction down~
ToRoom    96
#ENDEXIT

#ENDROOM

#ROOM
Vnum     98
Name     The Throne Room~
Sector   inside~
Flags    dark nomob indoors norecall noastral~
#EXIT
Direction south~
ToRoom    97
#ENDEXIT

#EXIT
Direction down~
ToRoom    99
#ENDEXIT

#ENDROOM

#ROOM
Vnum     99
Name     A Space in Time~
Sector   inside~
Flags    nomob indoors safe norecall nosummon noastral~
Desc     As if awakening from an unsettlingly surreal dream, you find yourself in
unfamiliar surroundings far from whence you came.  As the torpor wears
off and your eyes begin to refocus, you realize you have been brought
here by the Gods to receive a message of relative import.  Checking
through your inventory, you look for some sign of indication to validate
your purpose and assure you that the deconstruction of your mind has not
yet been put into play.
~
Reset O 0 41 1 99
#MUDPROG
Progtype  act_prog~
Arglist   p has entered~
Comlist   if isimmort($n)
  mpe You may have a note in your inventory...
else
  mpechoat $n You move closely to examine the orb before you...
  mpforce $n examine orb
  mpforce $n i
endif
~
#ENDPROG

#ENDROOM

#ENDAREA--]]
end

LoadArea();	-- EXECUTE THE AREA

-- EOF