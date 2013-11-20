-- ARAN.LUA
-- This is the zone-file for the city of Aran
-- Revised: 2013.11.13
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(GetAppSetting("dataPath") .. "\\modules\\module_area.lua")();

LoadArea()	-- EXECUTE THE AREA

function LoadArea()
	systemLog("=================== AREA 'INN OF THE SEVEN REALMS' INITIALIZING ===================");
	newArea = LCreateArea(1, "Inn of the Seven Realms");
	area.this = newArea;
	area.this.Author = "AmonGwareth";
	area.this.HighSoftRange = 60;
	area.this.HighHardRange = 60;
	area.this.HighEconomy = 45009000;

	FirstFloorMobs();
	SecondFloorMobs();
	BasementMobs();
	Objects();
	FirstFloorRooms();
	SecondFloorRooms();
	BasementRooms();
	systemLog("=================== AREA 'INN OF THE SEVEN REALMS' - COMPLETED ================");
end
	
function FirstFloorMobs()
	systemLog("=================== AREA 'INN OF THE SEVEN REALMS' - 1ST FLOOR MOBS ===================");
	mobile.this = CreateMobile(100, "bron barkeep", "Bron Ma'Ganor, Barkeep");
	mobile.this.LongDescription = "A tall, but extremely fat man is here.";
	mobile.this.Description = [[This tall, but extremely fat man of indeterminate age 
	stands behind the bar of this establishment.  His dark features mark him to be of 
	southern descent.  He has dark hair and a bushy, dark beard which have been 
	recently trimmed and his laugh echoes throughout the room.]];
	mobile.this.Attacks = "kick";

	-- Create a shop for Bron
	shop.this = CreateShop(130, 90, 7, 21);

	mobile.this = CreateMobile(101, "chalan cook", "Chalan Ma'Ganor, Cook");
	mobile.this.LongDescription = "A large woman with dark, brown hair and a kind face is here.";
	mobile.this.Description = [[Wife of Bron, the proprietor of the Inn, Chalan does 
	most of the cooking and runs the household staff.   She is a large woman with 
	dark brown hair and a sharp tongue, but a kind face.]];
	mobile.this.Gender = "female";

	mobile.this = CreateMobile(102, "rashan leftenant soldier", "Leftenant Rashan Aseph");
	mobile.this.LongDescription = "An exhausted soldier of medium height is here.";
	mobile.this.Description = [[A thick man of medium height with a dark, trimmed 
	beard and mustache.  He has a look about him as if he is exhausted and is near 
	his breaking point.  His eyes are haunted, as if he has seen more than one ever 
	should.  But, there is also a sense of strength, of force of will, about him and he 
	carries himself with assurance and steadfastness.]];
	mobile.this.Attacks = "trip kick";
	mobile.this.DEfenses = "dodge parry";

	mobile.this = CreateMobile(103, "liase healer", "Liase Al'verran");
	mobile.this.LongDescription = "A young enthusiastic cleric is here.";
	mobile.this.Description = [[description goes here]];
	mobile.this.Class = "cleric";
	mobile.this.Gender = "female";
	mobile.this.Defenses = "heal";

	mobile.this = CreateMobile(104, "natania child", "Natania Ma'Ganor");
	mobile.this.LongDescription = "A small child with greasy, dark hair is here.";
	mobile.this.Description = [[A small child of fewer than ten years, Natania has greasy, 
	dark hair and smudges on her face and hands from helping her mother in the kitchen.]];
	mobile.this.Gender = "female";
	mobile.this.ActFlags = "npc sentinel wimpy";

	mobile.this = CreateMobile(105, "alania elf", "Alania Telkhat, Healer");
	mobile.this.LongDescription = "An aged elf is here.";
	mobile.this.Description = [[An elf, of the Caorlei bloodline, who appears to have many 
	years behind her, Alania Telkhat has worked as a healer and counselor for many years. 
	She is wearing plain, but comfortable clothes and has a look of unease on her face as 
	she surveys the crowds of patrons in the Inn.]];
	mobile.this.Race = "elf";
	mobile.this.Class = "cleric";
	mobile.this.Gender = "female";
	mobile.this.Speaks = "common elven";
	mobile.this.Defenses = "heal";

	mobile.this = CreateMobile(106, "attan sailor", "Captain Attan Al'sha'if");
	mobile.this.LongDescription = "A sailor with trinkets in his braided hair is here.";
	mobile.this.Description = [[A short, thin man with braided hair, a curled mustache, and the 
	look of someone who is ill at ease on land, Attan Al'sha'if is the Captain of a ship in the Dusharan 
	Harbor.  He has trinkets hanging from his braids, a broad hat in his dirty hands, and is looking 
	around at the patrons of the Inn with a mixture of humor and disdain.]];
	mobile.this.Attacks = "gouge kick";
	mobile.this.Defenses = "parry";

	newMob = LCreateMob(107, "patron");
	mobile.this = newMob;
	mobile.this.ShortDescription = "Bar Patron";
	mobile.this.LongDescription = "A bar patron is here.";
	mobile.this.Description = [[A nondescript person is common clothes is drinking a mug of ale.]]
	mobile.this.ActFlags = "npc stayarea";

	newMob = LCreateMob(108, "adventurer weary");
	mobile.this = newMob;
	mobile.this.ShortDescription = "Weary Adventurer";
	mobile.this.LongDescription = "A weary adventurer is here."
	mobile.this.Description = [[A nondescript person who is looking for adventure, but is wearily eating 
	a bowl of stew and drinking from a tankard of ale.]];
	mobile.this.ActFlags = "npc stayarea";
	-- Randomly say something about the War or the Undead or some facet of the Realms

	newMob = LCreateMob(109, "barmaid server");
	mobile.this = newMob;
	mobile.this.ShortDescription = "Barmaid";
	mobile.this.LongDescription = "A barmaid is here."; 
	mobile.this.Description = [[A nondescript woman who deftly weaves through the patrons and tables to 
	serve steaming bowls of food and tankards of ale.]];
	mobile.this.Gender = "female";
	mobile.this.ActFlags = "npc stayarea wimpy";

	newMob = LCreateMob(110, "bouncer");
	mobile.this = newMob;
	mobile.this.ShortDescription = "Burly Bouncer";
	mobile.this.LongDescription = "A very large man with massive arms is here.";
	mobile.this.Description = [[A very large man with arms as thick as tree trunks.  He has a very sour look 
	on his face and appears to be someone you don�t wish to mess with.]];
	mobile.this.ActFlags = "npc stayarea";
	mobile.this.Attacks = "punch";
	-- Randomly cross arms and glare

	newMob = LCreateMob(111, "storyteller bard");
	mobile.this = newMob;
	mobile.this.ShortDescription = "Lively Bard";
	mobile.this.LongDescription = "A lively and enthusiastic storyteller is here.";
	mobile.this.Description = [[An older man with long hair tied into braids and a long, curled and waxed 
	mustache that he continually twists, the Bard plays and sings with equal talent. He songs tell stories 
	of ancient days, of great heroes and damsels in distress, while his music is both haunting and 
	lively in turns and as the crowd demands.  Despite the music and song he manages to flash a smile 
	at every passing barmaid and frequently takes a draught from the mug sitting near his chair.]];
	mobile.this.Class = "Thief";
	mobile.this.Attacks = "backstab";
	mobile.this.Defenses = "dodge";
	-- MUD PROGS
	-- Singing
	-- Wink or pinch barmaids as they pass by
