-- HUMAN.LUA
-- This is the Human Race file for the MUD
-- Revised: 2013.11.25
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadRace()
	newRace = LCreateRace("Human", 0);
	race.this = newRace;
	race.this.ClassRestriction = 512;
	race.this.Language = 1;
	race.this.MinimumAlignment = -1000;
	race.this.MaximumAlignment = 1000;
	race.this.ExperienceMultiplier = 100;
	race.this.Height = 66;
	race.this.Weight = 150;
	
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