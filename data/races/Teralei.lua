-- TERALEI.LUA
-- This is the Teralei Race file for the MUD
-- Revised: 2014.03.11
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadRace()
	newRace = LCreateRace("Teralei", 5);
	race.this = newRace;
	race.this.ClassRestriction = 848;
	race.this.StrengthBonus = -1;
	race.this.DexterityBonus = 2;
	race.this.IntelligenceBonus = 1;
	race.this.ConstitutionBonus = -1;
	race.this.CharismaBonus = 1;
	race.this.LuckBonus = 1;
	race.this.Health = -2;
	race.this.Mana = 10;
	race.this.Resistance = 2;
	race.this.Language = 2;
	race.this.MinimumAlignment = -1000;
	race.this.MaximumAlignment = 1000;
	race.this.ExperienceMultiplier = 112;
	race.this.Height = 63;
	race.this.Weight = 110;
	race.this.ThirstMod = -3;
	
	LoadWhereNames(race.this);
end

function LoadWhereNames(race)
	race:AddWhereName("<used as light>     ");
	race:AddWhereName("<worn on finger>    ");
	race:AddWhereName("<worn on finger>    ");
	race:AddWhereName("<worn around neck>  ");
	race:AddwhereName("<worn around neck>  ");
	race:AddWhereName("<worn on body>      ");
	race:AddwhereName("<worn on head>      ");
	race:AddWhereName("<worn on legs>      ");
	race:AddWhereName("<worn on feet>      ");
	race:AddWhereName("<worn on hands>     ");
	race:AddWhereName("<worn on arms>      ");
	race:AddWhereName("<worn as shield>    ");
	race:AddWhereName("<worn about body>   ");
	race:AddWhereName("<worn about waist>  ");
	race:AddWhereName("<worn around wrist> ");
	race:AddWhereName("<worn around wrist> ");
	race:AddWhereName("<wielded>           ");
	race:AddWhereName("<held>              ");
	race:AddWhereName("<dual wielded>      ");
	race:AddWhereName("<worn on ears>      ");
	race:AddWhereName("<worn on eyes>      ");
	race:AddWhereName("<missile wielded>   ");
	race:AddWhereName("<worn on back>      ");
	race:AddWhereName("<worn over face>    ");
	race:AddWhereName("<worn around ankle> ");
	race:AddWhereName("<worn around ankle> ");
end

LoadRace();

-- EOF