end

function SecondFloorMobs()
	systemLog("=================== AREA 'INN OF THE SEVEN REALMS' - 2ND FLOOR MOBS ===================");
	mobile.this = CreateMobile(112, "arina noble", "Arina Tal'shon");
	mobile.this.LongDescription = ";
	mobile.this.Description = [[]];
	mobile.this.Gender = "female";

	mobile.this = CreateMobile(113, "ismail servant", "Ismail Nikhet");
	mobile.this.LongDescription = ";
	mobile.this.Description = [[]];
	mobile.this.Race = "elf";
	mobile.this.Speaks = "common elven";

	mobile.this = CreateMobile(114, "dazhor dwarf mercenary", "Dazhor Ren");
	mobile.this.LongDescription = ";
	mobile.this.Description = [[]];
	mobile.this.Gender = "male";
	mobile.this.Race = "dwarf";
	mobile.this.Speaks = "common dwarven";
	mobile.this.Attacks = "kick bash";
	mobile.this.Defenses = "parry";

	mobile.this = CreateMobile(115, "manservant old elder", "Elderly Manservant");
	mobile.this.LongDescription = ";
	mobile.this.Description = [[]];
	mobile.this.Gender = "male";

	mobile.this = CreateMobile(116, "mercenary veteran", "Grizzled Mercenary");
	mobile.this.LongDescription = ";
	mobile.this.Description = [[]];
	mobile.this.Gender = "male";
	mobile.this.Attacks = "kick trip";
	mobile.this.Defenses = "parry";
