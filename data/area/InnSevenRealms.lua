-- INNSEVENREALMS.LUA
-- This is the zone-file for the Inn of the Seven Realms
-- Revised: 2014.08.07
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_area.lua")();

function LoadArea()
	LBootLog("=================== AREA 'INN OF THE SEVEN REALMS' INITIALIZING ===================");
	newArea = LCreateArea(2, "Inn of the Seven Realms");
	area.this = newArea;
	area.this.Author = "AmonGwareth";
	area.this.HighSoftRange = 60;
	area.this.HighHardRange = 60;
	area.this.HighEconomy = 45009000;
	area.this.ResetFrequency = 60;
	area.this:SetFlags("nopkill");
	area.this.ResetMessage = "A shiver runs up your spine...";
	
	FirstFloorMobs();
	SecondFloorMobs();
	BasementMobs();
	Objects();
	FirstFloorRooms();
	SecondFloorRooms();
	BasementRooms();
	LBootLog("=================== AREA 'INN OF THE SEVEN REALMS' - COMPLETED ================");
end
	
function FirstFloorMobs()
	LBootLog("=================== AREA 'INN OF THE SEVEN REALMS' - 1ST FLOOR MOBS ===================");
	mobile = CreateMobile(10000, "bron barkeep", "Bron Ma'Ganor, Barkeep");
	mobile.LongDescription = "A tall, but extremely fat man is here.";
	mobile.Description = [[This tall, but extremely fat man of indeterminate age 
	stands behind the bar of this establishment.  His dark features mark him to be of 
	southern descent.  He has dark hair and a bushy, dark beard which have been 
	recently trimmed and his laugh echoes throughout the room.]];
	mobile.ActFlags = "Immortal NoAttack";
	
	-- Create a shop for Bron
	shop = CreateShop(10030, 90, 7, 21);
	
	mobile:AddShop(shop);
	
	-- MUDPROG
	-- Wipe down the bar
	-- Stare wistfully up at the crest on the wall
	
	mobile = CreateMobile(10001, "chalan cook", "Chalan Ma'Ganor, Cook");
	mobile.LongDescription = "A large woman with dark, brown hair and a kind face is here.";
	mobile.Description = [[Wife of Bron, the proprietor of the Inn, Chalan does 
	most of the cooking and runs the household staff.   She is a large woman with 
	dark brown hair and a sharp tongue, but a kind face.]];
	mobile.Gender = "female";
	-- MUDPROG
	-- Stir the cauldron
	-- Check the oven
	-- Mutter about the "lazy barmaids"
	-- Mutter about "that blasted Bard"
	
	mobile = CreateMobile(10002, "rashan leftenant soldier", "Leftenant Rashan Aseph");
	mobile.LongDescription = "An exhausted soldier of medium height is here.";
	mobile.Description = [[A thick man of medium height with a dark, trimmed 
	beard and mustache.  He has a look about him as if he is exhausted and is near 
	his breaking point.  His eyes are haunted, as if he has seen more than one ever 
	should.  But, there is also a sense of strength, of force of will, about him and he 
	carries himself with assurance and steadfastness.]];
	mobile.Attacks = "trip kick";
	mobile.Defenses = "dodge parry";

	mobile = CreateMobile(10003, "liase healer", "Liase Al'verran");
	mobile.LongDescription = "A young enthusiastic cleric is here.";
	mobile.Description = [[description goes here]];
	mobile.Class = "cleric";
	mobile.Gender = "female";
	mobile.Defenses = "heal";

	mobile = CreateMobile(10004, "natania child", "Natania Ma'Ganor");
	mobile.LongDescription = "A small child with greasy, dark hair is here.";
	mobile.Description = [[A small child of fewer than ten years, Natania has greasy, 
	dark hair and smudges on her face and hands from helping her mother in the kitchen.]];
	mobile.Gender = "female";
	mobile.ActFlags = "npc sentinel wimpy";

	mobile = CreateMobile(10005, "alania elf", "Alania Telkhat, Healer");
	mobile.LongDescription = "An aged elf is here.";
	mobile.Description = [[An elf, of the Caorlei bloodline, who appears to have many 
	years behind her, Alania Telkhat has worked as a healer and counselor for many years. 
	She is wearing plain, but comfortable clothes and has a look of unease on her face as 
	she surveys the crowds of patrons in the Inn.]];
	mobile.Race = "elf";
	mobile.Class = "cleric";
	mobile.Gender = "female";
	mobile.Speaks = "common elven";
	mobile.Defenses = "heal";

	mobile = CreateMobile(10006, "attan sailor", "Captain Attan Al'sha'if");
	mobile.LongDescription = "A sailor with trinkets in his braided hair is here.";
	mobile.Description = [[A short, thin man with braided hair, a curled mustache, and the 
	look of someone who is ill at ease on land, Attan Al'sha'if is the Captain of a ship in the Dusharan 
	Harbor.  He has trinkets hanging from his braids, a broad hat in his dirty hands, and is looking 
	around at the patrons of the Inn with a mixture of humor and disdain.]];
	mobile.Attacks = "gouge kick";
	mobile.Defenses = "parry";

	mobile = CreateMobile(10007, "patron", "Bar Patron");
	mobile.LongDescription = "A bar patron is here.";
	mobile.Description = [[A nondescript person is common clothes is drinking a mug of ale.]]
	mobile.ActFlags = "npc stayarea";

	mobile = CreateMobile(10008, "adventurer weary", "Weary Adventurer");
	mobile.LongDescription = "A weary adventurer is here."
	mobile.Description = [[A nondescript person who is looking for adventure, but is wearily eating 
	a bowl of stew and drinking from a tankard of ale.]];
	mobile.ActFlags = "npc stayarea";
	-- MUD PROG
	-- Randomly say something about the War or the Undead or some facet of the Realms

	mobile = CreateMobile(10009, "barmaid server", "Barmaid");
	mobile.LongDescription = "A barmaid is here."; 
	mobile.Description = [[A nondescript woman who deftly weaves through the patrons and tables to 
	serve steaming bowls of food and tankards of ale.]];
	mobile.Gender = "female";
	mobile.ActFlags = "npc stayarea wimpy";

	mobile = CreateMobile(10010, "bouncer", "Burly Bouncer");
	mobile.LongDescription = "A very large man with massive arms is here.";
	mobile.Description = [[A very large man with arms as thick as tree trunks.  He has a very sour look 
	on his face and appears to be someone you don’t wish to mess with.]];
	mobile.ActFlags = "npc stayarea";
	mobile.Attacks = "punch";
	mobile:AddMudProg(CreateMudProg("rand_prog", "25", 
	[[
		LMobEmote(" crosses his arms and glares at everyone around him.");
	]]));
	mobile:AddMudProg(CreateMudProg("rand_prog", "25", 
	[[
		LMobEmote(" snorts and glares at everyone around him.");
	]]));

	mobile = CreateMobile(10011, "storyteller bard", "Lively Bard");
	mobile.LongDescription = "A lively and enthusiastic storyteller is here.";
	mobile.Description = [[An older man with long hair tied into braids and a long, curled and waxed 
	mustache that he continually twists, the Bard plays and sings with equal talent. He songs tell stories 
	of ancient days, of great heroes and damsels in distress, while his music is both haunting and 
	lively in turns and as the crowd demands.  Despite the music and song he manages to flash a smile 
	at every passing barmaid and frequently takes a draught from the mug sitting near his chair.]];
	mobile.Class = "Thief";
	mobile.Attacks = "backstab";
	mobile.Defenses = "dodge";
	mobile:AddMudProg(CreateMudProg("rand_prog", "15", 
	[[
		LMobEmote(", sings 'Fa la la la la, la la la la'");
	]]));
	mobile:AddMudProg(CreateMudProg("rand_prog", "15", 
	[[
		LMobEmote(", sings 'Do Re Mi Fa So La Ti Do'");
	]]));
	mobile:AddMudProg(CreateMudProg("rand_prog", "15", 
	[[
		LMobEmote(", sings 'Mi Mi Mi Mi'");
		LMobCommand("cough");
		LMobEmote(", sings 'La La La La'");
	]]));
	
	-- Wink or pinch barmaids as they pass by
	
	mobile = CreateMobile(10018, "cat tabby", "Tabby Cat");
	mobile.LongDescription = "A tabby cat is purring here.";
	mobile.Description = [[This small tabby cat has orange fur, white whiskers, and a demanding attitude. 
	She looks at you with obvious disdain.]];
	mobile.Race = "cat";
	mobile.Gender = "female";
	mobile.ActFlags = "wimpy";
	mobile.Speaks = "mammal";
	mobile.Speaking = "mammal";
	mobile.BodyParts = "head legs heart brains guts feet eye tail claws";
	mobile.Attacks = "claws bite";
	mobile.ActFlags = "npc stayarea wimpy";
	mobile.AffectedBy = "infrared detect_invis";
	mobile:AddMudProg(CreateMudProg("rand_prog", "10", 
	[[ 
		LMobEmote(" flicks her tail.");
	]]));
	mobile:AddMudProg(CreateMudProg("rand_prog", "10", 
	[[ 
		LMobEmote(" purrs contentedly.");
	]]));
	mobile:AddMudProg(CreateMudProg("rand_prog", "10", 
	[[ 
		LMobEmote(" begins to clean her paw.");
	]]));
	mobile:AddMudProg(CreateMudProg("rand_prog", "10", 
	[[ 
		LMobEmote(" paws at the ground looking for a good spot to sleep.");
	]]));

	-- Random on entry, rubs against player's leg
