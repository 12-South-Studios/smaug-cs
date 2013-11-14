-- ARAN.LUA
-- This is the zone-file for the city of Aran
-- Revised: 2013.11.13
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(GetAppSetting("dataPath") .. "\\modules\\module_base.lua")();

systemLog("=================== AREA 'ARAN' INITIALIZING ===================");
newArea = LCreateArea(1, "Aran of the Southern Gate");
area.this = newArea;
area.this.Author = "AmonGwareth";
area.this.HighSoftRange = 60;
area.this.HighHardRange = 60;
area.this.HighEconomy = 45009000;
area.this.ResetMessage = "A warm breeze blows in from across the bay...";

systemLog("=================== AREA 'ARAN' - MOBILES ===================");
newMob = LCreateMobile(100, "bron barkeep");
mobile.this = newMob;
mobile.this.ShortDescription = "Bron Ma'Ganor, Barkeep";
mobile.this.LongDescription = "A tall, but extremely fat man is here.";
mobile.this.Description = [[This tall, but extremely fat man of indeterminate age 
stands behind the bar of this establishment.  His dark features mark him to be of 
southern descent.  He has dark hair and a bushy, dark beard which have been 
recently trimmed and his laugh echoes throughout the room.]];

newMob = LCreateMob(101, "chalan cook");
mobile.this = newMob;
mobile.this.ShortDescription = "Chalan Ma'Ganor, Cook";
mobile.this.LongDescription = "A large woman with dark, brown hair and a kind face is here.";
mobile.this.Description = [[Wife of Bron, the proprietor of the Inn, Chalan does 
most of the cooking and runs the household staff.   She is a large woman with 
dark brown hair and a sharp tongue, but a kind face.]];
mobile.this.Gender = "female";

newMob = LCreateMob(102, "rashan leftenant soldier");
mobile.this = newMob;
mobile.this.ShortDescription = "Leftenant Rashan Aseph";
mobile.this.LongDescription = "An exhausted soldier of medium height is here.";
mobile.this.Description = [[A thick man of medium height with a dark, trimmed 
beard and mustache.  He has a look about him as if he is exhausted and is near 
his breaking point.  His eyes are haunted, as if he has seen more than one ever 
should.  But, there is also a sense of strength, of force of will, about him and he 
carries himself with assurance and steadfastness.]];

newMob = LCreateMob(103, "liase healer");
mobile.this = newMob;
mobile.this.ShortDescription = "Liase Al'verran");
mobile.this.LongDescription = "A young enthusiastic cleric is here.";
mobile.this.Description = [[]];
mobile.this.Class = "cleric";
mobile.this.Gender = "female";

newMob = LCreateMob(104, "natania child");
mobile.this = newMob;
mobile.this.ShortDescription = "Natania Ma'Ganor";
mobile.this.LongDescription = "A small child with greasy, dark hair is here.";
mobile.this.Description = [[A small child of fewer than ten years, Natania has greasy, 
dark hair and smudges on her face and hands from helping her mother in the kitchen.]];
mobile.this.Gender = "female";

newMob = LCreateMob(105, "alania elf");
mobile.this = newMob;
mobile.this.ShortDescription = "Alania Telkhat, Healer");
mobile.this.LongDescription = "An aged elf is here.";
mobile.this.Description = [[An elf, of the Caorlei bloodline, who appears to have many 
years behind her, Alania Telkhat has worked as a healer and counselor for many years. 
She is wearing plain, but comfortable clothes and has a look of unease on her face as 
she surveys the crowds of patrons in the Inn.]];
mobile.this.Race = "elf";
mobile.this.Class = "cleric";
mobile.this.Gender = "female";

newMob = LCreateMob(106, "attan sailor");
mobile.this = newMob;
mobile.this.ShortDescription = "Captain Attan Al'sha'if");
mobile.this.LongDescription = "A sailor with trinkets in his braided hair is here.";
mobile.this.Description = [[A short, thin man with braided hair, a curled mustache, and the 
look of someone who is ill at ease on land, Attan Al'sha'if is the Captain of a ship in the Dusharan 
Harbor.  He has trinkets hanging from his braids, a broad hat in his dirty hands, and is looking 
around at the patrons of the Inn with a mixture of humor and disdain.]];

newMob = LCreateMob(107, "patron");
mobile.this = newMob;
mobile.this.ShortDescription = "Bar Patron");
mobile.this.LongDescription = "A bar patron is here.";
mobile.this.Description = [[A nondescript person is common clothes is drinking a mug of ale.]]

newMob = LCreateMob(108, "adventurer weary");
mobile.this = newMob;
mobile.this.ShortDescription = "Weary Adventurer");
mobile.this.LongDescription = "A weary adventurer is here."
mobile.this.Description = [[A nondescript person who is looking for adventure, but is wearily eating 
a bowl of stew and drinking from a tankard of ale.]];

newMob = LCreateMob(109, "barmaid server");
mobile.this = newMob;
mobile.this.ShortDescription = "Barmaid");
mobile.this.LongDescription = "A barmaid is here."; 
mobile.this.Description = [[A nondescript woman who deftly weaves through the patrons and tables to 
serve steaming bowls of food and tankards of ale.]];
mobile.this.Gender = "female";