end

function BasementMobs()
	systemLog("=================== AREA 'INN OF THE SEVEN REALMS' - BASEMENT MOBS ===================");
	mobile.this = CreateMobile(117, "giant rat", "Giant Rat");
	mobile.this.LongDescription = ";
	mobile.this.Description = [[]];
	mobile.this.Race = "rat";
	mobile.this.Speaks = "rodent";
	mobile.this.Speaking = "rodent";
	mobile.this.BodyParts = "head legs heart brains guts feet eye tail claws";
	mobile.this.Attacks = "bite";
	mobile.this.ActFlags = "npc stayarea aggressive";
	mobile.this.AffectedBy = "infrared";
end

function Objects()
	systemLog("=================== AREA 'INN OF THE SEVEN REALMS' - OBJECTS ===================");
	object.this = CreateObject(100, "stained apron", "armor");
	object.this.ShortDescription = "an apron with unidentifiable stains on it";
	object.this.LongDescription = "You see an apron with numerous, unidentifiable stains on it here.";
	object.this.WearFlags = "take body";
	object.this:SetStats(5, 100, 0, 0, 0);

	object.this = CreateObject(101, "wooden spoon")
	object.this.ShortDescription = "a spoon made of wood";
	object.this.LongDescription = "You see a spoon made of wood here.";
	object.this:SetStats(1, 5, 0, 0, 0);

	object.this = CreateObject(102, "studded leather hauberk", "armor");
	object.this.ShortDescription = "a hauberk made of studded leather";
	object.this.LongDescription = "You see a hauberk made of studded leather here.";
	object.this.WearFlags = "take body";
	object.this:SetStats(40, 1200, 0, 0, 0);

	object.this = CreateObject(103, "linen pants", "armor");
	object.this.ShortDescription = "a pair of linen pants";
	object.this.LongDescription = "You see a pair of linen pants here.";
	object.this.WearFlags = "take legs";
	object.this:SetStats(10, 250, 0, 0, 0);

	object.this = CreateObject(104, "heavy leather boots", "armor");
	object.this.ShortDescription = "a pair of heavy leather boots";
	object.this.LongDescription = "You see a pair of heavy boots made of leather here.";
	object.this.WearFlags = "take feet";
	object.this:SetStats(20, 400, 0, 0, 0);

	object.this = CreateObject(105, "fine blue cloak", "armor");
	object.this.ShortDescription = "a blue cloak made of fine material";
	object.this.LongDescription = "You see a blue cloak made of the finest material here.";
	object.this.WearFlags = "take back";
	object.this:SetStats(10, 1500, 0, 0, 0);

	object.this = CreateObject(106, "longsword", "weapon");
	object.this.ShortDescription = "a long metal sword";
	object.this.LongDescription = "You see a long metallic sword here.";
	object.this.WearFlags = "take wield";
	object.this:SetValues(10, 1, 6, 3, 0, 0);
	object.this:SetStats(15, 1600, 0, 0, 0);

	object.this = CreateObject(107, "parchment roll", "paper");
	object.this.ShortDescription = "a roll of parchment";
	object.this.LongDescription = "You see a rolled piece of parchment here.";
	object.set:SetStats(1, 40, 0, 0, 0);

	object.this = CreateObject(108, "long silk robe", "armor");
	object.this.ShortDescription = "a long robe made of silk";
	object.this.LongDescription = "You see a long robe made of the finest silk here.";
	object.this.WearFlags = "take body";
	object.this:SetStats(10, 2250, 0, 0, 0);

	object.this = CreateObject(109, "fine slippers", "armor");
	object.this.ShortDescription = "a pair of fine slippers";
	object.this.LongDescription = "You see a pair of finely made slippers here.";
	object.this.WearFlags = "take feet";
	object.this:SetStats(5, 750, 0, 0, 0);

	object.this = CreateObject(110, "symbol of melesa", "weapon");
	object.this.ShortDescription = "A Symbol of Melesa";
	object.this.LongDescription = "You see a heavy gold-encrusted Symbol to the goddess Melesa here.";
	object.this.WearFlags = "take wield";
	object.this:SetValues(12, 2, 8, 7, 0, );
	object.this:SetStats(40, 4000, 0, 0, 0);
	-- TODO Should the Symbol have some sort of affect or weaponspell?

	object.this = CreateObject(111, "wrinkled tunic", "armor");
	object.this.ShortDescription = "a wrinkled tunic";
	object.this.LongDescription = "You see a soiled and wrinkled tunic here.";
	object.this.WearFlags = "take body";
	object.this:SetStats(10, 75, 0, 0, 0);

	object.this = CreateObject(112, "linen hose", "armor");
	object.this.ShortDescription = "some linen hose";
	object.this.LongDescription = "You see some linen hose here.";
	object.this.WearFlags = "take legs";
	object.this:SetStats(10, 250, 0, 0, 0);
	
	object.this = CreateObject(113, "battered pot", "drinkcontainer");
	object.this.ShortDescription = "a battered pot";
	object.this.LongDescription = "You see a worn and battered metal pot here.";
	object.this:SetValues(15, 15, 0, 0, 0);
	object.this:SetStats(25, 25, 0, 0, 0);
	
	object.this = CreateObject(114, "linen shirt", "armor");
	object.this.ShortDescription = "a linen shirt";
	object.this.LongDescription = "You see a shirt made of linen here.";
	object.this.WearFlags = "take body";
	object.this:SetStats(10, 250, 0, 0, 0);
	
	object.this = CreateObject(115, "leather pants", "armor");
	object.this.ShortDescription = "a pair of leather pants";
	object.this.LongDescription = "You see a pair of leather pants here.";
	object.this.WearFlags = "take legs";
	object.this:SetStats(20, 400, 0, 0, 0);
	
	-- Jeweled Earrings
	-- Short Sword
	-- Woolen Shirt
	-- Woolen Pants
	-- Leather Boots
	-- Dagger
	-- Woolen Dress
	-- Slippers
	-- Empty Tankard
	-- Leather Straps
	-- Mug of Ale
	-- Glass of Wine
	-- Day-old Bread
	-- Bowl of Stew
	-- Hunk of Hard Cheese

	-- Huge Cask
	-- Tall Candelabra
	-- Table
	-- Stone Oven
	-- Large Cauldron
	-- Cupboard
	-- Barrel
	-- Crate
	-- Small Bed
	-- Small Dresser
	-- Large Bed
	-- Large Dresser
	-- Metal-bound Chest
	-- Wooden Chest
	-- Small Metallic Key
	-- Broken Crate
	-- Table