end

function SecondFloorMobs()
	LBootLog("=================== AREA 'INN OF THE SEVEN REALMS' - 2ND FLOOR MOBS ===================");
	mobile = CreateMobile(10012, "arina noble", "Arina Tal'shon");
	mobile.LongDescription = "";
	mobile.Description = [[]];
	mobile.Gender = "female";

	mobile = CreateMobile(10013, "ismail servant", "Ismail Nikhet");
	mobile.LongDescription = "";
	mobile.Description = [[]];
	mobile.Race = "elf";
	mobile.Speaks = "common elven";

	mobile = CreateMobile(10014, "dazhor dwarf mercenary", "Dazhor Ren");
	mobile.LongDescription = "";
	mobile.Description = [[]];
	mobile.Gender = "male";
	mobile.Race = "dwarf";
	mobile.Speaks = "common dwarven";
	mobile.Attacks = "kick bash";
	mobile.Defenses = "parry";

	mobile = CreateMobile(10015, "manservant old elder", "Elderly Manservant");
	mobile.LongDescription = "";
	mobile.Description = [[]];
	mobile.Gender = "male";

	mobile = CreateMobile(10016, "mercenary veteran", "Grizzled Mercenary");
	mobile.LongDescription = "";
	mobile.Description = [[]];
	mobile.Gender = "male";
	mobile.Attacks = "kick trip";
	mobile.Defenses = "parry";
