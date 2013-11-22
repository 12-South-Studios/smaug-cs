-- HERBS.LUA
-- This is the Herbs data file for the MUD
-- Revised: 2013.11.21
-- Author: Jason Murdick
-- Version: 1.0

LoadHerbs()

function LoadHerbs()
	herb.this = CreateHerb("pipeweed");
	herb.this.Rounds = 0;
	herb.this.DamageMessage = "smoke";

	herb.this = CreateHerb("black gwyvel");
	herb.this.Rounds = 12;
	herb.this.DamageMessage = "smoke";
	herb.this:SetTarget(1);
	herb.this.MinimumPosition = 7;
	herb.this.Slot = 1;
	herb.this.HitVictimMessage = "You start to cough and choke!");
	LSetCode(herb.this, "spell_smaug");
	herb.this:AddAffect(CreateSmaugAffect("", 13, "-10", 0));

	herb.this = CreateHerb("vermeir");
	herb.this.Rounds = 4;
	herb.this.HitVictimMessage = "You feel a sparkling warmth flow through your limbs.";
	herb.this.HitRoomMessage = "$N leans back, relishing the flavor of the vermeir.";
	herb.this:SetTarget(3);
	herb.this.MinimumPosition = 5;
	herb.this.Slot = 2;
	LSetCode(herb.this, "spell_smaug");
	herb.this:AddAffect(CreateSmaugAffect("", 14, '20', 0));

	herb.this = CreateHerb("nooracht");
	herb.this.Rounds = 4;
	herb.this.WearOffMessage = "Your feet slowly lower to the ground.";
	herb.this.HitVictimMessage = "The acrid taste of the smoke overwhelms your senses.  You feel light-headed.";
	herb.this.HitRoomMessage = "$n grimaces at the acrid flavor of the nooracht.";
	herb.this:SetTarget(3);
	herb.this.MinimumPosition = 5;
	herb.this.Slot = 3;
	LSetCode(herb.this, "spell_smaug");
	herb.this:AddAffect(CreateSmaugAffect("", 12, "20", 0));
	herb.this:AddAffect(CreateSmaugAffect("30", 0, "", 2097152));
	
	herb.this = CreateHerb("oocadaal");
	herb.this.HitVictimMessage = "The sharp flavor of the smoke sends a flow of warmth through your body.";
	herb.this.HitRoomMessage = "$N's eyes water as the smoke flows around $m.";
	herb.this:SetTarget(3);
	herb.this.Slot = 4;
	LSetCode(herb.this, "spell_smaug");
	herb.this:AddAffect(CreateSmaugAffect("", 13, "20", 0));
	
	herb.this = CreateHerb("breadl");
	herb.this.MinimumPosition = 5;
	herb.this.Slot = 5;
	herb.this:SetTarget(3);
	herb.this.WearOffMessage = "Everything about you takes on it's normal drab tone.";
	herb.this.HitVictimMessage = "Life takes on a brighter, crisper outlook as you savor the breadl's flavor.";
	herb.this.HitRoomMessage = "$N smiles as $s enjoys the mellow flavor of the breadl.";
	LSetCode(herb.this, "spell_smaug");
	herb.this:AddAffect(CreateSmaugAffect("10", 0, "", 4194304));
end

function CreateHerb(name)
	newHerb = LCreateSkill(name, "herb");
	herb.this = newHerb;
	return herb.this;
end

function CreateSmaugAffect(duration, location, modifier, flags)
	newAffect = LCreateSmaugAffect(duration, location, modifier, flags);
	affect.this = newAffect;
	return affect.this;
end

-- EOF