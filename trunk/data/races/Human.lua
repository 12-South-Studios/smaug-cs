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

--[[	
WhereName  <worn on finger>    ~
WhereName  <worn on finger>    ~
WhereName  <worn around neck>  ~
WhereName  <worn around neck>  ~
WhereName  <worn on body>      ~
WhereName  <worn on head>      ~
WhereName  <worn on legs>      ~
WhereName  <worn on feet>      ~
WhereName  <worn on hands>     ~
WhereName  <worn on arms>      ~
WhereName  <worn as shield>    ~
WhereName  <worn about body>   ~
WhereName  <worn about waist>  ~
WhereName  <worn around wrist> ~
WhereName  <worn around wrist> ~
WhereName  <wielded>           ~
WhereName  <held>              ~
WhereName  <dual wielded>      ~
WhereName  <worn on ears>      ~
WhereName  <worn on eyes>      ~
WhereName  <missile wielded>   ~
WhereName  <worn on back>  ~
WhereName  <worn over face>  ~
WhereName  <worn around ankle>  ~
WhereName  <worn around ankle>  ~
WhereName  <BUG Inform Nivek>  ~
WhereName  <BUG Inform Nivek>  ~
WhereName  <BUG Inform Nivek>  ~
End
--]]
end

LoadRace();

-- EOF