end

function BasementMobs()
	LBootLog("=================== AREA 'INN OF THE SEVEN REALMS' - BASEMENT MOBS ===================");
	mobile = CreateMobile(10017, "giant rat", "Giant Rat");
	mobile.LongDescription = "A massive rat with red eyes glares at you.";
	mobile.Description = [[This rat is a monstrosity!  Its easily larger than your average dog and 
	has reddish eyes that glare back at you.  Its tail is long, pink and flicks about nervously or 
	angrily, you cannot tell.  Bits and scraps of food hang from its mouth and are stuck in its fur.]];
	mobile.Race = "rat";
	mobile.Speaks = "rodent";
	mobile.Speaking = "rodent";
	mobile.BodyParts = "head legs heart brains guts feet eye tail claws";
	mobile.Attacks = "bite";
	mobile.ActFlags = "npc stayarea aggressive";
	mobile.AffectedBy = "infrared";
end

function Objects()
	LBootLog("=================== AREA 'INN OF THE SEVEN REALMS' - OBJECTS ===================");
	object = CreateObject(10000, "stained apron", "armor");
	object.ShortDescription = "an apron with unidentifiable stains on it";
	object.LongDescription = "You see an apron with numerous, unidentifiable stains on it here.";
	object.WearFlags = "take body";
	object:SetStats(5, 100, 0, 0, 0);

	object = CreateObject(10001, "wooden spoon")
	object.ShortDescription = "a spoon made of wood";
	object.LongDescription = "You see a spoon made of wood here.";
	object:SetStats(1, 5, 0, 0, 0);

	object = CreateObject(10002, "studded leather hauberk", "armor");
	object.ShortDescription = "a hauberk made of studded leather";
	object.LongDescription = "You see a hauberk made of studded leather here.";
	object.WearFlags = "take body";
	object:SetStats(40, 1200, 0, 0, 0);

	object = CreateObject(10003, "linen pants", "armor");
	object.ShortDescription = "a pair of linen pants";
	object.LongDescription = "You see a pair of linen pants here.";
	object.WearFlags = "take legs";
	object:SetStats(10, 250, 0, 0, 0);

	object = CreateObject(10004, "heavy leather boots", "armor");
	object.ShortDescription = "a pair of heavy leather boots";
	object.LongDescription = "You see a pair of heavy boots made of leather here.";
	object.WearFlags = "take feet";
	object:SetStats(20, 400, 0, 0, 0);

	object = CreateObject(10005, "fine blue cloak", "armor");
	object.ShortDescription = "a blue cloak made of fine material";
	object.LongDescription = "You see a blue cloak made of the finest material here.";
	object.WearFlags = "take back";
	object:SetStats(10, 1500, 0, 0, 0);

	object = CreateObject(10006, "longsword", "weapon");
	object.ShortDescription = "a long metal sword";
	object.LongDescription = "You see a long metallic sword here.";
	object.WearFlags = "take wield";
	object:SetValues(10, 1, 6, 3, 0, 0);
	object:SetStats(15, 1600, 0, 0, 0);

	object = CreateObject(10007, "parchment roll", "paper");
	object.ShortDescription = "a roll of parchment";
	object.LongDescription = "You see a rolled piece of parchment here.";
	object:SetStats(1, 40, 0, 0, 0);

	object = CreateObject(10008, "long silk robe", "armor");
	object.ShortDescription = "a long robe made of silk";
	object.LongDescription = "You see a long robe made of the finest silk here.";
	object.WearFlags = "take body";
	object:SetStats(10, 2250, 0, 0, 0);

	object = CreateObject(10009, "fine slippers", "armor");
	object.ShortDescription = "a pair of fine slippers";
	object.LongDescription = "You see a pair of finely made slippers here.";
	object.WearFlags = "take feet";
	object:SetStats(5, 750, 0, 0, 0);

	object = CreateObject(10010, "symbol of melesa", "weapon");
	object.ShortDescription = "A Symbol of Melesa";
	object.LongDescription = "You see a heavy gold-encrusted Symbol to the goddess Melesa here.";
	object.WearFlags = "take wield";
	object:SetValues(12, 2, 8, 7, 0, 0);
	object:SetStats(40, 4000, 0, 0, 0);
	-- TODO Should the Symbol have some sort of affect or weaponspell?

	object = CreateObject(10011, "wrinkled tunic", "armor");
	object.ShortDescription = "a wrinkled tunic";
	object.LongDescription = "You see a soiled and wrinkled tunic here.";
	object.WearFlags = "take body";
	object:SetStats(10, 75, 0, 0, 0);

	object = CreateObject(10012, "linen hose", "armor");
	object.ShortDescription = "some linen hose";
	object.LongDescription = "You see some linen hose here.";
	object.WearFlags = "take legs";
	object:SetStats(10, 250, 0, 0, 0);
	
	object = CreateObject(10013, "battered pot", "drinkcontainer");
	object.ShortDescription = "a battered pot";
	object.LongDescription = "You see a worn and battered metal pot here.";
	object:SetValues(15, 15, 0, 0, 0, 0);
	object:SetStats(25, 25, 0, 0, 0);
	
	object = CreateObject(10014, "linen shirt", "armor");
	object.ShortDescription = "a linen shirt";
	object.LongDescription = "You see a shirt made of linen here.";
	object.WearFlags = "take body";
	object:SetStats(10, 250, 0, 0, 0);
	
	object = CreateObject(10015, "leather pants", "armor");
	object.ShortDescription = "a pair of leather pants";
	object.LongDescription = "You see a pair of leather pants here.";
	object.WearFlags = "take legs";
	object:SetStats(20, 400, 0, 0, 0);
	
	-- 10016 Jeweled Earrings
	-- 10017 Short Sword
	-- 10018 Woolen Shirt
	-- 10019 Woolen Pants
	-- 10020 Leather Boots
	-- 10021 Dagger
	-- 10022 Woolen Dress
	-- 10023 Slippers
	-- 10024 Empty Tankard
	-- 10025 Leather Straps
	-- 10026 Mug of Ale
	-- 10027 Glass of Wine
	-- 10028 Day-old Bread
	-- 10029 Bowl of Stew
	-- 10030 Hunk of Hard Cheese

	-- 10031 Huge Cask
	-- 10032 Tall Candelabra
	-- 10033 Table
	-- 10034 Stone Oven
	-- 10035 Large Cauldron
	-- 10036 Cupboard
	-- 10037 Barrel
	-- 10038 Crate
	-- 10039 Small Bed
	-- 10040 Small Dresser
	-- 10041 Large Bed
	-- 10042 Large Dresser
	-- 10043 Metal-bound Chest
	-- 10044 Wooden Chest
	-- 10045 Small Metallic Key
	-- 10046 Broken Crate
	-- 10047 Table
	-- 10048 Fireplace
	-- 10049 Debris
	
	-- 10050 Boiled Leather Jerkin
	-- 10051 Double-Bladed Axe
	-- 10052 Greatsword
	-- 10053 Leather Shoes