end

function FirstFloorRooms()
	systemLog("=================== AREA 'INN OF THE SEVEN REALMS' - 1ST FLOOR ROOMS ====================");
	room.this = CreateRoom(100, "A wood-paneled room", "inside", area.this);
	room.this.Description = [[This wood-paneled room is devoid of any furniture or decorations on the walls, 
	save for a large hearth.  A fire cackles merrily within the pit, spreading a welcome warmth to the room, 
	and causing shadows on the other walls to dance wildly.]];
	room.this:AddExtraDescription("hearth fireplace", [[A large, stone fireplace stands within the south wall 
	of the room.  It appears to be made of river stone, worn by years of water and later by the hands of many 
	visitors.  No decorations adorn the mantle atop the hearth.]]);
	AddExitToRoom(room.this, "east", 101, "A wooden door is to the east", "isdoor closed");

	room.this = CreateRoom(101, "Common Room (Northwest)", "inside", area.this);
	room.this.Description = [[This huge room is noisy and filled with numerous tables, patrons, and above all 
	else, noise.  Dozens of people of all stripes, creeds, and origin are enjoying themselves at the tables while 
	a host of servers attempt to deliver food and drink to out-stretched hands and thirsty mouths.  A large bar 
	dominates one side of the room, tables and booths fill much of the rest.  To the north an arched doorway 
	leads into the back of the Inn, likely to the kitchens and storerooms.]];
	room.this:AddExit(102, "east", "A path amongst the tables leads east");
	room.this:AddExit(103, "south", "A path amongst the tables leads south");
	room.this:AddExit(104, "north", "An arched doorway leads out of the Common Room");
	room.this:AddExit(100, "west", "A wooden door leads west out of the room");

	SetupBarPatron(room.this, 101, 2);
	SetupWearingAdventurer(room.this, 101, 1);

	room.this = CreateRoom(102, "Common Room (North)", "inside", area.this);
	room.this.Description = [[This huge room is noisy and filled with numerous tables, patrons, and above all else, 
	noise.  Dozens of people of all stripes, creeds, and origin are enjoying themselves at the tables while a host of 
	servers attempt to deliver food and drink to out-stretched hands and thirsty mouths.  A large bar dominates this 
	side of the room, tables and booths fill much of the rest.  Above the bar hangs a massive shield flanked by two 
	large greatswords.]];
	room.this:AddExtraDescription("shield", [[This is a massive, square body shield with a center of wood and 
	a rim of metal. It appears functional and not just decorative and has a crest you cannot identify on its surface.]]);
	room.this:AddExtraDescription("crest", [[A large crest of a tree, flanked by two knights standing rigid with pikes,  
	A banner scrolls beneath the tree with the phrase: Honor and Duty.]]);
	room.this:AddExit(101, "west", "A path amongst the tables leads west");
	room.this:AddExit(105, "east", "A path amongst the tables leads east");
	room.this:AddExit(107, "south", "A path amongst the tables leads south");
	-- Huge Cask
	
	-- Spawn Bron with an Apron here
	reset.this = AddMobileToRoom(room.this, 100, 102, 1);
	EquipOnMobile(reset.this, 100, 3);
	-- Metal Key
	
	-- Bron sells:
	-- Mug of Ale
	-- Glass of Wine
	-- Day-old Bread
	-- Bowl of Stew
	-- Hunk of Hard Cheese

	SetupBarmaid(room.this, 102, 1);

	room.this = CreateRoom(103, "Common Room (Southwest)", "inside", area.this);
	room.this.Description = [[This huge room is noisy and filled with numerous tables, patrons, and above all else, 
	noise.  Dozens of people of all stripes, creeds, and origin are enjoying themselves at the tables while a host of 
	servers attempt to deliver food and drink to out-stretched hands and thirsty mouths.  A stairwell to the west 
	leads up to the second floor of the Inn.]];
	room.this:AddExit(101, "north", "A path amongst the tables leads north");
	room.this:AddExit(107, "east", "A path amongst the tables leads east");
	room.this:AddExit(114, "up", "A stairwell to the west leads up");

	-- Alania
		-- silken robe, fine slippers
	reset.this = AddMobileToRoom(room.this, 105, 1);

	SetupBarmaid(room.this, 103, 1);

	room.this = CreateRoom(104, "Hallway", "inside", area.this);
	room.this:AddExit(108, "west", "An open doorway leads into a kitchen to the west");
	room.this:AddExit(110, "east", "The hallway continues to the east");
	AddExitToRoom(room.this, "south", 111, "A wooden door is to the north", "isdoor closed locked");
	
	room.this = CreateRoom(105, "Common Room (Northeast)", "inside", area.this);
	room.this:AddExit(106, "south", "A path amongst the tables leads south");
	room.this:AddExit(102, "west", "A path amongst the tables leads west");
	-- Liase
		-- robe, fine slippers, symbol of melesa
	SetupBarmaid(room.this, 105, 1);

	room.this = CreateRoom(106, "Common Room (Southeast)", "inside", area.this);
	room.this:AddExit(105, "north", "A path amongst the tables leads north");
	room.this:AddExit(107, "west", "A path amongst the tables leads west");

	-- Spawn Rashan
		-- hauberk, linen pants, leather boots, cloak, longsowrd, and parchment
	reset.this = AddMobileToRoom(room.this, 103, 106, 1);

	SetupBarPatron(room.this, 106, 1);
	SetupWearingAdventurer(room.this, 106, 2);

	room.this = CreateRoom(107, "Common Room (South)", "inside", area.this);
	room.this:AddExit(106, "east", "A path amongst the tables leads east");
	room.this:AddExit(102, "north", "A path amongst the tables leads north");
	room.this:AddExit(103, "west", "A path amongst the tables leads west");
	
	-- Attan
		-- linen shirt, leather pants, leather boots, jeweled earrings, short sword
	-- Bouncer
		-- leather pants, leather boots, leather straps

	room.this = CreateRoom(108, "Kitchen", "inside", area.this);
	room.this:AddExit(109, "north", "The kitchen continues to the north");
	room.this:AddExit(104, "east", "An open doorway leads out of the kitchen to the east");
	-- Stone Oven
	-- Large Cauldron
	
	-- Spawn Chalon
		-- apron, wooden spoon
	reset.this = AddMobileToRoom(room.this, 101, 108, 1);

	room.this = CreateRoom(109, "Kitchen", "inside", area.this);
	room.this:AddExit(108, "south", "The kitchen continues to the south");
	-- Cupboard
	-- Barrel
	-- Crate
	
	-- Spawn Natania
		 -- wrinkled tunic, linen hose, and battered pot
	reset.this = AddMobileToRoom(room.this, 104, 109, 1);
	
	room.this = CreateRoom(110, "Hallway", "inside", area.this);
	room.this:AddExit(104, "west", "The hallway continues to the west");
	AddExitToRoom(room.this, "east", 113, "A wooden door is to the east", "isdoor closed locked");
	
	room.this = CreateRoom(111, "Bedroom", "inside", area.this);
	AddExitToRoom(room.this, "south", 104, "A wooden door is to the south", "isdoor closed locked");
	room.this:AddExit(112, "east", "The bedroom continues to the west");
	-- Table
	-- Wooden Chest
	
	room.this = CreateRoom(112, "Bedroom", "inside", area.this);
	room.this:AddExit(111, "west", "The bedroom continues to the east");
	-- Small Bed
	-- Small Dresser
	
	room.this = CreateRoom(113, "Storeroom", "inside", area.this);
	room.this:AddExit(125, "north", "Stairs to the north lead down into the basement");
	AddExitToRoom(room.this, "west", 110, "A wooden door is to the west", "isdoor closed locked");
