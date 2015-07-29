-- ARAN/ROOMS.LUA
-- This is the rooms-file for the Aran Zone
-- Revised: 2015.07.29
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_area.lua")();

LBootLog("=================== AREA 'ARAN' - ROOMS ====================");
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
AddExitToRoom(room, "west", 107, "An open doorway leads west into the Inn", "isdoor");
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
	
room = CreateRoom(10120, "Desara Avenue", "city", area.this);
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

-- EOF