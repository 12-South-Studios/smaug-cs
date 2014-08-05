-- HERBS.LUA
-- This is the Herbs data file for the MUD
-- Revised: 2013.11.21
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadHerbs()
	herb = CreateHerb("pipeweed");
	herb.Rounds = 0;
	herb.DamageMessage = "smoke";

	herb = CreateHerb("black gwyvel");
	herb.Rounds = 12;
	herb.DamageMessage = "smoke";
	herb:SetTargetByValue(1);
	herb.MinimumPosition = 7;
	herb.Slot = 1;
	herb.HitVictimMessage = "You start to cough and choke!";
	herb.SpellFunctionName = "spell_smaug";
	herb:AddAffect(CreateSmaugAffect("", 13, "-10", 0));

	herb = CreateHerb("vermeir");
	herb.Rounds = 4;
	herb.HitVictimMessage = "You feel a sparkling warmth flow through your limbs.";
	herb.HitRoomMessage = "$N leans back, relishing the flavor of the vermeir.";
	herb:SetTargetByValue(3);
	herb.MinimumPosition = 5;
	herb.Slot = 2;
	herb.SpellFunctionName = "spell_smaug";
	herb:AddAffect(CreateSmaugAffect("", 14, '20', 0));

	herb = CreateHerb("nooracht");
	herb.Rounds = 4;
	herb.WearOffMessage = "Your feet slowly lower to the ground.";
	herb.HitVictimMessage = "The acrid taste of the smoke overwhelms your senses.  You feel light-headed.";
	herb.HitRoomMessage = "$n grimaces at the acrid flavor of the nooracht.";
	herb:SetTargetByValue(3);
	herb.MinimumPosition = 5;
	herb.Slot = 3;
	herb.SpellFunctionName = "spell_smaug";
	herb:AddAffect(CreateSmaugAffect("", 12, "20", 0));
	herb:AddAffect(CreateSmaugAffect("30", 0, "", 2097152));
	
	herb = CreateHerb("oocadaal");
	herb.HitVictimMessage = "The sharp flavor of the smoke sends a flow of warmth through your body.";
	herb.HitRoomMessage = "$N's eyes water as the smoke flows around $m.";
	herb:SetTargetByValue(3);
	herb.Slot = 4;
	herb.SpellFunctionName = "spell_smaug";
	herb:AddAffect(CreateSmaugAffect("", 13, "20", 0));
	
	herb = CreateHerb("breadl");
	herb.MinimumPosition = 5;
	herb.Slot = 5;
	herb:SetTargetByValue(3);
	herb.WearOffMessage = "Everything about you takes on it's normal drab tone.";
	herb.HitVictimMessage = "Life takes on a brighter, crisper outlook as you savor the breadl's flavor.";
	herb.HitRoomMessage = "$N smiles as $s enjoys the mellow flavor of the breadl.";
	herb.SpellFunctionName = "spell_smaug";
	herb:AddAffect(CreateSmaugAffect("10", 0, "", 4194304));
end

ObjectID = 0;
function CreateHerb(name)
    ObjectID = ObjectID + 1;

	newHerb = LCreateHerb(ObjectID, name, "herb");
	herb.this = newHerb;
	return herb.this;
end

function CreateSmaugAffect(duration, location, modifier, flags)
	newAffect = LCreateSmaugAffect(duration, location, modifier, flags);
	affect.this = newAffect;
	return affect.this;
end

LoadHerbs();

-- EOF