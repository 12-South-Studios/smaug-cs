-- MODULE_AREA.LUA
-- This is the Area Module for the MUD
-- Revised: 2013.11.18
-- Author: Jason Murdick

function AddMobileToRoom(room, mob, room, limit)
	newReset = LCreateReset("mob", 0, mob, room, limit);
	reset.this = newReset;
	room:AddReset(reset.this);
	return reset.this;
end

function EquipOnMobile(masterReset, object, location)
	parentReset:AddReset("equip", 0, object, location, 0);
end

function AddExitToRoom(room, direction, location, description, flags)
	newExit = LCreateExit(direction, location, description);
	exit.this = newExit;
	exit.this.Flags = flags;
	room:AddExit(exit.this);
	return exit.this;
end

function CreateMobile(vnum, keywords, name)
	newMob = LCreateMobile(100, "bron barkeep");
	mobile.this = newMob;
	mobile.this.ShortDescription = "Bron Ma'Ganor, Barkeep";
	mobile.this.Speaks = "common";
	mobile.this.Speaking = "common";
	mobile.this.BodyParts = "head arms legs heart guts hands feet ear eye";
	mobile.this.ActFlags = "npc sentinel";
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
	
	if (objectTypes = nil) then
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

-- EOF