end

function FirstFloorRooms()
	LBootLog("=================== AREA 'INN OF THE SEVEN REALMS' - 1ST FLOOR ROOMS ====================");
	room = LCreateRoom(10000, "A wood-paneled room", "inside", area.this);
	room.Description = [[This wood-paneled room is devoid of any furniture or decorations on the walls, 
	save for a large hearth.  A fire cackles merrily within the pit, spreading a welcome warmth to the room, 
	and causing shadows on the other walls to dance wildly.]];
	room:AddExtraDescription("hearth fireplace", [[A large, stone fireplace stands within the south wall 
	of the room.  It appears to be made of river stone, worn by years of water and later by the hands of many 
	visitors.  No decorations adorn the mantle atop the hearth.]]);
	AddExitToRoom(room, "east", 10001, "A wooden door is to the east", "isdoor closed");
	AddObjectToRoom(room, 10048, 10000, 1);	-- Fireplace
	AddDoorReset(room, "north", "closed");	-- Close the Door
	
	room = LCreateRoom(10001, "Common Room (Northwest)", "inside", area.this);
	room.Description = [[This huge room is noisy and filled with numerous tables, patrons, and above all 
	else, noise.  Dozens of people of all stripes, creeds, and origin are enjoying themselves at the tables while 
	a host of servers attempt to deliver food and drink to out-stretched hands and thirsty mouths.  A large bar 
	dominates one side of the room, tables and booths fill much of the rest.  To the north an arched doorway 
	leads into the back of the Inn, likely to the kitchens and storerooms.]];
	AddExitToRoom(room, "east", 10002, "A path amongst the tables leads east");
	AddExitToRoom(room, "south", 10003, "A path amongst the tables leads south");
	AddExitToRoom(room, "north", 10004, "An arched doorway leads out of the Common Room");
	AddExitToRoom(room, "west", 10000, "A wooden door leads west out of the room");
	AddObjectToRoom(room, 10033, 10001, 1);	-- Table
	AddObjectToRoom(room, 10032, 10001, 1);	-- Candelabra
	SetupBarPatron(room, 10001, 2);			-- Bar Patron	
	SetupWearyAdventurer(room, 10001, 1);	-- Weary Adventurer
	AddDoorReset(room, "west", "closed");	-- Close the Door
	
	room = LCreateRoom(10002, "Common Room (North)", "inside", area.this);
	room.Description = [[This huge room is noisy and filled with numerous tables, patrons, and above all else, 
	noise.  Dozens of people of all stripes, creeds, and origin are enjoying themselves at the tables while a host of 
	servers attempt to deliver food and drink to out-stretched hands and thirsty mouths.  A large bar dominates this 
	side of the room, tables and booths fill much of the rest.  Above the bar hangs a massive shield flanked by two 
	large greatswords.]];
	room:AddExtraDescription("shield", [[This is a massive, square body shield with a center of wood and 
	a rim of metal. It appears functional and not just decorative and has a crest you cannot identify on its surface.]]);
	room:AddExtraDescription("crest", [[A large crest of a tree, flanked by two knights standing rigid with pikes,  
	A banner scrolls beneath the tree with the phrase: Honor and Duty.]]);
	AddExitToRoom(room, "west", 100001, "A path amongst the tables leads west");
	AddExitToRoom(room, "east", 100005, "A path amongst the tables leads east");
	AddExitToRoom(room, "south", 100007, "A path amongst the tables leads south");
	AddObjectToRoom(room, 10031, 10002, 1);	-- Large Cask
	AddObjectToRoom(room, 10032, 10002, 1);	-- Candelabra
	SetupBarmaid(room, 10002, 1);			-- Barmaid
	
	-- Bron Ma'Ganor
	reset = AddMobileToRoom(room, 10000, 10002, 1);
	EquipOnMobile(reset, 10000, 3);
	EquipOnMobile(reset, 10045, 0);

	-- Bron's Items for Sale
	GiveToMobile(reset, 10026, 99);			-- Mug of Ale
	GiveToMobile(reset, 10027, 99);			-- Glass of Wine
	GiveToMobile(reset, 10028, 99);			-- Day-old Bread
	GiveToMobile(reset, 10029, 99);			-- Bowl of Stew
	GiveToMobile(reset, 10030, 99);			-- Hunk of Hard Cheese

	room = LCreateRoom(10003, "Common Room (Southwest)", "inside", area.this);
	room.Description = [[This huge room is noisy and filled with numerous tables, patrons, and above all else, 
	noise.  Dozens of people of all stripes, creeds, and origin are enjoying themselves at the tables while a host of 
	servers attempt to deliver food and drink to out-stretched hands and thirsty mouths.  A stairwell to the west 
	leads up to the second floor of the Inn.]];
	AddExitToRoom(room, "north", 100001, "A path amongst the tables leads north");
	AddExitToRoom(room, "east", 100007, "A path amongst the tables leads east");
	AddExitToRoom(room, "up", 10014, "A stairwell to the west leads up");
	AddObjectToRoom(room, 10033, 10003, 1);	-- Table
	AddObjectToRoom(room, 10032, 10003, 1);	-- Candelabra
	SetupBarmaid(room, 10003, 1);			-- Barmaid
	
	-- Alania Telkhat
	reset = AddMobileToRoom(room, 10005, 10003, 1);
	EquipOnMobile(reset, 10008, 3);			-- Long Silken Robe
	EquipOnMobile(reset, 10009, 6);			-- Fine Slippers

	room = LCreateRoom(10004, "Hallway", "inside", area.this);
	AddExitToRoom(room, "west", 10008, "An open doorway leads into a kitchen to the west");
	AddExitToRoom(room, "east", 10010, "The hallway continues to the east");
	AddExitToRoom(room, "south", 10011, "A wooden door is to the north", "isdoor closed");
	AddDoorReset(room, "north", "closed");	-- Close the Door
	
	room = LCreateRoom(10005, "Common Room (Northeast)", "inside", area.this);
	AddExitToRoom(room, "south", 10006, "A path amongst the tables leads south");
	AddExitToRoom(room, "west", 10002, "A path amongst the tables leads west");
	AddObjectToRoom(room, 10033, 10005, 2);	-- Table
	AddObjectToRoom(room, 10032, 10005, 1);	-- Candelabra
	SetupBarmaid(room, 10005, 1);			-- Barmaid
	
	-- Liase Al'verran
	reset = AddMobileToRoom(room, 10003, 10005, 1);
	EquipOnMobile(reset, 10008, 3);			-- Long Silken Robe
	EquipOnMobile(reset, 10009, 6);			-- Fine Slippers
	EquipOnMobile(reset, 1010, 14);			-- Symbol of Melesa

	room = LCreateRoom(10006, "Common Room (Southeast)", "inside", area.this);
	AddExitToRoom(room, "north", 10005, "A path amongst the tables leads north");
	AddExitToRoom(room, "west", 10007, "A path amongst the tables leads west");
	AddObjectToRoom(room, 10033, 10006, 2);	-- Table
	AddObjectToRoom(room, 10032, 10006, 1);	-- Candelabra
	SetupBarPatron(room, 10006, 1);			-- Bar Patron
	SetupWearyAdventurer(room, 10006, 2);	-- Weary Adventurer
	
	-- Rashan Aseph
	reset = AddMobileToRoom(room, 10003, 10006, 1);
	EquipOnMobile(reset, 10002, 3);			-- Battered Mail Hauberk
	EquipOnMobile(reset, 10003, 5);			-- Linen Pants
	EquipOnMobile(reset, 10004, 6);			-- Leather Boots
	EquipOnMobile(reset, 10005, 19);			-- Fine Blue Cloak
	EquipOnMobile(reset, 10006, 13);			-- Longsword (TODO Sheathed)
	EquipOnMobile(reset, 10007, 14);			-- Parchment
	
	room = LCreateRoom(10007, "Common Room (South)", "inside", area.this);
	AddExitToRoom(room, "east", 10006, "A path amongst the tables leads east");
	AddExitToRoom(room, "north", 10002, "A path amongst the tables leads north");
	AddExitToRoom(room, "west", 10003, "A path amongst the tables leads west");
	AddObjectToRoom(room, 10033, 10007, 1);	-- Table
	AddObjectToRoom(room, 10032, 10007, 1);	-- Candelabra
	
	-- Attan Al'sha'if
	reset = AddMobileToRoom(room, 10006, 10007, 1);
	EquipOnMobile(reset, 10014, 3);			-- Linen Shirt
	EquipOnMobile(reset, 10015, 5);			-- Leather Pants
	EquipOnMobile(reset, 10020, 6);			-- Leather Boots
	EquipOnMobile(reset, 10016, 16);			-- Jeweled Earrings
	EquipOnMobile(reset, 10017, 13);			-- Short Sword (TODO Sheathed)
		
	-- Burly Bouncer
	reset = AddMobileToRoom(room, 10010, 10007, 2);
	EquipOnMobile(reset, 10015, 5);			-- Leather Pants
	EquipOnMobile(reset, 10020, 6);			-- Leather Boots
	EquipOnMobile(reset, 10025, 3);			-- Leather Straps

	room = LCreateRoom(10008, "Kitchen", "inside", area.this);
	AddExitToRoom(room, "north", 10009, "The kitchen continues to the north");
	AddExitToRoom(room, "east", 10004, "An open doorway leads out of the kitchen to the east");
	AddObjectToRoom(room, 10034, 10008, 1);	-- Stone Oven
	AddObjectToRoom(room, 10035, 10008, 1);	-- Cauldron
	AddObjectToRoom(room, 10032, 10008, 1);	-- Candelabra
	
	-- Chalon Ma'Ganor
	reset = AddMobileToRoom(room, 10001, 10008, 1);
	EquipOnMobile(reset, 10000, 3);			-- Stained Apron
	EquipOnMobile(reset, 10001, 14);			-- Wooden Spoon
	
	room = LCreateRoom(10009, "Kitchen", "inside", area.this);
	AddExitToRoom(room, "south", 10008, "The kitchen continues to the south");
	AddObjectToRoom(room, 10036, 10009, 1);	-- Cupboard
	AddObjectToRoom(room, 10037, 10009, 2);	-- Barrel
	AddObjectToRoom(room, 10038, 10009, 2);	-- Crate
	
	-- Natania Ma'Ganor
	reset = AddMobileToRoom(room, 10004, 10009, 1);
	EquipOnMobile(reset, 10011, 3);			-- Wrinkled Tunic
	EquipOnMobile(reset, 10012, 5);			-- Linen Hose
	EquipOnMobile(reset, 10013, 14);			-- Battered Pot
	
	room = LCreateRoom(10010, "Hallway", "inside", area.this);
	AddExitToRoom(room, "west", 10004, "The hallway continues to the west");
	exit.this = AddExitToRoom(room, "east", 10013, "A wooden door is to the east", "isdoor closed locked");
	exit.this.Key = 10045;
	AddDoorReset(room, "east", "closed");	-- Close the Door
	
	room = LCreateRoom(10011, "Bedroom", "inside", area.this);
	AddExitToRoom(room, "south", 10004, "A wooden door is to the south", "isdoor closed");
	AddExitToRoom(room, "east", 10012, "The bedroom continues to the west");
	AddObjectToRoom(room, 10047, 10011, 1);	-- Table
	AddObjectToRoom(room, 10044, 10011, 1);	-- Wooden Chest
	AddObjectToRoom(room, 10032, 10011, 1);	-- Candelabra
	AddDoorReset(room, "west", "closed");	-- Close the Door
	
	room = LCreateRoom(10012, "Bedroom", "inside", area.this);
	AddExitToRoom(room, "west", 10011, "The bedroom continues to the east");
	AddObjectToRoom(room, 10039, 10012, 1);	-- Small Bed
	AddObjectToRoom(room, 10040, 10012, 1);	-- Small Dresser
	AddMobileToRoom(room, 10018, 10012, 1);	-- Cat
	
	room = LCreateRoom(10013, "Storeroom", "inside", area.this);
	AddExitToRoom(room, "north", 10025, "Stairs to the north lead down into the basement");
	exit.this = AddExitToRoom(room, "west", 110, "A wooden door is to the west", "isdoor closed locked");
	exit.this.Key = 10045;
	
	AddObjectToRoom(room, 10037, 10013, 4);	-- Barrel
	AddObjectToRoom(room, 10038, 10013, 2);	-- Crate
	AddObjectToRoom(room, 10031, 10013, 1);	-- Large Cask
	AddDoorReset(room, "west", "closed");	-- Close the Door
