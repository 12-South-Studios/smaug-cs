-- ARAN.LUA
-- This is the zone-file for the Aran
-- Revised: 101013.12.16
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_area.lua")();

LoadArea()	-- EXECUTE THE AREA

function LoadArea()
	systemLog("=================== AREA 'ARAN' INITIALIZING ===================");
	newArea = LCreateArea(3, "Aran");
	area.this = newArea;
	area.this.Author = "AmonGwareth";
	area.this.HighSoftRange = 60;
	area.this.HighHardRange = 60;
	area.this.HighEconomy = 45009000;
	area.this.ResetFrequency = 60;
	area.this:SetFlags("nopkill");
	area.this.ResetMessage = "A shiver runs up your spine...";
	
	Mobs();
	Objects();
	Rooms();
	
	systemLog("=================== AREA 'ARAN' - COMPLETED ================");
end
	
function Mobs()
	systemLog("=================== AREA 'ARAN' - MOBS ===================");
	mobile = CreateMobile(10101, "laeban tay shopkeeper armourer", "Laeban Tay the Armourer");
	mobile.LongDescription = "Laeban Tay, a burly Dwarven armourer, is working at the forge.";
	mobile.Description = [[A squat and burly Dwarf who looks as though he has seen a few rough 
	years is working hard at the forge. He has numerous scars on his arms, chest, and face which 
	give him a surly appearance. However, you can see by the way he carefully beats the metal 
	that he loves his work.]];
	mobile.Race = "dwarf";
	mobile:AddConversation("untol", [[Laeban spits and says, "The city of thieves! Watch  your 
	purse and your back if you travel in Untol. I do hear that the Brothel has some hidden secrets.]]);
	mobile:AddConversation("aviaes", [[Laeban laughs and says, "Hah! Given the past relations 
	between my people and the Elves you can imagine my reluctance to visit the town which they 
	inhabit except in the most dire of times.]]);
	mobile:AddConversation("saios", [[Laeban says, "A town as corrupt as it is rich. Watch your 
	purse and neck while in that town or you may find yourself lying in either a gutter or on the block!]]);
	mobile:AddConversation("itibonia", [[Laeban says, "I know almost nothing of the woman."]]);
	mobile:AddConversation("vaitya", [[Laeban says, "Considered by humans to be a goddess. Hah! 
	The only true immortals are those in the Annals of History and I have never heard of Vaitya in that."]]);
	mobile:AddConversation("againos", [[Laeban says, "I know little of Againos aside from what I have 
	seen here in Aran. I have never seen nor read of her in the Annals of History."]]);
	mobile:AddConversation("palurien", [[Laeban says, "Makh'dughor is a trusted friend of the Dwarves 
	and has been for as long as the Annals of History have been recorded. The Weaponsmaster, as we 
	Dwarves call him, taught my people the intricacies of strategy and tactics long ago."]]);
	mobile:AddConversation("fulos kazar", [[Laeban says, "The more ruthless of the brothers, Fulos has 
	killed several men from what I hear. You should watch your back while you are near him, for he might 
	just stab it!"]]);
	mobile:AddConversation("toc kazar", [[Laeban says, "Toc's father was a good friend, but his sons 
	are rotten to the core! I hear they hire their...goods...in the back room in a locked chest."]]);
	mobile:AddConversation("gisela batol", [[Laeban says, "A woman whom I have learned to steer 
	clear of. Gisela has approached me many a time and each time I have spurned her advances. 
	Why she would want an old, grizzled dwarf like me is beyond my imagining!"]]);
	
	mobile = CreateMobile(10102, "guard guardsman", "Dusharan Guardsman");
	mobile.LongDescription = "A Dusharan Guardsman stands here keeping the peace.";
	mobile.Description = [[THis guardsman has light skin, is tall, as most of his people are, and fair 
	haired.  His steel gray eyes watch the crowd intently for signs of trouble, though there seldom 
	is any.]];
	
	mobile = CreateMobile(10103, "captain guard guardsman", "Captain of the Guard");
	mobile.LongDescription = "The Captain of the Dusharan Guard is ordering his troops around.";
	mobile.Description = [[This tall, lean man looks as though he has seen many rough years. He looks 
	to be in very good shape, is clean shaven, and his armor and weapons look like they are in 
	excellent shape.  He is ordering his troops around with an air of confidence and authority.]];
	
	mobile = CreateMobile(10104, "nelar atinaea shopkeeper weaponsmith", "Nelar Atinae the Weaponsmith");
	mobile.LongDescription = "Nelar Atinae, a large and mean looking man, is here beating on a rod of steel.";
	mobile.Description = [[This tall, broad-shouldered man looks as though he could bite through the road 
	of metal on which he is pounding furiously. He is wearing pants which are covered in grime and a 
	smock which is also quite dirty. His face and hands are covered in soot from the forge fire, but his 
	eyes gleam in the firelight and his teeth are extremely white.]];
	
	mobile = CreateMobile(10105, "gisela batol shopkeeper", "Gisela Batol");
	mobile.LongDescription = "Gisela Batol, a plump woman watches you curiously from behind the counter.";
	mobile.Description = [[This fairly large, plump, but somewhat exotic woman is standing behind the 
	counter idly shuffling items while she attempts to hide the fact that she is watching you intently. 
	You sense a strange air surrounding her, something predatory.]];
	mobile.Gender = "female";
	
	mobile = CreateMobile(10106, "high priest palurien trainer", "High Priest of Palurien");
	mobile.LongDescription = "The High Priest of Palurien is here instructing some of the lesser priests.";
	mobile.Description = [[This tall, broad-shouldered man stands before you teaching several of the 
	lesser priests of Palurien the travails of the god and also the arts of warfare. He resembles nothing 
	like any of the other priests in the city, wearing studded leather armor and carrying a broadsword 
	at his side.  
	
	His face and arms are covered with scars. As you enter he turns towards you and you notice that 
	his dark hair is pulled back into a pony tail which reaches to mid-back.]];

	mobile = CreateMobile(10107, "priest palurien", "Priest of Palurien");
	mobile.LongDescription = "A Priest of Palurien stands before you.";
	mobile.Description = [[This priest stands before you wearing studded leather armor and carrying a 
	sword on his belt. His hair is pulled back and tied behind him.]];

	mobile = CreateMobile(10108, "healer priest palurien", "Healer of Palurien");
	mobile.LongDescription = "An elderly Priest of Palurien is kneeling at an altar.";
	mobile.Description = [[This elderly man is wearing robes, in great contrast to the other priests you 
	encountered in the temple, and he is kneeling in front of an ancient altar to Palurien. His white hair 
	is long and cascades down over his shoulders.]];

	mobile = CreateMobile(10109, "toc kazar shopkeeper", "Toc Kazar");
	mobile.LongDescription = "Toc Kazar, the elder of the Kazar brothers, stands here stocking a shelf.";
	mobile.Description = [[This man looks fairly well-travelled, but is otherwise rather unremarkable. He is 
	wearing fine clothing of wool and looks like he grooms himself several times a day. He is whistling as 
	he stocks some shelves with rare food items from Astabar.]];

	mobile = CreateMobile(10110, "fulos kazar", "Fulos Kazar");
	mobile.LongDescription = "Fulos Kazar, one of the Kazar brothers, stands here behind the counter.";
	mobile.Description = [[An unremarkable man of medium-build with brown hair and eyes and a deceptively 
	cheerful disposition. He is wearing expensive clothing which looks to have been made of silk or some 
	other fine fabric and is wearing quite a bit of jewelry.]];

	mobile = CreateMobile(10111, "priest againos", "Priest of Againos");
	mobile.LongDescription = "A Priest of Againos stands here worshipping the Goddess of Dreams.";
	mobile.Description = [[This priest is dressed in white robes, looks well-groomed, and smiles as you 
	enter. His face is open and full of compassion and peace.]];

	mobile = CreateMobile(10112, "high priest vaitya", "High Priest of Vaitya");
	mobile.LongDescription = "The High Priest of Vaitya is here preaching fire and brimstone.";
	mobile.Description = [[This middle-aged man is dressed in finely-woven clothes and looks extremely 
	well-groomed. He is waving his arms in the air and preaching loudly about the rewards for the faithful 
	and the punishments for sinners. There is a strange look of a fanatic about him.]];

	mobile = CreateMobile(10113, "itibonia healer", "Itibonia the Healer");
	mobile.LongDescription = "Itibonia, an elderly High Elf sits here at her table meditating.";
	mobile.Description = [[This ancient elf is dressed in a dress made of lace and has her hair pulled back 
	into a long briad which nearly reaches the floor. Her long, slender hands are free of wrinkles, as is 
	her still beautiful face. In fact, there is something other-wordly about her which you cannot place your 
	finger on.]];
	mobile.Race = "elf";
	mobile.Gender = "female";
	
	mobile = CreateMobile(10114, "beggar", "a beggar");
	mobile.LongDescription = "A beggar is shambling along moaning of his woes.";
	mobile.Description = [[This man is wearing filthy rags and is covered with mud and worse. It looks as 
	if he lost one of his arms and one or both of his eyes at some point in time.]];

	mobile = CreateMobile(10115, "worshipper vaitya", "Worshipper of Vaitya");
	mobile.LongDescription = "A man worshipping the goddess Vaitya kneels here.";
	mobile.Description = [[A well-dressed man is kneeling here worshipping the goddess Vaitya. His hands 
	are clasped in front of him, his head is bowed in reverence, and he is murmuring prayers.]];

	mobile = CreateMobile(10116, "worshipper vaitya", "Worshipper of Vaitya");
	mobile.LongDescription = "A woman worshipping the goddess Vaitya kneels here.";
	mobile.Description = [[A well-dressed woman is kneeling here worshipping the goddess Vaitya. Her 
	hands are clasped in front of her, her head is bowed in reverence, and she is murmuring prayers.]];
	mobile.Gender = "female";
	
	mobile = CreateMobile(10117, "traveller", "a traveller");
	mobile.LongDescription = "A traveller is here looking for a place to stay for the night.";
	mobile.Description = [[This mud-splattered traveller is walking the streets looking for a place to stay 
	for the night. Two bags are clasped tightly under one arm.]];

	mobile = CreateMobile(10118, "child", "a child");
	mobile.LongDescription = "A small child is here playing in the dirt.";
	mobile.Description = [[This small child is covered in dust, but is smiling and playing with some sticks 
	and toys in the dirt.]];

	mobile = CreateMobile(10119, "citizen", "a citizen");
	mobile.LongDescription = "A citizen of Dushara is here.";
	mobile.Description = [[A well-dressed and upstanding citizen of Dushara is here enjoying the day.]];
	
	mobile = CreateMobile(10120, "adventurer", "an adventurer");
	mobile.LongDescription = "An adventurer who has seen many strange things is here.";
	mobile.Description = [[This hardy and seasoned adventurer is wearing stained travelling clothes, a 
	worn backpack, and carrying a walking stick.]];

	mobile = CreateMobile(10121, "street urchin", "a street urchin");
	mobile.LongDescription = "A street urchin is lurking here.";
	mobile.Description = [[This young man is wearing ill-fitting clothes that are stained and patched.  
	His face and hands are dirty and his eyes dart about nervously.]];
	
	mobile = CreateMobile(10122, "drunken priest palurien", "Drunken Priest of Palurien");
	mobile.LongDescription = "A drunken priest of Palurien is here.";
	mobile.Description = [[This priest is wearing the armored garb of the other priests, though his are 
	very stained. His beard is unkempt, fresh and dried spittle runs from his slack mouth, and his 
	breath smells like a sewer. His skin is slightly pale-white, clammy to the touch, and covered in a 
	thin sheen of perspiration.  His eyes have a drunken and wild look to them.]];

	mobile = CreateMobile(10123, "bartender shopkeeper priest", "Bartender of Palurien");
	mobile.LongDescription = "A powerful-looking, but smiling bartender is here.";
	mobile.Description = [[This bartender is wearing the garb of the other priests of Palurien, but 
	seems to have a little more weight on him than the others. His thick fingers deftly handle 
	the mugs and bottles behind the bar as easily as if he were swinging a sword.]];
end

function Objects()

end

function Rooms()
	systemLog("=================== AREA 'ARAN' - ROOMS ====================");
	room = CreateRoom(10101, "Khamlos Avenue", "city", area.this);
	room.Description = [[You are standing in the middle of a broad and busy street which cuts through 
	the middle of the city known as Aran of the Southern Gate. This main thoroughfare thruogh the city leads 
	from the Southern Gate itself to the Inner Gate leading deeper into the metropolis known as Dushara.  
	
	Tall and imposing buildings loom on either side of this busy avenue. To the west rises the six story building 
	known as the Inn of the Seven Planes, which also serves as the Adventurer's Guild for the entire region.  
	On the eastern side of the avenue lies the Temple of Palurien, a large and impressive structure which is 
	perhaps the oldest structure in the city.]];
	room:AddExtraDescription("inn guild", [[An enormous structure which rises six stories over the street. 
	It has well over one hundred rooms, several large common rooms, a host of serving girls, and three thousand 
	stones worth of bouncers. Run by Bron Ma'Ganor, a tall but fat man of indeterminate age, the inn's rooms 
	are, for the most part, small and the prices aren't cheap, but they are clean, the stew is always warm, the 
	bread fresh, and the ale good.]]);
	room:AddExtraDescription("temple", [[The Temple of Palurien is thought to be the oldest or one of the 
	oldest structures in Aran of the Southern Gate. It was built during the second millennia by a forgotten 
	High King who was enamored by the many metals of the world and the arts of war. What he hoped to gain 
	beyond the favor of Palurien is not known, but for many centuries the temple was the focus of the city and 
	indeed of the region. The Temple is built of granite and is covered with ornamentations and statues, many 
	of which are composed of rare metals. Most of the valuable metals, however, were removed long ago.]]);
	AddExitToRoom(room, "west inn guild", 107, "An open doorway leads west into the Inn", "isdoor");
	AddExitToRoom(room, "north", 10102, "This broad avenue continues north, deeper into the city", "");
	AddExitToRoom(room, "east", 10136, "Broad marble steps climb up to huge stone doors which stand open and inviting.", "isdoor");
	AddExitToRoom(room, "south", 10105, "This broad avenue continues south towards the Southern Gate.", "");
	
	room = CreateRoom(10102, "Intersection of Khamlos Avenue and Desara Avenue", "city", area.this);
	-- south 10101
	-- north 10103
	-- west 10120
	-- east 10124
	
	room = CreateRoom(10103, "Khamlos Avenue", "city", area.this);
	-- north 10104
	-- south 10102
	-- west 10129
	-- east 10130
	
	room = CreateRoom(10104, "Intersection of Khamlos Avenue and Osara Avenue", "city", area.this);
	-- south 10103
	-- west 10127
	-- east 10126
	
	room = CreateRoom(10105, "Intersection of Khamlos Avenue and Finara Avenue", "city", area.this);
	-- north 10101
	-- south 10106
	-- west 10107
	-- east 10108
	
	room = CreateRoom(10106, "Inside the Southern Gate", "city", area.this);
	-- north 10105
	-- west 10111
	-- east 10112
	
	room = CreateRoom(10107, "Finara Avenue", "city", area.this);
	-- east 10105
	-- west 10109
	-- north 10115
	
	room = CreateRoom(10108, "Finara Avenue", "city", area.this);
	-- west 10105
	-- east 10110
	-- north 10116
	
	room = CreateRoom(10109, "Intersection of Finara Avenue and Borto Street", "city", area.this);
	-- east 10107
	-- southwest 10113
	-- north 10117
	
	room = CreateRoom(10110, "Intersection of Finara Avenue and Tiete Street", "city", area.this);
	-- west 10108
	-- southeast 10114
	
	room = CreateRoom(10111, "Western Gatehouse", "inside", area.this);
	-- east 10106
	
	room = CreateRoom(10112, "Eastern Gatehouse", "inside", area.this);
	-- west 10106
	
	room = CreateRoom(10113, "Bottom Floor of the Southwest Tower", "inside", area.this);
	-- northeast 10109
	-- up 10134
	
	room = CreateRoom(10114, "Bottom Floor of the Southeast Tower", "inside", area.this);
	-- northwest 10110
	-- up 10135
	
	room = CreateRoom(10115, "Entrance to the Temple of Vaitya", "inside", area.this);
	-- south, 10107
	
	room = CreateRoom(10116, "Grocery of the Southern Gate", "inside", area.this);
	-- south 10108
	
	room = CreateRoom(10117, "Borto Street", "city", area.this);
	-- south 10109
	-- north 10118
	
	room = CreateRoom(10118, "Intersection of Desara Avenue and Borto Street", "city", area.this);
	-- south 10117
	-- north 10119
	-- east 10120
	
	room = CreateRoom(10119, "Borto Street", "city", area.this);
	-- south 10118
	-- north 10128
	
	room = CreateRoom(10120 "Desara Avenue", "city", area.this);
	-- west 10118
	-- east 10102
	-- south 10133
	
	room = CreateRoom(10121, "Tiete Street", "city", area.this);
	-- south 10110
	-- north 10122
	-- west 10132
	
	room = CreateRoom(10122, "Intersection of Desara Avenue and Tiete Street", "city", area.this);
	-- north 10123
	-- south 21011
	-- west 10124
	
	room = CreateRoom(10123, "Tiete Street", "city", area.this);
	-- south 10122
	-- north 10125
	
	room = CreateRoom(10124, "Desara Avenue", "city", area.this);
	-- west 10102
	-- east 10122
	
	room = CreateRoom(10125, "Intersection of Osara Avenue and Tiete Street", "city", area.this);
	-- south 10123
	-- west 10126
	
	room = CreateRoom(10126, "Osara Avenue", "city", area.this);
	-- east 10125
	-- west 10104
	-- south 10131
	
	room = CreateRoom(10127, "Osara Avenue", "city", area.this);
	-- east 10104
	-- west 10128
	
	room = CreateRoom(10128, "Intersection of Osara Avenue and Borto Street", "city", area.this);
	-- east 10127
	-- south 10119
	
	room = CreateRoom(10129, "The Troll's Mane", "inside", area.this);
	-- east 10103
	
	room = CreateRoom(10130, "Atinae's Weapons", "inside", area.this);
	-- west 10103
	
	room = CreateRoom(10131, "Entrance to the Temple of Againos", "inside", area.this);
	-- north 10126
	-- south 10142
	
	room = CreateRoom(10132, "House of Healing", "inside", area.this);
	-- east 10121
	
	room = CreateRoom(10133, "Gisela's Generalities", "inside", area.this);
	-- north 10120
	-- up 10143
	
	room = CreateRoom(10134, "Top Floor of the Southwest Tower", "inside", area.this);
	-- down 10113
	
	room = CreateRoom(10135, "Top Floor of the Southeast Tower", "inside", area.this);
	-- down 10114
	
	room = CreateRoom(10136, "Entrance to the Temple of Palurien", "inside", area.this);
	-- west 10101
	-- east 10137
	
	room = CreateRoom(10137, "Narthex <Temple of Palurien>", "inside", area.this);
	-- west 10136
	-- north 10138
	-- east 10140
	-- south 10139
	
	room = CreateRoom(10138, "Barracks <Temple of Palurien", "inside", area.this);
	-- south 10137
	
	room = CreateRoom(10139, "The Dented Shield <Temple of Palurien>", "inside", area.this);
	-- north 10137
	
	room = CreateRoom(10140, "Practice Chamber <Temple of Palurien>", "inside", area.this);
	-- west 10137
	-- east 10141
	
	room = CreateRoom(10141, "Sanctuary of Palurien <Temple of Palurien>", "inside", area.this);
	-- west 10140
	
	room = CreateRoom(10142, "Sanctuary of Againos <Temple of Againos>", "inside", area.this);
	-- north 10131
	
	room = CreateRoom(10143, "Bedroom", "inside", area.this);
	-- down 10133
	
end

-- EOF 