end
	
function SecondFloorRooms()
	systemLog("=================== AREA 'INN OF THE SEVEN REALMS' - 2ND FLOOR ROOMS ====================");
	room.this = CreateRoom(114, "Hallway (2nd Floor)", "inside", area.this);
	room.this:AddExit(115, "east", "The hallway continues to the east");
	room.this:AddExit(103, "down", "A stairwell to the west leads down");
	
	room.this = CreateRoom(115, "Hallway (2nd Floor)", "inside", area.this);
	room.this:AddExit(114, "west", "The hallway continues to the west");
	room.this:AddExit(116, "east", "The hallway continues to the east");
	AddExitToRoom(room.this, "south", 119, "A wooden door is to the south", "isdoor closed");
	
	room.this = CreateRoom(116, "Hallway (2nd Floor)", "inside", area.this);
	room.this:AddExit(115, "west", "The hallway continues to the west");
	room.this:AddExit(117, "north", "The hallway continues to the north");
	AddExitToRoom(room.this, "south", 120, "A wooden door is to the south", "isdoor closed");
	
	room.this = CreateRoom(117, "Hallway (2nd Floor)", "inside", area.this);
	room.this:AddExit(116, "south", "The hallway continues to the south");
	room.this:AddExit(118, "north", "The hallway continues to the north");
	AddExitToRoom(room.this, "west", 121, "A wooden door is to the west", "isdoor closed");
	
	room.this = CreateRoom(118, "Hallway (2nd Floor)", "inside", area.this);
	room.this:AddExit(117, "south", "The hallway continues to the south");	
	AddExitToRoom(room.this, "west", 122, "A wooden door is to the west", "isdoor closed");
	AddExitToRoom(room.this, "east", 123, "A wooden door is to the east", "isdoor closed");
	-- Dazhor Ren
	-- Grizzled Mercenary
	
	room.this = CreateRoom(119, "Inn Room (2nd Floor)", "inside", area.this);
	AddExitToRoom(room.this, "north", 115, "A wooden door is to the north", "isdoor closed");
	-- Ismail Nikhet
	-- Small Bed
	-- Small Dresser
	
	room.this = CreateRoom(120, "Inn Room (2nd Floor)", "inside", area.this);
	AddExitToRoom(room.this, "south", 116, "A wooden door is to the north", "isdoor closed");
	-- Small Bed
	-- Small Dresser
	
	room.this = CreateRoom(121, "Inn Room (2nd Floor)", "inside", area.this);
	AddExitToRoom(room.this, "east", 117, "A wooden door is to the east", "isdoor closed");
	-- Small Bed
	-- Small Dresser
	
	room.this = CreateRoom(122, "Inn Room (2nd Floor)", "inside", area.this);
	AddExitToRoom(room.this, "east", 118, "A wooden door is to the east", "isdoor closed");
	-- Small Bed
	-- Small Dresser
	
	room.this = CreateRoom(123, "Inn Suite (2nd Floor)", "inside", area.this);
	AddExitToRoom(room.this, "west", 118, "A wooden door is to the west", "isdoor closed");
	room.this:AddExit(124, "south", "The suite continues to the south");
	-- Large Bed
	-- Large Dresser
	-- Metal-bound Chest
	
	-- Arina Tal'shon
	-- Elderly Manservant
	
	room.this = CreateRoom(124, "Inn Suite (2nd Floor)", "inside", area.this);
	room.this:AddExit(123, "north", "The suite continues to the north");