end
	
function SecondFloorRooms()
	LBootLog("=================== AREA 'INN OF THE SEVEN REALMS' - 2ND FLOOR ROOMS ====================");
	room = LCreateRoom(10014, "Hallway (2nd Floor)", "inside", area.this);
	AddExitToRoom(room, "east", 10015, "The hallway continues to the east");
	AddExitToRoom(room, "down", 10003, "A stairwell to the west leads down");
	
	room = LCreateRoom(10015, "Hallway (2nd Floor)", "inside", area.this);
	AddExitToRoom(room, "west", 10014, "The hallway continues to the west");
	AddExitToRoom(room, "east", 10016, "The hallway continues to the east");
	AddExitToRoom(room, "south", 10019, "A wooden door is to the south", "isdoor closed");
	AddDoorReset(room, "south", "closed");	-- Close the Door
	
	room = LCreateRoom(10016, "Hallway (2nd Floor)", "inside", area.this);
	AddExitToRoom(room, "west", 10015, "The hallway continues to the west");
	AddExitToRoom(room, "north", 10017, "The hallway continues to the north");
	AddExitToRoom(room, "south", 10020, "A wooden door is to the south", "isdoor closed");
	AddDoorReset(room, "south", "closed");	-- Close the Door
	
	room = LCreateRoom(10017, "Hallway (2nd Floor)", "inside", area.this);
	AddExitToRoom(room, "south", 10016, "The hallway continues to the south");
	AddExitToRoom(room, "north", 10018, "The hallway continues to the north");
	AddExitToRoom(room, "west", 10021, "A wooden door is to the west", "isdoor closed");
	AddDoorReset(room, "west", "closed");	-- Close the Door
	
	room = LCreateRoom(10018, "Hallway (2nd Floor)", "inside", area.this);
	AddExitToRoom(room, "south", 10017, "The hallway continues to the south");	
	AddExitToRoom(room, "west", 10022, "A wooden door is to the west", "isdoor closed");
	AddExitToRoom(room, "east", 10023, "A wooden door is to the east", "isdoor closed");
	AddObjectToRoom(room, 10047, 10018, 1);	-- Table
	AddObjectToRoom(room, 10032, 10018, 1);	-- Candelabra
	AddDoorReset(room, "west", "closed");	-- Close the Door
	AddDoorReset(room, "east", "closed");	-- Close the Door
	-- Dazhor Ren
		-- Leather Pants
		-- Leather Jerkin
		-- Heavy Leather Boots
		-- Double-Bladed Axe
	-- Grizzled Mercenary
		-- Leather Pants
		-- Leather Jerkin
		-- Heavy Leather Boots
		-- Greatsword
	
	room = LCreateRoom(10019, "Inn Room (2nd Floor)", "inside", area.this);
	AddExitToRoom(room, "north", 10015, "A wooden door is to the north", "isdoor closed");
	AddObjectToRoom(room, 10039, 10019, 1);	-- Small Bed
	AddObjectToRoom(room, 10040, 10019, 1);	-- Small Dresser
	AddDoorReset(room, "north", "closed");	-- Close the Door
	-- Ismail Nikhet
		-- Linen Pants
		-- Linen Shirt
		-- Leather Shoes
	
	room = LCreateRoom(10020, "Inn Room (2nd Floor)", "inside", area.this);
	AddExitToRoom(room, "south", 10016, "A wooden door is to the north", "isdoor closed");
	AddObjectToRoom(room, 10039, 10020, 1);	-- Small Bed
	AddObjectToRoom(room, 10040, 10020, 1);	-- Small Dresser
	AddDoorReset(room, "north", "closed");	-- Close the Door
	
	room = LCreateRoom(10021, "Inn Room (2nd Floor)", "inside", area.this);
	AddExitToRoom(room, "east", 10017, "A wooden door is to the east", "isdoor closed");
	AddObjectToRoom(room, 10039, 10021, 1);	-- Small Bed
	AddObjectToRoom(room, 10040, 10021, 1);	-- Small Dresser
	AddDoorReset(room, "east", "closed");	-- Close the Door
	
	room = LCreateRoom(10022, "Inn Room (2nd Floor)", "inside", area.this);
	AddExitToRoom(room, "east", 10018, "A wooden door is to the east", "isdoor closed");
	AddObjectToRoom(room, 10039, 10022, 1);	-- Small Bed
	AddObjectToRoom(room, 10040, 10022, 1);	-- Small Dresser
	AddDoorReset(room, "east", "closed");	-- Close the Door
	
	room = LCreateRoom(10023, "Inn Suite (2nd Floor)", "inside", area.this);
	AddExitToRoom(room, "west", 10018, "A wooden door is to the west", "isdoor closed");
	AddExitToRoom(room, "south", 10024, "The suite continues to the south");
	AddObjectToRoom(room, 10047, 10023, 1);	-- Table
	AddObjectToRoom(room, 10032, 10023, 1);	-- Candelabra
	AddDoorReset(room, "west", "closed");	-- Close the Door
	-- Elderly Manservant
		-- Woolen Pants
		-- Woolen Shirt
		-- Leather Shoes
		
	room = LCreateRoom(10024, "Inn Suite (2nd Floor)", "inside", area.this);
	AddExitToRoom(room, "north", 10023, "The suite continues to the north");
	AddObjectToRoom(room, 10041, 10024, 1);	-- Large Bed
	AddObjectToRoom(room, 10042, 10024, 1);	-- Large Dresser
	AddObjectToRoom(room, 10043, 10024, 1);	-- Metal-bound Chest
	-- Arina Tal'shon
		-- Silk Robe
		-- Slippers