newMob = LCreateMob(110, "bouncer");
mobile.this = newMob;
mobile.this.ShortDescription = "Burly Bouncer");
mobile.this.LongDescription = "A very large man with massive arms is here.";
mobile.this.Description = [[A very large man with arms as thick as tree trunks.  He has a very sour look 
on his face and appears to be someone you don’t wish to mess with.]];

newMob = LCreateMob(111, "storyteller bard");
mobile.this = newMob;
mobile.this.ShortDescription = "Lively Bard";
mobile.this.LongDescription = "A lively and enthusiastic storyteller is here.";
mobile.this.Description = [[]];
mobile.this.Class = "Thief";

-- 2nd Floor
-- Arina Tal’shon
-- Ismail Nikhet
-- Dazhor Ren
-- Elderly Manservant
-- Grizzled Mercenary

-- Basement
-- Giant Rat




systemLog("=================== AREA 'ARAN' - OBJECTS ===================");
-- Stained Apron
-- Wooden Spoon
-- Studded Leather Hauberk
-- Linen Pants
-- Heavy Leather Boots
-- Fine Blue Cloak
-- Longsword
-- Parchment Roll
-- Long Silk Robe
-- Fine Slippers
-- Symbol of Melesa
-- Wrinkled Tunic
-- Linen Hose
-- Battered Pot
-- Linen Shirt
-- Leather Pants
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


systemLog("=================== AREA 'ARAN' - ROOMS ====================");
newRoom = LCreateRoom(100, "A wood-paneled room");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");
room.this.Description = [[This wood-paneled room is devoid of any furniture or decorations on the walls, save for a large hearth.  A fire cackles merrily 
within the pit, spreading a welcome warmth to the room, and causing shadows on the other walls to dance wildly.]];
room.this:AddExtraDescription("hearth fireplace", [[A large, stone fireplace stands within the south wall of the room.  It appears to be made of river stone, 
worn by years of water and later by the hands of many visitors.  No decorations adorn the mantle atop the hearth.]]);
newExit = LCreateExit("east", 101, "A wooden door leads east out of the room.");
exit.this = newExit;
exit.this.Flags = "isdoor closed";
room.this:AddExit(exit.this);

newRoom = LCreateRoom(101, "Common Room (Northwest)");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");
room.this.Description = [[This huge room is noisy and filled with numerous tables, patrons, and above all else, noise.  Dozens of people of all stripes, creeds, 
and origin are enjoying themselves at the tables while a host of servers attempt to deliver food and drink to out-stretched hands and thirsty mouths.  A large 
bar dominates one side of the room, tables and booths fill much of the rest.  To the north an arched doorway leads into the back of the Inn, likely to the 
kitchens and storerooms.]];
room.this:AddExit(102, "east", "A path amongst the tables leads east deeper into the Common Room");
room.this:AddExit(103, "south", "A path amongst the tables leads south through the Common Room");
room.this:AddExit(104, "north", "An arched doorway leads out of the Common Room.");
room.this:AddExit(100, "west", "A wooden door leads west out of the room.");

newRoom = LCreateRoom(102, "Common Room (North)");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");

newRoom = LCreateRoom(103, "Common Room (Southwest)");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");

newRoom = LCreateRoom(104, "Hallway");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");

newRoom = LCreateRoom(105, "Common Room (Northeast)");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");

newRoom = LCreateRoom(106, "Common Room (Southeast)");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");

newRoom = LCreateRoom(107, "Common Room (South)");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");

newRoom = LCreateRoom(108, "Kitchen");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");

newRoom = LCreateRoom(109, "Kitchen");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");

newRoom = LCreateRoom(110, "Hallway");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");

newRoom = LCreateRoom(111, "Bedroom");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");

newRoom = LCreateRoom(112, "Bedroom");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");

newRoom = LCreateRoom(113, "Storeroom");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");

newRoom = LCreateRoom(114, "Hallway (2nd Floor)");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");

newRoom = LCreateRoom(115, "Hallway (2nd Floor)");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");

newRoom = LCreateRoom(116, "Hallway (2nd Floor)");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");

newRoom = LCreateRoom(117, "Hallway (2nd Floor)");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");

newRoom = LCreateRoom(118, "Hallway (2nd Floor)");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");

newRoom = LCreateRoom(119, "Inn Room (2nd Floor)");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");

newRoom = LCreateRoom(120, "Inn Room (2nd Floor)");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");

newRoom = LCreateRoom(121, "Inn Room (2nd Floor)");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");

newRoom = LCreateRoom(122, "Inn Room (2nd Floor)");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");

newRoom = LCreateRoom(123, "Inn Suite (2nd Floor)");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");

newRoom = LCreateRoom(124, "Inn Suite (2nd Floor)");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");

newRoom = LCreateRoom(125, "Basement (South)");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");

newRoom = LCreateRoom(126, "Basement (Southwest)");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");

newRoom = LCreateRoom(127, "Basement (Northwest)");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");

newRoom = LCreateRoom(128, "Basement (North)");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");

newRoom = LCreateRoom(129, "Basement (Northeast)");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");

newRoom = LCreateRoom(130, "Basement (Southeast)");
room.this = newRoom;
area.this:AddRoom(room.this);
room.this:SetSector("inside");

systemLog("=================== AREA 'ARAN' - COMPLETED ================");
-- EOF 