end
	
function BasementRooms()
	systemLog("=================== AREA 'INN OF THE SEVEN REALMS' - BASEMENT ROOMS ====================");
	room.this = CreateRoom(125, "Basement (South)", "inside", area.this);
	room.this.Description = [[Years ago this basement may have been used for storage, but now it is 
	home to piles of debris.  The shattered remnants of crates and barrels scattered about, while 
	piles of detritus cover the stone floor.]];
	room.this:AddExit(113, "south", "Stairs to the south lead up out of the basement.");
	room.this:AddExit(126, "west", "A path amongst the debris leads west");
	room.this:AddExit(130, "east", "A path amongst the debris leads east");
	room.this:AddExit(128, "north", "A path amongst the debris leads north");
	
	room.this = CreateRoom(126, "Basement(Southwest)", "inside", area.this);
	room.this.Description = [[Years ago this basement may have been used for storage, but now it is 
	home to piles of debris.  The shattered remnants of crates and barrels scattered about, while 
	piles of detritus cover the stone floor.]];
	room.this:AddExit(125, "east", "A path amongst the debris leads east");
	room.this:AddExit(127, "north", "A path amongst the debris leads north");
	AddMobileToRoom(room.this, 117, 126, 3);
	
	room.this = CreateRoom(127, "Basement(Northwest)", "inside", area.this);
	room.this.Description = [[Years ago this basement may have been used for storage, but now it is 
	home to piles of debris.  The shattered remnants of crates and barrels scattered about, while 
	piles of detritus cover the stone floor.]];
	room.this:AddExit(126, "south", "A path amongst the debris leads south");
	room.this:AddExit(128, "east", "A path amongst the debris leads east");
		
	room.this = CreateRoom(128, "Basement(North)", "inside", area.this);
	room.this.Description = [[Years ago this basement may have been used for storage, but now it is 
	home to piles of debris.  The shattered remnants of crates and barrels scattered about, while 
	piles of detritus cover the stone floor.  A pile of stones that appear to have come from 
	the north wall lay scattered about and a gaping hole leads into unknown darkness.]];
	room.this:AddExtraDescription("hole", [[This gaping hole in the wall appears to have been forced 
	from the other side beyond the basement.  What or whom could have done this you do not know!]]);
	room.this:AddExit(127, "west", "A path amongst the debris leads west");
	room.this:AddExit(125, "south", "A path amongst the debris leads south");
	room.this:AddExit(129, "east", "A path amongst the debris leads east");
	-- TODO Add exit here to Catacombs
	AddMobileToRoom(room.this, 117, 128, 2);
	
	room.this = CreateRoom(129, "Basement(Northeast)", "inside", area.this);
	room.this.Description = [[Years ago this basement may have been used for storage, but now it is 
	home to piles of debris.  The shattered remnants of crates and barrels scattered about, while 
	piles of detritus cover the stone floor.]];
	room.this:AddExit(128, "west", "A path amongst the debris leads west");
	room.this:AddExit(130, "south", "A path amongst the debris leads south");
	AddMobileToRoom(room.this, 117, 129, 3);
		
	room.this = CreateRoom(130, "Basement(Southeast)", "inside", area.this);
	room.this.Description = [[Years ago this basement may have been used for storage, but now it is 
	home to piles of debris.  The shattered remnants of crates and barrels scattered about, while 
	piles of detritus cover the stone floor.]];
	room.this:AddExit(129, "north", "A path amongst the debris leads north");
	room.this:AddExit(125, "west", "A path amongst the debris leads west");
	AddMobileToRoom(room.this, 117, 130, 2);
end

function SetupBarPatron(room, location, quantity)
	newReset = LCreateReset("mob", 0, 107, location, quantity);
	reset.this = newReset;
	room:AddReset(reset.this);
	
	--reset.this:AddReset("equip", 0, object, location, 0);
	-- Woolen Shirt
	-- Woolen Pants
	-- Leather Boots
	-- Dagger (Sheathed)
end

function SetupWearyAdventurer(room, location, quantity)
	newReset = LCreateReset("mob", 0, 108, location, quantity);
	reset.this = newReset;
	room:AddReset(reset.this);
	
	--reset.this:AddReset("equip", 0, object, location, 0);
	-- Woolen Shirt
	-- Woolen Pants
	-- Leather Boots
	-- Dagger (Sheathed)
end

function SetupBarmaid(room, location, quantity)
	newReset = LCreateReset("mob", 0, 109, location, quantity);
	reset.this = newReset;
	room:AddReset(reset.this);
	
	--reset.this:AddReset("equip", 0, object, location, 0);
	-- Woolen Dress
	-- Slippers
	-- Empty Tankard
end

-- EOF 