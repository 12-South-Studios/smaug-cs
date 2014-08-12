-- MODULE_AREA.LUA
-- This is the Area Module for the MUD
-- Revised: 2013.11.18
-- Author: Jason Murdick

function AddMobileToRoom(roomRef, mob, room, limit)
	newReset = LCreateReset("mob", 0, mob, room, limit);
	reset.this = newReset;
	roomRef:AddReset(reset.this);
	return reset.this;
end

function AddObjectToRoom(roomRef, object, room, limit)
	newReset = LCreateReset("obj", 0, object, room, limit);
	reset.this = newReset;
	roomRef:AddReset(reset.this);
	return reset.this;
end

function EquipOnMobile(parentReset, object, location)
	parentReset:AddReset("equip", 0, object, location, 0);
end

function GiveToMobile(parentReset, object, quantity)
	parentReset:AddReset("give", 0, object, quantity, 0);
end

function AddExitToRoom(room, direction, location, description, flags)
	newExit = LCreateExit(direction, location, description);
	exit.this = newExit;
	exit.this.Flags = flags;
	room:AddExitObject(exit.this);
	return exit.this;
end

function AddDoorReset(room, direction, state)
	newReset = LCreateReset("door", 0, DirectionToNumber(direction), DoorStateToNumber(state), 0);
	reset.this = newReset;
	room:AddReset(reset.this);
	return reset.this;
end

function CreateMobile(vnum, keywords, name)
	newMob = LCreateMobile(vnum, keywords);
	mobile.this = newMob;
	mobile.this.ShortDescription = name;
	mobile.this.Speaks = "common";
	mobile.this.Speaking = "common";
	mobile.this.BodyParts = "head arms legs heart guts hands feet ear eye";
	mobile.this.ActFlags = "npc sentinel";
	mobile.this.Position = "standing";
	mobile.this.DefensivePosition = "standing";
	return mobile.this;
end

function CreateShop(buyRate, sellRate, openHour, closeHour)
	newShop = LCreateShop(buyRate, sellRate, openHour, closeHour);
	shop.this = newShop;
	return shop.this;
end

function CreateObject(vnum, name, objectTypes)
	newObject = LCreateObject(vnum, name);	
	object.this = newObject;
	
	if (objectTypes == nil) then
		object.this.Type = "none";
	else 
		object.this.Type = objectTypes;	
	end
	
	object.this.WearFlags = "take";
	return object.this;
end

function CreateRoom(vnum, name, sector, area)
	newRoom = LCreateRoom(vnum, name);
	room.this = newRoom;
	area:AddRoom(room.this);
	room.this:SetSector(sector);
	return room.this;
end

function CreateMudProg(progType, progArgList, progScript)
	newProg = LCreateMudProg(progType);
	mprog.this = newProg;
	mprog.this.ArgList = progArgList;
	mprog.this.Script = progScript;
	return mprog.this;
end

function DirectionToNumber(direction)
	local directions = {}
	directions["north"] = 0;
	directions["n"] = 0;
	directions["east"] = 1;
	directions["e"] = 1;
	directions["south"] = 2;
	directions["s"] = 2;
	directions["west"] = 3;
	directions["w"] = 3;
	directions["up"] = 4;
	directions["u"] = 4;
	directions["down"] = 5;
	directions["d"] = 5;
	directions["northeast"] = 6;
	directions["ne"] = 6;
	directions["northwest"] = 7;
	directions["nw"] = 7;
	directions["southeast"] = 8;
	directions["se"] = 8;
	directions["southwest"] = 9;
	directions["sw"] = 9;
	directions["somewhere"] = 10;
	
	local val;
    if (directions[direction] == nil) then
		val = 10;
	else 
		val = directions[direction];
	end
	
	return val;
end

function DoorStateToNumber(state)
	local val = 0;
	
	if (state == "closed") then
		val = 1;
	end
	
	return val;
end

-- EOF
