-- TAEMIER.LUA
-- This is the Taemier Race file for the MUD
-- Revised: 2014.03.11
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadRace()
	newRace = LCreateRace("Taemier", 4);
	race.this = newRace;
	race.this.ClassRestriction = 769;
	race.this.StrengthBonus = 1;
	race.this.WisdomBonus = 1;
	race.this.ConstitutionBonus = 2;
	race.this.CharismaBonus = -1;
	race.this.Health = 6;
	race.this.Mana = -6;
	race.this:AddAffectedBy("infrared");
	race.this.Language = 4;
	race.this.MinimumAlignment = -1000;
	race.this.MaximumAlignment = 1000;
	race.this.ExperienceMultiplier = 96;
	race.this.Height = 54;
	race.this.Weight = 140;
	
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