end
	
function BasementRooms()
	LBootLog("=================== AREA 'INN OF THE SEVEN REALMS' - BASEMENT ROOMS ====================");
	room = LCreateRoom(10025, "Basement (South)", "inside", area.this);
	room.Description = [[Years ago this basement may have been used for storage, but now it is 
	home to piles of debris.  The shattered remnants of crates and barrels scattered about, while 
	piles of detritus cover the stone floor.]];
	AddExitToRoom(room, "south", 10013, "Stairs to the south lead up out of the basement.");
	AddExitToRoom(room, "west", 10026, "A path amongst the debris leads west");
	AddExitToRoom(room, "east", 10030, "A path amongst the debris leads east");
	AddExitToRoom(room, "north", 10028, "A path amongst the debris leads north");
	AddObjectToRoom(room, 10049, 10025, 2);	-- Debris
	
	room = LCreateRoom(10026, "Basement(Southwest)", "inside", area.this);
	room.Description = [[Years ago this basement may have been used for storage, but now it is 
	home to piles of debris.  The shattered remnants of crates and barrels scattered about, while 
	piles of detritus cover the stone floor.]];
	AddExitToRoom(room, "east", 10025, "A path amongst the debris leads east");
	AddExitToRoom(room, "north", 10027, "A path amongst the debris leads north");
	AddMobileToRoom(room, 10017, 10026, 3);	-- Giant Rats
	AddObjectToRoom(room, 10046, 10026, 2);	-- Broken Crate
	AddObjectToRoom(room, 10049, 10026, 1);	-- Debris
	
	room = LCreateRoom(10027, "Basement(Northwest)", "inside", area.this);
	room.Description = [[Years ago this basement may have been used for storage, but now it is 
	home to piles of debris.  The shattered remnants of crates and barrels scattered about, while 
	piles of detritus cover the stone floor.]];
	AddExitToRoom(room, "south", 10026, "A path amongst the debris leads south");
	AddExitToRoom(room, "east", 10028, "A path amongst the debris leads east");
	AddObjectToRoom(room, 10046, 10027, 1);	-- Broken Crate
	AddObjectToRoom(room, 10049, 10027, 2);	-- Debris
	
	room = LCreateRoom(10028, "Basement(North)", "inside", area.this);
	room.Description = [[Years ago this basement may have been used for storage, but now it is 
	home to piles of debris.  The shattered remnants of crates and barrels scattered about, while 
	piles of detritus cover the stone floor.  A pile of stones that appear to have come from 
	the north wall lay scattered about and a gaping hole leads into unknown darkness.]];
	room:AddExtraDescription("hole", [[This gaping hole in the wall appears to have been forced 
	from the other side beyond the basement.  What or whom could have done this you do not know!]]);
	AddExitToRoom(room, "west", 10027, "A path amongst the debris leads west");
	AddExitToRoom(room, "south", 10025, "A path amongst the debris leads south");
	AddExitToRoom(room, "east", 10029, "A path amongst the debris leads east");
	-- TODO Add exit here to Catacombs
	AddMobileToRoom(room, 10017, 10028, 2);	-- Giant Rats
	AddObjectToRoom(room, 10049, 10026, 2);	-- Debris
	
	room = LCreateRoom(10029, "Basement(Northeast)", "inside", area.this);
	room.Description = [[Years ago this basement may have been used for storage, but now it is 
	home to piles of debris.  The shattered remnants of crates and barrels scattered about, while 
	piles of detritus cover the stone floor.]];
	AddExitToRoom(room, "west", 10028, "A path amongst the debris leads west");
	AddExitToRoom(room, "south", 10030, "A path amongst the debris leads south");
	AddMobileToRoom(room, 10017, 10029, 3);	-- Giant Rats
	AddObjectToRoom(room, 10046, 10029, 2);	-- Broken Crate
	AddObjectToRoom(room, 10049, 10029, 1);	-- Debris
	
	room = LCreateRoom(10030, "Basement(Southeast)", "inside", area.this);
	room.Description = [[Years ago this basement may have been used for storage, but now it is 
	home to piles of debris.  The shattered remnants of crates and barrels scattered about, while 
	piles of detritus cover the stone floor.]];
	AddExitToRoom(room, "north", 10029, "A path amongst the debris leads north");
	AddExitToRoom(room, "west", 10025, "A path amongst the debris leads west");
	AddMobileToRoom(room, 10017, 10030, 2);	-- Giant Rats
	AddObjectToRoom(room, 10046, 10027, 1);	-- Broken Crate
	AddObjectToRoom(room, 10049, 10027, 2);	-- Debris
end

function SetupBarPatron(room, location, quantity)
	reset = AddMobileToRoom(room, 10007, location, quantity);
	EquipOnMobile(reset, 10018, 3);			-- Woolen Shirt
	EquipOnMobile(reset, 10019, 5);			-- Woolen Pants
	EquipOnMobile(reset, 10020, 6);			-- Leather Boots
end

function SetupWearyAdventurer(room, location, quantity)
	reset = AddMobileToRoom(room, 10008, location, quantity);
	EquipOnMobile(reset, 10018, 3);			-- Woolen Shirt
	EquipOnMobile(reset, 10019, 5);			-- Woolen Pants
	EquipOnMobile(reset, 10020, 6);			-- Leather Boots
	EquipOnMobile(reset, 10021, 13);			-- Dagger (TODO Sheathed)
end

function SetupBarmaid(room, location, quantity)
	reset = AddMobileToRoom(room, 10009, location, quantity);
	EquipOnMobile(reset, 10022, 3);			-- Woolen Dress
	EquipOnMobile(reset, 10023, 6);			-- Slippers
	EquipOnMobile(reset, 10024, 14);			-- Empty Tankard
end

LoadArea()	-- EXECUTE THE AREA

-- EOF 